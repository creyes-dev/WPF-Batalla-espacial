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
            int filaEnemigaAsalto = 64;
            int filaEnemigaDefensora = 64;
            int filaEnemigaExploradora = 160;
            int filaEnemigaCazadora = 192;

            Random numeroAzar = new Random();
            Espacio espacio = new Espacio("Espacio", canvas, 0, 0, 900, 1521);

            // TODO: Debe venir por parametros
            NaveJugador jugador = new NaveJugador("jugador", canvas, 20, filaJugador, 64, 64);

            int duracionDesplazamiento = 5;
            int periodoInvisibilidad = numeroAzar.Next(0, 200);
            int periodoSigilo = numeroAzar.Next(0, 200);

            List<NaveEnemiga> navesEnemigas = new List<NaveEnemiga>();

            NaveEnemiga naveEnemiga = new NaveEnemigaAsalto("Asalto1", canvas, 0, 0, 64, 64, filaEnemigaAsalto, duracionDesplazamiento, periodoInvisibilidad, periodoSigilo);

            duracionDesplazamiento = 5;
            periodoInvisibilidad = numeroAzar.Next(0, 200);
            periodoSigilo = numeroAzar.Next(0, 200);

            NaveEnemiga naveEnemiga2 = new NaveEnemigaAsalto("Asalto2", canvas, 0, 0, 64, 64, filaEnemigaAsalto, duracionDesplazamiento, periodoInvisibilidad, periodoSigilo);

            duracionDesplazamiento = 5;
            periodoInvisibilidad = numeroAzar.Next(0, 200);
            periodoSigilo = numeroAzar.Next(0, 200);

            NaveEnemiga naveEnemiga3 = new NaveEnemigaDefensora("Defensora1", canvas, 0, 0, 64, 64, filaEnemigaDefensora, duracionDesplazamiento, periodoInvisibilidad, periodoSigilo);

            duracionDesplazamiento = 5;
            periodoInvisibilidad = numeroAzar.Next(0, 200);
            periodoSigilo = numeroAzar.Next(0, 200);

            NaveEnemiga naveEnemiga4 = new NaveEnemigaExploradora("Exploradora1", canvas, 0, 0, 64, 64, filaEnemigaExploradora, duracionDesplazamiento, periodoInvisibilidad, periodoSigilo);

            duracionDesplazamiento = 5;
            periodoInvisibilidad = numeroAzar.Next(0, 200);
            periodoSigilo = numeroAzar.Next(0, 200);

            NaveEnemiga naveEnemiga5 = new NaveEnemigaCazadora("Cazadora1", canvas, 0, 0, 64, 64, filaEnemigaCazadora, duracionDesplazamiento, periodoInvisibilidad, periodoSigilo);

            navesEnemigas.Add(naveEnemiga);
            navesEnemigas.Add(naveEnemiga2);
            navesEnemigas.Add(naveEnemiga3);
            navesEnemigas.Add(naveEnemiga4);
            navesEnemigas.Add(naveEnemiga5);

            Nivel nuevoNivel = new Nivel(espacio, nroNivel, jugador, navesEnemigas);

            return nuevoNivel;
        }

    }
}
