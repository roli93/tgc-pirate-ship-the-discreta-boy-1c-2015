﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Sound;
using TgcViewer;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    public abstract class Efecto
    {
        TgcStaticSound sound;

        public Efecto()
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
