using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpNeat
{



    /*
     * 
     * 
     * 
     * make a species class with a representer genome and then all the members of the species 
     * 
     * maybe move the mutation stuff into the species 
     * 
     * maybe also move the network eval into the species 
     * 
     * give neat class a species list
     * 
     * 
     * 
     * 
     */
    class Program
    {
        static void Main(string[] args)
        {
            /* testing new network module
            Node sense1 = new Node();
            sense1.NodeNum = 0;
            Node sense2 = new Node();
            sense2.NodeNum = 1;
            sense1.NodeType = NodeType.Sensor;
            sense2.NodeType = NodeType.Sensor;
            Node hidden1 = new Node();
            hidden1.NodeNum = 2;
            Node hidden2 = new Node();
            hidden2.NodeNum = 3;
            Node hidden3 = new Node();
            hidden3.NodeNum = 4;
            Node hidden4 = new Node();
            hidden4.NodeNum = 5;
            hidden1.NodeType = NodeType.Hidden;
            hidden2.NodeType = NodeType.Hidden;
            hidden3.NodeType = NodeType.Hidden;
            hidden4.NodeType = NodeType.Hidden;
            Node output1 = new Node();
            output1.NodeNum = 6;
            output1.NodeType = NodeType.Output;

            output1.PreviousLayerNeurons.Add(hidden4);
            output1.Weights.Add(1);
            hidden4.PreviousLayerNeurons.Add(hidden3);
            hidden4.Weights.Add(1);
            hidden3.PreviousLayerNeurons.Add(hidden2);
            hidden3.Weights.Add(1);
            hidden2.PreviousLayerNeurons.Add(hidden1);
            hidden2.Weights.Add(1);
            hidden1.PreviousLayerNeurons.Add(sense1);
            hidden1.Weights.Add(1);
            hidden2.PreviousLayerNeurons.Add(sense1);
            hidden2.Weights.Add(1);

            Network network = new Network();
            network.Nodes.Add(output1);
            network.Nodes.Add(hidden1);
            network.Nodes.Add(hidden2);
            network.Nodes.Add(hidden3);
            network.Nodes.Add(hidden4);
            network.Nodes.Add(sense1);
            network.Nodes.Add(sense2);

            

            //testing distance from sensor
            Console.WriteLine(network.distanceFromSensor(output1));

            double[] inputs = new double[2];
            inputs[0] = 1;
            inputs[1] = 1;

            Console.WriteLine(network.computeNetwork(inputs)[0]);

            Node test = new Node();
            test.Value = 100;
            */





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

            Neat neat = new Neat();
           
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

            //neat.computeFitness(inputs, expectedOutput);



            Indiv daBest;
            
            
           
           for(int i = 0; i < 50; i++)
           {
                neat.computeFitness(inputs, expectedOutput);
                neat.adjustPopFit();
                
                neat.speciateMate();
                
                neat.mutatePop();
                neat.computeFitness(inputs, expectedOutput);
                neat.Population = neat.Population.OrderBy(o => o.Fitness).ToList();
                //daBest = neat.Population[neat.Population.Count - 1];
                //Console.WriteLine(neat.Population.Count - 1);
                //Indiv daWorst = neat.Population[0];
                
                Console.WriteLine("Generation: " + i);
                Console.WriteLine("Average Number of Nodes: " + neat.averageNumNodes());
                Console.WriteLine("Average Number of Connections: " + neat.averageNumConnections());
                Console.WriteLine("Average Fitness: " + neat.averageFitness());
                Console.WriteLine();
                //Console.WriteLine(daBest.toString());
                //Console.WriteLine(neat.sumDifferenceOutputs(daBest, inputs, expectedOutput));
                //Console.WriteLine(daBest.Fitness);

                
                Console.WriteLine();
                //Console.WriteLine(daWorst.toString());
                //Console.WriteLine(daWorst.Fitness);
           }

           
          

        }
    }
}
