using BulletSharp;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BulletTest
{
    public abstract class GameObject 
    {
        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 NormalMatrix = Matrix4.Identity;
        public Vector3 Color = new Vector3(1, 1, 1);

        public string Name { get; set; } = "undefined GameObject instance";

        private CollisionShape _shape = null;
        private CollisionShapeType _shapeTypeInternal = CollisionShapeType.Cube;
        private RigidBodyConstructionInfo _shapeRigidConstructionInfo = null;
        private RigidBody _rigidBody = null;
        private float _mass = 0;
        //private GhostObject _ghostObject;

        
        public GameObject(PhysicsSetupInfo physics)
        {
            _mass = physics.Mass;
            _shapeTypeInternal = physics.CollisionShape;

            if(_shapeTypeInternal == CollisionShapeType.Cube)
                _shapeRigidConstructionInfo = new RigidBodyConstructionInfo(physics.Mass, new DefaultMotionState(), new BoxShape(0.5f, 0.5f, 0.5f));
            else if(_shapeTypeInternal == CollisionShapeType.Sphere)
                _shapeRigidConstructionInfo = new RigidBodyConstructionInfo(physics.Mass, new DefaultMotionState(), new SphereShape(0.5f));
            else
                _shapeRigidConstructionInfo = new RigidBodyConstructionInfo(physics.Mass, new DefaultMotionState(), new ConvexHullShape()); // TODO!

            if (physics.ResponseType == ResponseType.Manual)
            {
                _shapeRigidConstructionInfo.AngularSleepingThreshold = 1f;
                _shapeRigidConstructionInfo.AngularDamping = 0f;
                _shapeRigidConstructionInfo.LinearDamping = 0.0f;
                _shapeRigidConstructionInfo.LinearSleepingThreshold = 1f;
            }
            else
            {
                _shapeRigidConstructionInfo.AngularSleepingThreshold = 0.01f;    // Standard: 1
                _shapeRigidConstructionInfo.AngularDamping = 0.0f;             // Standard: 0
                _shapeRigidConstructionInfo.LinearDamping = 0.0f;                 // Standard: 0
                _shapeRigidConstructionInfo.LinearSleepingThreshold = 0.1f;     // Standard: 0
            }
            _shapeRigidConstructionInfo.Friction = physics.Friction;
            _shapeRigidConstructionInfo.RollingFriction = physics.Friction;
            _shapeRigidConstructionInfo.LocalInertia = _shapeRigidConstructionInfo.CollisionShape.CalculateLocalInertia(_mass);
            
            _rigidBody = new RigidBody(_shapeRigidConstructionInfo);
            _rigidBody.SpinningFriction = physics.Friction;

            if(physics.ResponseType == ResponseType.Manual)
            {
                _rigidBody.AngularFactor = new BulletSharp.Math.Vector3(0, 0, 0);
            }
            else if(physics.ResponseType == ResponseType.Automatic)
            {
                
            }
            else if(physics.ResponseType == ResponseType.Static)
            {

            }

            _rigidBody.UserObject = this;
        }
        

        public abstract void Update(KeyboardState ks, MouseState ms);

        
        public void SetPosition(float x, float y, float z)
        {
            BulletSharp.Math.Matrix t = _rigidBody.MotionState.WorldTransform;
            t.Origin = new BulletSharp.Math.Vector3(x, y, z);
            _rigidBody.MotionState.WorldTransform = t;
            _rigidBody.CenterOfMassTransform = t;
        }

        public void SetPositionKinematic(float x, float y, float z)
        {
            BulletSharp.Math.Matrix newTransform = _rigidBody.MotionState.WorldTransform;
            newTransform.Origin = new BulletSharp.Math.Vector3(x, y, z);
            _rigidBody.MotionState.WorldTransform = newTransform;
        }

        public void MoveOffset(float x, float y, float z)
        {
            if (x != 0 || y != 0 || z != 0)
            {
                _rigidBody.Activate(true);
                /*
                BulletSharp.Math.Matrix t = _rigidBody.MotionState.WorldTransform;
                t.Origin = t.Origin + new BulletSharp.Math.Vector3(x, y, z);
                _rigidBody.MotionState.WorldTransform = t;
                _rigidBody.CenterOfMassTransform = t;
                */

                //_rigidBody.Translate(_rigidBody.WorldTransform.Origin + new BulletSharp.Math.Vector3(x, y, z));
                //_rigidBody.ApplyCentralImpulse(new BulletSharp.Math.Vector3(x, y, z));
                //_rigidBody.ApplyCentralForce(new BulletSharp.Math.Vector3(x,y,z));
                _rigidBody.LinearVelocity = (new BulletSharp.Math.Vector3(x, y, z));
                //_rigidBody.Translate(_rigidBody.WorldTransform.Origin + new BulletSharp.Math.Vector3(x, y, z));
            }
            else
            {
                //_rigidBody.LinearVelocity = (new BulletSharp.Math.Vector3(x, y, z));
            }
        }

        public void MoveKinematic(float x, float y, float z)
        {
            //            btTransform newTrans;
            BulletSharp.Math.Matrix newTransform = _rigidBody.MotionState.WorldTransform;
            newTransform.Origin += new BulletSharp.Math.Vector3(
                x * Window.GetCurrentWindow().DeltaTimeFactor, 
                y * Window.GetCurrentWindow().DeltaTimeFactor, 
                z * Window.GetCurrentWindow().DeltaTimeFactor
                );
            _rigidBody.MotionState.WorldTransform = newTransform;
        }
        

        public void SetScale(float x, float y, float z)
        {
            if (_rigidBody.IsInWorld)
            {
                throw new Exception("Scaling not allowed after an object is added to the world.");
            }
            _rigidBody.CollisionShape.LocalScaling = new BulletSharp.Math.Vector3(MathHelper.Max(x, float.Epsilon), MathHelper.Max(y, float.Epsilon), MathHelper.Max(z, float.Epsilon));
            _rigidBody.SetMassProps(_shapeRigidConstructionInfo.Mass, _rigidBody.CollisionShape.CalculateLocalInertia(_shapeRigidConstructionInfo.Mass));

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

        public override string ToString()
        {
            return Name;
        }
    }
}
