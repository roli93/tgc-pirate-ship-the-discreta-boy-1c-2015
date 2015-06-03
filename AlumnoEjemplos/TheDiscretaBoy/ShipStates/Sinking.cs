using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy.ShipStates
{
    public class Sinking : ShipState
    {
        public override bool isDead(GenericShip ship)
        {
            return true;
        }

        public override void renderOnlyVisible(GenericShip ship, float elapsedTime)
        {
            ship.ship.rotateZ((float)Math.PI * elapsedTime);
            base.renderOnlyVisible(ship, elapsedTime);
        }
    }
}
