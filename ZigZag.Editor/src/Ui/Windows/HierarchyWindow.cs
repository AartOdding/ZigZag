using ImGuiNET;
using System;


namespace ZigZag.Editor.Ui.Windows
{
    class HierarchyWindow : DockableWindow
    {
        public HierarchyWindow(string name) : base(name)
        {
            
        }

        public Core.Project Project
        {
            get;
            set;
        }

        protected override void DrawImplementation(Style style)
        {
            DrawNode(Project);
        }

        private void DrawNode(Core.Node node)
        {
            ImGuiTreeNodeFlags flags = 0;
            flags |= ImGuiTreeNodeFlags.DefaultOpen;
            flags |= ImGuiTreeNodeFlags.OpenOnDoubleClick;
            flags |= ImGuiTreeNodeFlags.OpenOnArrow;
            flags |= ImGuiTreeNodeFlags.SpanFullWidth;
            flags |= ImGuiTreeNodeFlags.AllowItemOverlap;
            //flags |= m_objectSelection.isSelected(object) ? ImGuiTreeNodeFlags_Selected : 0;
            flags |= node.ChildNodeCount == 0 ? ImGuiTreeNodeFlags.Bullet : ImGuiTreeNodeFlags.Leaf;

            if(ImGui.TreeNodeEx(node.GetType().FullName))
            {
                foreach (var c in node.ChildNodes)
                {
                    DrawNode(c);
                }
                ImGui.TreePop();
            }

        }
    }
}
