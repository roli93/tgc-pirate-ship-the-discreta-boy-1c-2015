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
        public Triumph()
        {
            this.spritePath = "\\Texturas\\winner.png";
        }

        public override string soundDirectory()
        {
            return "Sound\\ta_da.wav";
        }
    }
}
