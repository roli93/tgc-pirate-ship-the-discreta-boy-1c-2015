using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.TheDiscretaBoy.WeatherElements
{
    public class Weather
    {
        public Rain rain;
        public Wind wind;

        public Weather() 
        {
            this.rain = new Rain();
            this.wind = new Wind();
        }

        public void render()
        {
            rain.render();
        }
    }
}
