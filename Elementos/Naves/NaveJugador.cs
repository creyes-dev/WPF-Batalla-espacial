using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPF_BatallaEspacial.Elementos.Estados;
using WPF_BatallaEspacial.Elementos.Disparos;

namespace WPF_BatallaEspacial.Elementos.Naves
{
    public class NaveJugador : Nave
    {
        public NaveJugador( string nombre, Canvas canvas, 
                            int posicionX, int posicionY, int ancho, int largo)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            jugador = true;
            Vidas = 3;
            PeriodoInvulnerabilidad = 300;
            Estado = EstadoNave.Invulnerable;
        }

        protected override void CargarCañones()
        {
            // Un cañon es un elemento dibujable que no se dibuja en el Canvas 
            // para ahorrar recursos
            string nombreCañon = Nombre + "_CañonFrontal";
            Cañon nuevoCañon = new Cañon(nombreCañon, Canvas,
                                         0, 0, 7, 14, 0,
                                         rutaAbsolutaImagenDisparo,
                                         ObjetosComunes.Direccion.Superior,
                                         30, 30);
            Cañones.Add(nuevoCañon);
        }

        protected override void RedibujarNave()
        {
            Canvas.SetLeft(elementoDibujable, this.Posicion.PosicionX);
        }

        protected override void AsignarDirectoriosImagenes()
        {
            rutaAbsolutaImagenNave = Environment.CurrentDirectory + @"\Imagenes\player.png";
            rutaAbsolutaImagenDisparo = Environment.CurrentDirectory + @"\Imagenes\rayo1.png";
            rutaAbsolutaImagenDestruccion = Environment.CurrentDirectory + @"\Imagenes\player_explosion.png";
        }

        protected override void ActualizarCoordenadas()
        {
            // Hook method:
            // No es necesario obtener las coordenadas para el jugador 
            // porque automáticamente al moverse se actualiza 
            // Direccion.PosicionX y Direccion.PosicionY
        }

        public override void Desplazarse(ObjetosComunes.Direccion direccion)
        {
            if (Estado != EstadoNave.Invisible)
            {
                if (direccion == ObjetosComunes.Direccion.Izquierda)
                {
                    if (Posicion.PosicionX < 5)
                        Posicion.PosicionX = 0;
                    else
                        Posicion.PosicionX -= 5;
                }
                else
                {
                    if (Posicion.PosicionX >= Canvas.Width - Dimenciones.Ancho)
                        Posicion.PosicionX = (int)Canvas.Width - Dimenciones.Ancho;
                    else
                        Posicion.PosicionX += 5;
                }
            }
        }

    }
}
