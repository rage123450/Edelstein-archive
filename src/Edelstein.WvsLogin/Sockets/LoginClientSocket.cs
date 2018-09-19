using System;
using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.WvsLogin.Logging;
using Lamar;

namespace Edelstein.WvsLogin.Sockets
{
    public class LoginClientSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private WvsLogin _wvsLogin;

        public LoginClientSocket(IContainer container, IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
            this._wvsLogin = container.GetInstance<WvsLogin>();
        }

        public override void OnPacket(InPacket packet)
        {
            var operation = (LoginRecvOperations) packet.Decode<short>();

            switch (operation)
            {
                case LoginRecvOperations.CheckPassword:
                    var password = packet.Decode<string>();
                    var username = packet.Decode<string>();

                    Logger.Info($"Attempted login with {username} and {password}");
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
        }
    }
}