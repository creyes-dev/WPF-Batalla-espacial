using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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
        protected Random numeroAlAzar;

        protected int periodoInvisibilidad;
        protected int periodoInvencibilidad;

        public int PeriodoUltimoDisparo { get; set; }

        public Nave(string nombre, Canvas canvas, string directorioImagen, 
                    int posicionX, int posicionY, int ancho, int largo, 
                    string directorioImagenDisparo, string directorioImagenAnimacionDestruccion) 
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            Disparos = new List<Disparo>();
            rutaRelativaImagenNave = directorioImagen;
            this.CargarImagen();

            numeroAlAzar = new Random();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 5);
            //timer.Tick += new EventHandler(NaveJugador_XXX); // Evento de detener la transparencia
            timer.Start();
        }

        // @"img/player.png"
        private void CargarImagen()
        {
            Image Imagen = new Image();
            Imagen.Source = new BitmapImage(new Uri(rutaRelativaImagenNave, UriKind.Relative));
            Imagen.Name = "NaveJugador";
            Imagen.Height = this.Dimenciones.Largo;
            Imagen.Width = this.Dimenciones.Ancho;

            elementoDibujable = Imagen;
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

                    //foreach (Disparo disparo in disparos)
                    //{
                    //    disparo.PosicionY -= 10;
                    //    disparo.Redibujar();
                    //}
                }
            }
        }

        public override void Disparar()
        {
            // TODO: cambiar
            //DisparoDisponible = true;

            //if (EstaViva && DisparoDisponible)
            //{
            //    // Obtener la localizacion del origen del disparo (punto medio de la nave)
            //    int puntoInicioDisparoX = (int)(this.PosicionX + (this.Tamanio / 2.0));
            //    int puntoInicioDisparoY = (int)(this.PosicionY + (this.Tamanio / 2.0));

            //    puntoInicioDisparoX = (int)(puntoInicioDisparoX - (7.0 / 2.0)); // TODO

            //    string rutaImagenDisparo = @"img/rayo1.png";
            //    Disparo disparo = new Disparo(canvas, "vuv", puntoInicioDisparoX, 500, 7, 32, rutaImagenDisparo);
            //    this.disparos.Add(disparo);
            //}
        }

        public override void Desplazarse(ObjetosComunes.Direccion direccion)
        {
            if (EstaViva)
            {
                if (direccion == ObjetosComunes.Direccion.Izquierda)
                {
                    if (Posicion.PosicionX < 5)
                        Posicion.PosicionY = 0;
                    else
                        Posicion.PosicionX -= 5;
                }
                else
                {
                    Posicion.PosicionX += 5;
                }
            }
        }
        
        // Eventos subscribibles al tick del timer


    }
}
