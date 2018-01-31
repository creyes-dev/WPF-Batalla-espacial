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
    public class NaveEnemigaExploradora : NaveEnemiga
    {
        public NaveEnemigaExploradora(string nombre, Canvas canvas, 
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
            generadorCaminos = new GeneradorCaminoVueloCurvas();
        }

        protected override void CargarCañones()
        {
            int test = 35;
            int min_test = test - 10;
            int max_test = test + 10;

            string nombreCañonFrontal = Nombre + "_CañonFrontal";
            Cañon cañonFrontal = new Cañon(nombreCañonFrontal, Canvas,
                                             0, 0, 7, 14, 0,
                                             rutaAbsolutaImagenDisparo,
                                             ObjetosComunes.Direccion.Inferior,
                                             min_test, max_test);

            string nombreCañonIzquierdo = Nombre + "_CañonIzquierdo";
            Cañon cañonIzquierdo = new Cañon(nombreCañonIzquierdo, Canvas,
                                             0, 0, 7, 14, -16,
                                             rutaAbsolutaImagenDisparo,
                                             ObjetosComunes.Direccion.InferiorIzquierda,
                                             min_test, max_test);

            string nombreCañonDerecho = Nombre + "_CañonDerecho";
            Cañon cañonDerecho = new Cañon(nombreCañonDerecho, Canvas,
                                           0, 0, 7, 14, 16,
                                           rutaAbsolutaImagenDisparo,
                                           ObjetosComunes.Direccion.InferiorDerecha,
                                           min_test, max_test);

            Cañones.Add(cañonFrontal);
            Cañones.Add(cañonIzquierdo);
            Cañones.Add(cañonDerecho);
        }
        
        protected override void AsignarDirectoriosImagenes()
        {
            rutaAbsolutaImagenNave = Environment.CurrentDirectory + @"\Imagenes\enemiga3.png";
            rutaAbsolutaImagenDisparo = Environment.CurrentDirectory + @"\Imagenes\rayo4.png";
            rutaAbsolutaImagenDestruccion = Environment.CurrentDirectory + @"\Imagenes\enemiga1_explosion.png";
        }
    }
}
