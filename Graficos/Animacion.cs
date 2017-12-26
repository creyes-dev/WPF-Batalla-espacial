using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WPF_BatallaEspacial.ObjetosComunes;

namespace WPF_BatallaEspacial.Graficos
{
    public class Animacion
    {
        public static void DesplazarElemento(Canvas canvas, UIElement elemento, Storyboard storyboard, Direccion direccion, double puntoInicial, double puntoFinal, double duracion = 1, bool cicloInfinito = false)
        {
            DoubleAnimation desplazamiento = new DoubleAnimation();
            desplazamiento.From = puntoInicial;
            desplazamiento.To = puntoFinal;

            if (duracion != 0)
                desplazamiento.Duration = new Duration(TimeSpan.FromSeconds(duracion));

            desplazamiento.BeginTime = TimeSpan.FromSeconds(0);
            desplazamiento.SpeedRatio = 1;

            storyboard.Children.Add(desplazamiento);
            Storyboard.SetTarget(desplazamiento, elemento);

            string propiedadCanvas;
            if (direccion == Direccion.Inferior || direccion == Direccion.Superior)
                propiedadCanvas = "(Canvas.Top)";
            else
                propiedadCanvas = "(Canvas.Left)";

            Storyboard.SetTargetProperty(desplazamiento, new PropertyPath(propiedadCanvas));

            if (cicloInfinito)
                desplazamiento.RepeatBehavior = RepeatBehavior.Forever;

            storyboard.Begin(canvas, true);
        }

    }
}
