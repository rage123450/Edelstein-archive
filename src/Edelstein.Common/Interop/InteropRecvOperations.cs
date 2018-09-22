namespace Edelstein.Common.Interop
{
    public enum InteropRecvOperations
    {
        ServerRegister = 0x0,
        ServerUpdate = 0x1,
        
        MigrationRequest = 0x5,
        MigrationRegisterResult = 0x6
    }
}