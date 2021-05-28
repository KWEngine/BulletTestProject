using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace BulletTest
{
    public enum ResponseType
    {
        Automatic,
        Manual,
        Static
    }

    public enum CollisionShapeType
    {
        Cube,
        Sphere,
        Custom
    }

    public struct PhysicsSetupInfo
    {
        private float _friction;
        public float Friction
        {
            get
            {
                return _friction;
            }
            set
            {
                _friction = MathHelper.Clamp(value, 0, 1);
            }
        }

        private float _mass;
        public float Mass
        {
            get
            {
                return _mass;
            }
            set
            {
                _mass = MathHelper.Max(value, 0);
            }
        }

        private float _restitution;
        public float Restitution
        {
            get
            {
                return _restitution;
            }
            set
            {
                _restitution = MathHelper.Max(value, 0);
            }
        }

        private ResponseType _responseType;
        public ResponseType ResponseType
        {
            get
            {
                return _responseType;
            }
            set
            {
                _responseType = value;
            }
        }

        private CollisionShapeType _shapeType;
        public CollisionShapeType CollisionShape
        {
            get
            {
                return _shapeType;
            }
            set
            {
                _shapeType = value;
            }
        }

        public PhysicsSetupInfo(float mass = 0, CollisionShapeType shape = CollisionShapeType.Cube, ResponseType type = ResponseType.Static)
        {
            _mass = MathHelper.Max(mass, 0);
            _friction = 0.49f;
            _restitution = 0.0f;
            _responseType = type;
            _shapeType = shape;
        }
        
    }
}
