using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF_BatallaEspacial.Elementos
{
    public abstract class ElementoDibujable : ObjetosComunes.ObjetoJuego
    {
        public string Nombre { get; set; }
        protected Canvas Canvas { get; set; }
        protected UIElement elementoDibujable;

        public ElementoDibujable(string nombre, Canvas canvas, 
            int posicionX, int posicionY, int ancho, int largo)
            : base(posicionX, posicionY, ancho, largo)
        {
            Nombre = nombre;
            Canvas = canvas;
        }

        public abstract void Dibujarse();
        
        public void Removerse()
        {
            Canvas.Children.Remove(elementoDibujable);
        }
                
    }
}


