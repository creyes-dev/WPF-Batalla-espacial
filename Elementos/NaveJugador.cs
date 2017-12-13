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
        public NaveJugador( string nombre, Canvas canvas, 
                            int posicionX, int posicionY, int ancho, int largo)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {

        }

        protected override void AsignarDirectoriosImagenes()
        {
            rutaRelativaImagenNave = "../Imagenes/player.png";
            rutaRelativaImagenDisparo = "../Imagenes/rayo1.png";
            rutaRelativaImagenDestruccion = "../Imagenes/player.png";
        }

        public override void Disparar()
        {

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
