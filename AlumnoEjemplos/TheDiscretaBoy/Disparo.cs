﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    class Disparo : Efecto
    {
        public override string soundDirectory()
        {
            return "Sound\\gun_shot.wav";
        }
    }
}
