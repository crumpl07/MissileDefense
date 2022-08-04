using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpNeat
{
    class Species
    {

        private Indiv headIndiv;
        private List<Indiv> speciesList;
        private Random rand = new Random();

        public Species()
        {
            speciesList = new List<Indiv>();
        }

        public Species(Indiv headIndiv)
        {
            this.headIndiv = headIndiv;
            speciesList = new List<Indiv>();

        }

        public void mateSpecies(double populationMatingPorportion)
        {
            speciesList = speciesList.OrderBy(o => o.Fitness).ToList();
            int lowerBound = (int)(speciesList.Count - speciesList.Count * populationMatingPorportion);
            int upperBound = speciesList.Count;
            if((upperBound - lowerBound % 2) == 0)
            {
                lowerBound -= 1;
            }

            int populationBalanceFactor = lowerBound - upperBound;
            int midPoint = (populationBalanceFactor / 2) + lowerBound;
            List<Indiv> newSpeciesList = new List<Indiv>();

            for (int i = 0; i < midPoint; i++)
            {
                newSpeciesList.Add(crossOver(speciesList[lowerBound + i], speciesList[upperBound - i - 1]));
            }
        }

        public Indiv crossOver(Indiv parent1, Indiv parent2)
        {
            int networkSize = 0;
            Indiv child = new Indiv(parent1.NumInputNodes, parent2.NumOutputNodes);
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
                if (lengthOfdisjoint(parent1, parent2, i) == 0)
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
                    while (i < disjointEnd && i < networkSize)
                    {
                        connections.Add(fitParent.Connections[i]);
                        i++;
                    }
                }

            }

            for (int i = 0; i < connections.Count; i++)
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

        public int lengthOfdisjoint(Indiv parent1, Indiv parent2, int index)
        {
            int length = 0;
            Indiv longParent;
            Indiv shortParent;
            if (parent1.Connections.Count > parent2.Connections.Count)
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


            for (int i = index; i < longParent.Connections.Count; i++)
            {
                if (i >= shortParent.Connections.Count)
                {
                    length += longParent.Connections.Count - shortParent.Connections.Count;
                    return length;
                }
                else
                {
                    if (longParent.Connections[i].InnovNum != shortParent.Connections[i].InnovNum)
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

        internal List<Indiv> SpeciesList { get => speciesList; set => speciesList = value; }


    }
}
