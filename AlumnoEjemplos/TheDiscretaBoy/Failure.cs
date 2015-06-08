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
        public Failure()
        {
            this.spritePath = "\\Texturas\\failure.png";
        }
        
        public override string soundDirectory()
        {
            return "Sound\\fail_trombone_3.wav";
        }
    }
}
