using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;


namespace WPF_BatallaEspacial.Elementos
{
    public class Espacio : ElementoDibujable
    {
        string rutaRelativaImagenEspacio;
        Storyboard storyboardDesplazamiento;

        public Espacio(string nombre, Canvas canvas,
                    int posicionX, int posicionY, int ancho, int largo)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {

        }

        public override void Dibujarse()
        {
            CargarEnCanvas();
        }

        public void CargarEnCanvas()
        {
            AsignarDirectoriosImagenes();
            CargarImagen();
            PosicionarImagenEnCanvas();
        }

        private void AsignarDirectoriosImagenes()
        {
            rutaRelativaImagenEspacio = "../Imagenes/background.png";
        }

        private void CargarImagen()
        {
            Image Imagen = new Image();
            Imagen.Source = new BitmapImage(new Uri(rutaRelativaImagenEspacio, UriKind.Relative));
            Imagen.Name = Nombre;
            Imagen.Height = this.Dimenciones.Largo;
            Imagen.Width = this.Dimenciones.Ancho;
            elementoDibujable = Imagen;
        }

        private void PosicionarImagenEnCanvas()
        {
            Image Imagen = (Image)elementoDibujable;
            Canvas.Children.Add(Imagen);
            Canvas.SetLeft(Imagen, this.Posicion.PosicionX);
            Canvas.SetTop(Imagen, this.Posicion.PosicionY);
            Canvas.SetZIndex(Imagen, 0);
        }

        public void DesplazarImagen(double puntoInicial, double puntoFinal, double duracion = 0, bool cicloInfinito = false)
        {
            Image imagen = (Image)elementoDibujable;
            storyboardDesplazamiento = new Storyboard();

            DoubleAnimation desplazamiento = new DoubleAnimation();
            desplazamiento.From = puntoInicial;
            desplazamiento.To = puntoFinal;

            if (duracion != 0)
            {
                desplazamiento.Duration = new Duration(TimeSpan.FromSeconds(duracion));
            }

            desplazamiento.BeginTime = TimeSpan.FromSeconds(0);
            desplazamiento.SpeedRatio = 1;

            storyboardDesplazamiento.Children.Add(desplazamiento);
            Storyboard.SetTarget(desplazamiento, imagen);
            Storyboard.SetTargetProperty(desplazamiento, new PropertyPath("(Canvas.Top)"));

            if (cicloInfinito)
            {
                desplazamiento.RepeatBehavior = RepeatBehavior.Forever;
            }

            storyboardDesplazamiento.Begin(Canvas, true);
        }

    }
}
