using System;
using System.Linq;
using DotNetty.Transport.Channels;
using Edelstein.Common.Interop.Game;
using Edelstein.Common.Packets;
using Edelstein.Database;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.WvsLogin.Logging;
using Edelstein.WvsLogin.Packets;
using Lamar;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.WvsLogin.Sockets
{
    public class LoginClientSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private IContainer _container;
        private WvsLogin _wvsLogin;

        private Account _account;
        private WorldInformation _selectedWorld;
        private ChannelInformation _selectedChannel;

        public LoginClientSocket(IContainer container, IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
            this._container = container;
            this._wvsLogin = container.GetInstance<WvsLogin>();
        }

        public override void OnPacket(InPacket packet)
        {
            var operation = (LoginRecvOperations) packet.Decode<short>();

            switch (operation)
            {
                case LoginRecvOperations.CheckPassword:
                    this.OnCheckPassword(packet);
                    break;
                case LoginRecvOperations.GuestIDLogin:
                    break;
                case LoginRecvOperations.AccountInfoRequest:
                    break;
                case LoginRecvOperations.WorldInfoRequest:
                    this.OnWorldInfoRequest(packet);
                    break;
                case LoginRecvOperations.SelectWorld:
                    this.OnSelectWorld(packet);
                    break;
                case LoginRecvOperations.CheckUserLimit:
                    this.OnCheckUserLimit(packet);
                    break;
                case LoginRecvOperations.ConfirmEULA:
                    break;
                case LoginRecvOperations.SetGender:
                    break;
                case LoginRecvOperations.CheckPinCode:
                    break;
                case LoginRecvOperations.UpdatePinCode:
                    break;
                case LoginRecvOperations.WorldRequest:
                    this.OnWorldInfoRequest(packet);
                    break;
                case LoginRecvOperations.LogoutWorld:
                    break;
                case LoginRecvOperations.ViewAllChar:
                    this.OnViewAllChar(packet);
                    break;
                case LoginRecvOperations.SelectCharacterByVAC:
                    break;
                case LoginRecvOperations.VACFlagSet:
                    break;
                case LoginRecvOperations.CheckNameChangePossible:
                    break;
                case LoginRecvOperations.RegisterNewCharacter:
                    break;
                case LoginRecvOperations.CheckTransferWorldPossible:
                    break;
                case LoginRecvOperations.SelectCharacter:
                    break;
                case LoginRecvOperations.MigrateIn:
                    break;
                case LoginRecvOperations.CheckDuplicatedID:
                    this.OnCheckDuplicateID(packet);
                    break;
                case LoginRecvOperations.CreateNewCharacter:
                    this.OnCreateNewCharacter(packet);
                    break;
                case LoginRecvOperations.CreateNewCharacterInCS:
                    break;
                case LoginRecvOperations.DeleteCharacter:
                    break;
                case LoginRecvOperations.AliveAck:
                    break;
                case LoginRecvOperations.ExceptionLog:
                    break;
                case LoginRecvOperations.SecurityPacket:
                    break;
                case LoginRecvOperations.EnableSPWRequest:
                    this.OnEnableSPWRequest(packet);
                    break;
                case LoginRecvOperations.CheckSPWRequest:
                    this.OnCheckSPWRequest(packet);
                    break;
                case LoginRecvOperations.EnableSPWRequestByACV:
                    this.OnEnableSPWRequest(packet);
                    break;
                case LoginRecvOperations.CheckSPWRequestByACV:
                    this.OnCheckSPWRequest(packet);
                    break;
                case LoginRecvOperations.CheckOTPRequest:
                    break;
                case LoginRecvOperations.CheckDeleteCharacterOTP:
                    break;
                case LoginRecvOperations.CreateSecurityHandle:
                    break;
                case LoginRecvOperations.SSOErrorLog:
                    break;
                case LoginRecvOperations.ClientDumpLog:
                    break;
                case LoginRecvOperations.CheckExtraCharInfo:
                    break;
                case LoginRecvOperations.CreateNewCharacter_Ex:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Logger.Debug($"Received packet operation {Enum.GetName(typeof(LoginRecvOperations), operation)}");
        }

        public override void OnDisconnect()
        {
            throw new NotImplementedException();
        }

        private void OnCheckPassword(InPacket packet)
        {
            var password = packet.Decode<string>();
            var username = packet.Decode<string>();

            using (var p = new OutPacket(LoginSendOperations.CheckPasswordResult))
            {
                using (var db = this._container.GetInstance<DataContext>())
                {
                    var account = db.Accounts
                        .Include(a => a.Data)
                        .Include(a => a.Characters)
                        .ThenInclude(c => c.InventoryEquipped)
                        .ThenInclude(i => i.Items)
                        .Include(c => c.Characters)
                        .ThenInclude(c => c.InventoryEquippedCash)
                        .ThenInclude(i => i.Items)
                        .SingleOrDefault(a => a.Username.Equals(username));
                    byte result = 0x0;

                    if (account == null) result = 0x5;
                    else
                    {
                        if (!BCrypt.Net.BCrypt.Verify(password, account.Password)) result = 0x4;
                    }

                    p.Encode<byte>(result);
                    p.Encode<byte>(0);
                    p.Encode<int>(0);

                    if (result == 0x0)
                    {
                        this._account = account;

                        p.Encode<int>(account.ID); // pBlockReason
                        p.Encode<byte>(0); // pBlockReasonIter
                        p.Encode<byte>(0); // nGradeCode
                        p.Encode<short>(0); // nSubGradeCode
                        p.Encode<byte>(0); // nCountryID
                        p.Encode<string>(account.Username); // sNexonClubID
                        p.Encode<byte>(0); // nPurchaseEXP
                        p.Encode<byte>(0); // ChatUnblockReason
                        p.Encode<long>(0); // dtChatUnblockDate
                        p.Encode<long>(0); // dtRegisterDate
                        p.Encode<int>(4); // nNumOfCharacter
                        p.Encode<byte>(1); // v44
                        p.Encode<byte>(0); // sMsg

                        p.Encode<long>(0); // dwHighDateTime
                    }

                    SendPacket(p);
                }
            }
        }

        private void OnWorldInfoRequest(InPacket packet)
        {
            var latestConnectedWorld = 0;

            this._wvsLogin.InteropClients.ForEach(c =>
            {
                var worldInformation = c.Socket.WorldInformation;

                latestConnectedWorld = worldInformation.ID;
                using (var p = new OutPacket(LoginSendOperations.WorldInformation))
                {
                    worldInformation.Encode(p);
                    p.Encode<short>(0); // nBalloonCount
                    SendPacket(p);
                }
            });

            using (var p = new OutPacket(LoginSendOperations.WorldInformation))
            {
                p.Encode<byte>(0xFF);
                SendPacket(p);
            }

            // TODO: Proper latest connected world
            using (var p = new OutPacket(LoginSendOperations.LatestConnectedWorld))
            {
                p.Encode<int>(latestConnectedWorld);
                SendPacket(p);
            }
        }

        private void OnCheckUserLimit(InPacket packet)
        {
            using (var p = new OutPacket(LoginSendOperations.CheckUserLimitResult))
            {
                p.Encode<byte>(0); // bOverUserLimit
                p.Encode<byte>(0); // bPopulateLevel

                SendPacket(p);
            }
        }

        private void OnSelectWorld(InPacket packet)
        {
            packet.Decode<byte>();

            var worldID = packet.Decode<byte>();
            var channelID = packet.Decode<byte>() + 1;

            using (var p = new OutPacket(LoginSendOperations.SelectWorldResult))
            {
                byte result = 0x0;
                var world = this._wvsLogin.InteropClients
                    .Select(c => c.Socket.WorldInformation)
                    .SingleOrDefault(w => w.ID == worldID);
                var channel = world?.Channels.SingleOrDefault(c => c.ID == channelID);

                if (world == null) result = 0x1;
                if (channel == null) result = 0x1;

                p.Encode<byte>(result);

                if (result == 0)
                {
                    this._selectedWorld = world;
                    this._selectedChannel = channel;

                    using (var db = this._container.GetInstance<DataContext>())
                    {
                        var data = _account.Data.SingleOrDefault(d => d.WorldID == worldID);

                        if (data == null)
                        {
                            data = new AccountData
                            {
                                WorldID = worldID,
                                SlotCount = 3
                            };

                            _account.Data.Add(data);
                            db.Update(_account);
                            db.SaveChanges();
                        }

                        var characters = _account.Characters.Where(c => c.WorldID == worldID).ToList();

                        p.Encode<byte>((byte) characters.Count);
                        characters.ForEach(c =>
                        {
                            c.EncodeStats(p);
                            c.EncodeLook(p);

                            p.Encode<bool>(false);
                            p.Encode<bool>(false);
                        });

                        p.Encode<bool>(!string.IsNullOrEmpty(_account.SPW)); // bLoginOpt TODO: proper bLoginOpt stuff
                        p.Encode<int>(data.SlotCount); // nSlotCount
                        p.Encode<int>(0); // nBuyCharCount
                    }
                }

                SendPacket(p);
            }
        }

        private void OnCheckDuplicateID(InPacket packet)
        {
            var name = packet.Decode<string>();

            using (var db = this._container.GetInstance<DataContext>())
            {
                var isDuplicatedID = db.Characters.Any(c => c.Name.Equals(name));

                using (var p = new OutPacket(LoginSendOperations.CheckDuplicatedIDResult))
                {
                    p.Encode<string>(name);
                    p.Encode<bool>(isDuplicatedID);

                    SendPacket(p);
                }
            }
        }

        private void OnCreateNewCharacter(InPacket packet)
        {
            var name = packet.Decode<string>();
            var job = packet.Decode<int>();
            var subJob = packet.Decode<short>();
            var face = packet.Decode<int>();
            var hair = packet.Decode<int>() + packet.Decode<int>();
            var skin = packet.Decode<int>();
            var top = packet.Decode<int>();
            var bottom = packet.Decode<int>();
            var shoes = packet.Decode<int>();
            var weapon = packet.Decode<int>();
            var gender = packet.Decode<byte>();

            using (var db = this._container.GetInstance<DataContext>())
            {
                var character = new Character
                {
                    Name = name,
                    WorldID = _selectedWorld.ID,
                    Job = 0,
                    Face = face,
                    Hair = hair,
                    Skin = (byte) skin,
                    Gender = gender
                };

                // TODO: Inventory management
                var equipped = character.InventoryEquipped.Items;
                var topItem = new ItemSlotEquip();
                var bottomItem = new ItemSlotEquip();
                var shoesItem = new ItemSlotEquip();
                var weaponItem = new ItemSlotEquip();

                topItem.Slot = 5;
                topItem.TemplateID = top;
                bottomItem.Slot = 6;
                bottomItem.TemplateID = bottom;
                shoesItem.Slot = 7;
                shoesItem.TemplateID = shoes;
                weaponItem.Slot = 11;
                weaponItem.TemplateID = weapon;

                equipped.Add(topItem);
                equipped.Add(bottomItem);
                equipped.Add(shoesItem);
                equipped.Add(weaponItem);

                _account.Characters.Add(character);
                db.Update(_account);
                db.SaveChanges();

                using (var p = new OutPacket(LoginSendOperations.CreateNewCharacterResult))
                {
                    p.Encode<bool>(false);
                    character.EncodeStats(p);
                    character.EncodeLook(p);
                    p.Encode<bool>(false);
                    p.Encode<bool>(false);
                    SendPacket(p);
                }
            }
        }

        private void OnEnableSPWRequest(InPacket packet)
        {
            packet.Decode<bool>(); // ?
            packet.Decode<int>(); // dwCharacterID
            packet.Decode<string>(); // sMacAddress
            packet.Decode<string>(); // sMacAddressWithHDDSerial
            var spw = packet.Decode<string>();

            if (!string.IsNullOrEmpty(_account.SPW)) return;
            if (BCrypt.Net.BCrypt.Verify(spw, _account.Password))
            {
                using (var p = new OutPacket(LoginSendOperations.EnableSPWResult))
                {
                    p.Encode<bool>(false);
                    p.Encode<byte>(0x16);
                    SendPacket(p);
                }

                return;
            }

            using (var db = this._container.GetInstance<DataContext>())
            {
                _account.SPW = BCrypt.Net.BCrypt.HashPassword(spw);
                db.Update(_account);
                db.SaveChanges();
            }
        }

        private void OnCheckSPWRequest(InPacket packet)
        {
            var spw = packet.Decode<string>();
            var characterID = packet.Decode<int>();
            packet.Decode<string>(); // sMacAddress
            packet.Decode<string>(); // sMacAddressWithHDDSerial

            if (string.IsNullOrEmpty(_account.SPW)) return;
            if (!BCrypt.Net.BCrypt.Verify(spw, _account.SPW))
            {
                using (var p = new OutPacket(LoginSendOperations.CheckSPWResult))
                {
                    p.Encode<bool>(false); // Unused byte
                    SendPacket(p);
                }

                return;
            }
        }

        private void OnViewAllChar(InPacket packet)
        {
            var worlds = this._wvsLogin.InteropClients.Select(c => c.Socket.WorldInformation).ToList();
            var allCharacters = this._account.Characters.Where(c => worlds.Any(w => c.WorldID == w.ID)).ToList();

            using (var p = new OutPacket(LoginSendOperations.ViewAllCharResult))
            {
                p.Encode<byte>(0x1);
                p.Encode<int>(worlds.Count);
                p.Encode<int>(allCharacters.Count);
                SendPacket(p);
            }
            
            if (worlds.Any() && allCharacters.Any())
            {
                worlds.ForEach(w =>
                {
                    var characters = allCharacters.Where(c => c.WorldID == w.ID).ToList();

                    using (var p = new OutPacket(LoginSendOperations.ViewAllCharResult))
                    {
                        p.Encode<byte>(0x0);
                        p.Encode<byte>(w.ID);
                        p.Encode<byte>((byte) characters.Count);

                        characters.ForEach(c =>
                        {
                            c.EncodeStats(p);
                            c.EncodeLook(p);
                            p.Encode<bool>(false);
                        });

                        p.Encode<bool>(!string.IsNullOrEmpty(_account.SPW));
                        SendPacket(p);
                    }
                });
            }
        }
    }
}