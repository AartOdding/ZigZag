

namespace ZigZag.Core.Parameters
{
    class IntParameter : NumericalParameter
    {
        public long Value
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
                Value = ((NumericalParameter)ListenedParameter).GetInt(0);
            }
        }

        public override long GetInt(int index)
        {
            return m_value;
        }

        public override double GetFloat(int index)
        {
            return m_value;
        }

        private long m_value;
    }
}
