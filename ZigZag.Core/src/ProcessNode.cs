
namespace ZigZag.Core
{
    public class ProcessNode : AbstractNode
    {
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

        public void Execute()
        {

        }

        public virtual void Process()
        {

        }

        private AbstractExecutor m_executor;
    }
}
