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
            Random numeroAzar = new Random();
            NaveJugador jugador = new NaveJugador("jugador", canvas, 20, 400, 64, 64);

            int duracionDesplazamiento = 5;
            int periodoInvisibilidad = numeroAzar.Next(0, 200);
            int periodoSigilo = numeroAzar.Next(0, 200);

            List<NaveEnemiga> navesEnemigas = new List<NaveEnemiga>();
            NaveEnemiga naveEnemiga = new NaveEnemiga("NaveEnemiga", canvas, 0, 0, 64, 64, duracionDesplazamiento, periodoInvisibilidad, periodoSigilo);
            navesEnemigas.Add(naveEnemiga);

            Nivel nuevoNivel = new Nivel(nroNivel, jugador, navesEnemigas);

            return nuevoNivel;
        }

    }
}
