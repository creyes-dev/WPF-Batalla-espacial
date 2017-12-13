using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_BatallaEspacial.Logica;
using WPF_BatallaEspacial.Elementos;

namespace WPF_BatallaEspacial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Nivel nivelActual; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.BatallaEspacialCanvas.Children.Clear();

            nivelActual = NivelFactory.Construir(1, BatallaEspacialCanvas);
            nivelActual.Jugador.CargarEnCanvas();

        }

    }
}
