using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigZag.SceneGraph
{
    internal class MouseButtonState
    {
        public const float DoubleClickIntervalSeconds = 0.5f;

        public MouseButtonState(MouseButton button)
        {
            Button = button;
            IsPressed = false;
        }

        public MouseButton Button
        {
            get;
            private set;
        }

        public bool IsPressed
        {
            get;
            private set;
        }

        public Node SubscribedNode
        {
            get;
            private set;
        }

        public Node PreviousSubscribedNode
        {
            get;
            private set;
        }

        // subscribingNode is allowed to be null if no node is subscribed
        public void Press(Node subscribingNode, out bool isDoubleClick)
        {
            var timeNow = DateTime.Now;
            var timeElapsed = (timeNow - m_lastPressTime).TotalSeconds;
            m_lastPressTime = timeNow;

            PreviousSubscribedNode = SubscribedNode;
            SubscribedNode = subscribingNode;

            IsPressed = true;
            isDoubleClick = timeElapsed <= DoubleClickIntervalSeconds && SubscribedNode == PreviousSubscribedNode;
        }

        public void Release()
        {
            IsPressed = false;
        }

        private DateTime m_lastPressTime = new DateTime();
    }
}
