using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletTest
{
    class Immovable : GameObject
    {
        public Immovable(CollisionShapeType type, PhysicsSetupInfo pInfo = new PhysicsSetupInfo())
            : base(type, pInfo)
        {

        }

        public override void Update(KeyboardState ks, MouseState ms)
        {
            
        }
    }
}
