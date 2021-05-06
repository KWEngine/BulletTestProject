using BulletSharp;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BulletTest
{
    abstract class GameObject
    {
        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 NormalMatrix = Matrix4.Identity;
        public Vector3 Color = new Vector3(1, 1, 1);

        private CollisionShape _shape = null;
        private CollisionShapeType _type = CollisionShapeType.Cube;
        private RigidBodyConstructionInfo _shapeRigidConstructionInfo = null;
        private RigidBody _rigidBody = null;

        public GameObject(CollisionShapeType type, PhysicsSetupInfo physics)
        {
            _type = type;
            if (type == CollisionShapeType.Cube)
                _shape = new BoxShape(0.5f);
            else if (type == CollisionShapeType.Sphere)
                _shape = new SphereShape(0.5f);
            else
                _shape = new BoxShape(0.5f);
            //TODO: Add Custom...

            _shapeRigidConstructionInfo = new RigidBodyConstructionInfo(physics.Mass, new DefaultMotionState(), _shape);
            if (physics.Mass != 0)
            {
                _shapeRigidConstructionInfo.LocalInertia = _shape.CalculateLocalInertia(physics.Mass);
            }

            _shapeRigidConstructionInfo.Friction = physics.Friction;
            _shapeRigidConstructionInfo.RollingFriction = physics.Friction;
            _shapeRigidConstructionInfo.Restitution = physics.Restitution;


            // AngularFactor verhindert oder begünstigt das Rotieren bei Kollisionen
            _shapeRigidConstructionInfo.AngularSleepingThreshold = 0.01f;
            _shapeRigidConstructionInfo.AngularDamping = 0f;
            _shapeRigidConstructionInfo.LinearDamping = 0f;
            //_shapeRigidConstructionInfo.LinearSleepingThreshold = 0.9f;
            _rigidBody = new RigidBody(_shapeRigidConstructionInfo);
            _rigidBody.CollisionFlags = physics.ResponseType == ResponseType.TwoWay ? CollisionFlags.None : physics.ResponseType == ResponseType.OneWay ? CollisionFlags.KinematicObject : CollisionFlags.StaticObject;

            /* Default values:
                AngularDamping              = 0
                AngularFactor               = 1;1;1
                AngularSleepingThreshold    = 1
                AngularVelocity             = 0;0;0
                ContactDamping              = 0.1
                
                Gravity                     = 0;0;0
                LinearDamping               = 0
                LinearFactor                = 1;1;1
                LinearSleepingThreshold     = 0.8
                Restitution                 = 0
                
                Friction                    = 0.5
                SpinningFriction            = 0
                RollingFriction             = 0
            */
        }

        public abstract void Update(KeyboardState ks, MouseState ms);

        public void SetPosition(float x, float y, float z)
        {
            _rigidBody.Translate(new BulletSharp.Math.Vector3(x, y, z));
        }

        public void SetScale(float x, float y, float z)
        {
            if(_rigidBody.IsInWorld)
            {
                throw new Exception("Scaling not allowed after an object is added to the world.");
            }
            _rigidBody.CollisionShape.LocalScaling = new BulletSharp.Math.Vector3(MathHelper.Max(x, float.Epsilon), MathHelper.Max(y, float.Epsilon), MathHelper.Max(z, float.Epsilon));
            //_rigidBody.LocalInertia = _rigidBody.CollisionShape.CalculateLocalInertia(_shapeRigidConstructionInfo.Mass);
            DiscreteDynamicsWorld dw = Window.GetCurrentWorld().GetCollisionWorld();
            dw.UpdateAabbs();
        }
        
        public void UpdateModelMatrix()
        {
            ModelMatrix = CollisionGlobals.CreateOpenTKMatrixFromBulletMatrix(_rigidBody.WorldTransform, _rigidBody.CollisionShape.LocalScaling);
            //ModelMatrix = CollisionGlobals.CreateOpenTKMatrixFromBulletMatrix(_rigidBody.MotionState.WorldTransform);
            NormalMatrix = Matrix4.Invert(Matrix4.Transpose(ModelMatrix));
        }

        public RigidBody GetRigidBody()
        {
            return _rigidBody;
        }
    }
}
