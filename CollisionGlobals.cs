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

        public static Matrix4 CreateOpenTKMatrixFromBulletMatrix(BulletSharp.Math.Matrix src, BulletSharp.Math.Vector3 scale)
        {
            return new Matrix4(
                src.M11 * scale.X, src.M12 * scale.X, src.M13 * scale.X, src.M14 * scale.X,
                src.M21 * scale.Y, src.M22 * scale.Y, src.M23 * scale.Y, src.M24 * scale.Y,
                src.M31 * scale.Z, src.M32 * scale.Z, src.M33 * scale.Z, src.M34 * scale.Z,
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

        public static void CreateModelMatrix(ref Vector3 s, ref Quaternion r, ref Vector3 t, out Matrix4 m)
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

        public static void CreateModelMatrix(float sX, float sY, float sZ, float rX, float rY, float rZ, float rW, float tX, float tY, float tZ, out Matrix4 m)
        {
            m = Matrix4.CreateFromQuaternion(new Quaternion(rX, rY, rZ, rW));

            m.Row0 *= sX;
            m.Row1 *= sY;
            m.Row2 *= sZ;

            m.Row3.X = tX;
            m.Row3.Y = tY;
            m.Row3.Z = tZ;
            m.Row3.W = 1.0f;
        }
    }


}
