using System;
using Edelstein.Network.Crypto;
using Xunit;

namespace Edelstein.Network.Tests.Crypto
{
    public class ShandaCipherTest
    {
        [Fact]
        public void TransformResultIsAccurate()
        {
            const uint data = uint.MaxValue;

            var input = BitConverter.GetBytes(data);
            var encrypted = ShandaCipher.EncryptTransform(input);
            var decrypted = ShandaCipher.DecryptTransform(encrypted);

            Assert.Equal(data, BitConverter.ToUInt32(decrypted));
        }

        [Fact]
        public void EncryptTransformResultIsAccurate()
        {
            const uint data = uint.MaxValue;

            var result = ShandaCipher.EncryptTransform(BitConverter.GetBytes(data));

            Assert.Equal(0xCCE29458, BitConverter.ToUInt32(result));
            Assert.NotEqual(data, BitConverter.ToUInt32(result));
        }

        [Fact]
        public void DecryptTransformResultIsAccurate()
        {
            const uint data = uint.MaxValue;

            var result = ShandaCipher.DecryptTransform(BitConverter.GetBytes(data));

            Assert.Equal(0xE5C5D387, BitConverter.ToUInt32(result));
            Assert.NotEqual(data, BitConverter.ToUInt32(result));
        }
    }
}