

using System.Text.Json;

namespace ZigZag.Core.Parameters
{
    class FloatParameter : NodeParameter, INumericalParameter
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

        public override bool Accepts(NodeParameter parameter)
        {
            return parameter is INumericalParameter;
        }

        public override void Update()
        {
            if (IsListening() && ListenedParameter.Changed)
            {
                Value = ((INumericalParameter)ListenedParameter).GetFloat(0);
            }
        }

        public long GetInt(int index)
        {
            return (long)m_value;
        }

        public double GetFloat(int index)
        {
            return m_value;
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            throw new System.NotImplementedException();
        }

        public void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            throw new System.NotImplementedException();
        }

        private double m_value;
    }
}
