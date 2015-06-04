using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy.ShipStates
{
    public class Bouncing : ShipState
    {
        public ShipState postBounceState;

        public Bouncing(ShipState postBounceState)
        {
            this.postBounceState = postBounceState;
        }

        public override bool isDead(GenericShip ship)
        {
            return false;
        }

        public override void renderOnlyVisible(GenericShip ship, float elapsedTime)
        {
            ship.updatePosition();
            ship.bounceAndThen(this.postBounceState, elapsedTime);
            
            ship.moveForward(elapsedTime);
            
            base.renderOnlyVisible(ship, elapsedTime);
        }

        public override void renderAction(GenericShip ship, float elapsedTime)
        {
            ship.handleInput(elapsedTime);
        }
    }
}
