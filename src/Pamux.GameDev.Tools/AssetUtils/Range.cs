using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.Tools.AssetUtils
{
    public class Range<T>
    {
        public T Min; // Inclusive
        public T Max; // Exclusive

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }
    }
}
