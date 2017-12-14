using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF_BatallaEspacial.Elementos
{
    public class NaveJugador : Nave
    {
        public int Vidas { get; set; }

        public int periodoDisponibilidadNuevoDisparo;
        public int periodoDesdeUltimoDisparo;

        public NaveJugador( string nombre, Canvas canvas, 
                            int posicionX, int posicionY, int ancho, int largo)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            Vidas = 3;
            periodoDisponibilidadNuevoDisparo = 0;
        }

        protected override void AsignarDirectoriosImagenes()
        {
            rutaRelativaImagenNave = "../Imagenes/player.png";
            rutaRelativaImagenDisparo = "../Imagenes/rayo1.png";
            rutaRelativaImagenDestruccion = "../Imagenes/player.png";
        }

        public override void Disparar()
        {
            if (EstaViva)
            {
                if (periodoDesdeUltimoDisparo >= periodoDisponibilidadNuevoDisparo)
                {
                    // Obtener la localizacion del origen del disparo (punto medio de la nave)
                    int puntoInicioDisparoX = (int)(Posicion.PosicionX + (Dimenciones.Ancho / 2.0));
                    int puntoInicioDisparoY = (int)(Posicion.PosicionY + (Dimenciones.Largo / 2.0));

                    // TODO: El nombre del disparo se debe definir en la clase abstracta nave
                    Disparo disparo = new Disparo("Disparo" + numeroAlAzar.Next(0, 32199170).ToString(), this.Canvas, puntoInicioDisparoX, puntoInicioDisparoY, 7, 32, rutaRelativaImagenDisparo);
                    Disparos.Add(disparo);

                    periodoDesdeUltimoDisparo = 0;
                }
            }
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

    }
}
