using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletTest
{
    class PhysicsObject : GameObject
    {
        public PhysicsObject(CollisionShapeType type, PhysicsSetupInfo pInfo = new PhysicsSetupInfo())
            : base(type, pInfo)
        {

        }

        public override void OnCollision(GameObject collider)
        {

        }

        public override void Update(KeyboardState ks, MouseState ms)
        {
            
        }
    }
}
