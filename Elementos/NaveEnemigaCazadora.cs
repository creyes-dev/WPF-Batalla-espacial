using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF_BatallaEspacial.ObjetosComunes;
using WPF_BatallaEspacial.Graficos;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_BatallaEspacial.Elementos
{
    public class NaveEnemigaCazadora : NaveEnemiga
    {
        public NaveEnemigaCazadora(string nombre, Canvas canvas, 
                            int posicionX, int posicionY, int ancho, int largo,
                            int posicionVerticalPorDefecto,
                            int duracionDesplazamiento = 5, 
                            int periodoInvisibilidad = 0, 
                            int periodoModoSigilo = 0,
                            int vidas = 1)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo,
                    posicionVerticalPorDefecto, duracionDesplazamiento, periodoInvisibilidad, 
                    periodoModoSigilo, vidas)
        {
            generadorCaminos = new GeneradorCaminoVueloLineal();

            // Suscribirse al evento de animación completa 
            // para cambiar de algoritmo de generación del camino del movimiento 
            animacionEjeX.Completed += CambiarAlgoritmoCaminoMovimiento;
        }

        private void CambiarAlgoritmoCaminoMovimiento(object sender, EventArgs e)
        {
            if (generadorCaminos.GetType() == typeof(GeneradorCaminoVueloLineal))
            {
                generadorCaminos = new GeneradorCaminoVueloOndulado();
            }
            else
            {
                if (generadorCaminos.GetType() == typeof(GeneradorCaminoVueloOndulado))
                {
                    generadorCaminos = new GeneradorCaminoVueloCurvas();
                }
                else
                {
                    generadorCaminos = new GeneradorCaminoVueloLineal();
                }
            }                
        }

        protected override void AsignarDirectoriosImagenes()
        {
            rutaAbsolutaImagenNave = Environment.CurrentDirectory + @"\Imagenes\enemiga4.png";
            rutaAbsolutaImagenDisparo = Environment.CurrentDirectory + @"\Imagenes\rayo5.png";
            rutaAbsolutaImagenDestruccion = Environment.CurrentDirectory + @"\Imagenes\enemiga1_explosion.png";
        }



    }
}
