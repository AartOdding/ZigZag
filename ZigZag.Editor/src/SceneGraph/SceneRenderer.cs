using System.Collections.Generic;
using ZigZag.SceneGraph;
using ZigZag.OpenGL;
using ZigZag.Mathematics;

using OpenTK.Graphics.OpenGL;

namespace ZigZag.Editor.SceneGraph
{
    class SceneRenderer
    {
        public SceneRenderer(Scene scene)
        {
            m_shader = new Shader(
                Resources.Shaders.GeometryVertexShaderSource,
                Resources.Shaders.GeometryFragmentShaderSource);

            m_scene = scene;
        }

        public void Render(Rectangle area, float windowWidth, float windowHeight)
        {
            if (m_scene.RootNode is not null)
            {
                List<GeometryDrawData> drawDatas = new List<GeometryDrawData>();

                CreateDrawDataRecursive(m_scene.RootNode, drawDatas);

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
                    drawData.Bind();
                    GL.DrawElements(PrimitiveType.Triangles, drawData.IndexCount, DrawElementsType.UnsignedInt, 0);
                }

                foreach (GeometryDrawData drawData in drawDatas)
                {
                    drawData.Release();
                }
            }
        }

        private void CreateDrawDataRecursive(Node node, List<GeometryDrawData> drawDatas)
        {
            if (node is GeometryNode geometryNode)
            {
                drawDatas.Add(new GeometryDrawData(ref geometryNode.GeometryRef()));
            }
            foreach (Node child in node.Children)
            {
                CreateDrawDataRecursive(child, drawDatas);
            }
        }

        private readonly Shader m_shader;
        private readonly Scene m_scene;
    }
}
