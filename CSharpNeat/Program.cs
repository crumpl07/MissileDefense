using System;
using System.Collections.Generic;

namespace CSharpNeat
{
    class Program
    {
        static void Main(string[] args)
        {
            Neat neat = new Neat();

            Indiv indiv = new Indiv(2, 1);
            Indiv indiv1 = new Indiv(2, 1);

            indiv.Fitness = 0;
            indiv1.Fitness = 10;

            int innovNum = 3;

            for (int i = 0; i < 5; i++)
            {
                
                innovNum = indiv.mutate(innovNum);
                innovNum = indiv1.mutate(innovNum);
                
            }

            Console.WriteLine(indiv.toString());
            Console.WriteLine(indiv1.toString());

            Indiv test = neat.crossOver(indiv, indiv1);

            Console.WriteLine(test.toString());
            

            
            
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
            


            

            neat.initializePop(100, 2, 1);
            

           
           for(int i = 0; i < 50; i++)
           {
                neat.computeFitness(inputs, expectedOutput);
                Console.WriteLine(neat.Population.Count);
                neat.adjustPopFit();
                Console.WriteLine(neat.Population.Count);
                neat.speciateMate();
                Console.WriteLine(neat.Population.Count);
                neat.mutatePop();
                Console.WriteLine(neat.Population.Count + " " + i);
           }


            Indiv daBest = neat.Population[neat.Population.Count - 1];
            daBest.assembleNetwork();
            Console.WriteLine(daBest.toString());


            Console.WriteLine(daBest.feedForward(neat.returnRow(inputs, 1))[0]);
            
        }
    }
}
