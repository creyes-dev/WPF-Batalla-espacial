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
        
        protected DispatcherTimer timer;

        protected string rutaRelativaImagenNave;
        protected string rutaRelativaImagenDisparo;
        protected string rutaRelativaImagenDestruccion;

        protected Random numeroAlAzar;

        protected int periodoInvisibilidad;
        protected int periodoInvencibilidad;

        public int PeriodoUltimoDisparo { get; set; }

        public Nave(string nombre, Canvas canvas, 
                    int posicionX, int posicionY, int ancho, int largo) 
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            Disparos = new List<Disparo>();
            //this.CargarImagen();

            numeroAlAzar = new Random();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 5);
            //timer.Tick += new EventHandler(NaveJugador_XXX); // Evento de detener la transparencia
            timer.Start();
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
        }

        protected override void Dibujarse()
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
        }

        public abstract void Disparar();
        public abstract void Desplazarse(Direccion direccion);

        //public override void Disparar()
        //{
        //    // TODO: cambiar
        //    //DisparoDisponible = true;

        //    //if (EstaViva && DisparoDisponible)
        //    //{
        //    //    // Obtener la localizacion del origen del disparo (punto medio de la nave)
        //    //    int puntoInicioDisparoX = (int)(this.PosicionX + (this.Tamanio / 2.0));
        //    //    int puntoInicioDisparoY = (int)(this.PosicionY + (this.Tamanio / 2.0));

        //    //    puntoInicioDisparoX = (int)(puntoInicioDisparoX - (7.0 / 2.0)); // TODO

        //    //    string rutaImagenDisparo = @"img/rayo1.png";
        //    //    Disparo disparo = new Disparo(canvas, "vuv", puntoInicioDisparoX, 500, 7, 32, rutaImagenDisparo);
        //    //    this.disparos.Add(disparo);
        //    //}
        //}

        //public override void Desplazarse(ObjetosComunes.Direccion direccion)
        //{
        //    if (EstaViva)
        //    {
        //        if (direccion == ObjetosComunes.Direccion.Izquierda)
        //        {
        //            if (Posicion.PosicionX < 5)
        //                Posicion.PosicionY = 0;
        //            else
        //                Posicion.PosicionX -= 5;
        //        }
        //        else
        //        {
        //            Posicion.PosicionX += 5;
        //        }
        //    }
        //}
        
        // Eventos subscribibles al tick del timer


    }
}
