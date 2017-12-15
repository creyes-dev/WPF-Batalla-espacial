using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_BatallaEspacial.Elementos;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF_BatallaEspacial.Logica
{
    public class NivelFactory
    {
        public static Nivel Construir(int nroNivel, Canvas canvas)
        {
            NaveJugador jugador = new NaveJugador("jugador", canvas, 20, 400, 64, 64);

            List<NaveEnemiga> navesEnemigas = new List<NaveEnemiga>();
            NaveEnemiga naveEnemiga = new NaveEnemiga("NaveEnemiga", canvas, 0, 0, 64, 64);
            
            navesEnemigas.Add(naveEnemiga);

            Nivel nuevoNivel = new Nivel(nroNivel, jugador, navesEnemigas);

            return nuevoNivel;
        }

    }
}
