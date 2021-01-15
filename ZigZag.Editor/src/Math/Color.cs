using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigZag.Editor.Math
{
    readonly struct Color
    {
        public readonly float Red;
        public readonly float Green;
        public readonly float Blue;
        public readonly float Alpha;

        public uint U32()
        {
            return Ui.Color.U32(Red, Green, Blue, Alpha);
        }
    }
}
