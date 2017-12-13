using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_BatallaEspacial.Graficos;
using System.Windows.Media.Imaging;

namespace WPF_BatallaEspacial.Elementos
{
    public class Disparo
    {
        public string Nombre { get; set; }
        public int Ancho { get; set; }
        public int Largo { get; set; }

        public int PosicionX { get; set; }
        public int PosicionY { get; set; }

        public Canvas canvas { get; set; }

        public Image SpriteSheet { get; set; }
        AnimacionFrameSprites animacion { get; set; }

        public Disparo(Canvas lienzo, string nombre, int puntoX, int puntoY, int ancho, int largo, string rutaImagen)
        {
            Nombre = nombre;
            Ancho = ancho;
            Largo = largo; // TODO: Alto no largo
            PosicionX = puntoX;
            PosicionY = puntoY;

            canvas = lienzo;

            CargarImagen(rutaImagen);

            canvas.Children.Add(SpriteSheet);
            Canvas.SetLeft(SpriteSheet, puntoX);
            Canvas.SetTop(SpriteSheet, puntoY);

            string rutaFramesAnimacion = Environment.CurrentDirectory + @"\Imagenes\rayo1.png"; // TODO: COrregir
            animacion = new AnimacionFrameSprites(rutaFramesAnimacion, ancho, largo, 1, 16, SpriteSheet);

            animacion.IniciarAnimacion(16, false, 1); // TODO: Frame por defecto es un parametro por defecto... 
        }

        public void CargarImagen(string rutaImagen)
        {
            SpriteSheet = new Image();
            SpriteSheet.Source = new BitmapImage(new Uri(rutaImagen, UriKind.Relative));
            SpriteSheet.Name = Nombre;
            SpriteSheet.Height = Largo;
            SpriteSheet.Width = Ancho;
        }

        public void Redibujar()
        {
            Canvas.SetTop(SpriteSheet, this.PosicionX);
            Canvas.SetTop(SpriteSheet, this.PosicionY);
        }

    }
}
