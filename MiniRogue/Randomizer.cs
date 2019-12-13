using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRogue
{
    internal static class Randomizer
    {
       public static Random rnd;

        public static void Setup(int seed) { rnd = new Random(seed); }
    }
}
