using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace WPF_BatallaEspacial.Elementos.Espacio
{
    public class Planeta : ElementoDibujable
    {
        Color ColorCentral;
        Color ColorBorde;

        public Planeta( string nombre, Canvas canvas, 
                        int posicionX, int posicionY, int ancho, int largo,
                        Color colorCentral, Color colorBorde)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            ColorCentral = colorCentral;
            ColorBorde = colorBorde;
        }

        public override void Dibujarse()
        {
            // Crear figura
            Ellipse elipsePlaneta = new Ellipse();
            elipsePlaneta.Name = Nombre;
            elipsePlaneta.Width = Dimenciones.Ancho;
            elipsePlaneta.Height = Dimenciones.Largo;
            elipsePlaneta.Visibility = Visibility.Visible;

            // Generar el relleno del elipse
            RadialGradientBrush radialGradient = new RadialGradientBrush();
            radialGradient.GradientOrigin = new Point(0.5, 0.5);

            // Set the gradient center to the center of the area being painted.
            radialGradient.Center = new Point(0.5, 0.5);

            // Set the radius of the gradient circle so that it extends to
            // the edges of the area being painted.
            radialGradient.RadiusX = 0.5;
            radialGradient.RadiusY = 0.5;

            // Create four gradient stops.
            radialGradient.GradientStops.Add(new GradientStop(ColorCentral, 0.1));
            radialGradient.GradientStops.Add(new GradientStop(ColorBorde, 1));

            // Freeze the brush (make it unmodifiable) for performance benefits.
            radialGradient.Freeze();

            elipsePlaneta.Fill = radialGradient;

            // Asignar el elipse a la variable del elemento dibujable
            elementoDibujable = elipsePlaneta;

            // Cargar en canvas
            Canvas.Children.Add(elipsePlaneta);
            Canvas.SetLeft(elipsePlaneta, this.Posicion.PosicionX);
            Canvas.SetTop(elipsePlaneta, this.Posicion.PosicionY);
            Canvas.SetZIndex(elipsePlaneta, 0);
        }

    }
}
