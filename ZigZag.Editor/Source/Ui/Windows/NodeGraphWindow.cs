﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using ZigZag.SceneGraph;
using ZigZag.Mathematics;


namespace ZigZag.Editor.Ui.Windows
{
    class NodeGraphWindow : DockableWindow
    {
        public NodeGraphWindow(string name) : base(name)
        {
            m_scene = new Scene();
            m_renderer = new SceneGraph.SceneRenderer(m_scene, true);

            GeometryBuilder builder = new GeometryBuilder();
            builder.Color = new Mathematics.Color(0.5f, 0.5f, 1, 0.5f);
            builder.AddRectangle(new Rectangle(0, 0, 900, 900));

            m_rootNode = new GeometryNode();
            m_rootNode.Geometry = builder.Build();
            m_rootNode.Position = new Vector2(10, 10);
            m_scene.RootNode = m_rootNode;

            m_rootNode.OnMousePressed = e => e.Subscribe = e.Button == MouseButton.Left;
            m_rootNode.OnMouseDragged = e => m_rootNode.Position += e.Delta;

            for (int i = 0; i < 5; ++i)
            {
                var n = new SceneGraph.CircleTestWidget(m_rootNode);
                n.Position = new Vector2(i * 80, i * 80);
            }

            m_squareNode = new SceneGraph.SquareTestWidget(m_rootNode);
            m_squareNode.Position = new Vector2(300, 300);
        }

        protected override void DrawImplementation(Style style)
        { }

        public void Render(float windowWidth, float windowHeight)
        {
            m_renderer.Render(new Rectangle(ContentPos.X, ContentPos.Y, ContentSize.X, ContentSize.Y), windowWidth, windowHeight);
        }

        private float rotation = 0;
        private GeometryNode m_rootNode;
        private GeometryNode m_squareNode;
        internal Scene m_scene;
        private SceneGraph.SceneRenderer m_renderer;
    }
}
