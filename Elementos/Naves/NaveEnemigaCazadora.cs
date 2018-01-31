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
using WPF_BatallaEspacial.Elementos.CaminoMovimiento;
using WPF_BatallaEspacial.Elementos.Disparos;

namespace WPF_BatallaEspacial.Elementos.Naves
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

        protected override void CargarCañones()
        {
            int test = 35;
            int min_test = test - 10;
            int max_test = test + 10;

            string nombreCañonFrontalIzquierdo = Nombre + "_CañonFrontalIzquierdo";
            Cañon cañonFrontalIzquierdo = new Cañon(nombreCañonFrontalIzquierdo, Canvas,
                                             0, 0, 7, 14, -16,
                                             rutaAbsolutaImagenDisparo,
                                             ObjetosComunes.Direccion.Inferior,
                                             min_test - 5, max_test + 5);

            string nombreCañonFrontalDerecho = Nombre + "_CañonFrontalIzquierdo";
            Cañon cañonFrontalDerecho = new Cañon(nombreCañonFrontalIzquierdo, Canvas,
                                             0, 0, 7, 14, 16,
                                             rutaAbsolutaImagenDisparo,
                                             ObjetosComunes.Direccion.Inferior,
                                             min_test - 5, max_test + 5);

            string nombreCañonIzquierdo = Nombre + "_CañonIzquierdo";
            Cañon cañonIzquierdo = new Cañon(nombreCañonIzquierdo, Canvas,
                                             0, 0, 7, 14, -32,
                                             rutaAbsolutaImagenDisparo,
                                             ObjetosComunes.Direccion.InferiorIzquierda,
                                             min_test - 7, max_test + 7);

            string nombreCañonDerecho = Nombre + "_CañonDerecho";
            Cañon cañonDerecho = new Cañon(nombreCañonDerecho, Canvas,
                                           0, 0, 7, 14, 32,
                                           rutaAbsolutaImagenDisparo,
                                           ObjetosComunes.Direccion.InferiorDerecha,
                                           min_test + 3, max_test - 3);

            Cañones.Add(cañonFrontalIzquierdo);
            Cañones.Add(cañonFrontalDerecho);
            Cañones.Add(cañonIzquierdo);
            Cañones.Add(cañonDerecho);
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
