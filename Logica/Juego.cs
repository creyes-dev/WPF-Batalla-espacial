using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_BatallaEspacial.Logica;
using WPF_BatallaEspacial.Elementos;
using WPF_BatallaEspacial.ObjetosComunes;

namespace WPF_BatallaEspacial.Logica
{
    public class Juego
    {
        Canvas canvas;
        MainWindow ventana;
        Nivel nivelActual;
        DispatcherTimer timer;

        public Juego(Canvas lienzo, MainWindow ventanaPrincipal)
        {
            canvas = lienzo;
            ventana = ventanaPrincipal;
        }

        public void IniciarJuego()
        {
            // Limpiar el canvas
            canvas.Children.Clear();

            nivelActual = NivelFactory.Construir(1, canvas);
            nivelActual.Jugador.CargarEnCanvas();

            IniciarCiclosJuego();
        }

        private void IniciarCiclosJuego()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(ProcesarCiclo);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 16);
            timer.Start();
        }

        private void ProcesarCiclo(object sender, EventArgs e)
        {
            // Mostrar puntaje

            if (nivelActual.Jugador.Vidas == 0)
            {
                string gameover = "";
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.Left))
                    nivelActual.Jugador.Desplazarse(Direccion.Izquierda);
                if (Keyboard.IsKeyDown(Key.Right))
                    nivelActual.Jugador.Desplazarse(Direccion.Derecha);
                if (Keyboard.IsKeyDown(Key.Space))
                    nivelActual.Jugador.Disparar();

                nivelActual.Jugador.Dibujarse();
            }
        }
    }
}
