using System;
using System.Collections.Generic;
using System.Text;

namespace BulletTest
{
    class GameWorld
    {
        List<GameObject> _gameObjects = new List<GameObject>();

        public void Add(GameObject g)
        {
            _gameObjects.Add(g);
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
