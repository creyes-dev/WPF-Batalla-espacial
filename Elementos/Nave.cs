using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPF_BatallaEspacial.Graficos;
using WPF_BatallaEspacial.ObjetosComunes;

namespace WPF_BatallaEspacial.Elementos
{
    public abstract class Nave : ElementoDibujable
    {
        public List<Disparo> Disparos { get; set; }
        protected AnimacionFrameSprites animacion { get; set; }

        public bool EstaViva { get; set; }

        public bool EstaInvisible { get; set; }     // TODO: NO estoy seguro de que tengan que ser properties
        public bool EstaInvencible { get; set; }
        protected int periodoInvisibilidad;
        protected int periodoInvencibilidad;
        
        protected bool jugador; // TODO: Eliminar

        protected string rutaRelativaImagenNave;
        protected string rutaRelativaImagenDisparo;
        protected string rutaRelativaImagenDestruccion;

        public int PeriodoRecuperacionDisparo { get; set; }

        protected int periodoDesdeUltimoDisparo;

        protected Random numeroAlAzar;

        public Nave(string nombre, Canvas canvas, 
                    int posicionX, int posicionY, int ancho, int largo) 
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            numeroAlAzar = new Random();
            Disparos = new List<Disparo>();
            EstaViva = true;
        }

        // TODO: El template method puede ser una implementacion de un metodo abstracto

        // Para cargar la imagen es necesario cargar las imagenes de disparos y explosion
        
        // Cargar en canvas
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

        // Diferencias entre el jugador y la nave enemiga
        // La nave enemiga se desplaza cuando quiere
        // El jugador se desplaza cuando el usuario interactúa con el sistema
        public abstract void Desplazarse(Direccion direccion);

        public abstract void Disparar();

        // TODO: Actualizar}
        //
        //asdasdasd 



        // Dibujarse (Diferencias entre el jugador y una nave enemiga)
        // 1. Actualizar coordenadas (Diferencias entre el jugador y una nave enemiga) el jugador tiene la coordenada actualizada producto de 
        // 2. Definir opacidad (esta invisible = no ha iniciado su animacion o la ha iniciado
        // 3. Mover sus propios disparos

        public override void Dibujarse()
        {
            // 1. Actualizar el estado (invisible o indestructible)

            // 2. Obtener coordenadas
            ActualizarCoordenadas();

            // 3. Redibujar
            Redibujar();

            // 4. Definir opacidad
            if (EstaInvisible)
                elementoDibujable.Opacity = 0;
            else
                if(EstaInvencible)
                    elementoDibujable.Opacity = numeroAlAzar.Next(4, 9) / 10.0;

            // 5. Mover los disparos
            MoverDisparos();

            // 6. Dibujar los disparos y remover aquellos que quedaron fuera de rango
            List<Disparo> disparosFueraRango = new List<Disparo>();

            foreach (Disparo disparo in Disparos)
            {
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
                disparoFueraRango.Removerse();
            }
        }

        protected abstract void ActualizarCoordenadas();
        protected abstract void Redibujar();
        protected abstract void MoverDisparos();

        public void Destruirse()
        {
            string rutaFramesAnimacion = Environment.CurrentDirectory + @"\Imagenes\player_explosion.png"; // TODO: COrregir
            Image imagenNave = (Image) elementoDibujable;
            animacion = new AnimacionFrameSprites(rutaFramesAnimacion, Dimenciones.Ancho, Dimenciones.Largo, 1, 16, imagenNave);
            animacion.IniciarAnimacion(16, true, 0); // TODO: Frame por defecto es un parametro por defecto...
            EstaViva = false;
        }

    }
}
