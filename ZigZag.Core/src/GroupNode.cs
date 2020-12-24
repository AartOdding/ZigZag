

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
    }
}
