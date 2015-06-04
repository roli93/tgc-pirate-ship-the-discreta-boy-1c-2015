using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy.ShipStates
{
    public class Resurrecting : ShipState
    {
        public double resurrectingElapsedTime;
        internal Timer twinkler = new Timer(1000F);
        
        public override bool isDead(GenericShip ship)
        {
            return false;
        }

        public override void renderOnlyVisible(GenericShip ship, float elapsedTime)
        {
            ship.returnToOrigin();
            base.renderOnlyVisible(ship, elapsedTime);

            if (this.resurrectingElapsedTime < .7)
            {
                this.resurrectingElapsedTime += elapsedTime;
                twinkler.doWhenItsTimeTo(() => {
                    ship.renderMesh(elapsedTime);
                }, elapsedTime);
            }
            else
            {
                ship.status = new Alive();
                this.resurrectingElapsedTime = 0;
            }
        }

        public override void renderAction(GenericShip ship, float elapsedTime)
        {
            ship.handleInput(elapsedTime);
        }
    }
}
