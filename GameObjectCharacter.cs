using System;
using System.Collections.Generic;
using System.Text;
using BulletSharp;

namespace BulletTest
{
    public class GameObjectCharacter : KinematicCharacterController
    {
        //public GameObjectCharacter()

        internal GameObjectCharacter(PairCachingGhostObject ghostObject, ConvexShape convexShape, float stepHeight, int upAxis = 1) : base(ghostObject, convexShape, stepHeight, upAxis)
        {
        }
    }
}
