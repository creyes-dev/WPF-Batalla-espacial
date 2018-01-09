using System;
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
        public int PosicionVerticalPorDefecto;
        // contenedor de animaciones de la figura
        Path camino;
        Storyboard storyboard;
        bool enMovimiento;

        // componentes horizontales y verticales de la animación que sigue el camino
        DoubleAnimationUsingPath animacionEjeX;
        DoubleAnimationUsingPath animacionEjeY;

        IGeneradorCaminoVuelo generadorCaminos;

        public NaveEnemiga(string nombre, Canvas canvas, 
                            int posicionX, int posicionY, int ancho, int largo,
                            int posicionVerticalPorDefecto,
                            int duracionDesplazamiento = 5, 
                            int periodoInvisibilidad = 0, 
                            int periodoModoSigilo = 0,
                            int vidas = 1)
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            jugador = false;
            DuracionDesplazamiento = duracionDesplazamiento;
            generadorCaminos = new GeneradorCaminoVueloLineal();
            PosicionVerticalPorDefecto = posicionVerticalPorDefecto;
            PeriodoRecuperacionDisparo = 10;
            PeriodoInvisibilidad = periodoInvisibilidad;
            PeriodoModoSigilo = periodoModoSigilo;
            Vidas = vidas;
        }

        // Implementación del método abstracto para asociar las imagenes que 
        // se monstrarán en el canvas
        protected override void AsignarDirectoriosImagenes()
        {
            rutaRelativaImagenNave = "../Imagenes/player.png";
            rutaRelativaImagenDisparo = "../Imagenes/rayo1.png";
            rutaRelativaImagenDestruccion = "../Imagenes/player_explosion.png";
        }

        // Implementación del método abstracto para desplazar la nave
        public override void Desplazarse(ObjetosComunes.Direccion direccion)
        {
            AnimarDesplazamiento(direccion);
        }

        // Implementación del método abstracto para disparar
        public override void IniciarDisparo()
        {
            if (Estado == EstadoNave.ModoBatalla)
            {
                if (PeriodoDesdeUltimoDisparo >= PeriodoRecuperacionDisparo)
                {
                    // Obtener la localizacion del origen del disparo (punto medio de la nave)
                    int puntoInicioDisparoX = (int)(Posicion.PosicionX + (Dimenciones.Ancho / 2.0));
                    int puntoInicioDisparoY = (int)(Posicion.PosicionY + (Dimenciones.Largo / 2.0));

                    // TODO: El nombre del disparo se debe definir en la clase abstracta nave
                    Disparo disparo = new Disparo("Disparo" + numeroAlAzar.Next(0, 32199170).ToString(), this.Canvas, puntoInicioDisparoX, puntoInicioDisparoY, 7, 32, rutaRelativaImagenDisparo);
                    Disparos.Add(disparo);

                    PeriodoDesdeUltimoDisparo = 0;
                }
                PeriodoDesdeUltimoDisparo += 1;
            }
        }

        // Implementación de los pasos del TemplateMethod de dibujar
        
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
        protected override void Redibujar()
        {
            // TODO: Corregir, no me parece correcto que redibujar = Desplazarse
            AnimarDesplazamiento(Direccion.Derecha);
        }

        // 4. Mover los disparos
        protected override void MoverDisparos()
        {
            // La nave enemiga puede disparar en cualquier momento
            Disparar();

            foreach (Disparo disparo in Disparos)
            {
                disparo.Posicion.PosicionY += 5;
                disparo.Dibujarse();
            }
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
                    int limiteVertical = Convert.ToInt32(Canvas.Height) + Dimenciones.Largo;
                    PathGeometry caminoOnda = generadorCaminos.ObtenerCamino(posicionInicial, posicionFinal, limiteVertical);
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

                    enMovimiento = true;
                }
            }
        }

        private void AnimacionCompletada(object sender, EventArgs e)
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

            Posicion posicionInicial = new Posicion {
                PosicionX = posicionXAlien,
                PosicionY = posicionYAlien };

            int posicionXAlienFinal = 0;
            if (posicionXAlien == 0)
            {
                posicionXAlienFinal = Convert.ToInt32(this.Canvas.Width - Dimenciones.Ancho);
            }

            Posicion posicionFinal = new Posicion {
                PosicionX = posicionXAlienFinal,
                PosicionY = this.PosicionVerticalPorDefecto };
            
            Canvas.Children.Remove(camino);
            int limiteVertical = Convert.ToInt32(Canvas.Height) + Dimenciones.Largo;
            PathGeometry nuevoCamino = generadorCaminos.ObtenerCamino(posicionInicial, posicionFinal, limiteVertical);
            
            // El componente horizontal y vertical de la animación seguirá el nuevo camino generado
            animacionEjeX.PathGeometry = nuevoCamino;
            animacionEjeY.PathGeometry = nuevoCamino;

            // Iniciar la animación
            storyboard.Begin(Canvas, true);
        }

    }
}
