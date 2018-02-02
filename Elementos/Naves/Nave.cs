using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPF_BatallaEspacial.Elementos.Estados;
using WPF_BatallaEspacial.Elementos.Disparos;
using WPF_BatallaEspacial.Graficos;
using WPF_BatallaEspacial.ObjetosComunes;

namespace WPF_BatallaEspacial.Elementos.Naves
{
    public abstract class Nave : ElementoDibujable
    {
        public EstadoNave Estado { get; set; }
        protected AnimacionFrameSprites animacion { get; set; }
        protected Random numeroAlAzar;
        public int Vidas { get; set; }

        protected List<Cañon> Cañones;
        protected bool jugador; // TODO: Eliminar

        public int PeriodoInvulnerabilidad { get; set; }
        public int PeriodoInvisibilidad { get; set; }
        public int PeriodoModoSigilo { get; set; }
        public int PeriodoDesdeDestruccion { get; set; } // TODO: Eliminar, hay que consultar por la propiedad Finalizar de la animación

        public int PeriodoDesdeInvulnerabilidad { get; set; }
        public int PeriodoDesdeInvisibilidad { get; set; }
        public int PeriodoDesdeModoSigilo { get; set; }

        protected string rutaAbsolutaImagenNave;
        protected string rutaAbsolutaImagenDisparo;
        protected string rutaAbsolutaImagenDestruccion;
                
        public Nave(string nombre, Canvas canvas, 
                    int posicionX, int posicionY, int ancho, int largo) 
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            Cañones = new List<Cañon>();
            numeroAlAzar = new Random();
            Estado = EstadoNave.Invisible;
        }

        // El template method puede ser una implementacion de un metodo abstracto
        // Para cargar la imagen es necesario cargar las imagenes de disparos y explosion
        // Cargar en canvas
        public void CargarEnCanvas()
        {
            // 1. Obtener los directorios de las imagenes asociadas a la nave
            AsignarDirectoriosImagenes();
            // 2. Cargar el elemento dibujable de la nave
            CargarImagen();
            // 3. Cargar los componentes de la nave (cañones, turbinas de propulsión, etc)
            CargarComponentes();
            // 4. Cargar todos los recursos obtenidos en el canvas
            PosicionarImagenEnCanvas();
        }

        // 1. Obtener los directorios de las imagenes asociadas a la nave
        protected abstract void AsignarDirectoriosImagenes();

        // 2. Cargar el elemento dibujable de la nave
        protected void CargarImagen()
        {
            Image Imagen = new Image();
            Imagen.Source = new BitmapImage(new Uri(rutaAbsolutaImagenNave, UriKind.Absolute));
            Imagen.Name = "NaveJugador";
            Imagen.Height = this.Dimenciones.Largo;
            Imagen.Width = this.Dimenciones.Ancho;
            elementoDibujable = Imagen;
        }

        // 3. Cargar los componentes de la nave (cañones, turbinas de propulsión, etc)
        protected void CargarComponentes()
        {
            // Cargar los cañones de la nave
            CargarCañones();
        }

        protected abstract void CargarCañones();
         
        // 4. Cargar todos los recursos obtenidos en el canvas
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
                // Si hay un cañon con disparo disponible proceder a disparar
                foreach (Cañon canion in Cañones)
                {
                    if (canion.DisparoDisponible)
                    {
                        // Reposicionar canion
                        int posicionMitadNaveY = this.Posicion.PosicionY + (Dimenciones.Largo / 2);
                        int posicionMitadNaveX = this.Posicion.PosicionX + (Dimenciones.Ancho / 2);
                        
                        canion.Posicion.PosicionY = posicionMitadNaveY;
                        canion.Posicion.PosicionX = posicionMitadNaveX + canion.PosicionHorizontalRelativa;

                        // Iniciar disparo
                        canion.IniciarDisparo();
                    }
                }
            }
        }
        
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
        }

        // 1. Actualizar el estado
        public void ActualizarEstado()
        {
            if (Estado != EstadoNave.ModoBatalla)
            {
                if (Estado == EstadoNave.Invulnerable)
                {
                    if (PeriodoDesdeInvulnerabilidad < PeriodoInvulnerabilidad)
                    {
                        PeriodoDesdeInvulnerabilidad += 1;
                    }   
                    else
                    {
                        foreach (Cañon canion in Cañones)
	                    {
		                    canion.AprontarseParaNuevoDisparo();
                            Estado = EstadoNave.ModoBatalla;
	                    }
                    }   
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
                               // si no posee vidas y sigue invisible es porque esta lista para ser removida, solo se removera en caso de que no posea mas disparos
                                if (Disparos.Count == 0) 
                                {
                                    Removible = true; // Todo: Fin del modo invisibilidad
                                }
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
                                foreach (Cañon canion in Cañones)
                                {
                                    canion.AprontarseParaNuevoDisparo();
                                }
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
                                        if (CantidadDisparos > 0)
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

        // 2. Obtener coordenadas
        protected abstract void ActualizarCoordenadas();

        // 3. Redibujar
        public void Redibujar()
        {
            RedibujarNave();
            RedibujarCaniones();
        }

        protected abstract void RedibujarNave();

        protected void RedibujarCaniones()
        {
            foreach (Cañon canion in Cañones)
            {
                canion.Dibujarse();
            }
        }

        // 4. Definir opacidad
        protected void DefinirOpacidad()
        {
            elementoDibujable.Opacity = 10;
            if (Estado == EstadoNave.Invisible || Estado == EstadoNave.ModoSigilo) 
                elementoDibujable.Opacity = 0;
            else
                if (Estado == EstadoNave.Invulnerable)
                    elementoDibujable.Opacity = numeroAlAzar.Next(4, 9) / 10.0;
        }

        public List<Disparo> Disparos
        {
            get
            {
                List<Disparo> disparos = new List<Disparo>();
                foreach (Cañon canion in Cañones)
                {
                    disparos.AddRange(canion.Disparos);
                }
                return disparos;
            }
        }

        protected int CantidadDisparos 
        {
            get
            {
                int cantDisparos = 0;
                foreach (Cañon cañon in Cañones)
                {
                    cantDisparos += cañon.CantidadDisparos;
                }
                return cantDisparos;
            }
        }
                
        public void Destruirse()
        {
            if (Estado == EstadoNave.ModoBatalla)
            {
                Image imagenNave = (Image)elementoDibujable;
                animacion = new AnimacionFrameSprites(rutaAbsolutaImagenDestruccion, Dimenciones.Ancho, Dimenciones.Largo, 1, 18, imagenNave);
                animacion.IniciarAnimacion(16, true);
                Vidas = Vidas - 1;
		        PeriodoDesdeDestruccion = 0;
                Estado = EstadoNave.Destruida;
            }
        }
        
    }
}
