using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ImGuiNET;


namespace ZigZag.Editor.Ui
{
    class FlatStyle : Style
    {
        ImColor ViewportColor = new ImColor();
        ImColor MainMenuColor = new ImColor();


        public FlatStyle()
        {
            ViewportColor.Value = new System.Numerics.Vector4(1, 0, 0, 1);
            MainMenuColor.Value = new System.Numerics.Vector4(0, 1, 0, 1);
        }

        public override void BeginOverall()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 1);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);

            ImGui.PushStyleColor(ImGuiCol.Border, ImGui.GetColorU32(ImGuiCol.DockingEmptyBg));
            ImGui.PushStyleColor(ImGuiCol.Separator, ImGui.GetColorU32(ImGuiCol.DockingEmptyBg));
            ImGui.PushStyleColor(ImGuiCol.TitleBg, ImGui.GetColorU32(ImGuiCol.DockingEmptyBg));
            ImGui.PushStyleColor(ImGuiCol.TitleBgActive, ImGui.GetColorU32(ImGuiCol.DockingEmptyBg));
        }
        public override void EndOverall()
        {
            ImGui.PopStyleColor(3);
            ImGui.PopStyleVar(2);
        }

        public override void BeginMainMenu(MainMenu mainMenu)
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
        }

        public override void EndMainMenu(MainMenu mainMenu)
        {
            ImGui.PopStyleVar(1);
        }

        public override void BeginDockableWindow(DockableWindow dockableWindow)
        {
            // Always push two, so we can always pop two. 

            ImGui.PushStyleColor(ImGuiCol.ChildBg, new Vector4(1, 0, 0, 1));
            ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(1, 0, 0, 1));

            if (dockableWindow.IsDocked)
            {
                // Because of how docking works internally it is currently not possible to change 
                // the color of the docked window's title header just before starting the window.
                // therefor all the window titles are given their color in BeginOverall(), and the
                // free windows will change their color here.
                ImGui.PushStyleColor(ImGuiCol.TitleBg, ImGui.GetColorU32(ImGuiCol.TitleBg));
                ImGui.PushStyleColor(ImGuiCol.TitleBgActive, ImGui.GetColorU32(ImGuiCol.TitleBgActive));
            }
            else
            {
                ImGui.PushStyleColor(ImGuiCol.TitleBg, ImGui.GetColorU32(ImGuiCol.MenuBarBg));
                ImGui.PushStyleColor(ImGuiCol.TitleBgActive, ImGui.GetColorU32(ImGuiCol.MenuBarBg));
            }
        }

        public override void EndDockableWindow(DockableWindow dockableWindow)
        {
            ImGui.PopStyleColor(4);
        }
    }
}
