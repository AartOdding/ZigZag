

namespace ZigZag.Core.Parameters
{
    class FloatParameter : NumericalParameter
    {
        public double Value
        {
            get
            {
                return m_value;
            }
            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    Changed = true;
                }
            }
        }

        public override void Update()
        {
            if (IsListening() && ListenedParameter.Changed)
            {
                Value = ((NumericalParameter)ListenedParameter).GetFloat(0);
            }
        }

        public override long GetInt(int index)
        {
            return (long)m_value;
        }

        public override double GetFloat(int index)
        {
            return m_value;
        }

        private double m_value;
    }
}
