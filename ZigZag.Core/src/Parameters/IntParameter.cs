

using System.Text.Json;

namespace ZigZag.Core.Parameters
{
    class IntParameter : NodeParameter, INumericalParameter
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

        public override bool Accepts(NodeParameter parameter)
        {
            return parameter is INumericalParameter;
        }

        public override void Update()
        {
            if (IsListening() && ListenedParameter.Changed)
            {
                Value = ((INumericalParameter)ListenedParameter).GetInt(0);
            }
        }

        public long GetInt(int index)
        {
            return m_value;
        }

        public double GetFloat(int index)
        {
            return m_value;
        }

        internal override void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            throw new System.NotImplementedException();
        }

        internal override void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            throw new System.NotImplementedException();
        }

        private long m_value;
    }
}
