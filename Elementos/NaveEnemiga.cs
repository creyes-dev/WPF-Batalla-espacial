﻿using System;
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

namespace WPF_BatallaEspacial.Elementos
{
    public class NaveEnemiga : Nave
    {
        public int DuracionDesplazamiento { get; set; }
        public int Puntaje { get; set; }
        // contenedor de animaciones de la figura
        Path camino;
        Storyboard storyboard;

        // componentes horizontales y verticales de la animación que sigue el camino
        DoubleAnimationUsingPath animacionEjeX;
        DoubleAnimationUsingPath animacionEjeY;
        
        IGeneradorCamino generadorCaminos;

        public NaveEnemiga(string nombre, Canvas canvas, 
                            int posicionX, int posicionY, int ancho, int largo,
                            int duracionDesplazamiento = 5)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            jugador = false;
            DuracionDesplazamiento = duracionDesplazamiento;
            generadorCaminos = new GeneradorCaminoLinea();
            PeriodoRecuperacionDisparo = 10;
        }

        public override void Desplazarse(ObjetosComunes.Direccion direccion)
        {
            AnimarDesplazamiento(direccion);
        }

        protected void AnimarDesplazamiento(Direccion direccion)
        {
            if (camino == null)
            {

                Image imagen = (Image)this.elementoDibujable;

                string nombreAnimacion = Nombre + "_animacion";

                // La animación de transformación es instanciada, registrada y asociada a la imagen 
                TransformGroup grupoTransformaciones = new TransformGroup();
                imagen.RenderTransform = grupoTransformaciones;

                TranslateTransform animacionTranslateTransform = new TranslateTransform();
                this.Canvas.RegisterName(nombreAnimacion, animacionTranslateTransform);
                grupoTransformaciones.Children.Add(animacionTranslateTransform);

                // Obtiene un camino con forma de onda y orientada hacia la dirección del movimiento
                PathGeometry caminoOnda = generadorCaminos.ObtenerCamino(direccion,
                    Posicion.PosicionY, 0, Convert.ToInt32(this.Canvas.Width - 64));
                caminoOnda.Freeze();

                // Registrar el camino en el canvas
                Path nuevoCamino = new Path();
                nuevoCamino.Data = caminoOnda;

                camino = nuevoCamino;
                Canvas.Children.Add(camino);

                // Crear la animación que mueve la figura horizontalmente
                animacionEjeX = new DoubleAnimationUsingPath();
                animacionEjeX.PathGeometry = caminoOnda;
                animacionEjeX.Duration = TimeSpan.FromSeconds(DuracionDesplazamiento);
                animacionEjeX.Source = PathAnimationSource.X; // movimiento sobre el eje X

                // Asociar la animación para que oriente la propiedad X de
                // la animación de transformación
                Storyboard.SetTargetName(animacionEjeX, nombreAnimacion);
                Storyboard.SetTargetProperty(animacionEjeX,
                    new PropertyPath(TranslateTransform.XProperty));

                // Crear la animación que mueve la figura verticalmente
                animacionEjeY = new DoubleAnimationUsingPath();
                animacionEjeY.PathGeometry = caminoOnda;
                animacionEjeY.Duration = TimeSpan.FromSeconds(DuracionDesplazamiento);
                animacionEjeY.Source = PathAnimationSource.Y; // movimiento sobre el eje Y

                // Asociar la animacion para que oriente la propiedad Y de
                // la animación de transformación
                Storyboard.SetTargetName(animacionEjeY, nombreAnimacion);
                Storyboard.SetTargetProperty(animacionEjeY,
                    new PropertyPath(TranslateTransform.YProperty));

                // Crear el Storyboard para contener y aplicar las animaciones
                storyboard = new Storyboard();
                //pathAnimationStoryboard.RepeatBehavior = RepeatBehavior.Forever;
                storyboard.Children.Add(animacionEjeX);
                storyboard.Children.Add(animacionEjeY);

                // Suscribir el procedimiento que atiende la finalización de la animación
                animacionEjeX.Completed += AnimacionCompletada;

                // Iniciar la animación
                storyboard.Begin(Canvas);
            }
        }

        public override void Disparar()
        {
            if (EstaViva && !EstaInvencible)
            {
                if (periodoDesdeUltimoDisparo >= PeriodoRecuperacionDisparo)
                {
                    // Obtener la localizacion del origen del disparo (punto medio de la nave)
                    int puntoInicioDisparoX = (int)(Posicion.PosicionX + (Dimenciones.Ancho / 2.0));
                    int puntoInicioDisparoY = (int)(Posicion.PosicionY + (Dimenciones.Largo / 2.0));

                    // TODO: El nombre del disparo se debe definir en la clase abstracta nave
                    Disparo disparo = new Disparo("Disparo" + numeroAlAzar.Next(0, 32199170).ToString(), this.Canvas, puntoInicioDisparoX, puntoInicioDisparoY, 7, 32, rutaRelativaImagenDisparo);
                    Disparos.Add(disparo);

                    periodoDesdeUltimoDisparo = 0;
                }
            }
        }

        public override void MoverDisparos()
        {
            // La nave enemiga dispara.
            Disparar();

            foreach (Disparo disparo in Disparos)
            {
                disparo.Posicion.PosicionY += 5;
                disparo.Dibujarse();
            }
        }

        private void AnimacionCompletada(object sender, EventArgs e)
        {
            Image imagen = (Image) elementoDibujable;

            // Obtiene la posición horizontal de la figura luego de que su animación ha 
            // finalizado
            Point coordenadaFigura = imagen.PointToScreen(new Point(0, 0));
            Point coordenadaCanvas = Canvas.PointFromScreen(new Point(0, 0));

            // Posicion de la figura en la ventana = posicion del canvas + 
            int posicionXAlien = Convert.ToInt32(coordenadaFigura.X) + Convert.ToInt32(coordenadaCanvas.X);
            bool moverHaciaDerecha = (posicionXAlien == 0);

            Canvas.Children.Remove(camino);

            PathGeometry nuevoCamino;

            // Obtiene un camino con forma de onda y orientada hacia la dirección correcta
            if (moverHaciaDerecha)
            {
                nuevoCamino = generadorCaminos.ObtenerCamino(Direccion.Derecha, Posicion.PosicionY, 0, Convert.ToInt32(Canvas.Width - Dimenciones.Ancho));
            }
            else
            {
                nuevoCamino = generadorCaminos.ObtenerCamino(Direccion.Izquierda, Posicion.PosicionY, 0, Convert.ToInt32(Canvas.Width - Dimenciones.Ancho));
            }

            // El componente horizontal y vertical de la animación seguirá el nuevo camino generado
            animacionEjeX.PathGeometry = nuevoCamino;
            animacionEjeY.PathGeometry = nuevoCamino;

            // Iniciar la animación
            storyboard.Begin(Canvas, true);
        }

        protected override void AsignarDirectoriosImagenes()
        {
            rutaRelativaImagenNave = "../Imagenes/player.png";
            rutaRelativaImagenDisparo = "../Imagenes/rayo1.png";
            rutaRelativaImagenDestruccion = "../Imagenes/player.png";
        }

    }
}
