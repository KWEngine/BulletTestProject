using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletTest
{
    class Player : GameObject
    {
        public Player(CollisionShapeType type)
            : base(type)
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
            GetRigidBody().LinearVelocity = velocity;

        }
    }
}
