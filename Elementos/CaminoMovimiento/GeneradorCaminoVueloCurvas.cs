using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_BatallaEspacial.ObjetosComunes;

namespace WPF_BatallaEspacial.Elementos.CaminoMovimiento
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
            int puntoNeutroY = 160;

            int posicionMaximaY = puntoNeutroY + 64;
            int posicionMinimaY = puntoNeutroY - 64;
            
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
                int posicionXTemporal = posicionXActual;
                
                // Generar puntos intermedios
                while (posicionXTemporal != posicionXActual + (anchoCiclo * direccion))
                {
                    if (direccion == 1)
                    {
                        posicionXTemporal = numero.Next(posicionXTemporal + 60,
                                                        posicionXTemporal + 120);
                    }
                    else
                    {
                        posicionXTemporal = numero.Next(posicionXTemporal - 120,
                                                        posicionXTemporal - 60);
                    }

                    posicionYActual = numero.Next(posicionYActual - 80, posicionYActual + 80);

                    if (((posicionXTemporal > posicionXActual + anchoCiclo) && (direccion == 1)) ||
                        ((posicionXTemporal < posicionXActual - anchoCiclo) && (direccion == -1)))
                    {
                        posicionXTemporal = posicionXActual + anchoCiclo * direccion;
                    }

                    if (posicionYActual > posicionMaximaY)
                    {
                        posicionYActual = posicionMaximaY;
                    }

                    if (posicionYActual < posicionMinimaY)
                    {
                        posicionYActual = posicionMinimaY;
                    }

                    segmentoBezier.Points.Add(new Point(posicionXTemporal, posicionYActual));
                }
            }

            return camino;
        }
    }
}
