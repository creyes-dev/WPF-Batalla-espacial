using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_BatallaEspacial.ObjetosComunes
{
    public class ObjetoJuego
    {
        public ObjetoJuego(int posicionX, int posicionY, int ancho, int largo)
        {
            Posicion = new Posicion {
                PosicionX = posicionX,
                PosicionY = posicionY
            };

            Dimenciones = new Dimenciones {
                Ancho = ancho,
                Largo = largo
            };
        }

        public Posicion Posicion { get; set; }
        public Dimenciones Dimenciones { get; set; }
    }
}
