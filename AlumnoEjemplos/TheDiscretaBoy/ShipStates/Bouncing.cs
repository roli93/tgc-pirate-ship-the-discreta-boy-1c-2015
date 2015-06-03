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
    }
}
