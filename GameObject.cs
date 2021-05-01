using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletTest
{
    class GameObject
    {
        public Matrix4 ModelMatrix = Matrix4.Identity;
        private Vector3 Position = Vector3.Zero;
        public Quaternion Rotation = Quaternion.Identity;
        public Vector3 Scale = new Vector3(1, 1, 1);
        public Matrix4 NormalMatrix = Matrix4.Identity;
        public Vector3 Color = new Vector3(1, 1, 1);
        
        public void SetPosition(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
            UpdateModelMatrix();
        }

        public void SetScale(float x, float y, float z)
        {
            Scale = new Vector3(MathHelper.Max(x, float.Epsilon), MathHelper.Max(y, float.Epsilon), MathHelper.Max(z, float.Epsilon));
            UpdateModelMatrix();
        }

        private void UpdateModelMatrix()
        {
            CreateModelMatrix(ref Scale, ref Rotation, ref Position, out ModelMatrix);
            NormalMatrix = Matrix4.Invert(Matrix4.Transpose(ModelMatrix));
        }

        private static void CreateModelMatrix(ref Vector3 s, ref Quaternion r, ref Vector3 t, out Matrix4 m)
        {
            m = Matrix4.CreateFromQuaternion(r);

            m.Row0 *= s.X;
            m.Row1 *= s.Y;
            m.Row2 *= s.Z;

            m.Row3.X = t.X;
            m.Row3.Y = t.Y;
            m.Row3.Z = t.Z;
            m.Row3.W = 1.0f;
        }
    }
}
