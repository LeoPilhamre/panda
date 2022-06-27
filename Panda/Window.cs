using System;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;


namespace Panda
{
    public sealed class Window : IDisposable
    {

        public static IWindow? window;


        readonly WindowOptions options;


        public Window(string title, int width, int height, bool resizable = false, bool maximized = false)
        {
            options = WindowOptions.Default;
            options.Title = title;
            options.Size = new Vector2D<int>(width, height);
            options.WindowBorder = resizable ? WindowBorder.Resizable : WindowBorder.Fixed;
            options.WindowState = maximized ? WindowState.Maximized : WindowState.Normal;
        }


        internal void Load(Action onOpen, Action onClose, Action<float> onUpdate, Action onRender, Action<Key> onKeyDown, Action<Key> onKeyUp)
        {
            window = Silk.NET.Windowing.Window.Create(options);

            window.Load += () => {
                IInputContext input = window.CreateInput();
                for (int i = 0; i < input.Keyboards.Count; i++)
                {
                    input.Keyboards[i].KeyDown  += (_, key, _) => onKeyDown((Key) key);
                    input.Keyboards[i].KeyUp    += (_, key, _) => onKeyUp((Key) key);
                }

                window.Center();

                onOpen();
            };
            window.Update += dt => onUpdate((float) dt);
            window.Closing += onClose;
            window.Render += _ => onRender();

            window.Run();
        }


        public void Close() => window?.Close();


        public void Dispose() => window?.Dispose();

    }
}