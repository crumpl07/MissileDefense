using System;
using System.Collections.Generic;

namespace CSharpNeat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Indiv indiv = new Indiv(2, 1);
            Indiv indiv1 = new Indiv(2, 1);
            Neat neat = new Neat();

            indiv.assembleNetwork();
            indiv.mutate(indiv.Connections.Count + 1);
            indiv1.mutate(indiv1.Connections.Count + 1);
            indiv1.assembleNetwork();

            Console.WriteLine(neat.compareDistance(indiv, indiv1));

        }
    }
}
