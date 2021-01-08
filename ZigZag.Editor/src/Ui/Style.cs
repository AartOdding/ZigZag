using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;


namespace ZigZag.Editor.Ui
{
    class Style
    {
        public virtual void BeginOverall() { }
        public virtual void EndOverall() { }

        public virtual void BeginMainMenu(MainMenu mainMenu) { }
        public virtual void EndMainMenu(MainMenu mainMenu) { }

        public virtual void BeginDockableWindow(DockableWindow dockableWindow) { }
        public virtual void EndDockableWindow(DockableWindow dockableWindow) { }

        public virtual void BeginDockableWindowInner(DockableWindow dockableWindow) { }
        public virtual void EndDockableWindowInner(DockableWindow dockableWindow) { }
    }
}
