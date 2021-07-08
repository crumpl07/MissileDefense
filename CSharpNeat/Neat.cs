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

        public double averageNumNodes()
        {
            double ret = 0.0;
            int nodeSum = 0;

            for (int i = 0; i < population.Count; i++)
            {
               nodeSum += population[i].Nodes.Count;
            }

            ret = (double)nodeSum / (double)population.Count;

            return ret;
        }

        public double averageNumConnections()
        {
            double ret = 0.0;
            int connectionSum = 0;

            for (int i = 0; i < population.Count; i++)
            {
                connectionSum += population[i].Connections.Count;
            }

            ret = (double)connectionSum / (double)population.Count;

            return ret;
        }

        public double averageFitness()
        {
            double ret = 0.0;
            double fitnessSum = 0;

            for (int i = 0; i < population.Count; i++)
            {
                fitnessSum += population[i].Fitness;
            }

            ret = fitnessSum / (double)population.Count;

            return ret;
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
                //Console.WriteLine(population[i].Fitness);
            }
            population = population.OrderBy(o => o.Fitness).ToList();
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
                    //Console.WriteLine("output: " + outputs[j] + " expected output: " + expectedOutputs[i, j]);
                    //Console.WriteLine(totalDiff);
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
            population = population.OrderBy(o => o.Fitness).ToList();

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
            double mutationProbability = .8;
            for(int i = 0; i < population.Count; i++)
            {
                if(rand.NextDouble() > mutationProbability)
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


        //Returns the length of the disjoint including the specified index 
        //lengthOfDisjoint returning a 0 means the innovnums at the specified index are the same
        public int lengthOfdisjoint(Indiv parent1, Indiv parent2, int index)
        {
            int length = 0;
            Indiv longParent;
            Indiv shortParent;
            if(parent1.Connections.Count > parent2.Connections.Count)
            {
                longParent = parent1;
                shortParent = parent2;
            }
            else
            {
                longParent = parent2;
                shortParent = parent1;
            }
            
            //Console.WriteLine("length of the longer Parent: " + longParent.Connections.Count + " length of the shorter parent: " + shortParent.Connections.Count);
            
            
            for(int i = index; i < longParent.Connections.Count; i++)
            {
                if(i >= shortParent.Connections.Count)
                {
                    length += longParent.Connections.Count - shortParent.Connections.Count;
                    return length;
                }
                else
                {
                    if(longParent.Connections[i].InnovNum != shortParent.Connections[i].InnovNum)
                    {
                        length++;
                    }
                    else
                    {
                        return length;
                    }
                }
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
            //Console.WriteLine(numInSpecies);

            pop.Add(indiv);

            double adjustedFit = indiv.Fitness / (double) numInSpecies;
            return adjustedFit;


        }


        //smaller number means the indivs are more closely related
        public double compareDistance(Indiv indiv1, Indiv indiv2)
        {
            double distance = 0;
            double totalNumGene = 1;
            double numExcessGene = 0;
            double numDisjointGene = 0;
            double avgWeightDiff = 0;
            
            
            //these are tuning variables. They let us change what charatiristics matter
            double c1 = 10.0;
            double c2 = 1.0;
            double c3 = 3.0;

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
            int count = 0;
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

        //this funciton is killing the node list
        public Indiv crossOver(Indiv parent1, Indiv parent2)
        {
            int networkSize = 0;
            Indiv child = new Indiv(parent1.NumInputNodes,parent2.NumOutputNodes);
            List<Connection> connections = new List<Connection>();
            List<Node> nodes = new List<Node>();
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

            for(int i = 0; i < connections.Count; i++)
            {
                if (!isInNetwork(nodes, connections[i].InNode))
                {
                    nodes.Add(fitParent.Nodes[fitParent.indexOfNode(connections[i].InNode)]);
                }
                if (!isInNetwork(nodes, connections[i].OutNode))
                {
                    nodes.Add(fitParent.Nodes[fitParent.indexOfNode(connections[i].OutNode)]);
                }
            }
            

            child.Connections = connections;
            nodes = nodes.OrderBy(o => o.NodeNum).ToList();
            child.Nodes = nodes;
            return child;
        }

        public Boolean isInNetwork(List<Node> nodes, Node nodeNum)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].NodeNum == nodeNum.NodeNum)
                    return true;
            }
            return false;
        }

    }
}
