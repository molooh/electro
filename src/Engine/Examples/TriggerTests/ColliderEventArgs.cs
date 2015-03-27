using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusee.Engine;

namespace Examples.TriggerTests
{
    public class ColliderEventArgs : EventArgs
    {
        private object _spColor;
        private object _colorParam;

        public ColliderEventArgs(Mesh _Cube1)
        {
            Console.WriteLine("Kollision!");
           
        }
        

    }
}
