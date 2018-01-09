using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_BatallaEspacial.ObjetosComunes;

namespace WPF_BatallaEspacial.Elementos
{
    public class GeneradorCaminoVueloLineal : IGeneradorCaminoVuelo
    {

        public PathGeometry ObtenerCamino(Posicion posicionInicial, Posicion posicionFinal, int movimientoVertical)
        {
            // El sentido horizontal del desplazamiento depende de la localizacion horizontal
            // de ambas posiciones
            int direccion = (posicionInicial.PosicionX < posicionFinal.PosicionX) ? 1 : -1;

            PathGeometry camino = new PathGeometry();
            PathFigure caminoFigura = new PathFigure();
            caminoFigura.StartPoint = new Point(posicionInicial.PosicionX, posicionInicial.PosicionY);

            PolyLineSegment segmentoLineal = new PolyLineSegment();
            caminoFigura.Segments.Add(segmentoLineal);
            camino.Figures.Add(caminoFigura);

            int adelantoRetraso;
            int cantCiclos = 3;

            int posicionXActual = posicionInicial.PosicionX;
            int posicionYActual = posicionInicial.PosicionY;

            Random numero = new Random();

            for (int i = 1; i <= cantCiclos; i++)
            {
                int anchoCiclo;

                // hay una probabilidad del 10% de que la nave descienda 
                if (numero.Next(0, 9) == 0)
                {
                    anchoCiclo = 0;
                    //posicionYActual = 
                }
                else
                {

                }


                // Si es el ultimo ciclo completo el ancho que falta para llenar hasta el punto máximo de X
                if (i == cantCiclos)
                {
                    if (direccion == 1)
                    {
                        anchoCiclo = Convert.ToInt32(posicionFinal.PosicionX) - posicionXActual;
                    }
                    else
                    {
                        anchoCiclo = posicionXActual;
                    }
                }
                else
                {
                    // No es el ultimo ciclo el ancho del mismo es al azar
                    anchoCiclo = numero.Next(80, 360);
                }

                adelantoRetraso = Convert.ToInt32(numero.Next(-1, 1));

                posicionXActual = posicionXActual + anchoCiclo * direccion;
                segmentoLineal.Points.Add(new Point(posicionXActual, posicionFinal.PosicionY));

                if (adelantoRetraso != 0)
                {
                    segmentoLineal.Points.Add(new Point(posicionXActual + Convert.ToInt32(numero.Next(64, 128)) * direccion * adelantoRetraso, posicionFinal.PosicionY));
                    segmentoLineal.Points.Add(new Point(posicionXActual, posicionFinal.PosicionY));
                }

            }
            return camino;
        }

    }
}
