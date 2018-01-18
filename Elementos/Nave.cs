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
        public int Vidas { get; set; }

        protected bool jugador; // TODO: Eliminar

        protected string rutaAbsolutaImagenNave;
        protected string rutaAbsolutaImagenDisparo;
        protected string rutaAbsolutaImagenDestruccion;

        public int PeriodoRecuperacionDisparo { get; set; }
        protected int PeriodoDesdeUltimoDisparo;
        
        protected Random numeroAlAzar;
        public EstadoNave Estado { get; set; }
        
        public Nave(string nombre, Canvas canvas, 
                    int posicionX, int posicionY, int ancho, int largo) 
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            numeroAlAzar = new Random();
            Disparos = new List<Disparo>();
            Estado = EstadoNave.Invisible;
        }

        // El template method puede ser una implementacion de un metodo abstracto
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
            Imagen.Source = new BitmapImage(new Uri(rutaAbsolutaImagenNave, UriKind.Absolute));
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

        public void Disparar()
        {
            if (Estado == EstadoNave.ModoBatalla)
            {
                IniciarDisparo();
            }
        }

        public abstract void IniciarDisparo();

        public override void Dibujarse()
        {
            // 1. Actualizar el estado (invisible o indestructible)
            ActualizarEstado();
            // 2. Obtener coordenadas
            ActualizarCoordenadas();
            // 3. Redibujar
            Redibujar();
            // 4. Definir opacidad
            DefinirOpacidad();
            // 5. Mover los disparos
            MoverDisparos();
            // 6. Dibujar los disparos y remover aquellos que quedaron fuera de rango
            DibujarRemoverDisparos();
        }

        public int PeriodoInvulnerabilidad { get; set; }
        public int PeriodoInvisibilidad { get; set; }
        public int PeriodoModoSigilo { get; set; }
        public int PeriodoDesdeDestruccion { get; set; } // TODO: Eliminar, hay que consultar por la propiedad Finalizar de la animación

        public int PeriodoDesdeInvulnerabilidad { get; set; }
        public int PeriodoDesdeInvisibilidad { get; set; }
        public int PeriodoDesdeModoSigilo { get; set; }

        public void ActualizarEstado()
        {
            if (Estado != EstadoNave.ModoBatalla)
            {
                if (Estado == EstadoNave.Invulnerable)
                {
                    if (PeriodoDesdeInvulnerabilidad < PeriodoInvulnerabilidad)
                        PeriodoDesdeInvulnerabilidad += 1;
                    else
                        Estado = EstadoNave.ModoBatalla;
                }
                else
                {
                    if (Estado == EstadoNave.Invisible)
                    {
                        if (PeriodoDesdeInvisibilidad < PeriodoInvisibilidad) 
                        {
                            PeriodoDesdeInvisibilidad += 1;
                        }
                        else
                        {
                           if (Vidas > 0) // TODO: AND Disparos. != 0, no se... es invisible cuando no tenes vidas y cuando no tenga disparos queda removible
                            {
                                PeriodoDesdeModoSigilo = 0;
                                Estado = EstadoNave.ModoSigilo; // TODO: Fin del modo de invisibilidad (si posee vidas es modo sigilo)
                            }
                            else
                            {
                                Removible = true; // Todo: Fin del modo invisibilidad
                            }
                        }
                    }
                    else
                    {
                        if (Estado == EstadoNave.ModoSigilo)
                        {
                            if (PeriodoDesdeModoSigilo < PeriodoModoSigilo)
                            {
                                PeriodoDesdeModoSigilo += 1;
                            }
                            else
                            {
                                Estado = EstadoNave.ModoBatalla;
                            }
                        }
                        else
                        {
                            if (Estado == EstadoNave.Destruida)
                            {
                                if (PeriodoDesdeDestruccion < 16) // TODO: Corregir
                                {
                                    PeriodoDesdeDestruccion += 1;
                                }
                                else
                                {
                                    // TODO: 
                                    // Luego de que pase la animación si o si dejar invisible
                                    // porque si tiene disparos todavia dejarlo invisible
                                    // porque si ya no tiene disparos dejarlo invisible

                                    if (Vidas == 0)
                                    {
                                        if (Disparos.Count > 0)
                                            Removible = false;
                                        else
                                            Removible = true;

                                        PeriodoDesdeInvisibilidad = 0;
                                        Estado = EstadoNave.Invisible;
                                    }
                                    else
                                    {
                                        PeriodoDesdeInvulnerabilidad = 0;
                                        Estado = EstadoNave.Invulnerable;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected abstract void ActualizarCoordenadas();
        protected abstract void Redibujar();

        protected void DefinirOpacidad()
        {
            elementoDibujable.Opacity = 10;
            if (Estado == EstadoNave.Invisible || Estado == EstadoNave.ModoSigilo) 
                elementoDibujable.Opacity = 0;
            else
                if (Estado == EstadoNave.Invulnerable)
                    elementoDibujable.Opacity = numeroAlAzar.Next(4, 9) / 10.0;
        }

        protected abstract void MoverDisparos();

        protected void DibujarRemoverDisparos()
        {
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

        public void Destruirse()
        {
            if (Estado == EstadoNave.ModoBatalla)
            {
                string rutaFramesAnimacion = this.rutaAbsolutaImagenDestruccion; // TODO: COrregir
                Image imagenNave = (Image)elementoDibujable;
                animacion = new AnimacionFrameSprites(rutaFramesAnimacion, Dimenciones.Ancho, Dimenciones.Largo, 1, 18, imagenNave);
                animacion.IniciarAnimacion(16, true); // TODO: Frame por defecto es un parametro por defecto...
                Vidas = Vidas - 1; // TODO :Cuidado!
		        PeriodoDesdeDestruccion = 0;
                Estado = EstadoNave.Destruida;
            }
        }
        
    }
}
