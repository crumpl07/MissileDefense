using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpNeat
{
    class Program
    {
        static void Main(string[] args)
        {
            Neat neat = new Neat();
            Indiv indiv = new Indiv(2, 1);

            Indiv indiv1 = new Indiv(2, 1);

            double[] test = { 0, 0 };


            
            
            
            /*
            //Testing lengthOfDisjoint() fuction

            Node temp1 = new Node();
            Node temp2 = new Node();

            Connection c1 = new Connection(temp1, temp2, 0, 0);
            Connection c2 = new Connection(temp1, temp2, 2, 1);
            Connection c3 = new Connection(temp1, temp2, 3, 2);
            Connection c4 = new Connection(temp1, temp2, 4, 3);
            Connection c5 = new Connection(temp1, temp2, 5, 4);
            Connection c6 = new Connection(temp1, temp2, 6, 5);
            Connection c7 = new Connection(temp1, temp2, 7, 6);
            Connection c8 = new Connection(temp1, temp2, 8, 7);
            Connection c9 = new Connection(temp1, temp2, 9, 8);

            List<Connection> cons = new List<Connection>();
            List<Connection> cons1 = new List<Connection>();

            cons.Add(c1);
            cons.Add(c2);
            cons.Add(c3);
            cons.Add(c8);
            cons.Add(c7);
            cons.Add(c2);
            cons.Add(c3);
            cons.Add(c8);

            cons1.Add(c4);
            cons1.Add(c5);
            cons1.Add(c6);
            cons1.Add(c9);
            cons1.Add(c7);
            cons1.Add(c5);
            cons1.Add(c6);
            cons1.Add(c9);

            indiv.Connections = cons;
            indiv1.Connections = cons1;

            Console.WriteLine("length of the disjoint: " + neat.lengthOfdisjoint(indiv, indiv1, 5));


            */

            /*

            //testing the assemble network and feed forward

            Node midNode = new Node(3, NodeType.Hidden);
            Connection c1 = new Connection(indiv.Nodes[0], midNode,1,2);
            Connection c2 = new Connection(midNode, indiv.Nodes[2], 1, 3);

            indiv.Connections.Add(c1);
            indiv.Connections.Add(c2);
            indiv.Nodes.Add(midNode);
                  
            indiv.Connections[0].Weight = 1;
            indiv.Connections[1].Weight = 1;
            Console.WriteLine(indiv.toString());

            Console.WriteLine(indiv.feedForward(test)[0]);

            //Console.WriteLine(indiv.Nodes[2].inputsToString());

            */


            /*
            for (int i = 0; i < 10; i++)
            {
                indiv.mutate(indiv.Nodes.Count);                
            }

            List<Node> nodeOrder = indiv.nodeOrder();

            Console.WriteLine(indiv.toString());
            Console.WriteLine();
            
            for(int i = 0; i < nodeOrder.Count; i++)
            {
                Console.WriteLine(nodeOrder[i].toString());
            }
            

            */
            //testing crossover
            /*
            for(int i = 0; i < 10; i++)
            {
                indiv.mutate(indiv.Nodes.Count);
                indiv1.mutate(indiv1.Nodes.Count);
            }

            Console.WriteLine(indiv.toString());
            Console.WriteLine(indiv1.toString());

            indiv.Fitness = 10;
            indiv1.Fitness = 0;

            Indiv child = neat.crossOver(indiv, indiv1);
            Console.WriteLine(child.toString());
            */

            //Console.WriteLine(indiv.toString());

            
            neat.initializePop(100, 2, 1);
            double[,] inputs = { 
                {0,0},
                {0,1},
                {1,0},
                {1,1}
            };
            double[,] expectedOutput = { 
                { 0 }, 
                { 1 }, 
                { 1 }, 
                { 0 } 
            };

            neat.computeFitness(inputs, expectedOutput);
            

            

            
            
           
           for(int i = 0; i < 50; i++)
           {
                neat.computeFitness(inputs, expectedOutput);
                
                neat.adjustPopFit();
               
                neat.speciateMate();
                
                neat.mutatePop();

                neat.computeFitness(inputs, expectedOutput);
                neat.Population = neat.Population.OrderBy(o => o.Fitness).ToList();
                Indiv daBest = neat.Population[neat.Population.Count - 1];
                Indiv daWorst = neat.Population[0];
                daBest.assembleNetwork();
                Console.WriteLine("Generation: " + i);
                Console.WriteLine("Average Number of Nodes: " + neat.averageNumNodes());
                Console.WriteLine("Average Number of Connections: " + neat.averageNumConnections());
                Console.WriteLine("Average Fitness: " + neat.averageFitness());
                Console.WriteLine(daBest.toString());
                //Console.WriteLine(neat.sumDifferenceOutputs(daBest, inputs, expectedOutput));
                Console.WriteLine(daBest.Fitness);

                Console.WriteLine(daWorst.toString());
                Console.WriteLine(daWorst.Fitness);
            }

            




        }
    }
}
