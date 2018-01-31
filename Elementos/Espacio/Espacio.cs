using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;


namespace WPF_BatallaEspacial.Elementos.Espacio
{
    public class Espacio : ElementoDibujable
    {
        string rutaRelativaImagenEspacio;
        Storyboard storyboardDesplazamiento;
        List<ElementoDibujable> Planetas;
        Random numeroAlAzar;

        public Espacio(string nombre, Canvas canvas,
                    int posicionX, int posicionY, int ancho, int largo)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            Planetas = new List<ElementoDibujable>();
            numeroAlAzar = new Random();
        }

        public override void Dibujarse()
        {
            AsignarDirectoriosImagenes();
            CargarImagen();
            PosicionarImagenEnCanvas();

            GenerarPlanetas();

            foreach (Planeta planeta in Planetas)
            {
                planeta.Dibujarse();
            }
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

        private void GenerarPlanetas()
        {
            int cantPlanetas = numeroAlAzar.Next(1, 6);

            for (int i = 1; i < cantPlanetas; i++)
            {
                int radio = numeroAlAzar.Next(25, 150);

                int coordenadaX = numeroAlAzar.Next(0, (int)Canvas.Width - (radio*2));
                int coordenadaY = numeroAlAzar.Next(0, (int)Canvas.Height - (radio*2));

                Color color1 = ObtenerColorPlaneta();
                Color color2 = ObtenerColorPlaneta();

                // TODO: Nombre del planeta
                Planeta planeta = new Planeta("planeta1" + numeroAlAzar.Next(0, 32199170), Canvas, coordenadaX, coordenadaY, radio, radio, color1, color2);
                Planetas.Add(planeta);
            }
        }

        private Color ObtenerColorPlaneta()
        {
            int numeroColor = numeroAlAzar.Next(0, 9);

            Color color = Colors.Black;
            if (numeroColor == 0) color = Colors.DarkGray;
            if (numeroColor == 1) color = Colors.Blue;
            if (numeroColor == 2) color = Colors.BlueViolet;
            if (numeroColor == 3) color = Colors.DarkBlue;
            if (numeroColor == 4) color = Colors.DarkGreen;
            if (numeroColor == 5) color = Colors.ForestGreen;
            if (numeroColor == 6) color = Colors.Red;
            if (numeroColor == 7) color = Colors.GreenYellow;
            if (numeroColor == 8) color = Colors.Yellow;
            if (numeroColor == 9) color = Colors.Orange;

            return color;
        }

    }
}
