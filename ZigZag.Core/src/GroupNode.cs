

namespace ZigZag.Core
{
    public class GroupNode : ProcessNode
    {
        public sealed override void Process()
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
    }
}
