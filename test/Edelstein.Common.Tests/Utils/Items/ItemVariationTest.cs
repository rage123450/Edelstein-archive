using Edelstein.Common.Utils;
using Edelstein.Common.Utils.Items;
using Xunit;

namespace Edelstein.Common.Tests.Utils.Items
{
    public class ItemVariationTest
    {
        [Fact]
        public void DefaultVariationResultIsAccurate()
        {
            var rand = new Rand32(100, 200, 300);
            var variation = new ItemVariation(rand, ItemVariationType.None);
            
            Assert.Equal(10, variation.Get(10));
            Assert.Equal(10, variation.Get(10));
            Assert.Equal(10, variation.Get(10));
        }
        
        [Fact]
        public void BetterVariationResultIsAccurate()
        {
            var rand = new Rand32(100, 200, 300);
            var variation = new ItemVariation(rand, ItemVariationType.Better);
            
            Assert.Equal(10, variation.Get(10));
            Assert.Equal(10, variation.Get(10));
            Assert.Equal(10, variation.Get(10));
        }
        
        [Fact]
        public void NormalVariationResultIsAccurate()
        {
            var rand = new Rand32(100, 200, 300);
            var variation = new ItemVariation(rand, ItemVariationType.Normal);
            
            Assert.Equal(10, variation.Get(10));
            Assert.Equal(11, variation.Get(10));
            Assert.Equal(12, variation.Get(10));
        }
        
        [Fact]
        public void GreatVariationResultIsAccurate()
        {
            var rand = new Rand32(100, 200, 300);
            var variation = new ItemVariation(rand, ItemVariationType.Great);
            
            Assert.Equal(10, variation.Get(10));
            Assert.Equal(11, variation.Get(10));
            Assert.Equal(10, variation.Get(10));
        }
        
        [Fact]
        public void GachaponVariationResultIsAccurate()
        {
            var rand = new Rand32(100, 200, 300);
            var variation = new ItemVariation(rand, ItemVariationType.Gachapon);
            
            Assert.Equal(12, variation.Get(10));
            Assert.Equal(11, variation.Get(10));
            Assert.Equal(10, variation.Get(10));
        }
    }
}