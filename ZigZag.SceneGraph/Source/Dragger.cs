

namespace ZigZag.SceneGraph
{
    public class Dragger
    {
        public Dragger(Node node, MouseButton mouseButton = MouseButton.Left)
        {
            m_node = node;
            m_node.MouseButtonPressEvent += OnPress;
            m_node.MouseButtonDragEvent += OnDrag;
            m_mouseButton = mouseButton;
        }

        private void OnPress(MouseButtonPressEvent e)
        {
            e.SetAccepted(e.Button == m_mouseButton);
        }

        private void OnDrag(MouseButtonDragEvent e)
        {
            m_node.Position += e.Delta;
        }

        private Node m_node;
        private MouseButton m_mouseButton;
    }
}
