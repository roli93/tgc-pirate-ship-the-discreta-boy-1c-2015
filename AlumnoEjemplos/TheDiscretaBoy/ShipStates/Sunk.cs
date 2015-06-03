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
    }
}
