using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpNeat
{
    class Indiv
    {

        private List<Node> nodes = new List<Node>();
        private List<Connection> connections = new List<Connection>();

        public Indiv()
        {

        }

        public void feedForward(double[] inputs)
        {
            Console.WriteLine("You are now feeding forward");
        }

    }
}
