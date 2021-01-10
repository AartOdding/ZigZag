using ImGuiNET;
using System.Numerics;


namespace ZigZag.Editor.Ui
{
    abstract class DockableWindow
    {
        public DockableWindow(string name)
        {
            Name = name;
            HasFocus = false;
            IsDocked = false;
            WantsToClose = false;
        }

        public string Name
        {
            get;
        }

        public bool HasFocus
        {
            get;
            private set;
        }

        public bool IsDocked
        {
            get;
            private set;
        }

        public bool WantsToClose
        {
            get;
            private set;
        }

        public void Draw(Style style)
        {
            bool wantsToStayOpen = true;

            //style.BeginDockableWindow(this);
            if (ImGui.Begin(Name, ref wantsToStayOpen, ImGuiWindowFlags.None))
            {
                var padding = ImGui.GetStyle().FramePadding.X;
                var windowMin = ImGui.GetWindowPos() + ImGui.GetWindowContentRegionMin() - ImGui.GetStyle().WindowPadding - new Vector2(0, 1);
                var windowMax = ImGui.GetWindowPos() + ImGui.GetWindowContentRegionMax() + ImGui.GetStyle().WindowPadding;
                var tabColor = HasFocus ? ((FlatStyle)style).OpenTabWithFocusColor : ((FlatStyle)style).OpenTabWithoutFocusColor;

                Drawing.DrawVariableThicknessRectangle(ImGui.GetWindowDrawList(), 
                    windowMin, windowMax, 0, padding, padding, padding, ((FlatStyle)style).ApplicationBackgroundColor);

                Drawing.DrawVariableThicknessRectangle(ImGui.GetWindowDrawList(),
                    windowMin + new Vector2(padding, 0), windowMax - new Vector2(padding, padding), 3, 3, tabColor);

                DrawImplementation(style);
                
                if (!wantsToStayOpen)
                {
                    WantsToClose = true;
                }
            }

            HasFocus = ImGui.IsWindowFocused(ImGuiFocusedFlags.RootAndChildWindows);
            IsDocked = ImGui.IsWindowDocked();

            ImGui.End();
            //style.EndDockableWindow(this);
        }

        protected abstract void DrawImplementation(Style style);
    }
}
