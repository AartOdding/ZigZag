using System.Text.Json;


namespace ZigZag.Core
{
    public class GroupNode : Node
    {
        public sealed override void Update()
        {

        }

        public virtual void EarlyUpdate()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public AbstractExecutor Executor
        {
            get
            {
                return m_executor;
            }

            set
            {
                m_executor = value;

                if (!(m_executor is null))
                {
                    m_executor.Node = this;
                }
            }
        }

        private AbstractExecutor m_executor;

        internal override void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            base.WriteJson(writer, options);

            writer.WritePropertyName(typeof(GroupNode).FullName);
            writer.WriteStartObject();

            writer.WriteString("Stuff", "yes");

            // When adding more properties make sure to add them here.

            writer.WriteEndObject();
        }
        internal override void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            base.ReadJson(ref reader, options);

            Serialization.Assert(true);
        }

    }
}
