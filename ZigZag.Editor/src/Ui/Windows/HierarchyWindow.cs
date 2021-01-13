

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

        }
    }
}
