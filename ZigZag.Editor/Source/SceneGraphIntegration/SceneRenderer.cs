using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using ZigZag.Mathematics;
using ZigZag.OpenGL;
using ZigZag.SceneGraph;


namespace ZigZag.Editor.SceneGraphIntegration
{
    class SceneRenderer
    {
        public SceneRenderer(Scene scene, bool scissor)
        {
            m_shader = new Shader(
                Resources.Shaders.GeometryVertexShaderSource,
                Resources.Shaders.GeometryFragmentShaderSource);

            m_scene = scene;
            m_scissor = scissor;
        }

        public void Render(Rectangle area, float windowWidth, float windowHeight)
        {
            if (m_scene.RootNode is not null)
            {
                if (m_scissor)
                {
                    GL.Enable(EnableCap.ScissorTest);
                    GL.Scissor((int)area.X, (int)(windowHeight - area.BottomLeft().Y), (int)area.Width, (int)area.Height);
                }

                List<GeometryDrawData> drawDatas = new List<GeometryDrawData>();

                CreateDrawDataRecursive(m_scene.RootNode, Transform2D.Identity, drawDatas);

                m_shader.Use();

                Vector2 zero = new Vector2(0, 0);
                Vector2 max = new Vector2(windowWidth, windowHeight);

                var gl_min = Utils.MapRange(area.TopLeft(), zero, max, new Vector2(-1, 1), new Vector2(1, -1));
                var gl_max = Utils.MapRange(area.BottomRight(), zero, max, new Vector2(-1, 1), new Vector2(1, -1));

                m_shader.SetVector2("opengl_viewport_min", gl_min);
                m_shader.SetVector2("opengl_viewport_max", gl_max);

                m_shader.SetVector2("viewport_min", new Vector2(0, 0));
                m_shader.SetVector2("viewport_max", new Vector2(area.Width, area.Height));

                foreach (GeometryDrawData drawData in drawDatas)
                {
                    m_shader.SetMatrix3("transform", drawData.Transform);

                    drawData.Bind();
                    GL.DrawElements(PrimitiveType.Triangles, drawData.IndexCount, DrawElementsType.UnsignedInt, 0);
                }

                foreach (GeometryDrawData drawData in drawDatas)
                {
                    drawData.Release();
                }

                if (m_scissor)
                {
                    GL.Disable(EnableCap.ScissorTest);
                }
            }
        }

        private void CreateDrawDataRecursive(Node node, Transform2D accumulatedTransform, List<GeometryDrawData> drawDatas)
        {
            Transform2D newAccumulatedTransform = node.GetNodeTransform() * accumulatedTransform;

            if (node is GeometryNode geometryNode)
            {
                GeometryDrawData drawData = new GeometryDrawData(ref geometryNode.GeometryRef());
                drawData.Transform = newAccumulatedTransform;
                drawDatas.Add(drawData);
            }
            foreach (Node child in node.Children)
            {
                CreateDrawDataRecursive(child, newAccumulatedTransform, drawDatas);
            }
        }

        private readonly Shader m_shader;
        private readonly Scene m_scene;
        private bool m_scissor;
    }
}
