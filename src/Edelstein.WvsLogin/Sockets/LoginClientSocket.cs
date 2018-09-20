using System;
using System.Linq;
using DotNetty.Transport.Channels;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.WvsLogin.Logging;
using Edelstein.WvsLogin.Packets;
using Lamar;

namespace Edelstein.WvsLogin.Sockets
{
    public class LoginClientSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private IContainer _container;
        private WvsLogin _wvsLogin;

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
                    break;
                case LoginRecvOperations.SelectWorld:
                    break;
                case LoginRecvOperations.CheckUserLimit:
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
                    break;
                case LoginRecvOperations.LogoutWorld:
                    break;
                case LoginRecvOperations.ViewAllChar:
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
                    break;
                case LoginRecvOperations.CreateNewCharacter:
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
                    break;
                case LoginRecvOperations.CheckSPWRequest:
                    break;
                case LoginRecvOperations.EnableSPWRequestByACV:
                    break;
                case LoginRecvOperations.CheckSPWRequestByACV:
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
                    var account = db.Accounts.SingleOrDefault(a => a.Username.Equals(username));
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
    }
}