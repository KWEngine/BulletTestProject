using BulletSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletTest
{
    class GameWorld
    {
        private List<GameObject> _gameObjects = new List<GameObject>();
        private Window _currentWindow = null;
        private CollisionDispatcher _colDispatcher = new CollisionDispatcher(CollisionGlobals.colConfiguration);
        private SequentialImpulseConstraintSolver _physicsSolver = new SequentialImpulseConstraintSolver();
        private DbvtBroadphase _broadphase = new DbvtBroadphase();
        private DiscreteDynamicsWorld _collisionWorld = null;

        public GameWorld(Window w)
        {
            _currentWindow = w;
            _collisionWorld = new DiscreteDynamicsWorld(_colDispatcher, _broadphase, _physicsSolver, CollisionGlobals.colConfiguration);
        }

        public DiscreteDynamicsWorld GetCollisionWorld()
        {
            return _collisionWorld;
        }

        public void Add(GameObject g)
        {
            _gameObjects.Add(g);
            //g.SetWorld(this);
            _collisionWorld.AddCollisionObject(g.GetRigidBody());
        }

        public void Delete(GameObject g)
        {
            _gameObjects.Remove(g);
        }

        public List<GameObject> GetGameObjects()
        {
            return _gameObjects;
        }
    }
}
