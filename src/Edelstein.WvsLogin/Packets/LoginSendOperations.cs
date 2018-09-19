namespace Edelstein.WvsLogin.Packets
{
    public enum LoginSendOperations
    {
        CheckPasswordResult = 0x0,
        GuestIDLoginResult = 0x1,
        AccountInfoResult = 0x2,
        CheckUserLimitResult = 0x3,
        SetAccountResult = 0x4,
        ConfirmEULAResult = 0x5,
        CheckPinCodeResult = 0x6,
        UpdatePinCodeResult = 0x7,
        ViewAllCharResult = 0x8,
        SelectCharacterByVACResult = 0x9,
        WorldInformation = 0xA,
        SelectWorldResult = 0xB,
        SelectCharacterResult = 0xC,
        CheckDuplicatedIDResult = 0xD,
        CreateNewCharacterResult = 0xE,
        DeleteCharacterResult = 0xF,
        MigrateCommand = 0x10,
        AliveReq = 0x11,
        AuthenCodeChanged = 0x12,
        AuthenMessage = 0x13,
        SecurityPacket = 0x14,
        EnableSPWResult = 0x15,
        DeleteCharacterOTPRequest = 0x16,
        CheckCrcResult = 0x17,
        LatestConnectedWorld = 0x18,
        RecommendWorldMessage = 0x19,
        CheckExtraCharInfoResult = 0x1A,
        CheckSPWResult = 0x1B
    }
}