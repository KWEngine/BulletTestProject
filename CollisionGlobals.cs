using BulletSharp;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletTest
{
    public enum CollisionShapeType
    {
        Cube,
        Sphere,
        Custom
    }

    static class CollisionGlobals
    {
        public static CollisionConfiguration colConfiguration = new DefaultCollisionConfiguration();

        public static Matrix4 CreateOpenTKMatrixFromBulletMatrix(BulletSharp.Math.Matrix src)
        {
            return new Matrix4(
                src.M11, src.M12, src.M13, src.M14,
                src.M21, src.M22, src.M23, src.M24,
                src.M31, src.M32, src.M33, src.M34,
                src.M41, src.M42, src.M43, src.M44
                );
        }

        public static BulletSharp.Math.Matrix CreateBulletMatrixFromSRT(BulletSharp.Math.Vector3 s, BulletSharp.Math.Quaternion r, BulletSharp.Math.Vector3 t)
        {
            Matrix4 m = Matrix4.CreateFromQuaternion(new Quaternion(r.X, r.Y, r.Z, r.W));

            m.Row0 *= s.X;
            m.Row1 *= s.Y;
            m.Row2 *= s.Z;

            m.Row3.X = t.X;
            m.Row3.Y = t.Y;
            m.Row3.Z = t.Z;
            m.Row3.W = 1.0f;

            return new BulletSharp.Math.Matrix
                (
                    m.M11, m.M12, m.M13, m.M14,
                    m.M21, m.M22, m.M23, m.M24,
                    m.M31, m.M32, m.M33, m.M34,
                    m.M41, m.M42, m.M43, m.M44
                );
        }
    }


}
