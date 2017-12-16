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
        int puntaje;

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
            
            foreach (NaveEnemiga nave in nivelActual.NavesEnemigas)
	        {
		        nave.CargarEnCanvas();
	        }

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
            // Redibujar los elementos del juego
            foreach (NaveEnemiga nave in nivelActual.NavesEnemigas)
            {
                nave.Dibujarse();
            }

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

            // Control de colisiones
            ControlarColisionesNavesEnemigas();
            ControlarColisionesJugador();
            
            // Mostrar puntaje
        
            // Verificar si ha terminado la partida

        }

        private bool HayColision(ElementoDibujable elemento1, ElementoDibujable elemento2)
        {
            bool hayColision = true;

            // Si el borde derecho del elemento1 se encuentra a la izquierda del borde izquierdo del elemento2 no hay colisión
            if ((elemento1.Posicion.PosicionX + elemento1.Dimenciones.Largo) < (elemento2.Posicion.PosicionX)) hayColision = false;
            // Si el borde izquierdo del elemento1 se encuentra a la derecha del borde derecho del elemento2 no hay colisión
            if (elemento1.Posicion.PosicionX > (elemento2.Posicion.PosicionX + elemento2.Dimenciones.Ancho)) hayColision = false;
            // Si el borde inferior del elemento1 se encuentra por encima del borde superior del elemento2 no hay colisión
            if ((elemento1.Posicion.PosicionY + elemento1.Dimenciones.Largo) < (elemento2.Posicion.PosicionY)) hayColision = false;
            // Si el borde superior del elemento1 se encuentra por debajo del borde inferior del elemento2 no hay colisión
            if (elemento1.Posicion.PosicionY > (elemento2.Posicion.PosicionY + elemento2.Dimenciones.Largo)) hayColision = false;
            
            return hayColision;
        }

        // Un disparo de una nave enemiga ha impactado en el jugador
        // o una nave enemiga ha impactado en el jugador
        private void ControlarColisionesNavesEnemigas()
        {
            if (nivelActual.Jugador.EstaViva)
            {

                // Si el disparo de una nave enemiga ha impactado en el jugador
                // destruir a la nave del jugador aunque la nave enemiga no se encuentre viva
                foreach (NaveEnemiga naveEnemiga in nivelActual.NavesEnemigas)
                {
                    foreach (Disparo disparo in naveEnemiga.Disparos)
                    {
                        if (HayColision(disparo, nivelActual.Jugador)) nivelActual.Jugador.Destruirse();
                    }
                }

                // Si la nave del jugador se encuentra viva y una nave enemiga que se encuentra viva
                // la toca entonces destruir ambas naves
                foreach (NaveEnemiga naveEnemiga in nivelActual.NavesEnemigas)
                {
                    if (naveEnemiga.EstaViva)
                    {
                        if (HayColision(naveEnemiga, nivelActual.Jugador))
                        {
                            naveEnemiga.Destruirse();
                            nivelActual.Jugador.Destruirse();
                        }
                    }
                }

            }
        }

        // Si el disparo del jugador ha impactado en una nave enemiga
        // que se encuentra viva la misma es destruida
        private void ControlarColisionesJugador()
        {
            foreach (Disparo disparo in nivelActual.Jugador.Disparos)
            {
                foreach (NaveEnemiga naveEnemiga in nivelActual.NavesEnemigas)
                {
                    if (naveEnemiga.EstaViva)
                    {
                        if (HayColision(naveEnemiga, disparo))
                        {
                            naveEnemiga.Destruirse();
                        }
                    }
                }
            }
        }

    }
}
