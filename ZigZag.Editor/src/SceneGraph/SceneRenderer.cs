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

        public void Render(Rectangle area)
        {
            if (m_scene.RootNode is not null)
            {
                List<GeometryDrawData> drawDatas = new List<GeometryDrawData>();

                CreateDrawDataRecursive(m_scene.RootNode, drawDatas);

                m_shader.Use();
                m_shader.SetVector2("viewport_min", new Vector2(-100, -100));
                m_shader.SetVector2("viewport_max", new Vector2(800, 800));

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
