using System;
using Edelstein.Network.Crypto;
using Xunit;

namespace Edelstein.Network.Tests.Crypto
{
    public class AESCipherTest
    {
        [Fact]
        public void TransformResultIsAccurate()
        {
            const int data = 0xABCDEF;
            const uint key = 0x100;

            var result = AESCipher.Transform(BitConverter.GetBytes(data), key);

            Assert.Equal(0x5883F451, BitConverter.ToInt32(result));
            Assert.NotEqual(data, BitConverter.ToInt32(result));
        }
    }
}