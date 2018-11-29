using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class RandomKit
    {
        public static long GetRandom()
        {
            var random = new Random();
            return random.Next(1, 1000000);
        }

    }
}
