using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;
using Microsoft.DirectX;

namespace AlumnoEjemplos.TheDiscretaBoy.Menus
{
    public delegate void ButtonCallback();
    public class MenuButton
    {
        private TgcSprite sprite;
        public Action<EjemploAlumno> callback;

        public MenuButton(float index, TgcSprite sprite, Action<EjemploAlumno> callback)
        {
            this.sprite = sprite;
            this.callback = callback;
            this.sprite.Scaling = new Vector2(.18f,.18f);
            this.sprite.Position = new Vector2(
                (GuiController.Instance.Panel3d.Size.Width - this.width()) / 2,
                100 + (index * ((float)ScreenHelper.size().Height/400) * this.height()));
        }

        private float width()
        {
            return this.sprite.Texture.Width * this.sprite.Scaling.X;
        }

        private float height()
        {
            return this.sprite.Texture.Height * this.sprite.Scaling.Y;
        }

        public void render()
        {
            this.sprite.render();
        }

        public float topX()
        {
            return this.bottomX() + this.width();
        }

        public float topY()
        {
            return this.bottomY() + this.height();
        }

        public float bottomX()
        {
            return this.sprite.Position.X;
        }

        public float bottomY()
        {
            return this.sprite.Position.Y;
        }

        public bool clicked(float clickedX, float clickedY)
        {
            return (clickedX > this.bottomX()) &&
                (clickedX < topX()) &&
                (clickedY > this.bottomY()) &&
                (clickedY < topY());
        }

        public void handleClick(EjemploAlumno game, float clickedX, float clickedY)
        {
            if(clicked(clickedX, clickedY))
            {
                callback(game);
            }
        }
    }
}
