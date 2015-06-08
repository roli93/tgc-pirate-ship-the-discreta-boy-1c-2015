using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils._2D;
using System.Drawing;
using TgcViewer;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    public abstract class BattleEnded : Efecto
    {
        public string spritePath;

        public override void show()
        {
            base.show();
            GuiController.Instance.ThirdPersonCamera.Enable = false;
            EjemploAlumno.Instance.menu.setHeader(this.spritePath);
            EjemploAlumno.Instance.stop();
        }
    }
}
