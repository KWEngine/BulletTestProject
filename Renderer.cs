using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace BulletTest
{
    class Renderer
    {
        private static int _programId = -1;
        private static int _uniformMVP = -1;
        private static int _uniformNormalMatrix = -1;
        private static int _uniformModelMatrix = -1;

        public static void Init()
        {
            _programId = GL.CreateProgram();
            int svert = -1;
            int sfrag = -1;

            string resourceNameVertexShader = "BulletTest.Shaders.renderer.vert";
            string resourceNameFragmentShader = "BulletTest.Shaders.renderer.frag";
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream(resourceNameVertexShader))
            {
                svert = LoadShader(s, ShaderType.VertexShader, _programId);
            }
            using (Stream s = assembly.GetManifestResourceStream(resourceNameFragmentShader))
            {
                sfrag = LoadShader(s, ShaderType.FragmentShader, _programId);
            }

            if (svert > 0 && sfrag > 0)
            {
                GL.BindAttribLocation(_programId, 0, "aPosition");
                GL.BindAttribLocation(_programId, 1, "aNormal");
                GL.BindFragDataLocation(_programId, 0, "color");
                GL.LinkProgram(_programId);
            }
            else
            {
                throw new Exception("Creating and linking shaders failed.");
            }

            _uniformMVP = GL.GetUniformLocation(_programId, "uMVP");
            _uniformModelMatrix = GL.GetUniformLocation(_programId, "uModelMatrix");
            _uniformNormalMatrix = GL.GetUniformLocation(_programId, "uNormalMatrix");

            GL.UseProgram(_programId);
        }

        private static int LoadShader(Stream pFileStream, ShaderType pType, int pProgram)
        {
            int address = GL.CreateShader(pType);
            using (StreamReader sr = new StreamReader(pFileStream))
            {
                string source = sr.ReadToEnd();
                GL.ShaderSource(address, source);
            }
            GL.CompileShader(address);
            GL.AttachShader(pProgram, address);
            return address;
        }

        public static void Draw(GameObject g, ref Matrix4 viewProjection)
        {
            Matrix4 mvp = g.ModelMatrix * viewProjection;
            GL.UniformMatrix4(_uniformMVP, false, ref mvp);
            GL.UniformMatrix4(_uniformModelMatrix, false, ref g.ModelMatrix);
            GL.UniformMatrix4(_uniformNormalMatrix, false, ref g.NormalMatrix);

            GL.BindVertexArray(PrimitiveCube.VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, PrimitiveCube.VertexCount);
            GL.BindVertexArray(0);
        }
    }
}
