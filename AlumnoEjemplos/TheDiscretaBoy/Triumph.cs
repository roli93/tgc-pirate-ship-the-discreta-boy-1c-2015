using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer;
using Microsoft.DirectX;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    class Triumph : BattleEnded
    {
        private TgcSprite sprite;

        public Triumph()
        {
            this.sprite = new TgcSprite();
            this.sprite.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\Texturas\\winner.png");
            this.sprite.Scaling = new Vector2(0.35f,0.35f);
            this.sprite.Position = TgcSpriteHelper.center(this.sprite);
        }

        public override string soundDirectory()
        {
            return "Sound\\ta_da.wav";
        }

        public override void show()
        {
            base.show();
            Notification.instance.sprite = this.sprite;
        }
    }
}
