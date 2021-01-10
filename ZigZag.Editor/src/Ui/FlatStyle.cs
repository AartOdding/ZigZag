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
        public uint ApplicationBackgroundColor { get; set; }
        public uint WindowBackgroundColor { get; set; }
        public uint MainMenuColor { get; set; }
        public uint OpenTabWithFocusColor { get; set; }
        public uint OpenTabWithoutFocusColor { get; set; }
        public uint HiddenTabColor { get; set; }
        public uint HoveredTabColor { get; set; }


        public FlatStyle()
        {
            ApplicationBackgroundColor = Color.U32(36, 36, 36);
            WindowBackgroundColor = Color.U32(22, 22, 22);
            MainMenuColor = Color.U32(36, 36, 36);

            HiddenTabColor = Color.U32(36, 36, 36);
            OpenTabWithFocusColor = Color.U32(63, 84, 97);
            OpenTabWithoutFocusColor = Color.U32(75, 75, 75);
            HoveredTabColor = Color.U32(73, 94, 107);
        }

        public override void BeginOverall()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.TabRounding, 0);

            ImGui.PushStyleColor(ImGuiCol.Separator, ApplicationBackgroundColor);
            ImGui.PushStyleColor(ImGuiCol.TitleBg, ApplicationBackgroundColor);
            ImGui.PushStyleColor(ImGuiCol.TitleBgActive, ApplicationBackgroundColor);
            ImGui.PushStyleColor(ImGuiCol.DockingEmptyBg, ApplicationBackgroundColor);
            ImGui.PushStyleColor(ImGuiCol.WindowBg, WindowBackgroundColor);
            ImGui.PushStyleColor(ImGuiCol.MenuBarBg, MainMenuColor);

            ImGui.PushStyleColor(ImGuiCol.Tab, HiddenTabColor);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocused, HiddenTabColor);
            ImGui.PushStyleColor(ImGuiCol.TabActive, OpenTabWithFocusColor);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocusedActive, OpenTabWithoutFocusColor);
            ImGui.PushStyleColor(ImGuiCol.TabHovered, HoveredTabColor);
        }

        public override void EndOverall()
        {
            ImGui.PopStyleColor(11);
            ImGui.PopStyleVar(3);
        }

        public override void BeginMainMenu(MainMenu mainMenu)
        {
        }

        public override void EndMainMenu(MainMenu mainMenu)
        {
        }

        public override void BeginDockableWindow(DockableWindow dockableWindow)
        {
        }

        public override void BeginDockableWindowInner(DockableWindow dockableWindow)
        {
        }

        public override void EndDockableWindowInner(DockableWindow dockableWindow)
        {
        }

        public override void EndDockableWindow(DockableWindow dockableWindow)
        {
        }
    }
}
