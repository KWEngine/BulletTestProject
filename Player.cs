using BulletSharp;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BulletTest
{
    
    class Player : GameObject
    {
        private float _speed = 0.2f;

        public Player(CollisionShapeType type, PhysicsSetupInfo pInfo = new PhysicsSetupInfo())
            : base(type, pInfo)
        {

        }

        /*public override void OnCollision(GameObject collider)
        {
            
        }*/

        public override void Update(KeyboardState ks, MouseState ms)
        {
            BulletSharp.Math.Vector3 velocity = new BulletSharp.Math.Vector3(0, 0, 0);

            if (ks[Keys.A])
            {
               velocity += new BulletSharp.Math.Vector3(-_speed, 0, 0);// * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            if (ks[Keys.D])
            {
                velocity += new BulletSharp.Math.Vector3(+_speed, 0, 0);// * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            if (ks[Keys.W])
            {
                velocity += new BulletSharp.Math.Vector3(0, 0, -_speed);// * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            if (ks[Keys.S])
            {
                velocity += new BulletSharp.Math.Vector3(0, 0, +_speed);// * Window.GetCurrentWindow().DeltaTimeFactor;
            }

            if(ks[Keys.Q])
            {
                velocity += new BulletSharp.Math.Vector3(0, -_speed, 0);// * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            if (ks[Keys.E])
            {
                velocity += new BulletSharp.Math.Vector3(0, +_speed, 0);// * Window.GetCurrentWindow().DeltaTimeFactor;
            }

            MoveKinematic(velocity.X, velocity.Y, velocity.Z);
            /*if (velocity.LengthSquared != 0)
            {
                GetRigidBody().LinearVelocity = velocity * 5; // * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            else
            {
                GetRigidBody().LinearVelocity = new BulletSharp.Math.Vector3(0, GetRigidBody().LinearVelocity.Y, 0); // * Window.GetCurrentWindow().DeltaTimeFactor;
            }
            */

            //Window.GetCurrentWorld().GetCollisionWorld().ContactTest(this.GetRigidBody(), )
            //GetRigidBody().

            
            if(HasGhostObject)
            {
                
                if (GetGhostObject().OverlappingPairs.Count > 1)
                {
                    //Debug.WriteLine(GetGhostObject().OverlappingPairs.Count);

                    AlignedCollisionObjectArray collisions = GetGhostObject().OverlappingPairs;
                    foreach(RigidBody b in collisions)
                    {
                        if(b.UserObject != null && !b.UserObject.Equals(this))
                        {
                            b.Activate(true);
                            Debug.WriteLine(b.UserObject.ToString());
                        }
                    }
                }
            }
            

            //Window.GetCurrentWorld().GetCollisionWorld().
        }

        
    }
}
