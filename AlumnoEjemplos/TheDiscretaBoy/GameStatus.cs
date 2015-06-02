using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    public abstract class GameStatus
    {
        public abstract void render(float elapsedTime, EjemploAlumno game);
    }
}
