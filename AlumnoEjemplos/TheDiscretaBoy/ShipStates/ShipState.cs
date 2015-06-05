using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy.ShipStates
{
    public abstract class ShipState
    {
        public abstract bool isDead(GenericShip ship);

        public virtual void updatePosition(GenericShip ship)
        {
            ship.placeAtSurface();
        }
        
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

        public void crashWithSky(GenericShip ship)
        {
            ship.bounce(new Alive());
        }

        public virtual void renderOnlyVisible(GenericShip ship, float elapsedTime)
        {
            ship.updatePosition();
            ship.renderMesh(elapsedTime);
        }

        public abstract void renderAction(GenericShip ship, float elapsedTime);

        public virtual void continueOrCompleteSinking(GenericShip ship)
        {
            if (ship.Rotation.Z >= Math.Abs(Math.PI))
                ship.status = new Sunk();
        }

        public void renderPlaying(PlayerShip ship, float elapsedTime)
        {
            this.renderOnlyVisible(ship, elapsedTime);
            this.renderAction(ship, elapsedTime);
        }
    }

}
