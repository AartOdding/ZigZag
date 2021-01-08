using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var drawList = ImGui.GetWindowDrawList();
                var windowPadding = ImGui.GetStyle().WindowPadding;
                var framePadding = ImGui.GetStyle().FramePadding;

                var windowPos = ImGui.GetWindowPos();
                var windowMin = windowPos + ImGui.GetWindowContentRegionMin() - windowPadding;
                var windowMax = windowPos + ImGui.GetWindowContentRegionMax() + windowPadding;

                //Drawing.DrawThickLineRectangle(drawList, min, max, new Vector2(10, 10), ImGui.GetColorU32(ImGuiCol.DockingPreview));

                var leftColMin = windowMin;
                leftColMin.Y -= 1;
                var leftColMax = windowMax;
                leftColMax.X = windowMin.X + framePadding.X;

                drawList.AddRectFilled(leftColMin, leftColMax, ImGui.GetColorU32(ImGuiCol.MenuBarBg));

                var rightColMin = windowMin;
                rightColMin.Y -= 1;
                rightColMin.X = windowMax.X - framePadding.X;

                drawList.AddRectFilled(rightColMin, windowMax, ImGui.GetColorU32(ImGuiCol.MenuBarBg));



                //drawList.AddLine(m, m + new Vector2(30, 0), Drawing.U32Color(0.2f, 0.2f, 0));
                //drawList.AddLine(min, min + new Vector2(30, 0), ImGui.GetColorU32(ImGuiCol.DragDropTarget));


                // When using child windows it seems completely impossible to
                // dock nodes into nodes that are already docked in the main 
                // docking viewport.
                //drawList.AddRectFilled(minpos, maxpos, );

                //if (ImGui.BeginChild(0, size))
                //{
                DrawImplementation(style);
                //}
                //ImGui.EndChild();


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
