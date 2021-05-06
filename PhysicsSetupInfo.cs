using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace BulletTest
{
    public enum ResponseType
    {
        TwoWay,
        OneWay,
        Static
    }

    struct PhysicsSetupInfo
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


        public PhysicsSetupInfo(float mass = 0, ResponseType type = ResponseType.Static)
        {
            mass = MathHelper.Max(mass, 0);
            _mass = mass;
            _friction = 1f;
            _restitution = 0f;
            _responseType = type;
        }
        
    }
}
