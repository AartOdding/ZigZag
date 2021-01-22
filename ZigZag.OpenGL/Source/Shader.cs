using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;


namespace ZigZag.OpenGL
{
    public class Shader
    {
        public Shader(string vertexSource, string fragmentSource)
        {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            CompileShader(vertexShader);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            CompileShader(fragmentShader);

            m_shaderHandle = GL.CreateProgram();
            GL.AttachShader(m_shaderHandle, vertexShader);
            GL.AttachShader(m_shaderHandle, fragmentShader);
            LinkProgram(m_shaderHandle);

            // When the shader program is linked, it no longer needs the individual shaders attacked to it; the compiled code is copied into the shader program.
            // Detach them, and then delete them.
            GL.DetachShader(m_shaderHandle, vertexShader);
            GL.DetachShader(m_shaderHandle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            GL.GetProgram(m_shaderHandle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(m_shaderHandle, i, out _, out _);
                var location = GL.GetUniformLocation(m_shaderHandle, key);
                m_uniformLocations.Add(key, location);
            }
        }

        public void Release()
        {
            if (m_released)
            {
                throw new Exception("Shader program has been released already");
            }
            if (m_shaderHandle != 0)
            {
                GL.DeleteProgram(m_shaderHandle);
                m_shaderHandle = 0;
                m_released = true;
            }
        }

        public void Use()
        {
            if (m_released)
            {
                throw new Exception("Shader program has been released");
            }
            GL.UseProgram(m_shaderHandle);
        }

        // The shader sources provided with this project use hardcoded layout(location)-s. If you want to do it dynamically,
        // you can omit the layout(location=X) lines in the vertex shader, and use this in VertexAttribPointer instead of the hardcoded values.
        public int GetAttributeLocation(string attributeName)
        {
            return GL.GetAttribLocation(m_shaderHandle, attributeName);
        }

        public void SetInt(string name, int data)
        {
            Use();
            GL.Uniform1(m_uniformLocations[name], data);
        }

        public void SetFloat(string name, float data)
        {
            Use();
            GL.Uniform1(m_uniformLocations[name], data);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            Use();
            GL.UniformMatrix4(m_uniformLocations[name], true, ref data);
        }

        public void SetVector3(string name, Vector3 data)
        {
            Use();
            GL.Uniform3(m_uniformLocations[name], data);
        }

        public void SetVector2(string name, Vector2 data)
        {
            Use();
            GL.Uniform2(m_uniformLocations[name], data);
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);

            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);

            if (code != (int)All.True)
            {
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        private int m_shaderHandle;
        private readonly Dictionary<string, int> m_uniformLocations = new Dictionary<string, int>();
        private bool m_released = false;
    }
}
