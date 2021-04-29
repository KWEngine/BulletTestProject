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
        private Matrix4 _viewMatrix = Matrix4.LookAt(0, 25, 25, 0, 0, 0, 0, 1, 0);
        private Matrix4 _projectionMatrix = Matrix4.Identity;
        private Matrix4 _viewProjectionMatrix = Matrix4.Identity;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            PrimitiveCube.Init();
            Renderer.Init();
        }

        protected override void OnFocusedChanged(FocusedChangedEventArgs e)
        {
            base.OnFocusedChanged(e);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
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
