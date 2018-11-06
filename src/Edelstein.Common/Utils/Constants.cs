using Edelstein.Database.Entities.Types;

namespace Edelstein.Common.Utils
{
    public static class Constants
    {
        public static int GetJobLevel(Job job)
        {
            var id = (int) job;
            var result = 0;

            if (id % 100 > 0 && id != 2001)
            {
                var v1 = id % 10;
                if (id / 10 == 43) v1 = (id - 430) / 2;
                var v2 = v1 + 2;
                if (v2 >= 2 && (v2 <= 4 || v2 <= 10 && (id / 100 == 22 || id == 2001)))
                    result = v2;
            }
            else result = 1;

            return result;
        }
    }
}