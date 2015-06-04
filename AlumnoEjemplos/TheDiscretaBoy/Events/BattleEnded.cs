using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils._2D;
using System.Drawing;
using TgcViewer;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    public abstract class BattleEnded : Evento
    {

        public override void show()
        {
            base.show();
            EjemploAlumno.Instance.battleEnded();
        }
    }
}
