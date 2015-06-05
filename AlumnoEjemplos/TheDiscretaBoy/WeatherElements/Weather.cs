using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy.WeatherElements
{
    public class Weather
    {
        public Rain rain;

        public Weather() 
        {
            this.rain = new Rain();
        }

        public void render()
        {
            rain.render();
        }
    }
}
