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
            int filaJugador = 536;
            int filaEnemigaDefensor = 64;
            int filaEnemigaAsalto;
            int filaEnemigaExplorador;


            Random numeroAzar = new Random();
            Espacio espacio = new Espacio("Espacio", canvas, 0, 0, 900, 1521);

            // TODO: Debe venir por parametros
            NaveJugador jugador = new NaveJugador("jugador", canvas, 20, filaJugador, 64, 64);

            int duracionDesplazamiento = 5;
            int periodoInvisibilidad = numeroAzar.Next(0, 200);
            int periodoSigilo = numeroAzar.Next(0, 200);

            List<NaveEnemiga> navesEnemigas = new List<NaveEnemiga>();
            NaveEnemiga naveEnemiga = new NaveEnemiga("NaveEnemiga1", canvas, 0, 0, 64, 64, filaEnemigaDefensor, duracionDesplazamiento, periodoInvisibilidad, periodoSigilo);

            duracionDesplazamiento = 5;
            periodoInvisibilidad = numeroAzar.Next(0, 200);
            periodoSigilo = numeroAzar.Next(0, 200);

            List<NaveEnemiga> navesEnemigas2 = new List<NaveEnemiga>();
            NaveEnemiga naveEnemiga2 = new NaveEnemiga("NaveEnemiga2", canvas, 0, 0, 64, 64, filaEnemigaDefensor, duracionDesplazamiento, periodoInvisibilidad, periodoSigilo);

            duracionDesplazamiento = 5;
            periodoInvisibilidad = numeroAzar.Next(0, 200);
            periodoSigilo = numeroAzar.Next(0, 200);

            List<NaveEnemiga> navesEnemigas3 = new List<NaveEnemiga>();
            NaveEnemiga naveEnemiga3 = new NaveEnemiga("NaveEnemiga", canvas, 0, 0, 64, 64, filaEnemigaDefensor, duracionDesplazamiento, periodoInvisibilidad, periodoSigilo);

            navesEnemigas.Add(naveEnemiga);
            navesEnemigas.Add(naveEnemiga2);
            navesEnemigas.Add(naveEnemiga3);

            Nivel nuevoNivel = new Nivel(espacio, nroNivel, jugador, navesEnemigas);

            return nuevoNivel;
        }

    }
}
