using System.Numerics;
using ImGuiNET;


namespace ZigZag.Editor.Ui
{
    abstract class DockableWindow
    {
        public DockableWindow(string name)
        {
            Name = name;
            HasFocus = false;
            IsDocked = false;
            IsOpen = true;
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

        public bool IsVisible
        {
            get
            {
                return m_visibleLastRender && IsOpen;
            }
        }

        private bool m_visibleLastRender = false;

        public bool IsOpen
        {
            get;
            set;
        }

        public Vector2 ContentPos
        {
            get;
            private set;
        }

        public Vector2 ContentSize
        {
            get;
            private set;
        }

        public Mathematics.Rectangle ContentArea
        {
            get
            {
                return new Mathematics.Rectangle(ContentPos.X, ContentPos.Y, ContentSize.X, ContentSize.Y);
            }
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
                    IsOpen = false;
                }

                ContentPos = windowMin + new Vector2(padding + 3, 3);
                ContentSize = (windowMax - new Vector2(padding + 3, padding + 3) - new Vector2(padding + 3, 3)) - windowMin;

                m_visibleLastRender = true;
            }
            else
            {
                m_visibleLastRender = false;
            }

            HasFocus = ImGui.IsWindowFocused(ImGuiFocusedFlags.RootAndChildWindows);
            IsDocked = ImGui.IsWindowDocked();

            ImGui.End();
            //style.EndDockableWindow(this);
        }

        protected abstract void DrawImplementation(Style style);
    }
}
