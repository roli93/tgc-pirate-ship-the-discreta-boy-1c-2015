using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Sound;
using TgcViewer;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    public abstract class Evento
    {
        TgcStaticSound sound;

        public Evento()
        {
            sound = new TgcStaticSound();
            sound.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + soundDirectory());
        }

        public abstract string soundDirectory();

        public virtual void show()
        {
            sound.play();
        }
    }
}
