using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpNeat
{
    class Neat
    {
        private double compatThresh;
        private int innovationCount;
        private List<Indiv> population;
        private Random rand = new Random();
        
        
        internal List<Indiv> Population { get => population; set => population = value; }

        public Neat()
        {
            innovationCount = 0;
            compatThresh = 0.25;
            population = new List<Indiv>();
        }
        public Neat(double compatThresh)
        {
            innovationCount = 0;
            this.compatThresh = compatThresh;
            population = new List<Indiv>();
        }

        public int sharingFunction(Indiv indiv, Indiv indiv1, double compatThresh)
        {

            if (compareDistance(indiv, indiv1) < compatThresh)
            {
                return 1;
            }

            return 0;

        }

        //creates a pop of individuals with the given parameters
        public void initializePop(int pop, int numInputNodes, int numOutputNodes)
        {
            for (int i = 0; i < pop; i++)
            {
                population.Add(new Indiv(numInputNodes, numOutputNodes));
            }
        }

        public double[] returnRow(double[,] matrix, int row)
        {
            double[] temp = new double[matrix.GetLength(0)]; 
            for(int i = 0; i < matrix.GetLength(1); i++)
            {
                temp[i] = matrix[row, i];
            }
            return temp;
        }

        //computes the fitness of the given population
        public void computeFitness(double[,] inputs, double[,] expectedOutputs)
        {
            for(int i = 0; i < population.Count; i++)
            {

                double totalDiff = sumDifferenceOutputs(population[i], inputs, expectedOutputs);
                population[i].Fitness = Math.Pow(expectedOutputs.Length - totalDiff,2);
            }
            population.OrderBy(o => o.Fitness).ToList();
        }

        public double sumDifferenceOutputs(Indiv indiv, double[,] inputs, double[,] expectedOutputs)
        {
            double totalDiff = 0.0;
            
            for(int i = 0; i < inputs.GetLength(0); i++)
            {
                double[] temp = returnRow(inputs, i);
                List<double> outputs = indiv.feedForward(temp);

                for (int j = 0; j < expectedOutputs.GetLength(1); j++)
                {

                    //This is set to zero because that is the loc of the desired output vale
                    //Console.WriteLine("expected out: " + expectedOutputs[i, j] + " Actual out: " + outputs[j]);
                    //Console.WriteLine("i: " + i + " j: " + j);
                    totalDiff += Math.Abs(outputs[j] - expectedOutputs[i,j]);
                }
            }

            return totalDiff;
        }

        //adjusts the fitness of the population based on the equation from the paper
        public void adjustPopFit()
        {
            
            for(int i = 0; i < population.Count; i++)
            {
                population[i].Fitness = adjustedFitness(population[i], population, compatThresh);
            }
        }

        //divides the population into species then mates
        //percentMating is the percentage of the population with the highest fitness that will pass their genes on
        public void speciateMate()
        {
            int populationSize = population.Count;
            population.OrderBy(o => o.Fitness).ToList();

            List<Indiv> nextGen = new List<Indiv>();

            while (population.Count > 0)
            {
                Indiv popHead = population[population.Count - 1];
                List<Indiv> spec = speciesList(popHead, population);
                population.Remove(popHead);

                for (int i = 0; i < spec.Count; i++)
                {
                    nextGen.Add(crossOver(popHead,spec[i]));
                    population.Remove(spec[i]);
                }
            }
            

            population = nextGen;
        }

        //mutates the popluation and increments the innovation count
        public void mutatePop()
        {
            for(int i = 0; i < population.Count; i++)
            {
                if(rand.NextDouble() > .80)
                {
                    innovationCount = population[i].mutate(innovationCount);
                }                
            }
            
        }
            
        //returns a list of all of the individauls in the same species as the given indiv
        public List<Indiv> speciesList(Indiv speciesExample, List<Indiv> population)
        {
            List<Indiv> ret = new List<Indiv>();
            for(int i = 0; i < population.Count; i++)
            {
                if(sharingFunction(speciesExample,population[i],compatThresh) == 1)
                {
                    ret.Add(population[i]);
                }
            }
            return ret;
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
            
            
            
            while (index < parent1.Connections.Count && index < parent2.Connections.Count && parent1.Connections[index].InnovNum != parent2.Connections[index].InnovNum)
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

            pop.Add(indiv);

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
            
            
            //these are tuning variables. They let us change what charatiristics matter
            double c1 = 1.0;
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
                    numExcessGene += (double)lengthOfdisjoint(indiv1, indiv2, i);
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

            //Console.WriteLine("Excess Genes: " + numExcessGene);
            //Console.WriteLine("Disjoint Genes: " + numDisjointGene);
            //Console.WriteLine("Average Weight Difference: " + avgWeightDiff);
            //Console.WriteLine("Total Number of Genes: " + numExcessGene);


            distance = (c1 * numExcessGene) / totalNumGene + (c2 * numDisjointGene) / totalNumGene + c3 * avgWeightDiff;
            
            return distance;

        }

        public Indiv crossOver(Indiv parent1, Indiv parent2)
        {
            parent1.Connections.OrderBy(o => o.InnovNum).ToList();
            parent2.Connections.OrderBy(o => o.InnovNum).ToList();
            int networkSize = 0;
            Indiv child = new Indiv(parent1.NumInputNodes,parent2.NumOutputNodes);
            List<Connection> connections = new List<Connection>();
            Indiv fitParent = parent1;

            if (parent1.Fitness >= parent2.Fitness)
            {
                networkSize = parent1.Connections.Count;
                fitParent = parent1;
            }
            if (parent2.Fitness > parent1.Fitness)
            {
                networkSize = parent2.Connections.Count;
                fitParent = parent2;
            }
            

            for (int i = 0; i < networkSize; i++)
            {
                if (lengthOfdisjoint(parent1, parent2,i) == 0)
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
                if (lengthOfdisjoint(parent1, parent2, i) > 0)
                {
                    int disjointEnd = lengthOfdisjoint(parent1, parent2, i) + i;
                    //Console.WriteLine(disjointEnd);
                    while(i < disjointEnd && i < networkSize)
                    {
                        
                        connections.Add(fitParent.Connections[i]);
                        i++;
                    }
                }

            }
            
            child.Connections = connections;
            return child;
        }

    }
}
