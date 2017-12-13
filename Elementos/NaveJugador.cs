using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF_BatallaEspacial.Elementos
{
    public class NaveJugador : Nave
    {


        public NaveJugador(string nombre, Canvas canvas, string directorioImagen, 
            int posicionX, int posicionY, int ancho, int largo,
            string directorioImagenDisparo, string directorioImagenAnimacionDestruccion)
            : base(nombre, canvas, directorioImagen,
            posicionX, posicionY, ancho, largo, 
            directorioImagenDisparo, directorioImagenAnimacionDestruccion)
        {

        }

        protected override void Dibujarse()
        {
            if (!EstaInvisible)
            {
                elementoDibujable = (Image)this.elementoDibujable;

                if (EstaInvencible)
                {
                    // Cuando está invencible parpadea
                    elementoDibujable.Opacity = numeroAlAzar.Next(4, 9) / 10.0;
                }
                else
                {
                    Canvas.SetLeft(elementoDibujable, this.Posicion.PosicionX);

                    //foreach (Disparo disparo in disparos)
                    //{
                    //    disparo.PosicionY -= 10;
                    //    disparo.Redibujar();
                    //}
                }
            }
        }

        public abstract void Disparar();
        public abstract void Desplazarse(ObjetosComunes.Direccion direccion);
    }
}
