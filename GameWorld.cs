using BulletSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private DynamicsWorld.InternalTickCallback _callback;

        public GameWorld(Window w)
        {
            _currentWindow = w;
            _collisionWorld = new DiscreteDynamicsWorld(_colDispatcher, _broadphase, _physicsSolver, CollisionGlobals.colConfiguration);
            _callback = new DynamicsWorld.InternalTickCallback(NotifyCollidingObjects);
            _collisionWorld.Broadphase.OverlappingPairCache.SetInternalGhostPairCallback(new GhostPairCallback());
        }

        public DiscreteDynamicsWorld GetCollisionWorld()
        {
            return _collisionWorld;
        }

        public void Add(GameObject g)
        {
            _gameObjects.Add(g);
            //g.SetWorld(this);
            _collisionWorld.AddRigidBody(g.GetRigidBody());
            if(g.HasGhostObject)
            {
                _collisionWorld.AddCollisionObject(g.GetGhostObject());
            }
            /*
            if(g.GetRigidBody().IsKinematicObject)
            {
                BroadphaseProxy proxy = g.GetRigidBody().BroadphaseHandle;
                if(proxy != null)
                {
                    proxy.CollisionFilterGroup = 1;
                    proxy.CollisionFilterMask = Int32.MaxValue;
                }
            }
            */
            _collisionWorld.SetInternalTickCallback(_callback);
//            _collisionWorld.AddCollisionObject();

        }

        public void Delete(GameObject g)
        {
            _gameObjects.Remove(g);
        }

        public List<GameObject> GetGameObjects()
        {
            return _gameObjects;
        }
        

        public void NotifyCollidingObjects(DynamicsWorld world, float timestep)
        {
            
            //Debug.WriteLine(   GetCollisionWorld().CollisionObjectArray.Count);
            int manifolds = GetCollisionWorld().Dispatcher.NumManifolds;
            for (int i = 0; i < manifolds; i++)
            {
                PersistentManifold pm = GetCollisionWorld().Dispatcher.GetManifoldByIndexInternal(i);
                CollisionObject a = pm.Body0;
                CollisionObject b = pm.Body1;

                int numContacts = pm.NumContacts;
                for (int j = 0; j < numContacts; j++)
                {
                    ManifoldPoint manifoldPoint = pm.GetContactPoint(j);
                    if (manifoldPoint.Distance < 0f)
                    {
                        if(a.UserObject != null && b.UserObject != null)
                        {
                            //Debug.WriteLine(a.UserObject.ToString() + " (" + a.ActivationState + ")");
                            //Debug.WriteLine(b.UserObject.ToString() + " (" + b.ActivationState + ")");
                            //Debug.WriteLine("-----------");
                        }
                        else
                        {
                            //Debug.WriteLine("null: " + a.WorldTransform.Origin);
                        }
                        

                        BulletSharp.Math.Vector3 posA = manifoldPoint.PositionWorldOnA;
                        BulletSharp.Math.Vector3 posB = manifoldPoint.PositionWorldOnB;
                        BulletSharp.Math.Vector3 normalAOnB = manifoldPoint.NormalWorldOnB;

                    }
                }
            }
            
        }
    }
}
