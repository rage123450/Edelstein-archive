using System.Collections.Generic;
using System.Linq;
using DotNetty.Transport.Channels;
using Edelstein.Common.Interop;
using Edelstein.Common.Interop.Game;
using Edelstein.Common.Packets;
using Edelstein.Database;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Database.Entities.Types;
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

        public Account Account { get; set; }
        private WorldInformation _selectedWorld;
        private ChannelInformation _selectedChannel;

        public LoginClientSocket(IContainer container, IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
            _container = container;
            _wvsLogin = container.GetInstance<WvsLogin>();
        }

        public override void OnPacket(InPacket packet)
        {
            var operation = (LoginRecvOperations) packet.Decode<short>();

            switch (operation)
            {
                case LoginRecvOperations.CheckPassword:
                    OnCheckPassword(packet);
                    break;
                case LoginRecvOperations.GuestIDLogin:
                    break;
                case LoginRecvOperations.AccountInfoRequest:
                    break;
                case LoginRecvOperations.WorldInfoRequest:
                    OnWorldInfoRequest(packet);
                    break;
                case LoginRecvOperations.SelectWorld:
                    OnSelectWorld(packet);
                    break;
                case LoginRecvOperations.CheckUserLimit:
                    OnCheckUserLimit(packet);
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
                    OnWorldInfoRequest(packet);
                    break;
                case LoginRecvOperations.LogoutWorld:
                    break;
                case LoginRecvOperations.ViewAllChar:
                    OnViewAllChar(packet);
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
                    OnCheckDuplicateID(packet);
                    break;
                case LoginRecvOperations.CreateNewCharacter:
                    OnCreateNewCharacter(packet);
                    break;
                case LoginRecvOperations.CreateNewCharacterInCS:
                    break;
                case LoginRecvOperations.DeleteCharacter:
                    OnDeleteCharacter(packet);
                    break;
                case LoginRecvOperations.AliveAck:
                    break;
                case LoginRecvOperations.ExceptionLog:
                    break;
                case LoginRecvOperations.SecurityPacket:
                    break;
                case LoginRecvOperations.EnableSPWRequest:
                    OnEnableSPWRequest(packet, false);
                    break;
                case LoginRecvOperations.CheckSPWRequest:
                    OnCheckSPWRequest(packet);
                    break;
                case LoginRecvOperations.EnableSPWRequestByACV:
                    OnEnableSPWRequest(packet, true);
                    break;
                case LoginRecvOperations.CheckSPWRequestByACV:
                    OnCheckSPWRequest(packet);
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
                    Logger.Warn($"Unhandled packet operation {operation}");
                    break;
            }
        }

        public override void OnDisconnect()
        {
            if (Account != null)
            {
                using (var db = _container.GetInstance<DataContext>())
                {
                    if (Account.State != AccountState.MigratingIn)
                        Account.State = AccountState.LoggedOut;

                    db.Update(Account);
                    db.SaveChanges();
                }
            }
        }

        private void OnCheckPassword(InPacket packet)
        {
            var password = packet.Decode<string>();
            var username = packet.Decode<string>();

            using (var p = new OutPacket(LoginSendOperations.CheckPasswordResult))
            {
                using (var db = _container.GetInstance<DataContext>())
                {
                    var account = db.Accounts
                        .Include(a => a.Data)
                        .Include(a => a.Characters)
                        .ThenInclude(c => c.Inventories)
                        .ThenInclude(c => c.Items)
                        .SingleOrDefault(a => a.Username.Equals(username));
                    byte result = 0x0;

                    if (account == null) result = 0x5;
                    else
                    {
                        if (account.State != AccountState.LoggedOut) result = 0x7;
                        if (!BCrypt.Net.BCrypt.Verify(password, account.Password)) result = 0x4;
                    }

                    p.Encode<byte>(result);
                    p.Encode<byte>(0);
                    p.Encode<int>(0);

                    if (result == 0x0)
                    {
                        Account = account;

                        account.State = AccountState.LoggingIn;
                        db.Update(account);
                        db.SaveChanges();

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

            _wvsLogin.InteropClients.ForEach(c =>
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
                var world = _wvsLogin.InteropClients
                    .Select(c => c.Socket.WorldInformation)
                    .SingleOrDefault(w => w.ID == worldID);
                var channel = world?.Channels.SingleOrDefault(c => c.ID == channelID);

                if (world == null) result = 0x1;
                if (channel == null) result = 0x1;

                p.Encode<byte>(result);

                if (result == 0)
                {
                    _selectedWorld = world;
                    _selectedChannel = channel;

                    using (var db = _container.GetInstance<DataContext>())
                    {
                        var data = Account.Data.SingleOrDefault(d => d.WorldID == worldID);

                        if (data == null)
                        {
                            data = new AccountData
                            {
                                WorldID = worldID,
                                SlotCount = 3
                            };

                            Account.Data.Add(data);
                            db.Update(Account);
                            db.SaveChanges();
                        }

                        var characters = Account.Characters.Where(c => c.WorldID == worldID).ToList();

                        p.Encode<byte>((byte) characters.Count);
                        characters.ForEach(c =>
                        {
                            c.EncodeStats(p);
                            c.EncodeLook(p);

                            p.Encode<bool>(false);
                            p.Encode<bool>(false);
                        });

                        p.Encode<bool>(!string.IsNullOrEmpty(Account.SPW)); // bLoginOpt TODO: proper bLoginOpt stuff
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

            using (var db = _container.GetInstance<DataContext>())
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

            using (var db = _container.GetInstance<DataContext>())
            {
                var character = new Character
                {
                    Name = name,
                    WorldID = _selectedWorld.ID,
                    Job = 0,
                    Face = face,
                    Hair = hair,
                    Skin = (byte) skin,
                    Gender = gender,
                    FieldID = 310000000,
                    FieldPortal = 0,
                    Level = 1,
                    HP = 50,
                    MaxHP = 50,
                    MP = 50,
                    MaxMP = 50
                };

                character.Inventories = new List<ItemInventory>();

                var inventories = character.Inventories;

                inventories.Add(new ItemInventory(ItemInventoryType.Equip, 24));
                inventories.Add(new ItemInventory(ItemInventoryType.Use, 24));
                inventories.Add(new ItemInventory(ItemInventoryType.Setup, 24));
                inventories.Add(new ItemInventory(ItemInventoryType.Etc, 24));
                inventories.Add(new ItemInventory(ItemInventoryType.Cash, 24));

                inventories.Add(new ItemInventory(ItemInventoryType.Equipped, 60 + 60 + 4 + 5));

                // TODO: Inventory management
                var equipped = character.GetInventory(ItemInventoryType.Equipped).Items;
                var topItem = new ItemSlotEquip();
                var bottomItem = new ItemSlotEquip();
                var shoesItem = new ItemSlotEquip();
                var weaponItem = new ItemSlotEquip();

                topItem.Slot = 5;
                topItem.Durability = 100;
                topItem.TemplateID = top;

                if (bottom > 0) // Resistance Overalls
                {
                    bottomItem.Slot = 6;
                    bottomItem.Durability = 100;
                    bottomItem.TemplateID = bottom;
                    equipped.Add(bottomItem);
                }

                shoesItem.Slot = 7;
                shoesItem.Durability = 100;
                shoesItem.TemplateID = shoes;
                weaponItem.Slot = 11;
                weaponItem.Durability = 100;
                weaponItem.TemplateID = weapon;

                equipped.Add(topItem);
                equipped.Add(shoesItem);
                equipped.Add(weaponItem);

                Account.Characters.Add(character);
                db.Update(Account);
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

        private void OnEnableSPWRequest(InPacket packet, bool vac)
        {
            packet.Decode<bool>(); // ?
            packet.Decode<int>(); // dwCharacterID

            if (vac) packet.Decode<int>(); // Unknown

            packet.Decode<string>(); // sMacAddress
            packet.Decode<string>(); // sMacAddressWithHDDSerial
            var spw = packet.Decode<string>();

            if (!string.IsNullOrEmpty(Account.SPW)) return;
            if (BCrypt.Net.BCrypt.Verify(spw, Account.Password))
            {
                using (var p = new OutPacket(LoginSendOperations.EnableSPWResult))
                {
                    p.Encode<bool>(false);
                    p.Encode<byte>(0x16);
                    SendPacket(p);
                }

                return;
            }

            using (var db = _container.GetInstance<DataContext>())
            {
                Account.SPW = BCrypt.Net.BCrypt.HashPassword(spw);
                db.Update(Account);
                db.SaveChanges();
            }
        }

        private void OnCheckSPWRequest(InPacket packet)
        {
            var spw = packet.Decode<string>();
            var characterID = packet.Decode<int>();
            packet.Decode<string>(); // sMacAddress
            packet.Decode<string>(); // sMacAddressWithHDDSerial

            if (string.IsNullOrEmpty(Account.SPW)) return;
            if (!BCrypt.Net.BCrypt.Verify(spw, Account.SPW))
            {
                using (var p = new OutPacket(LoginSendOperations.CheckSPWResult))
                {
                    p.Encode<bool>(false); // Unused byte
                    SendPacket(p);
                }

                return;
            }

            var world = _wvsLogin.InteropClients.Single(c => c.Socket.WorldInformation.ID == _selectedWorld.ID);

            using (var p = new OutPacket(InteropRecvOperations.MigrationRequest))
            {
                p.Encode<byte>((byte) ServerType.Game);
                p.Encode<byte>(_selectedChannel.ID);
                p.Encode<string>(SessionKey);
                p.Encode<int>(characterID);
                world.Socket.SendPacket(p);
            }
        }

        private void OnViewAllChar(InPacket packet)
        {
            var worlds = _wvsLogin.InteropClients.Select(c => c.Socket.WorldInformation).ToList();
            var allCharacters = Account.Characters.Where(c => worlds.Any(w => c.WorldID == w.ID)).ToList();

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

                        p.Encode<bool>(!string.IsNullOrEmpty(Account.SPW));
                        SendPacket(p);
                    }
                });
            }
        }

        private void OnDeleteCharacter(InPacket packet)
        {
            var spw = packet.Decode<string>();
            var characterID = packet.Decode<int>();

            byte result = 0x0;

            if (!BCrypt.Net.BCrypt.Verify(spw, Account.SPW)) result = 0x14;

            if (result == 0x0)
            {
                using (var db = _container.GetInstance<DataContext>())
                {
                    var character = Account.Characters.Single(c => c.ID == characterID);

                    Account.Characters.Remove(character);
                    db.Characters.Remove(character);
                    db.Update(Account);
                    db.SaveChanges();
                }
            }

            using (var p = new OutPacket(LoginSendOperations.DeleteCharacterResult))
            {
                p.Encode<int>(characterID);
                p.Encode<byte>(result);
                SendPacket(p);
            }
        }
    }
}