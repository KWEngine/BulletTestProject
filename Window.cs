using BulletSharp;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BulletTest
{
    class Window : GameWindow
    {
        private Matrix4 _viewMatrix = Matrix4.LookAt(0, 25, 25, 0, 0, 0, 0, 1, 0);
        private Matrix4 _projectionMatrix = Matrix4.Identity;
        private Matrix4 _viewProjectionMatrix = Matrix4.Identity;
        private ulong _frameCounter = 0;
        private static GameWorld _world;
        private static Window _window;
        private const float TIMESTEP_FIXED = 1 / 60f;

        public float DeltaTimeFactor
        {
            get
            {
                //Debug.WriteLine(_timestep / TIMESTEP_FIXED);
                return _timestep / TIMESTEP_FIXED;
            }
        }

        public static GameWorld GetCurrentWorld()
        {
            return _world;
        }

        public static Window GetCurrentWindow()
        {
            return _window;
        }

        private float _timestep = 16.666666f;
        public float TimeStep
        {
            get
            {
                return _timestep;
            }
        }

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) 
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _window = this;
            VSync = VSyncMode.Adaptive;
            _world = new GameWorld(this);
        }

        protected override void OnFocusedChanged(FocusedChangedEventArgs e)
        {
            base.OnFocusedChanged(e);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            PrimitiveCube.Init();
            Renderer.Init();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.ClearColor(0, 0, 0, 1);

            CollisionBody cube1 = new CollisionBody(new PhysicsSetupInfo(1, CollisionShapeType.Cube, ResponseType.Automatic));
            cube1.SetPosition(5, 15, 2.8f);
            cube1.Color = new Vector3(1, 0, 0);
            cube1.Name = "Roter Würfel (Player)";
            _world.Add(cube1);
            
            PhysicsSetupInfo cube2SetupInfo = new PhysicsSetupInfo(50, CollisionShapeType.Cube, ResponseType.Automatic);
            CollisionBody cube2 = new CollisionBody(cube2SetupInfo);
            cube2.SetPosition(5, 5, 0);
            cube2.SetScale(10, 10, 5);
            cube2.Name = "Grüner Würfel";
            cube2.Color = new Vector3(0, 1, 0);
            _world.Add(cube2);

            PhysicsSetupInfo playerPhysicsInfo = new PhysicsSetupInfo(5000, CollisionShapeType.Cube, ResponseType.Manual);
            playerPhysicsInfo.Friction = 1;
            playerPhysicsInfo.Restitution = 0;
            Player playerObject = new Player(playerPhysicsInfo);
            playerObject.SetScale(2, 2, 2);
            playerObject.SetPosition(-5, 0f, 0);
            playerObject.Color = new Vector3(0, 0, 1);
            playerObject.Name = "Player";
            _world.Add(playerObject);

            CollisionBody floor = new CollisionBody(new PhysicsSetupInfo(0, CollisionShapeType.Cube, ResponseType.Static));
            floor.SetPosition(0, -0.5f, 0);
            floor.SetScale(50, 1, 50);
            floor.Name = "Boden";
            floor.Color = new Vector3(1, 1, 1);
            _world.Add(floor);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            _frameCounter++;
            if (_frameCounter % 60 == 0)
            {
                Title = "Bullet test project: " + Math.Round(1f / _timestep, 1) + " fps";
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            List<GameObject> gameObjects = _world.GetGameObjects();
            foreach (GameObject g in gameObjects)
            {
                g.UpdateModelMatrix();
                Renderer.Draw(g, ref _viewProjectionMatrix);
            }

            SwapBuffers();

            
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, this.ClientRectangle.Size.X, this.ClientRectangle.Size.Y);
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, (float)this.ClientRectangle.Size.X / this.ClientRectangle.Size.Y, 0.1f, 100f);
            _viewProjectionMatrix = _viewMatrix * _projectionMatrix;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            _timestep = (float)args.Time;

            foreach(GameObject g in _world.GetGameObjects())
            {
                g.Update(KeyboardState, MouseState);
            }

            _world.GetCollisionWorld().StepSimulation(_timestep);

            //_world.NotifyCollidingObjects();
            /*
            int manifolds = _world.GetCollisionWorld().Dispatcher.NumManifolds;

            for(int i = 0; i < manifolds; i++)
            {
                PersistentManifold pm = _world.GetCollisionWorld().Dispatcher.GetManifoldByIndexInternal(i);
                CollisionObject a = pm.Body0;
                CollisionObject b = pm.Body1;

                int numContacts = pm.NumContacts;
                for(int j = 0; j < numContacts; j++)
                {
                    ManifoldPoint manifoldPoint = pm.GetContactPoint(j);
                    if(manifoldPoint.Distance < 0f)
                    {
                        BulletSharp.Math.Vector3 posA = manifoldPoint.PositionWorldOnA;
                        BulletSharp.Math.Vector3 posB = manifoldPoint.PositionWorldOnB;
                        BulletSharp.Math.Vector3 normalAOnB = manifoldPoint.NormalWorldOnB;

                    }
                }
            }
            */
            
        }
    }
}
