using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    class Notification
    {
        public static Notification instance = new Notification();
        public TgcSprite sprite;

        public void render()
        {
            if (sprite != null) TgcSpriteHelper.render(sprite);
        }

        public void dispose()
        {
            if (sprite != null) sprite.dispose();
        }
    }
}
