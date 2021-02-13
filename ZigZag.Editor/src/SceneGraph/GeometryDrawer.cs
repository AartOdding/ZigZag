using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigZag.SceneGraph;
using ZigZag.OpenGL;
using ZigZag.Mathematics;
using OpenTK.Graphics.OpenGL;


namespace ZigZag.Editor.SceneGraph
{
    class GeometryDrawer
    {
        public GeometryDrawer()
        {
            m_shader = new Shader(
                Resources.Shaders.GeometryVertexShaderSource,
                Resources.Shaders.GeometryFragmentShaderSource);

            m_drawData = new Dictionary<int, GeometryDrawData>();
        }

        public void AddGeometry(int id, ref Geometry geometry, uint z)
        {
            m_drawData.Add(id, new GeometryDrawData(ref geometry));
        }

        public void Draw(int w, int h)
        {
            m_shader.Use();

            m_shader.SetVector2("opengl_viewport_min", new Vector2(-1, 1));
            m_shader.SetVector2("opengl_viewport_max", new Vector2(1, -1));

            m_shader.SetVector2("viewport_min", new Vector2(0, 0));
            m_shader.SetVector2("viewport_max", new Vector2(w, h));

            foreach (var pair in m_drawData)
            {
                pair.Value.Bind();
                GL.DrawElements(PrimitiveType.Triangles, pair.Value.IndexCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        public void Release()
        {
            foreach (var pair in m_drawData)
            {
                pair.Value.Release();
            }
            m_drawData.Clear();
            m_shader.Release();
        }

        private readonly Shader m_shader;
        private readonly Dictionary<int, GeometryDrawData> m_drawData;
    }
}
