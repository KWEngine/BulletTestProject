using BulletSharp;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletTest
{
    class Window : GameWindow
    {
        private Matrix4 _viewMatrix = Matrix4.LookAt(10, 10, 10, 0, 0, 0, 0, 1, 0);
        private Matrix4 _projectionMatrix = Matrix4.Identity;
        private Matrix4 _viewProjectionMatrix = Matrix4.Identity;

        private static GameWorld _world;

        public static GameWorld GetCurrentWorld()
        {
            return _world;
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
            Console.WriteLine("Window()");
             _world = new GameWorld(this);
        }

        protected override void OnFocusedChanged(FocusedChangedEventArgs e)
        {
            base.OnFocusedChanged(e);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Console.WriteLine("OnLoad()");

            PrimitiveCube.Init();
            Renderer.Init();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.ClearColor(0, 0, 0, 1);

            GameObject cube1 = new GameObject(CollisionShapeType.Cube);
            cube1.SetPosition(5, 5, 5);
            cube1.Color = new Vector3(1, 0, 0);
            cube1.SetMass(0.5f);
            _world.Add(cube1);

            GameObject cube2 = new GameObject(CollisionShapeType.Cube);
            cube2.SetPosition(5, 5, 0);
            cube2.Color = new Vector3(0, 1, 0);
            cube2.SetMass(0.5f);
            _world.Add(cube2);

            GameObject cube3 = new GameObject(CollisionShapeType.Cube);
            cube3.SetPosition(0, 0.5f, 5);
            cube3.Color = new Vector3(0, 0, 1);
            cube3.SetMass(0.5f);
            _world.Add(cube3);

            GameObject floor = new GameObject(CollisionShapeType.Cube);
            floor.SetPosition(0, -0.5f, 0);
            floor.SetScale(50, 1, 50);
            floor.Color = new Vector3(1, 1, 1);
            floor.SetMass(0);
            _world.Add(floor);
            
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

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
            _world.GetCollisionWorld().StepSimulation(_timestep);

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
