using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Threading;

namespace WPF_BatallaEspacial.Graficos
{
    public class AnimacionFrameSprites
    {
        public List<ImageSource> framesSprite { get; set; }
        System.Windows.Controls.Image frame { get; set; }
        int NumeroFramePorDefecto { get; set; }
        int numeroFrame { get; set; }
        DispatcherTimer timer { get; set; }

        public AnimacionFrameSprites(string rutaArchivo, int anchoFrameSprite, int altoFrameSprite,
            int cantFilasFramesSprite, int cantColumnasFramesSprite, System.Windows.Controls.Image imagenDefecto)
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            framesSprite = new List<ImageSource>();
            Bitmap imagenOrigen = new Bitmap(rutaArchivo);
            for (int i = 0; i < cantFilasFramesSprite; i++)
                for (int j = 0; j < cantColumnasFramesSprite; j++)
                {
                    System.Drawing.Rectangle rectanguloFrameSprite = new System.Drawing.Rectangle(j * anchoFrameSprite, i * altoFrameSprite, anchoFrameSprite, altoFrameSprite);
                    Bitmap imagen = imagenOrigen.Clone(rectanguloFrameSprite, imagenOrigen.PixelFormat);
                    BitmapSource origenImagenFrameSprite = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(imagen.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    framesSprite.Add(origenImagenFrameSprite);
                }

            numeroFrame = 0;
            frame = imagenDefecto;
            imagenDefecto = new System.Windows.Controls.Image();
        }

        public void IniciarAnimacion(int framesPorMilisegundo, bool animarSoloUnaVez, int nroFramePorDefecto)
        {
            if (!timer.IsEnabled)
            {
                numeroFrame = nroFramePorDefecto;
                if (animarSoloUnaVez)
                {
                    timer.Tick += new EventHandler(AnimarFrameUnSoloCiclo);
                    NumeroFramePorDefecto = nroFramePorDefecto;
                }
                else
                {
                    timer.Tick += new EventHandler(AnimarFrameCiclos);
                }
                timer.Interval = new TimeSpan(0, 0, 0, 0, framesPorMilisegundo);
                timer.Start();
                this.CargarSiguienteFrame();
            }
        }

        public void ModificarFramesPorMilisegundo(int framesPorMilisegundos)
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, framesPorMilisegundos);
        }

        private void AnimarFrameCiclos(object sender, EventArgs e)
        {
            CargarSiguienteFrame();
        }

        private void DetenerAnimacionFrameCiclos(object sender, EventArgs e)
        {
            timer.Stop();
            frame.Source = framesSprite[0];
            numeroFrame = 0;
            timer.Tick -= new EventHandler(AnimarFrameCiclos);
        }

        private void AnimarFrameUnSoloCiclo(object sender, EventArgs e)
        {
            // TODO: Refactorizar
            if (numeroFrame >= framesSprite.LongCount())
            {
                timer.Stop();

                frame.Source = framesSprite[NumeroFramePorDefecto];
                timer.Tick -= new EventHandler(AnimarFrameUnSoloCiclo);
                return;
            }
            frame.Source = framesSprite[numeroFrame++];
        }

        // Obtiene el siguiente frame, si el numero de frame excede el total de frames
        // en la imagen, entonces cargar el primer frame reiniciando el ciclo
        private void CargarSiguienteFrame()
        {
            if (numeroFrame >= framesSprite.LongCount())
                numeroFrame = 1;

            frame.Source = framesSprite[numeroFrame++];
        }

    }
}
