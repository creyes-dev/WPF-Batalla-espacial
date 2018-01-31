using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPF_BatallaEspacial.Graficos;
using WPF_BatallaEspacial.ObjetosComunes;

namespace WPF_BatallaEspacial.Elementos.Disparos
{
    public class Cañon : ElementoDibujable
    {
        Direccion Direccion;
        public List<Disparo> Disparos { get; set; }
        Random numeroAlAzar = new Random();

        public int PosicionHorizontalRelativa { get; set; }
        public int CantidadDisparos { get { return Disparos.Count; } }
        
        public int PeriodoDesdeUltimoDisparo;
        public int PeriodoEsperaNuevoDisparoDesde;
        public int PeriodoEsperaNuevoDisparoHasta;
        public int Velocidad;

        private int PeriodoEsperaNuevoDisparo;
        private string rutaAbsolutaAnimacionDisparo;
        private int nroDisparo;

        private int incrementosVerticalesSucesivos;
        private int incrementoVerticalAnterior;
        private int incrementoHorizontalAnterior;

        public Cañon(string nombre, Canvas canvas, 
                    int posicionX, int posicionY, int ancho, int largo,
                    int posicionRelativa, string rutaAbsolutaSpriteDisparo, Direccion direccion, 
                    int periodoEsperaDesde = 0, int periodoEsperaHasta = 0, int velocidad = 5) 
            : base(nombre, canvas, posicionX, posicionY, ancho, largo)
        {
            rutaAbsolutaAnimacionDisparo = rutaAbsolutaSpriteDisparo;
            Disparos = new List<Disparo>();
            Direccion = direccion;
            PosicionHorizontalRelativa = posicionRelativa;

            PeriodoEsperaNuevoDisparoDesde = periodoEsperaDesde;
            PeriodoEsperaNuevoDisparoHasta = periodoEsperaHasta;
            Velocidad = velocidad;
        }

        public bool DisparoDisponible {
            get
            {
                return (PeriodoDesdeUltimoDisparo > PeriodoEsperaNuevoDisparo);
            } 
        }
        
        public void IniciarDisparo()
        {
            if (DisparoDisponible)
            {
                string nombreNuevoDisparo = Nombre + "_Disparo_" + nroDisparo.ToString();
                Disparo disparo = new Disparo(nombreNuevoDisparo, this.Canvas, Posicion.PosicionX, Posicion.PosicionY, 7, 32, rutaAbsolutaAnimacionDisparo);
                this.Disparos.Add(disparo);

                AprontarseParaNuevoDisparo();
            }
        }

        // Dibuja disparos y elimina disparos fuera de rango
        public override void Dibujarse()
        {
            if (Disparos.Count > 0)
            {
                int incrementoVertical = 0;
                int incrementoHorizontal = 0;

                List<Disparo> disparosFueraRango = new List<Disparo>();

                if (this.Direccion == ObjetosComunes.Direccion.Superior)
                {
                    incrementoVertical = Velocidad * -1;
                }
                else
                {
                    if (this.Direccion == ObjetosComunes.Direccion.Inferior)
                    {
                        incrementoVertical = Velocidad;
                    }
                    else
                    {
                        if (this.Direccion == ObjetosComunes.Direccion.InferiorIzquierda)
                        {
                            if (incrementosVerticalesSucesivos == 1)
                            {
                                incrementoVertical = 0;
                                incrementoHorizontal = -1;
                                incrementosVerticalesSucesivos = 0;
                            }
                            else
                            {
                                incrementoVertical = Velocidad * 2;
                                incrementosVerticalesSucesivos += 1;
                            }
                        }
                        else
                        {
                            if (this.Direccion == ObjetosComunes.Direccion.InferiorDerecha)
                            {
                                if (incrementosVerticalesSucesivos == 1)
                                {
                                    incrementoVertical = 0;
                                    incrementoHorizontal = 1;
                                    incrementosVerticalesSucesivos = 0;
                                }
                                else
                                {
                                    incrementoVertical = Velocidad * 2;
                                    incrementosVerticalesSucesivos += 1;
                                }
                            }
                        }

                    }
                }

                foreach (Disparo disparo in Disparos)
                {
                    // Mover disparo
                    disparo.Posicion.PosicionY += incrementoVertical;
                    disparo.Posicion.PosicionX += incrementoHorizontal;
                    disparo.Dibujarse();

                    if (disparo.Posicion.PosicionY < 0 - disparo.Dimenciones.Largo || disparo.Posicion.PosicionY > Canvas.Height + disparo.Dimenciones.Largo)
                    {
                        Canvas.Children.Remove(disparo.SpriteSheet);
                        disparosFueraRango.Add(disparo);
                    }
                }

                foreach (Disparo disparoFueraRango in disparosFueraRango)
                {
                    Disparos.Remove(disparoFueraRango);
                    disparoFueraRango.Removerse();
                }
            }

            PeriodoDesdeUltimoDisparo += 1;
        }

        // Actualiza el periodo desde el ultimo disparo y actualiza
        // el periodo de espera hasta el proximo disparo
        public void AprontarseParaNuevoDisparo()
        {
            PeriodoDesdeUltimoDisparo = 0;
            nroDisparo += 1;

            if (PeriodoEsperaNuevoDisparoDesde == PeriodoEsperaNuevoDisparoHasta)
            {
                PeriodoEsperaNuevoDisparo = PeriodoEsperaNuevoDisparoDesde;
            }
            else
            {
                PeriodoEsperaNuevoDisparo = numeroAlAzar.Next(PeriodoEsperaNuevoDisparoDesde, PeriodoEsperaNuevoDisparoHasta);
            }
        }

    }
}
