using System;
using System.Collections.Generic;

namespace CSharpNeat
{
    class Program
    {
        Random rand;
        static void Main(string[] args)
        {
            Program sample = new Program();
            sample.XorTesting();

            /**
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
            */
        }
        private void XorTesting(){
            rand = new Random();
            Neat manager = new Neat(rand);
            List<Indiv> members = new List<Indiv>();
            double topacc = 0;
            double popSize = 20;
            double tolerance = -1;//.07;
            double[] dataa = { 0, 0 };
            double[] datab = { 0, 1 };
            double[] datac = { 1, 0 };
            double[] datad = { 1, 1 };
            double[][] dataToFeed = { dataa, datab, datac, datad };
            double[] expected = { 0, 1, 1, 0 };
            for (int i = 0; i < popSize; i++)
            {
                Indiv temp = new Indiv(2, 1, rand);
                members.Add(temp);
            }
            while (topacc < 0.9)
            {
                double toterror = 0;
                for (int i = 0; i < members.Count; i++)
                {
                    double error = 0;
                    for (int ii = 0; ii < 4; ii++)
                    {
                        double tempvariable = members[i].feedForward(dataToFeed[ii])[0];
                        error += Math.Abs(expected[ii] - tempvariable);
                    }
                    toterror += error;
                    members[i].Fitness = 1 - (error / 4.0);
                    if (error/4.0 > topacc)
                    {
                        topacc = error / 4.0;
                    }
                }
                Console.WriteLine("Current Error: " + toterror);
                List<Species> species = new List<Species>();
                Species starter = new Species(members[0]);
                species.Add(starter);
                for (int i = 1; i < members.Count; i++) //First create every needed species
                {
                    Boolean flag = true;
                    for (int ii = 0; ii < species.Count; ii++)
                    {
                        if (species[ii].isCompatible(members[i], tolerance))
                        {
                            species[ii].addMember(members[i]);
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        species.Add(new Species(members[i]));
                    }
                }
                Console.WriteLine("Total number of species: " + species.Count);
                Console.WriteLine("Total number of netorks: " + members.Count);
                for (int i = 0; i < members.Count; i++) //Then look for dual memberships
                {
                    for (int ii = 0; ii < species.Count; ii++)
                    {
                        if (!species[ii].hasMember(members[i]) && species[ii].isCompatible(members[i], tolerance))
                        {
                            species[ii].addMember(members[i]);
                        }
                    }
                }
                for (int i = 0; i < species.Count; i++) //distribute fitness scores from each species
                {
                    species[i].calcFitness();
                    species[i].distFitness();
                }
                for (int i = 0; i < members.Count; i++) //calculate adjusted fitness scores
                {
                    members[i].compAdjustedFitness();
                    Console.WriteLine("Network " + i + " fitness: " + members[i].Fitness);
                }
                List<Indiv> newList = new List<Indiv>();
                for (int i = 0; i < members.Count; i++) //make a new generation
                {
                    Indiv parentA = doubleElim(members);
                    Indiv parentB = doubleElim(members);
                    Indiv temp = manager.crossOver(parentA, parentB);
                    newList.Add(temp);
                }
                manager.mutatePop(newList);
                members = newList;
            }
            Console.WriteLine("Click enter to close the window...");
            Console.ReadLine();
        }

        private Indiv doubleElim(List<Indiv> population)
        {
            int[] suitors = {rand.Next(population.Count),
            rand.Next(population.Count),
            rand.Next(population.Count),
            rand.Next(population.Count), 0, 0};
            if (population[suitors[0]].Fitness > population[suitors[1]].Fitness)
            {
                suitors[4] = suitors[0];
            } 
            else 
            {
                suitors[4] = suitors[1];
            }
            if (population[suitors[2]].Fitness > population[suitors[3]].Fitness)
            {
                suitors[5] = suitors[2];
            }
            else
            {
                suitors[5] = suitors[3];
            }
            if (population[suitors[4]].Fitness > population[suitors[5]].Fitness)
            {
                return population[suitors[4]];
            }
            else
            {
                return population[suitors[5]];
            }
        }
    }
}
