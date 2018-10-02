namespace Edelstein.Common.Utils
{
    public class Flag
    {
        private int[] data;

        public Flag(int bits)
        {
            data = new int[uBits >> 5]; 
            this.SetValue(0); 
        }
    }
}