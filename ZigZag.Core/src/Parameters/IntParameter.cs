using System.Text.Json;
using ZigZag.Core.Serialization;


namespace ZigZag.Core.Parameters
{
    public class IntParameter : NodeParameter, INumericalParameter
    {
        public IntParameter() : base() { }
        public IntParameter(Node node) : base(node) { }

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
            if (IsListening() && Listened.Changed)
            {
                Value = ((INumericalParameter)Listened).GetInt(0);
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

        public new void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("Value", m_value);
            writer.WriteEndObject();
        }

        public new void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);
            SerializationUtils.MustReadPropertyName(ref reader, "Value");
            SerializationUtils.MustReadTokenType(ref reader, JsonTokenType.Number);
            m_value = reader.GetInt64();
            SerializationUtils.FinishCurrentObject(ref reader);
        }

        private long m_value;
    }
}
