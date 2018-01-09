using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_BatallaEspacial.ObjetosComunes;

namespace WPF_BatallaEspacial.Elementos
{
    public interface IGeneradorCaminoVuelo
    {
        PathGeometry ObtenerCamino(Posicion posicionInicial, Posicion posicionFinal, int limiteMovimientoVertical);
    }
}
