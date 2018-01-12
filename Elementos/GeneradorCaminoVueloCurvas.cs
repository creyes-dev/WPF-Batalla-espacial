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
    public class GeneradorCaminoVueloCurvas : IGeneradorCaminoVuelo
    {
        public PathGeometry ObtenerCamino(Posicion posicionInicial, Posicion posicionFinal)
        {
            // El sentido horizontal del desplazamiento depende de la localizacion horizontal
            // de ambas posiciones
            int direccion = (posicionInicial.PosicionX < posicionFinal.PosicionX) ? 1 : -1;

            PathGeometry camino = new PathGeometry();
            PathFigure caminoFigura = new PathFigure();
            caminoFigura.StartPoint = new Point(posicionInicial.PosicionX, posicionInicial.PosicionY);

            PolyBezierSegment segmentoBezier = new PolyBezierSegment();
            caminoFigura.Segments.Add(segmentoBezier);
            camino.Figures.Add(caminoFigura);

            int anchoCiclo;
            int cantCiclos = 3;

            int posicionNeutraY = 250;

            int posicionMaximaY = posicionNeutraY - 150;
            int posicionMinimaY = posicionNeutraY + 150;

            int posicionXActual = posicionInicial.PosicionX;
            int posicionYActual = posicionInicial.PosicionY;

            Random numero = new Random();

            for (int i = 1; i <= cantCiclos; i++)
            {
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
                    anchoCiclo = numero.Next(200, 330);
                }

                // PosicionXActual es la posicon del ultimo ciclo procesado la posicion actual es la siguiente:
                int posicionX = posicionXActual;

                // Generar puntos intermedios
                while (posicionX != posicionXActual + (anchoCiclo * direccion))
                {
                    if (direccion == 1)
                    {
                        posicionX = numero.Next(posicionX + 60,
                                                    posicionX + 120);
                    }
                    else
                    {
                        posicionX = numero.Next(posicionX - 120,
                                                    posicionX - 60);
                    }

                    posicionNeutraY = numero.Next(posicionNeutraY - 80, posicionNeutraY + 80);

                    if (((posicionX > posicionXActual + anchoCiclo) && (direccion == 1)) ||
                        ((posicionX < posicionXActual - anchoCiclo) && (direccion == -1)))
                    {
                        posicionX = posicionXActual + anchoCiclo * direccion;
                    }

                    if (posicionNeutraY > posicionMaximaY)
                    {
                        posicionNeutraY = posicionMaximaY;
                    }

                    if (posicionNeutraY < posicionMinimaY)
                    {
                        posicionNeutraY = posicionMinimaY;
                    }

                    segmentoBezier.Points.Add(new Point(posicionX, posicionNeutraY));
                }

                posicionXActual = posicionX;
            }

            return camino;
        }

    }
}
