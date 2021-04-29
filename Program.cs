using System;
using OpenTK.Windowing.Desktop;

namespace BulletTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Window w = new Window(GameWindowSettings.Default, NativeWindowSettings.Default))
            {
                w.Run();
            }
        }
    }
}
