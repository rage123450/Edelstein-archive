using Edelstein.Network.Crypto;
using Xunit;

namespace Edelstein.Network.Tests.Crypto
{
    public class CIGCipherTest
    {
        [Fact]
        public void HashResultIsAccurate()
        {
            const uint key = 0x100;

            var result = IGCipher.InnoHash(key, 4, 0);

            Assert.Equal((uint) 0x2B1F7777, result);
            Assert.NotEqual(key, result);
        }
    }
}