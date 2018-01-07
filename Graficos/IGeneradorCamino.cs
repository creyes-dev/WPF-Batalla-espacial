using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_BatallaEspacial.ObjetosComunes;

namespace WPF_BatallaEspacial.Graficos
{
    public interface IGeneradorCamino
    {
        PathGeometry ObtenerCamino(Direccion orientacion, int puntoNeutroY, int valorMinimoX, int valorMaximoX, Posicion posicionInicial);
    }
}
