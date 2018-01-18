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
    public class Disparo : ElementoDibujable
    {
        public Image SpriteSheet { get; set; }
        AnimacionFrameSprites animacion { get; set; }

        public Disparo(string nombre, Canvas canvas, int posicionX, int posicionY, int ancho, int largo, string rutaImagen)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            CargarImagen(rutaImagen);

            this.Canvas.Children.Add(SpriteSheet);
            Canvas.SetLeft(SpriteSheet, posicionX);
            Canvas.SetTop(SpriteSheet, posicionY);

            string rutaFramesAnimacion = rutaImagen;
            animacion = new AnimacionFrameSprites(rutaFramesAnimacion, ancho, largo, 1, 16, SpriteSheet);

            animacion.IniciarAnimacion(16, false, 1); // TODO: Frame por defecto es un parametro por defecto... 
        }

        public void CargarImagen(string rutaImagen)
        {
            SpriteSheet = new Image();
            SpriteSheet.Source = new BitmapImage(new Uri(rutaImagen, UriKind.Relative)); // ..imagenes..rayo
            SpriteSheet.Name = Nombre;
            SpriteSheet.Height = Dimenciones.Largo;
            SpriteSheet.Width = Dimenciones.Ancho;
        }

        public override void Dibujarse()
        {
            Canvas.SetTop(SpriteSheet, Posicion.PosicionX);
            Canvas.SetTop(SpriteSheet, Posicion.PosicionY);
        }

    }
}
