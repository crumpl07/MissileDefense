using System;
using System.Collections.Generic;

namespace CSharpNeat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Indiv indiv = new Indiv(10, 10);

            double[] inputs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            List<double> outputs = new List<double>();

            //Console.WriteLine(indiv.Connections.Count);

            for (int i = 0; i < indiv.Connections.Count; i++)
            {
                Console.WriteLine(indiv.Connections[i].toString());
            }

            Console.WriteLine(indiv.networkSize());

            indiv.assembleNetwork();
            for (int i = 0; i < indiv.Nodes.Count; i++)
            {
                Console.WriteLine(indiv.Nodes[i].NodeType + " " + indiv.Nodes[i].NodeNum);
            }

            outputs = indiv.feedForward(inputs);
            for (int i = 0; i < outputs.Count; i++)
            {
                Console.WriteLine(outputs[i]);
            }


        }
    }
}
