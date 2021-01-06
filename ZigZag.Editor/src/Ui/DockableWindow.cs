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

            style.BeginDockableWindow(this);
            if (ImGui.Begin(Name, ref wantsToStayOpen, ImGuiWindowFlags.None))
            {
                var size = ImGui.GetContentRegionAvail();

                //if (ImGui.BeginChild(0, size))
                {
                    DrawImplementation(style);
                }
                //ImGui.EndChild();


                if (!wantsToStayOpen)
                {
                    WantsToClose = true;
                }
            }

            HasFocus = ImGui.IsWindowFocused(ImGuiFocusedFlags.RootAndChildWindows);
            IsDocked = ImGui.IsWindowDocked();

            ImGui.End();
            style.EndDockableWindow(this);
        }

        protected abstract void DrawImplementation(Style style);
    }
}
