using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    public class Paused : GameStatus
    {
        public override void render(float elapsedTime, EjemploAlumno game)
        {
            game.renderPaused(elapsedTime);
        }
    }
}
