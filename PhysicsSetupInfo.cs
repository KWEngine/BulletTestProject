using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace BulletTest
{
    struct PhysicsSetupInfo
    {
        private BulletSharp.Math.Vector3 _inertia;
        public Vector3 Inertia
        {
            get
            {
                return new Vector3(_inertia.X, _inertia.Y, _inertia.Z);
            }
            set
            {
                _inertia = new BulletSharp.Math.Vector3(value.X, value.Y, value.Z);
            }
        }

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


        public PhysicsSetupInfo(float mass = 0)
        {
            _mass = mass;
            _inertia = new BulletSharp.Math.Vector3(1, 1, 1);
            _friction = 1;
            _restitution = 0;
        }
        
    }
}
