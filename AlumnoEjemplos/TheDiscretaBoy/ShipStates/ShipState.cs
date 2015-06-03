using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy.ShipStates
{
    public abstract class ShipState
    {
        public abstract bool isDead(GenericShip ship);
        
        public bool isAlive(GenericShip ship)
        {
            return !this.isDead(ship);
        }

        public virtual void sink(GenericShip ship)
        {
            ship.status = new Sinking();
            ship.hundimiento.show();
        }

        public virtual void bounce(GenericShip ship, ShipState postBounceStatus)
        {
            ship.linearSpeed *= -1;
            ship.status = new Bouncing(postBounceStatus);
        }

        public void crash(GenericShip ship)
        {
            ship.reduceLife(25);
            ship.bounce(new Resurrecting());
        }

        public virtual void renderOnlyVisible(GenericShip ship, float elapsedTime)
        {
            ship.updatePosition();
            ship.updateCannonPosition();
            ship.renderMesh();
            ship.cannon.renderOnlyVisible(elapsedTime);
        }
    }

}
