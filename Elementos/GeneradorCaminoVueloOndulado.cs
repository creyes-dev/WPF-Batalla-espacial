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
    public class GeneradorCaminoVueloOndulado : IGeneradorCaminoVuelo
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
            int mitadCiclo;
            int distanciaPuntosCiclo;
            int distanciaEntreDosPuntosCiclo;
            int cantCiclos = 3;

            int puntoExtremoYPrimerCiclo = posicionInicial.PosicionY - 100;
            int puntoExtremoYSegundoCiclo = posicionInicial.PosicionY + 100;

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

                mitadCiclo = Convert.ToInt32(anchoCiclo / 2); // punto de inflexión
                distanciaPuntosCiclo = Convert.ToInt32(mitadCiclo / 3); // Distancia entre los puntos máximos y mínimos de Y
                distanciaEntreDosPuntosCiclo = mitadCiclo - (distanciaPuntosCiclo * 2);

                posicionXActual = posicionXActual + distanciaPuntosCiclo * direccion;
                segmentoBezier.Points.Add(new Point(posicionXActual, puntoExtremoYPrimerCiclo));
                posicionXActual = posicionXActual + distanciaEntreDosPuntosCiclo * direccion;
                segmentoBezier.Points.Add(new Point(posicionXActual, puntoExtremoYPrimerCiclo));
                posicionXActual = posicionXActual + distanciaPuntosCiclo * direccion;
                segmentoBezier.Points.Add(new Point(posicionXActual, posicionInicial.PosicionY));
                posicionXActual = posicionXActual + distanciaPuntosCiclo * direccion;
                segmentoBezier.Points.Add(new Point(posicionXActual, puntoExtremoYSegundoCiclo));
                posicionXActual = posicionXActual + distanciaEntreDosPuntosCiclo * direccion;
                segmentoBezier.Points.Add(new Point(posicionXActual, puntoExtremoYSegundoCiclo));
                posicionXActual = posicionXActual + distanciaPuntosCiclo * direccion;
                segmentoBezier.Points.Add(new Point(posicionXActual, posicionInicial.PosicionY));
            }

            return camino;
        }
    }
}
