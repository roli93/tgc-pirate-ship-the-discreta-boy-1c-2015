using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using System.Drawing;
using TgcViewer;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    public class Timer
    {
        private float time;
        private float frequency;
        private bool itsTime = false;

        public Timer(float frequency)
        {
            this.frequency = frequency;
        }

        public void doWhenItsTimeTo(System.Action whatToDo, float elapsedTime)
        {

            if (itsTime)
            {
                whatToDo();
                itsTime = false;
            }
            spendTime(elapsedTime);
        }

        private void spendTime(float elapsedTime)
        {
            if (time < Math.PI * 2)
                time += elapsedTime * (float)Math.PI * frequency;
            else
            {
                itsTime = true;
                this.time = 0;
            }
        }

    }

    public class Oscilator
    {
        private float frequency;
        private float rotation;
        private float amplitude;

        public Oscilator(float frequency, float amplitude)
        {
            this.frequency = frequency;
            this.amplitude = amplitude;
        }

        public float oscilation(float elapsedTime)
        {
            rotation += elapsedTime * 2 * (float)Math.PI * frequency;
            float result = (float)Math.Sin(rotation) * amplitude;
            return result;
        }
    }


    public class CircularBuffer<T> : List<T>
    {
        private int currentIndex = 0;

        public T GetNext()
        {
            T element = this.ElementAt<T>(currentIndex);
            currentIndex = (currentIndex + 1) % this.Count;
            return element;
        }

    }

    public class TgcSpriteHelper
    {
        public static Vector2 center(TgcSprite sprite)
        {
            Size screenSize = GuiController.Instance.Panel3d.Size;
            return new Vector2(FastMath.Max(screenSize.Width / 2 - (sprite.Scaling.X * sprite.Texture.Width) / 2, 0), FastMath.Max(screenSize.Height / 4 - (sprite.Scaling.Y * sprite.Texture.Height) / 2, 0));
        }

        public static void render(params TgcSprite[] sprites)
        {
            foreach (TgcSprite sprite in sprites)
            {
                GuiController.Instance.Drawer2D.beginDrawSprite();
                sprite.render();
                GuiController.Instance.Drawer2D.endDrawSprite();
            }
        }
    }

    public class ScreenHelper
    {
        public static Size size()
        {
            return GuiController.Instance.Panel3d.Size;
        }
    }

}
