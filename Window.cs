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

        private GameWorld _world = new GameWorld();

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Console.WriteLine("Window()");
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

            GameObject cube1 = new GameObject();
            cube1.SetPosition(5, 5, 5);
            _world.Add(cube1);

            GameObject cube2 = new GameObject();
            cube2.SetPosition(5, 5, 0);
            _world.Add(cube2);

            GameObject cube3 = new GameObject();
            cube3.SetPosition(0, 0.5f, 5);
            _world.Add(cube3);

            GameObject floor = new GameObject();
            floor.SetPosition(0, -0.5f, 0);
            _world.Add(floor);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            List<GameObject> gameObjects = _world.GetGameObjects();
            foreach (GameObject g in gameObjects)
            {
                Renderer.Draw(g, ref _viewProjectionMatrix);
            }

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, this.ClientRectangle.Size.X, this.ClientRectangle.Size.Y);
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 2, (float)this.ClientRectangle.Size.X / this.ClientRectangle.Size.Y, 0.1f, 100f);
            _viewProjectionMatrix = _viewMatrix * _projectionMatrix;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }
    }
}
