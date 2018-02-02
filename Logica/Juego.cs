using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using WPF_BatallaEspacial.Logica;
using WPF_BatallaEspacial.Elementos;
using WPF_BatallaEspacial.ObjetosComunes;
using WPF_BatallaEspacial.Elementos.Naves;
using WPF_BatallaEspacial.Elementos.Estados;
using WPF_BatallaEspacial.Elementos.Disparos;

namespace WPF_BatallaEspacial.Logica
{
    public class Juego
    {
        Canvas canvas;
        MainWindow ventana;
        Rectangle rectanguloFondo;
        Label labelGameOver;
        Label labelNivel;

        Nivel nivelActual;
        DispatcherTimer timer;
        int puntaje;

        bool gameOver;
        bool nivelFinalizado;

        int intervaloNuevoNivel = 60;
        int intervaloHastaComienzoNivel = 0;

        public Juego(Canvas lienzo, MainWindow ventanaPrincipal)
        {
            canvas = lienzo;
            ventana = ventanaPrincipal;

            CargarFondoTexto();
            CargarTextoGameOver();
            CargarTextoNivel();
        }

        private void CargarTextoGameOver()
        {
            labelGameOver = new Label();
            labelGameOver.Name = "lblGameOver";
            labelGameOver.Content = "Game Over";
            labelGameOver.FontFamily = new FontFamily("Century Gothic");
            labelGameOver.FontSize = 12;
            labelGameOver.FontStretch = FontStretches.UltraExpanded;
            labelGameOver.FontStyle = FontStyles.Italic;
            labelGameOver.FontWeight = FontWeights.UltraBold;
            labelGameOver.Foreground = Brushes.White;
        }

        private void VisibilidadTextoGameOver(bool visible)
        {
            // Elemento dibujable
            if (visible)
            {
                canvas.Children.Add(labelGameOver);
                Canvas.SetLeft(labelGameOver, 0);
                Canvas.SetTop(labelGameOver, 0);
                Canvas.SetZIndex(labelGameOver, 5);
            }
            else
            {
                canvas.Children.Remove(labelGameOver);
            }
        }

        private void CargarFondoTexto()
        {
            rectanguloFondo = new Rectangle();
            rectanguloFondo.Fill = System.Windows.Media.Brushes.Black;
            rectanguloFondo.Width = canvas.Width;
            rectanguloFondo.Height = canvas.Height;
        }

        private void CargarTextoNivel()
        {
            labelNivel = new Label();
            labelNivel.Name = "lblGameOver";
            labelNivel.Content = "Game Over";
            labelNivel.FontFamily = new FontFamily("Century Gothic");
            labelNivel.FontSize = 12;
            labelNivel.FontStretch = FontStretches.UltraExpanded;
            labelNivel.FontStyle = FontStyles.Italic;
            labelNivel.FontWeight = FontWeights.UltraBold;
        }

        public void IniciarJuego()
        {
            // Limpiar el canvas
            canvas.Children.Clear();
            nivelActual = NivelFactory.Construir(1, canvas);

            nivelActual.Espacio.Dibujarse();
            nivelActual.Espacio.DesplazarImagen(0, -1521 + 1011, 8, true);

            nivelActual.Jugador.CargarEnCanvas();
            
            foreach (NaveEnemiga nave in nivelActual.NavesEnemigas)
	        {
		        nave.CargarEnCanvas();
	        }

            IniciarCiclosJuego();
        }

        private void VisibilidadMensajeNuevoNivel(bool visible)
        {
            // Elemento dibujable
            if (visible)
            {
                canvas.Children.Add(labelNivel);
                Canvas.SetLeft(labelNivel, 0);
                Canvas.SetTop(labelNivel, 0);
                Canvas.SetZIndex(labelNivel, 5);

                canvas.Children.Add(rectanguloFondo);
                Canvas.SetLeft(rectanguloFondo, 0);
                Canvas.SetTop(rectanguloFondo, 0);
                Canvas.SetZIndex(labelNivel, 4);
            }
            else
            {
                canvas.Children.Remove(labelNivel);
            }
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
            // Mover al jugador
            if (Keyboard.IsKeyDown(Key.Left))
                nivelActual.Jugador.Desplazarse(Direccion.Izquierda);
            if (Keyboard.IsKeyDown(Key.Right))
                nivelActual.Jugador.Desplazarse(Direccion.Derecha);
            if (Keyboard.IsKeyDown(Key.Space))
                nivelActual.Jugador.Disparar();

            // Control de colisiones
            ControlarColisionesNavesEnemigas();
            ControlarColisionesJugador();

            // Redibujar o remover elementos
            if (!nivelActual.Jugador.Removible)
                nivelActual.Jugador.Dibujarse();
            else
                nivelActual.Jugador.Removerse();

            foreach (NaveEnemiga nave in nivelActual.NavesEnemigas)
            {
                nave.Dibujarse();
            }

            // Remover elementos
            nivelActual.RemoverNaves();

            // Mostrar puntaje
            
            // Verificar si ha terminado la partida
            if (nivelActual.Jugador.Vidas == 0 && !gameOver)
            {
                gameOver = true;
                VisibilidadTextoGameOver(true);
                nivelActual.Jugador.Removerse();
            }

            // Verificar si hay que pasar al siguiente nivel
            if (nivelActual.NavesEnemigas.Count == 0 && !nivelFinalizado)
            {
                nivelFinalizado = true;
                VisibilidadMensajeNuevoNivel(true);
            }
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
            if (nivelActual.Jugador.Estado == EstadoNave.ModoBatalla)
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
                    if (naveEnemiga.Estado == EstadoNave.ModoBatalla)
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
                    if (naveEnemiga.Estado == EstadoNave.ModoBatalla)
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
