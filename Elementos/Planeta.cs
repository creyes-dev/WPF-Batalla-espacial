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

namespace WPF_BatallaEspacial.Elementos
{
    public class Planeta : ElementoDibujable
    {

        public Planeta( string nombre, Canvas canvas, 
                        int posicionX, int posicionY, int ancho, int largo)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {

        }

        public override void Dibujarse()
        {
        //    CargarImagen();
        //    PosicionarEnCanvas();

            Ellipse elipsePlaneta = new Ellipse();
            elipsePlaneta.Name = Nombre;
            elipsePlaneta.Width = 150;
            elipsePlaneta.Height = 150;
            elipsePlaneta.Fill = Brushes.Azure;
            elipsePlaneta.Visibility = Visibility.Visible;

            Canvas.Children.Add(elipsePlaneta);
            Canvas.SetLeft(elipsePlaneta, this.Posicion.PosicionX);
            Canvas.SetTop(elipsePlaneta, this.Posicion.PosicionY);
            Canvas.SetZIndex(elipsePlaneta, 0);
        }

        private void CargarImagen()
        {


            //elementoDibujable = elipsePlaneta;
        }

        private void PosicionarEnCanvas()
        {
        //    Ellipse elipse = (Ellipse)elementoDibujable;
        //    Canvas.Children.Add(elipse);
        //    Canvas.SetLeft(elipse, this.Posicion.PosicionX);
        //    Canvas.SetTop(elipse, this.Posicion.PosicionY);
        //    Canvas.SetZIndex(elipse, 5);
        }

    }
}
