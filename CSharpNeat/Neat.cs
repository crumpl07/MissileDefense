using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpNeat
{
    class Neat
    {
        private double compatThresh;
        private List<Species> species;
        private List<Indiv> population;
        private Random rand = new Random();
        private List<Connection> innovationList;
        private int targetNumberSpecies;
        private readonly int speciesPopulationFactor = 10;
        private readonly double populationMatingPorportion = 0.4;
        private readonly double changeTopologyProb = 0.01;
        
        internal List<Indiv> Population { get => population; set => population = value; }
        public int TargetNumberSpecies { get => targetNumberSpecies; set => targetNumberSpecies = value; }

        public Neat(int numInputNodes, int numOutputNodes, int targetNumberSpecies)
        {
            this.population = new List<Indiv>();
            this.targetNumberSpecies = targetNumberSpecies;
            this.species = new List<Species>();
            this.innovationList = new List<Connection>();
            initializePop(targetNumberSpecies * speciesPopulationFactor, numInputNodes, numInputNodes);
        }
        public Neat(int numInputNodes, int numOutputNodes, int targetNumberSpecies, double compatThresh)
        {
            this.compatThresh = compatThresh;
            this.population = new List<Indiv>();
            this.targetNumberSpecies = targetNumberSpecies;
            this.species = new List<Species>();
            this.innovationList = new List<Connection>();
            initializePop(targetNumberSpecies * speciesPopulationFactor, numInputNodes, numInputNodes);
        }


        public Network train(List<DataSet> data)
        {
            Console.WriteLine("Begining training");
            Console.WriteLine("Training on " + data.Count + " datasets");

            foreach (DataSet d in data)
            {
                computePopulationFitness(d);
                Console.WriteLine("Average Population Fitness for this dataset: " + averageFitness());
                speciateTargetSpecies();
                Console.WriteLine("Target Species Number: " + targetNumberSpecies + " Actual Species Number: " + species.Count);
                mate();
                Console.WriteLine("Mated Population, population count: " + population.Count);
                mutatePopulation();
                Console.WriteLine("Mutated Population");
            }

            computePopulationFitness(data[0]);

            return new Network(population[population.Count - 1]);


        }

        public void mutatePopulation()
        {
            foreach(Indiv i in population)
            {
                //Console.WriteLine("Mutating an Indiv");
                i.mutateWeights();
                if(rand.NextDouble() < changeTopologyProb)
                {
                    innovationList.AddRange(i.mutateStructure(innovationList.Count));
                }
            }


        }

        public double averageDistanceFromRandomIndiv()
        {
            double distanceSum = 0;
            Indiv testIndiv = population[rand.Next(population.Count)];
            foreach (Indiv i in population)
            {
                distanceSum += compareDistance(testIndiv, i);
            }

            return distanceSum / population.Count;
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

        public int sharingFunction(Indiv indiv, Indiv indiv1)
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
            population = new List<Indiv>();
            for (int i = 0; i < pop; i++)
            {
                population.Add(new Indiv(numInputNodes, numOutputNodes));
            }

            innovationList = population[0].Connections;
        }


        //computes the fitness of the given population
        public void computePopulationFitness(DataSet d)
        {
            for(int i = 0; i < population.Count; i++)
            {
                double totalDiff = sumDifferenceOutputs(population[i], d.Inputs, d.TargetOutputs);
                population[i].Fitness = Math.Pow(d.TargetOutputs.Length - totalDiff,2);
                //Console.WriteLine(population[i].Fitness);
            }
            population = population.OrderBy(o => o.Fitness).ToList();
        }
   
        public double sumDifferenceOutputs(Indiv indiv, double[] inputs, double[] expectedOutputs)
        {
            double totalDiff = 0.0;
            Network net = new Network(indiv);
            double[] outputs = net.computeNetwork(inputs);

            for(int i = 0; i < expectedOutputs.Length; i++)
            {
                totalDiff += Math.Abs(outputs[i] - expectedOutputs[i]);
            }

            return totalDiff;
        }


        //adjusts the fitness of the population based on the equation from the paper
        

        public List<Indiv> removeList(List<Indiv> pop,List<Indiv> remove)
        {
            for(int i = 0; i < remove.Count(); i++)
            {
                pop.Remove(remove[i]);
            }
            return pop;
        }

        //divides the population into species then mates
        //percentMating is the percentage of the population with the highest fitness that will pass their genes on
        //this eleminates all but the best species

        public void mate()
        {
            adjustPopulationFitness();

            foreach(Species s in species)
            {
                s.mateSpecies(populationMatingPorportion);
            }
            population = new List<Indiv>();
            foreach (Species s in species)
            {
                population.AddRange(s.SpeciesList);
            }


        }

        public void speciate()
        {
            List<Indiv> tempPopList = population.ToList();
            while (tempPopList.Count >0)
            {
                Indiv example = tempPopList[rand.Next(tempPopList.Count)];
                Species temp = new Species(example);
                temp.SpeciesList = speciesList(example,tempPopList);
                species.Add(temp);
            }
        }

        //wip 
        public void speciateTargetSpecies()
        {
            compatThresh = averageDistanceFromRandomIndiv() / targetNumberSpecies;
            List<Indiv> tempPopList = population.ToList();
            while (tempPopList.Count > 0)
            {
                Indiv example = tempPopList[rand.Next(tempPopList.Count)];
                Species temp = new Species(example);
                temp.SpeciesList = speciesList(example, tempPopList);
                species.Add(temp);
            }



        }

        //mutates the popluation and increments the innovation count
            
        //returns a list of all of the individauls in the same species as the given indiv
        public List<Indiv> speciesList(Indiv speciesExample, List<Indiv> population)
        {
            List<Indiv> ret = new List<Indiv>();
            for(int i = 0; i < population.Count; i++)
            {
                if(sharingFunction(speciesExample,population[i]) == 1)
                {                    
                    ret.Add(population[i]);
                    population.RemoveAt(i);
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

        public void adjustPopulationFitness()
        {
            for(int j = 0; j < species.Count; j++)
            {
                for (int i = 0; i < species[j].SpeciesList.Count; i++)
                {
                    adjustedFitness(species[j].SpeciesList[i], species[j]);
                }
            }
            
        }

        public void adjustedFitness(Indiv indiv, Species spec)
        {
            double fitnessCoeff = 1.0;
            indiv.Fitness = (indiv.Fitness / ((double)spec.SpeciesList.Count)) * fitnessCoeff;

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

            //Console.WriteLine("Distance between the given indivs: " + distance);

            return distance;

        }

    }
}
