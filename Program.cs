using System;
using OpenTK.Windowing.Desktop;

namespace BulletTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GameWindowSettings gws = new GameWindowSettings();
            gws.IsMultiThreaded = false;
            gws.RenderFrequency = 0;
            gws.UpdateFrequency = 0;

            NativeWindowSettings nws = new NativeWindowSettings();
            nws.APIVersion = new Version(4, 5);
            nws.AutoLoadBindings = true;
            nws.Flags = OpenTK.Windowing.Common.ContextFlags.Debug;
            nws.IsFullscreen = false;
            nws.NumberOfSamples = 1;
            nws.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            nws.StartFocused = true;
            nws.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Fixed;

            using (Window w = new Window(gws, nws))
            {
                w.Run();
            }
        }
    }
}
