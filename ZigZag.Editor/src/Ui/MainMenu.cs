using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;


namespace ZigZag.Editor.Ui
{
    class MainMenu
    {
        public void Draw(Style style)
        {
            style.BeginMainMenu(this);
            ImGui.BeginMainMenuBar();

            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Save", "CTRL+S")) { }
                if (ImGui.MenuItem("Open", "CTRL+O")) { }
                if (ImGui.MenuItem("New", "CTRL+N")) { }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Edit"))
            {
                if (ImGui.MenuItem("Undo", "CTRL+Z")) { }
                if (ImGui.MenuItem("Redo", "CTRL+Y", false, false)) { }  // Disabled item
                ImGui.Separator();
                if (ImGui.MenuItem("Cut", "CTRL+X")) { }
                if (ImGui.MenuItem("Copy", "CTRL+C")) { }
                if (ImGui.MenuItem("Paste", "CTRL+V")) { }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("View"))
            {
                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();
            style.EndMainMenu(this);
        }
    }
}
