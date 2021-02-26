﻿using ZigZag.Editor.SceneGraphIntegration;
using ZigZag.Editor.Ui;
using ZigZag.Mathematics;
using ZigZag.SceneGraph;
using ZigZag.SceneGraph.Widgets;


namespace ZigZag.Editor.WiringEditor
{
    class WiringEditorWindow : DockableWindow
    {
        public WiringEditorWindow(string name) : base(name)
        {
            m_scene = new Scene();
            m_renderer = new SceneRenderer(m_scene, true);
            m_surface = new Surface();
            m_scene.RootNode = m_surface;

            for (int i = 0; i < 5; ++i)
            {
                var n = new CircleTestWidget(m_surface);
                n.Position = new Vector2(i * 80, i * 80);
            }

            m_squareNode = new SquareTestWidget(m_surface);
            m_squareNode.Position = new Vector2(300, 300);
        }

        protected override void DrawImplementation(Style style)
        { }

        public void Render(float windowWidth, float windowHeight)
        {
            m_renderer.Render(new Rectangle(ContentPos.X, ContentPos.Y, ContentSize.X, ContentSize.Y), windowWidth, windowHeight);
        }

        private InfinitePlane m_surface;
        private GeometryNode m_squareNode;
        internal Scene m_scene;
        private SceneRenderer m_renderer;
    }
}