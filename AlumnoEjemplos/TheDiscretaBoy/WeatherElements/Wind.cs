using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils._2D;
using System.IO;

namespace AlumnoEjemplos.TheDiscretaBoy.WeatherElements
{
    public class Wind
    {
        TgcStaticSound sound;

        public Wind()
        {
            this.sound = new TgcStaticSound();
            this.sound.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "Sound\\rowing.wav");
            this.sound.play(true);
        }
    }
}
