using BulletSharp;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletTest
{
    class GameObjectDynamic : GameObject
    {
        

        public GameObjectDynamic(CollisionShapeType type, PhysicsSetupInfo physics)
        {
            _type = type;
            if (type == CollisionShapeType.Cube)
                _shape = new BoxShape(0.5f);
            else if (type == CollisionShapeType.Sphere)
                _shape = new SphereShape(0.5f);
            else
                _shape = new ConvexHullShape();
            //TODO: Add Custom...

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

            _shapeRigidConstructionInfo = new RigidBodyConstructionInfo(physics.Mass, new DefaultMotionState(), _shape);
            if (physics.Mass != 0)
            {
                _shapeRigidConstructionInfo.LocalInertia = _shape.CalculateLocalInertia(physics.Mass);
            }

            _shapeRigidConstructionInfo.Friction = physics.Friction;
            _shapeRigidConstructionInfo.RollingFriction = physics.Friction;
            _shapeRigidConstructionInfo.Restitution = physics.Restitution;
            _shapeRigidConstructionInfo.AngularSleepingThreshold = 0.1f;
            _shapeRigidConstructionInfo.AngularDamping = 0f;
            _shapeRigidConstructionInfo.LinearDamping = 0f;
            _shapeRigidConstructionInfo.LinearSleepingThreshold = 0.8f;

            _rigidBody = new RigidBody(_shapeRigidConstructionInfo);
            _rigidBody.SpinningFriction = physics.Friction;
        }

        public override void Update(KeyboardState ks, MouseState ms)
        {
            
        }
    }
}
