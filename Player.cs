using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BulletTest
{
    class Player : GameObject
    {
        public Player(CollisionShapeType type, PhysicsSetupInfo pInfo = new PhysicsSetupInfo())
            : base(type, pInfo)
        {
//            GetRigidBody().SetDamping(0, 0);

        }

        public override void Update(KeyboardState ks, MouseState ms)
        {
            BulletSharp.Math.Vector3 velocity = new BulletSharp.Math.Vector3(0, 0, 0);

            if (ks[Keys.A])
            {
               velocity += new BulletSharp.Math.Vector3(-1, 0, 0) * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            if (ks[Keys.D])
            {
                velocity += new BulletSharp.Math.Vector3(+1, 0, 0) * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            if (ks[Keys.W])
            {
                velocity += new BulletSharp.Math.Vector3(0, 0, -1) * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            if (ks[Keys.S])
            {
                velocity += new BulletSharp.Math.Vector3(0, 0, +1) * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            velocity.Normalize();
            if (velocity.LengthSquared != 0)
            {
                Debug.WriteLine(Window.GetCurrentWindow().DeltaTimeFactor);
                GetRigidBody().Activate();
                GetRigidBody().LinearVelocity = velocity * 5; // * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            else
            {
                GetRigidBody().LinearVelocity = new BulletSharp.Math.Vector3(0, 0, 0); // * Window.GetCurrentWindow().DeltaTimeFactor;
            }

        }
    }
}
