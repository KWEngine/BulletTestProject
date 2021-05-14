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

        public string Name { get; set; } = "undefined GameObject instance";

        private CollisionShape _shape = null;
        private CollisionShapeType _type = CollisionShapeType.Cube;
        private RigidBodyConstructionInfo _shapeRigidConstructionInfo = null;
        private RigidBody _rigidBody = null;
        private GhostObject _ghostObject;

        public GameObject(CollisionShapeType type, PhysicsSetupInfo physics)
        {
            _type = type;
            if (type == CollisionShapeType.Cube)
                _shape = new BoxShape(0.5f);
            else if (type == CollisionShapeType.Sphere)
                _shape = new SphereShape(0.5f);
            else
                _shape = new ConvexHullShape();
            //TODO: Add Custom...

            _shapeRigidConstructionInfo = new RigidBodyConstructionInfo(physics.Mass, new DefaultMotionState(), _shape);
            if (physics.Mass != 0)
            {
                _shapeRigidConstructionInfo.LocalInertia = _shape.CalculateLocalInertia(physics.Mass);
            }

            _shapeRigidConstructionInfo.Friction = physics.Friction;
            _shapeRigidConstructionInfo.RollingFriction = physics.Friction;
            _shapeRigidConstructionInfo.Restitution = physics.Restitution;


            // Note to self: AngularFactor verhindert oder begünstigt das Rotieren bei Kollisionen
            
            if (physics.ResponseType == ResponseType.Manual)
            {
                _shapeRigidConstructionInfo.AngularSleepingThreshold = 0.1f;
                _shapeRigidConstructionInfo.AngularDamping = 0.1f;
                _shapeRigidConstructionInfo.LinearDamping = 0.0f;
                //_shapeRigidConstructionInfo.LinearSleepingThreshold = 0.8f;
            }
            else
            {
                _shapeRigidConstructionInfo.AngularSleepingThreshold = 1f;    // Standard: 1
                _shapeRigidConstructionInfo.AngularDamping = 0.0f;             // Standard: 0
                _shapeRigidConstructionInfo.LinearDamping = 0.0f;                 // Standard: 0
                _shapeRigidConstructionInfo.LinearSleepingThreshold = 0.8f;     // Standard: 0
            }

            
            _rigidBody = new RigidBody(_shapeRigidConstructionInfo);
            _rigidBody.SpinningFriction = physics.Friction;

            if(physics.ResponseType == ResponseType.Manual)
            {
                _rigidBody.AngularFactor = new BulletSharp.Math.Vector3(0, 0, 0);
                _rigidBody.ActivationState = ActivationState.ActiveTag | ActivationState.DisableDeactivation;
                _rigidBody.CollisionFlags = CollisionFlags.CharacterObject | CollisionFlags.KinematicObject;
                //_rigidBody.DeactivationTime = -1;

                
                _ghostObject = new GhostObject();
                _ghostObject.CollisionShape = _shape;
                _ghostObject.WorldTransform = _rigidBody.WorldTransform;
                
                
                
            }
            else if(physics.ResponseType == ResponseType.Automatic)
            {
                
            }
            else if(physics.ResponseType == ResponseType.Static)
            {

            }

            _rigidBody.UserObject = this;

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

            if(HasGhostObject)
            {
                _ghostObject.WorldTransform = newTransform;
                Debug.WriteLine(_ghostObject.WorldTransform.Origin);
            }
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
            /*_rigidBody.LinearVelocity = new BulletSharp.Math.Vector3(
                x * Window.GetCurrentWindow().DeltaTimeFactor,
                y * Window.GetCurrentWindow().DeltaTimeFactor,
                z * Window.GetCurrentWindow().DeltaTimeFactor
                );*/

            if (HasGhostObject)
            {
                _ghostObject.WorldTransform = newTransform;
                //Debug.WriteLine(_ghostObject.WorldTransform.Origin);
            }

            //            body->getMotionState()->getWorldTransform(newTrans);
            //            newTrans.getOrigin() += btVector3(0, 0.02, 0);
            //            body->getMotionState()->setWorldTransform(newTrans);
        }

        public void SetScale(float x, float y, float z)
        {
            if(_rigidBody.IsInWorld)
            {
                throw new Exception("Scaling not allowed after an object is added to the world.");
            }
            _rigidBody.CollisionShape.LocalScaling = new BulletSharp.Math.Vector3(MathHelper.Max(x, float.Epsilon), MathHelper.Max(y, float.Epsilon), MathHelper.Max(z, float.Epsilon));
            _rigidBody.SetMassProps(_shapeRigidConstructionInfo.Mass, _rigidBody.CollisionShape.CalculateLocalInertia(_shapeRigidConstructionInfo.Mass));

            if (HasGhostObject)
            {
                _ghostObject.CollisionShape.LocalScaling = new BulletSharp.Math.Vector3(MathHelper.Max(x, float.Epsilon), MathHelper.Max(y, float.Epsilon), MathHelper.Max(z, float.Epsilon));
            }

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

        public bool HasGhostObject
        {
            get
            {
                return _ghostObject != null;
            }
        }

        public GhostObject GetGhostObject()
        {
            return _ghostObject;
        }

        //public abstract void OnCollision(GameObject collider);
    }
}
