using System;
using System.Collections.Generic;
using System.Text;

namespace ZigZag.Core.Parameters
{
    public abstract class NumericalParameter : NodeParameter
    {
        public override bool Accepts(NodeParameter parameter)
        {
            return parameter is NumericalParameter;
        }

        public abstract long GetInt(int index);
        public abstract double GetFloat(int index);

    }
}
