using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy.ShipStates
{
    public class Sunk : ShipState
    {
        public override bool isDead(GenericShip ship)
        {
            return true;
        }

        public override void renderOnlyVisible(GenericShip ship, float elapsedTime)
        {
            ship.ship.move(0, -100F * elapsedTime, 0);
            ship.renderMesh(elapsedTime);
        }

        public override void renderAction(GenericShip ship, float elapsedTime) { }
    }
}
