using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPF_BatallaEspacial.ObjetosComunes;

namespace WPF_BatallaEspacial.Elementos
{
    public abstract class Nave : ElementoDibujable
    {
        public List<Disparo> Disparos { get; set; }
        public bool EstaViva { get; set; }

        public bool EstaInvisible { get; set; }     // TODO: NO estoy seguro de que tengan que ser properties
        public bool EstaInvencible { get; set; }
        protected int periodoInvisibilidad;
        protected int periodoInvencibilidad;
        
        protected DispatcherTimer timer;

        protected string rutaRelativaImagenNave;
        protected string rutaRelativaImagenDisparo;
        protected string rutaRelativaImagenDestruccion;

        protected Random numeroAlAzar;

        public Nave(string nombre, Canvas canvas, 
                    int posicionX, int posicionY, int ancho, int largo) 
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            numeroAlAzar = new Random();
            Disparos = new List<Disparo>();
            
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 16);
            //timer.Tick += new EventHandler(NaveJugador_XXX); // Evento de detener la transparencia
            timer.Start();

            EstaViva = true;
        }

        // TODO: El template method puede ser una implementacion de un metodo abstracto

        // Para cargar la imagen es necesario cargar las imagenes de disparos y explosion
        public void CargarEnCanvas()
        {
            AsignarDirectoriosImagenes();
            CargarImagen();
            PosicionarImagenEnCanvas();
        }

        protected abstract void AsignarDirectoriosImagenes();

        protected void CargarImagen()
        {
            Image Imagen = new Image();
            Imagen.Source = new BitmapImage(new Uri(rutaRelativaImagenNave, UriKind.Relative));
            Imagen.Name = "NaveJugador";
            Imagen.Height = this.Dimenciones.Largo;
            Imagen.Width = this.Dimenciones.Ancho;
            elementoDibujable = Imagen;
        }

        protected void PosicionarImagenEnCanvas()
        {
            Image Imagen = (Image) elementoDibujable;
            Canvas.Children.Add(Imagen);
            Canvas.SetLeft(Imagen, this.Posicion.PosicionX);
            Canvas.SetTop(Imagen, this.Posicion.PosicionY);
            Canvas.SetZIndex(Imagen, 3);
        }

        public override void Dibujarse()
        {
            if (!EstaInvisible)
            {
                elementoDibujable = (Image)this.elementoDibujable;
                
                if (EstaInvencible)
                {
                    // Cuando está invencible parpadea
                    elementoDibujable.Opacity = numeroAlAzar.Next(4, 9) / 10.0;
                }
                else
                {
                    Canvas.SetLeft(elementoDibujable, this.Posicion.PosicionX);
                }
            }

            foreach (Disparo disparo in Disparos)
            {
                disparo.Posicion.PosicionY -= 5;
                disparo.Dibujarse();
            }

            List<Disparo> disparosFueraRango = new List<Disparo>();

            foreach (Disparo disparo in Disparos)
            {
                disparo.Posicion.PosicionY -= 5;

                if (disparo.Posicion.PosicionY < 0 - disparo.Dimenciones.Largo || disparo.Posicion.PosicionY > Canvas.Height + disparo.Dimenciones.Largo)
                {
                    Canvas.Children.Remove(disparo.SpriteSheet);
                    disparosFueraRango.Add(disparo);
                }
                disparo.Dibujarse();
            }

            foreach (Disparo disparoFueraRango in disparosFueraRango)
            {
                Disparos.Remove(disparoFueraRango);
            }
        }

        public abstract void Disparar();
        public abstract void Desplazarse(Direccion direccion);


    }
}
