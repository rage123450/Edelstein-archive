using Edelstein.Common.Utils;
using Xunit;

namespace Edelstein.Common.Tests.Utils
{
    public class Rand32Test
    {
        [Fact]
        public void RandomResultIsAccurate()
        {
            var rand = new Rand32(100, 200, 300);
            
            Assert.Equal(0x2464C80, rand.Random());
        }
        
        [Fact]
        public void PrevRandomResultIsAccurate()
        {
            var rand = new Rand32(100, 200, 300);
            var prev = rand.Random();
            
            Assert.Equal(prev, rand.PrevRandom());
        }
    }
}