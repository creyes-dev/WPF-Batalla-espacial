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
using WPF_BatallaEspacial.Elementos.CaminoMovimiento;
using WPF_BatallaEspacial.Elementos.Estados;

namespace WPF_BatallaEspacial.Elementos.Naves
{
    public abstract class NaveEnemiga : Nave
    {
        public int DuracionDesplazamiento { get; set; }
        public int Puntaje { get; set; }
        public int PosicionVerticalPorDefecto;

        // contenedor de animaciones de la figura
        protected Path camino;
        protected Storyboard storyboard;
        protected bool enMovimiento;

        // componentes horizontales y verticales de la animación que sigue el camino
        protected DoubleAnimationUsingPath animacionEjeX;
        protected DoubleAnimationUsingPath animacionEjeY;

        protected IGeneradorCaminoVuelo generadorCaminos;

        public NaveEnemiga(string nombre, Canvas canvas, 
                            int posicionX, int posicionY, int ancho, int largo,
                            int posicionVerticalPorDefecto,
                            int duracionDesplazamiento, 
                            int periodoInvisibilidad, 
                            int periodoModoSigilo,
                            int vidas)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            jugador = false;
            animacionEjeX = new DoubleAnimationUsingPath();
            DuracionDesplazamiento = duracionDesplazamiento;
            //generadorCaminos = new GeneradorCaminoVueloCurvas();
            PosicionVerticalPorDefecto = posicionVerticalPorDefecto;
            PeriodoInvisibilidad = periodoInvisibilidad;
            PeriodoModoSigilo = periodoModoSigilo;
            Vidas = vidas;
        }

        // Las subclases de NaveEnemiga implementarán sus propios métodos para 
        // registrar sus recursos
        protected override abstract void AsignarDirectoriosImagenes();
        
        // Implementación del método abstracto para desplazar la nave
        public override void Desplazarse(ObjetosComunes.Direccion direccion)
        {
            AnimarDesplazamiento(direccion);
        }

        // 2. Obtener coordenadas
        protected override void ActualizarCoordenadas()
        {
            Point coordenadaRelativa = elementoDibujable.PointToScreen(new Point(0, 0));
            Point coordenadaCanvas = Canvas.PointFromScreen(new Point(0, 0));

            int coordenadaAbsolutaY = Convert.ToInt32(coordenadaRelativa.Y) + Convert.ToInt32(coordenadaCanvas.Y);
            int coordenadaAbsolutaX = Convert.ToInt32(coordenadaRelativa.X) + Convert.ToInt32(coordenadaCanvas.X);

            Posicion.PosicionX = coordenadaAbsolutaX;
            Posicion.PosicionY = coordenadaAbsolutaY;
        }
        
        // 3. Redibujar
        protected override void RedibujarNave()
        {
            // TODO: Corregir, no me parece correcto que redibujar = Desplazarse
            AnimarDesplazamiento(Direccion.Derecha);
            // Una nave enemiga puede disparar tan pronto 
            // como esté disponible el disparo
            Disparar();
        }

        protected void AnimarDesplazamiento(Direccion direccion)
        {
            if (Estado == EstadoNave.ModoSigilo || Estado == EstadoNave.ModoBatalla)
            {
                if (!enMovimiento)
                {
                    Image imagen = (Image)this.elementoDibujable;

                    string nombreAnimacion = Nombre + "_animacion";

                    // La animación de transformación es instanciada, registrada y asociada a la imagen 
                    TransformGroup grupoTransformaciones = new TransformGroup();
                    imagen.RenderTransform = grupoTransformaciones;

                    TranslateTransform animacionTranslateTransform = new TranslateTransform();
                    this.Canvas.RegisterName(nombreAnimacion, animacionTranslateTransform);
                    grupoTransformaciones.Children.Add(animacionTranslateTransform);

                    Posicion posicionInicial = new Posicion { PosicionX = 0,
                                                              PosicionY = this.PosicionVerticalPorDefecto };
                    Posicion posicionFinal = new Posicion { PosicionX = Convert.ToInt32(this.Canvas.Width - Dimenciones.Ancho),
                                                            PosicionY = this.PosicionVerticalPorDefecto};

                    // Obtiene un camino con forma de onda y orientada hacia la dirección del movimiento
                    PathGeometry caminoOnda = generadorCaminos.ObtenerCamino(posicionInicial, posicionFinal);
                    caminoOnda.Freeze();

                    // Registrar el camino en el canvas
                    Path nuevoCamino = new Path();
                    nuevoCamino.Data = caminoOnda;

                    camino = nuevoCamino;
                    Canvas.Children.Add(camino);

                    // Crear la animación que mueve la figura horizontalmente
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

                    enMovimiento = true;
                }
            }
        }

        protected void AnimacionCompletada(object sender, EventArgs e)
        {
            Image imagen = (Image) elementoDibujable;

            // Obtiene la posición horizontal de la figura luego de que su animación ha 
            // finalizado
            Point coordenadaFigura = imagen.PointToScreen(new Point(0, 0));
            Point coordenadaCanvas = Canvas.PointFromScreen(new Point(0, 0));

            // Posicion de la figura en la ventana = posicion del canvas + posicion de la figura

            // TODO: Modificar porque no es cero lo que estas buscando sino coordenadaCanvas.x
            // TODO: Por que Posicion.PosicionX No vale cero y posicionXAlien si?
            // TODO: Cambiale el nombre
            int posicionXAlien = Convert.ToInt32(coordenadaFigura.X) + Convert.ToInt32(coordenadaCanvas.X);
            int posicionYAlien = Convert.ToInt32(coordenadaFigura.Y) + Convert.ToInt32(coordenadaCanvas.Y);

            // Si la nave queda fuera del rango de vision reposicionarla
            if (posicionYAlien >= 950)
            {
                posicionYAlien = PosicionVerticalPorDefecto;

                if (numeroAlAzar.Next(0, 1) == 0)
                {
                    posicionXAlien = Dimenciones.Ancho * -1;
                }
                else
                {
                    posicionXAlien = Convert.ToInt32(coordenadaCanvas.X + Canvas.Width) + Dimenciones.Ancho;
                }
            }

            Posicion posicionInicial = new Posicion {
                PosicionX = posicionXAlien,
                PosicionY = posicionYAlien };

            int posicionXAlienFinal = 0;
            if (posicionXAlien <= 0)
            {
                posicionXAlienFinal = Convert.ToInt32(this.Canvas.Width - Dimenciones.Ancho);
            }

            Posicion posicionFinal = new Posicion {
                PosicionX = posicionXAlienFinal,
                PosicionY = this.PosicionVerticalPorDefecto };
            
            Canvas.Children.Remove(camino);
            PathGeometry nuevoCamino = generadorCaminos.ObtenerCamino(posicionInicial, posicionFinal);
            
            // El componente horizontal y vertical de la animación seguirá el nuevo camino generado
            animacionEjeX.PathGeometry = nuevoCamino;
            animacionEjeY.PathGeometry = nuevoCamino;

            // Iniciar la animación
            storyboard.Begin(Canvas, true);
        }

    }
}
