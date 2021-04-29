using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace BulletTest
{
    static class PrimitiveCube
    {
        private static int _VAO = -1;
        private static int _VBOVertices = -1;
        private static int _VBONormals = -1;

        private static float[] _vertices = new float[]
        {
            //front
            -0.5f, -0.5f, +0.5f,
            +0.5f, -0.5f, +0.5f,
            +0.5f, +0.5f, +0.5f,
            +0.5f, +0.5f, +0.5f,
            -0.5f, +0.5f, +0.5f,
            -0.5f, -0.5f, +0.5f,

            //back
            +0.5f, +0.5f, -0.5f,
            +0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, +0.5f, -0.5f,
            +0.5f, +0.5f, -0.5f,

            //right
            +0.5f, -0.5f, +0.5f,
            +0.5f, -0.5f, -0.5f,
            +0.5f, +0.5f, -0.5f,
            +0.5f, +0.5f, -0.5f,
            +0.5f, +0.5f, +0.5f,
            +0.5f, -0.5f, +0.5f,

            //left
            -0.5f, -0.5f, +0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, +0.5f, -0.5f,
            -0.5f, +0.5f, -0.5f,
            -0.5f, +0.5f, +0.5f,
            -0.5f, -0.5f, +0.5f,

            //top
            -0.5f, +0.5f, +0.5f,
            +0.5f, +0.5f, +0.5f,
            +0.5f, +0.5f, -0.5f,
            +0.5f, +0.5f, -0.5f,
            -0.5f, +0.5f, -0.5f,
            -0.5f, +0.5f, +0.5f,

            //bottom
            +0.5f, -0.5f, -0.5f,
            +0.5f, -0.5f, +0.5f,
            -0.5f, -0.5f, +0.5f,
            -0.5f, -0.5f, +0.5f,
            -0.5f, -0.5f, -0.5f,
            +0.5f, -0.5f, -0.5f
        };

        private static float[] _normals = new float[]
        {
            0,0,1,
            0,0,1,
            0,0,1,
            0,0,1,
            0,0,1,
            0,0,1,

            0,0,-1,
            0,0,-1,
            0,0,-1,
            0,0,-1,
            0,0,-1,
            0,0,-1,

            1,0,0,
            1,0,0,
            1,0,0,
            1,0,0,
            1,0,0,
            1,0,0,

            -1,0,0,
            -1,0,0,
            -1,0,0,
            -1,0,0,
            -1,0,0,
            -1,0,0,

            0,1,0,
            0,1,0,
            0,1,0,
            0,1,0,
            0,1,0,
            0,1,0,

            0,1,0,
            0,1,0,
            0,1,0,
            0,1,0,
            0,1,0,
            0,1,0
        };

        private static Vector3[] _hitbox = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, +0.5f),
            new Vector3(+0.5f, -0.5f, +0.5f),
            new Vector3(-0.5f, +0.5f, +0.5f),
            new Vector3(+0.5f, +0.5f, +0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(+0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, +0.5f, -0.5f),
            new Vector3(+0.5f, +0.5f, -0.5f)
        };

        public static void Init()
        {
            if (_VAO < 0)
            {
                _VAO = GL.GenVertexArray();
                GL.BindVertexArray(VAO);

                _VBOVertices = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, _VBOVertices);
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * 4, _vertices, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                _VBONormals = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, _VBONormals);
                GL.BufferData(BufferTarget.ArrayBuffer, _normals.Length * 4, _normals, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(1);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                GL.BindVertexArray(0);
            }
        }

        public static int VAO
        {
            get
            {
                return _VAO;
            }
        }

        public static int VertexCount
        {
            get
            {
                return 36;
            }
        }

    }
}
