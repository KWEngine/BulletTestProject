using BulletSharp;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BulletTest
{
    class GameObject : ICharacterController
    {
        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 NormalMatrix = Matrix4.Identity;
        public Vector3 Color = new Vector3(1, 1, 1);

        private CollisionShape _shape = null;
        private CollisionShapeType _type = CollisionShapeType.Cube;
        private RigidBodyConstructionInfo _shapeRigidConstructionInfo = null;
        private RigidBody _rigidBody = null;
        private GameWorld _currentWorld = null;

        public void SetWorld(GameWorld w)
        {
            _currentWorld = w;
        }

        public bool CanJump => throw new NotImplementedException();

        public bool OnGround => throw new NotImplementedException();

        public GameObject(CollisionShapeType type)
        {
            _type = type;
            if (type == CollisionShapeType.Cube)
                _shape = new BoxShape(0.5f, 0.5f, 0.5f);
            else if (type == CollisionShapeType.Sphere)
                _shape = new SphereShape(0.5f);
            else
                _shape = new BoxShape(0.5f);
            //TODO: Add Custom...

            _shapeRigidConstructionInfo = new RigidBodyConstructionInfo(0f, new DefaultMotionState(), _shape);
            _rigidBody = new RigidBody(_shapeRigidConstructionInfo);
        }

        public void SetMass(float m)
        {
            _rigidBody.SetMassProps(m, new BulletSharp.Math.Vector3(0, 0, 0));
        }

        public void SetPosition(float x, float y, float z)
        {
            _rigidBody.Translate(new BulletSharp.Math.Vector3(x, y, z));
        }

        public void SetScale(float x, float y, float z)
        {
           // if(_currentWorld != null)
            {
                //_rigidBody.CollisionShape.LocalScaling = new BulletSharp.Math.Vector3(MathHelper.Max(x, float.Epsilon), MathHelper.Max(y, float.Epsilon), MathHelper.Max(z, float.Epsilon));
                //DiscreteDynamicsWorld dw = _currentWorld.GetCollisionWorld();
                //dw.UpdateSingleAabb(_rigidBody);

                _rigidBody.WorldTransform.Decompose(out BulletSharp.Math.Vector3 scale, out BulletSharp.Math.Quaternion rotation, out BulletSharp.Math.Vector3 translation);
                scale.X = x;
                scale.Y = y;
                scale.Z = z;
                BulletSharp.Math.Matrix newMatrix = CollisionGlobals.CreateBulletMatrixFromSRT(scale, rotation, translation);
                _rigidBody.WorldTransform = newMatrix;
            }
           // else
           // {
           //     throw new Exception("Object not in world yet.");
           // }
        }
        
        public void UpdateModelMatrix()
        {
            ModelMatrix = CollisionGlobals.CreateOpenTKMatrixFromBulletMatrix(_rigidBody.WorldTransform);
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

        public RigidBody GetRigidBody()
        {
            return _rigidBody;
        }

        public void Jump()
        {
            throw new NotImplementedException();
        }

        public void Jump(BulletSharp.Math.Vector3 dir)
        {
            throw new NotImplementedException();
        }

        public void PlayerStep(CollisionWorld collisionWorld, float deltaTime)
        {
            throw new NotImplementedException();
        }

        public void PreStep(CollisionWorld collisionWorld)
        {
            throw new NotImplementedException();
        }

        public void Reset(CollisionWorld collisionWorld)
        {
            throw new NotImplementedException();
        }

        public void SetUpInterpolate(bool value)
        {
            throw new NotImplementedException();
        }

        public void SetVelocityForTimeInterval(ref BulletSharp.Math.Vector3 velocity, float timeInterval)
        {
            throw new NotImplementedException();
        }

        public void SetWalkDirection(ref BulletSharp.Math.Vector3 walkDirection)
        {
            throw new NotImplementedException();
        }

        public void Warp(ref BulletSharp.Math.Vector3 origin)
        {
            throw new NotImplementedException();
        }

        public void DebugDraw(DebugDraw debugDrawer)
        {
            throw new NotImplementedException();
        }

        public void UpdateAction(CollisionWorld collisionWorld, float deltaTimeStep)
        {
            throw new NotImplementedException();
        }
    }
}
