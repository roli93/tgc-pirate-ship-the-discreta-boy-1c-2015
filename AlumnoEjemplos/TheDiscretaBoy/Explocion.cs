using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Sound;
using TgcViewer;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    public class Explocion
    {
        TgcStaticSound sound;

        public Explocion()
        {
            sound = new TgcStaticSound();
            sound.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "Sound\\torpedo_impact.wav");
        }

        public void show()
        {
            sound.play();
        }
    }
}
