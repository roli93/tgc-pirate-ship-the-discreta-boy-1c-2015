using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy.ShipStates
{
    public class Alive : ShipState
    {
        public override bool isDead(GenericShip ship) 
        { 
            return false; 
        }

        public override void renderOnlyVisible(GenericShip ship, float elapsedTime)
        {
            ship.floatOrSink();
            ship.renderBar();
            base.renderOnlyVisible(ship, elapsedTime);
        }

        public override void renderAction(GenericShip ship, float elapsedTime)
        {
            ship.handleInput(elapsedTime);
        }
    }
}
