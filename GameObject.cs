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

        protected CollisionShape _shape = null;
        protected RigidBodyConstructionInfo _shapeRigidConstructionInfo = null;
        protected RigidBody _rigidBody = null;
        protected CollisionShapeType _type;

        /*
        public GameObject()
        {
            


            if (physics.ResponseType == ResponseType.Player)
            {
                // AngularFactor verhindert oder begünstigt das Rotieren bei Kollisionen
                _shapeRigidConstructionInfo.AngularSleepingThreshold = 0.01f;
                _shapeRigidConstructionInfo.AngularDamping = 1f;
                _shapeRigidConstructionInfo.LinearDamping = 0f;
                _shapeRigidConstructionInfo.LinearSleepingThreshold = 0.01f;
                _shapeRigidConstructionInfo.Friction = 1f;
                _shapeRigidConstructionInfo.RollingFriction = 1f;
            }
            else
            {
                // AngularFactor verhindert oder begünstigt das Rotieren bei Kollisionen
                _shapeRigidConstructionInfo.AngularSleepingThreshold = 0.1f;
                _shapeRigidConstructionInfo.AngularDamping = 0f;
                _shapeRigidConstructionInfo.LinearDamping = 0f;
                _shapeRigidConstructionInfo.LinearSleepingThreshold = 0.8f;
            }

            
            _rigidBody = new RigidBody(_shapeRigidConstructionInfo);
            _rigidBody.CollisionFlags = physics.ResponseType == ResponseType.Dynamic ? CollisionFlags.None : physics.ResponseType == ResponseType.Player ? CollisionFlags.None : CollisionFlags.StaticObject;
            _rigidBody.SpinningFriction = physics.Friction;
            if(physics.ResponseType == ResponseType.Player)
            {
                //  _rigidBody.ActivationState = ActivationState.DisableDeactivation;
                _rigidBody.AngularFactor = new BulletSharp.Math.Vector3(0, 0.1f, 0);
                //_rigidBody.
            }

            //_rigidBody.SetContactStiffnessAndDamping()

           
        }
        */

        public abstract void Update(KeyboardState ks, MouseState ms);

        /*
        public void SetPosition(float x, float y, float z)
        {
            _rigidBody.Translate(new BulletSharp.Math.Vector3(x, y, z));
        }
        */

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
    }
}
