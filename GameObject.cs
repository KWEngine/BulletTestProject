﻿using BulletSharp;
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

        public abstract void Update(KeyboardState ks, MouseState ms);

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
            _rigidBody.CollisionShape.LocalScaling = new BulletSharp.Math.Vector3(MathHelper.Max(x, float.Epsilon), MathHelper.Max(y, float.Epsilon), MathHelper.Max(z, float.Epsilon));
            DiscreteDynamicsWorld dw = Window.GetCurrentWorld().GetCollisionWorld();
            _rigidBody.Activate();
            dw.UpdateAabbs();
        }
        
        public void UpdateModelMatrix()
        {
            ModelMatrix = CollisionGlobals.CreateOpenTKMatrixFromBulletMatrix(_rigidBody.WorldTransform, _rigidBody.CollisionShape.LocalScaling);
            NormalMatrix = Matrix4.Invert(Matrix4.Transpose(ModelMatrix));
        }

        public RigidBody GetRigidBody()
        {
            return _rigidBody;
        }
    }
}
