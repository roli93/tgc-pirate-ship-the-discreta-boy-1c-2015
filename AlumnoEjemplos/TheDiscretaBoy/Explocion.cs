﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    public class Explocion : Efecto
    {
        public override string soundDirectory()
        {
            return "Sound\\torpedo_impact.wav";
        }
    }
}
