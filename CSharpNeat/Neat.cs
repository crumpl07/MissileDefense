using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpNeat
{
    class Neat
    {
        private int innovationCount;
        private List<Indiv> population;
        private Random rand = new Random();

        public Neat()
        {
            innovationCount = 0;
        }

        public int sharingFunction(Indiv indiv,Indiv indiv1, double compatThresh)
        {
            int count = 0;

            if(compareDistance(indiv,indiv1) > compatThresh)
            {
                count++;
            }
        
            return count;

        }

        public int lengthOfdisjoint(Indiv parent1, Indiv parent2, int index)
        {
            int length = 0;
            int longParent = 0;
            int shortParent = 0;
            if(parent1.Connections.Count > parent2.Connections.Count)
            {
                longParent = parent1.Connections.Count;
                shortParent = parent2.Connections.Count;
            }
            else
            {
                longParent = parent2.Connections.Count;
                shortParent = parent1.Connections.Count;
            }
            if(index >= shortParent && index <= longParent)
            {
                length = longParent - shortParent;
                return length;
            }
            
            while(parent1.Connections[index].InnovNum != parent2.Connections[index].InnovNum)
            {
                length++;
                index++;
            }

            return length;

        }

        public double adjustedFitness(Indiv indiv, List<Indiv> pop, double compatThresh)
        {
            

            int numInSpecies = 0;

            pop.Remove(indiv);

            for(int i = 0; i < pop.Count; i++)
            {
                numInSpecies += sharingFunction(indiv, pop[i], compatThresh);
            }

            double adjustedFit = indiv.Fitness / (double) numInSpecies;
            return adjustedFit;
        }

        public double compareDistance(Indiv indiv1, Indiv indiv2)
        {
            double distance = 0;
            double totalNumGene = 1;
            double numExcessGene = 0;
            double numDisjointGene = 0;
            double avgWeightDiff = 0;
            double c1 = 1.0;//these are tuning variables. They let us change the size of the species and what charatiristics matter
            double c2 = 1.0;
            double c3 = 1.0;

            if(indiv1.Connections.Count > indiv2.Connections.Count)
            {
                totalNumGene = indiv1.Connections.Count;
            }
            else
            {
                totalNumGene = indiv2.Connections.Count;
            }

            for(int i = 0; i < totalNumGene; i++)
            {
                if(lengthOfdisjoint(indiv1, indiv2, i) + i == totalNumGene)
                {
                    numExcessGene = (double)lengthOfdisjoint(indiv1, indiv2, i);
                    i = (int)totalNumGene;
                }
                else
                {
                    numDisjointGene += (double)lengthOfdisjoint(indiv1, indiv2, i);
                }
                
            }
            int count = 1;
            double totalweightDiff = 0;

            for(int i = 0; i < indiv1.Connections.Count; i++)
            {
                if(lengthOfdisjoint(indiv1,indiv2,i) == 0)
                {
                    totalweightDiff += Math.Abs(indiv1.Connections[i].Weight - indiv2.Connections[i].Weight);
                    count++;
                }
            }
            avgWeightDiff = totalweightDiff / (double)count;

            Console.WriteLine("Excess Genes: " + numExcessGene);
            Console.WriteLine("Disjoint Genes: " + numDisjointGene);
            Console.WriteLine("Average Weight Difference: " + avgWeightDiff);
            Console.WriteLine("Total Number of Genes: " + numExcessGene);


            distance = (c1 * numExcessGene) / totalNumGene + (c2 * numDisjointGene) / totalNumGene + c3 * avgWeightDiff;
            
            return distance;

        }

        public Indiv crossOver(Indiv parent1, Indiv parent2)
        {
            int networkSize = 0;
            Indiv temp = parent1;
            List<Connection> connections = new List<Connection>();
            Indiv fitParent;

            if (parent1.Fitness > parent2.Fitness)
            {
                networkSize = parent1.Connections.Count;
                fitParent = parent1;
            }
            if (parent2.Fitness > parent1.Fitness)
            {
                networkSize = parent2.Connections.Count;
                fitParent = parent2;
            }
            else
            {
                fitParent = parent1;
            }

            for (int i = 0; i < networkSize; i++)
            {
                if (parent1.Connections[i].InnovNum == parent2.Connections[i].InnovNum)
                {
                    if (rand.NextDouble() > .5)
                    {
                        connections.Add(parent1.Connections[i]);
                    }
                    else
                    {
                        connections.Add(parent2.Connections[i]);
                    }
                }
                if (parent1.Connections[i].InnovNum != parent2.Connections[i].InnovNum)
                {
                    int disjointEnd = lengthOfdisjoint(parent1, parent2, i) + i;
                    while(i < disjointEnd)
                    {
                        connections.Add(fitParent.Connections[i]);
                        i++;
                    }
                }

            }
            temp.Connections = connections;
            return temp;
        }

    }
}
