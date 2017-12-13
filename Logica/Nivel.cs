using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_BatallaEspacial.Elementos;

namespace WPF_BatallaEspacial.Logica
{
    public class Nivel
    {
        public int NroNivel { get; set; }
        public NaveJugador Jugador { get; set; }
        public List<NaveEnemiga> NavesEnemigas { get; set; }
        
        public Nivel(int nroNivel, NaveJugador jugador, List<NaveEnemiga> navesEnemigas)
        {
            NroNivel = nroNivel;
            Jugador = jugador;
            NavesEnemigas = navesEnemigas;
        }

    }
}
