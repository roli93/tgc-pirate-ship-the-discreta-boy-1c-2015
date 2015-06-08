using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils._2D;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using Microsoft.DirectX;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    class Failure : BattleEnded
    {
        public TgcSprite sprite;

        public Failure()
        {
            this.sprite = new TgcSprite();
            this.sprite.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\Texturas\\failure.png");
            this.sprite.Scaling = new Vector2(0.35f, 0.2f);
            this.sprite.Position = TgcSpriteHelper.center(this.sprite);
        }
        
        public override string soundDirectory()
        {
            return "Sound\\fail_trombone_3.wav";
        }

        public override void show()
        {
            base.show();
        }
    }
}
