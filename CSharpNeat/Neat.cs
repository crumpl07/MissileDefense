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

        public int lengthOfdisjoint(Indiv parent1, Indiv parent2, int index, int networkSize)
        {
            int length = 0;
            while(index < networkSize)
            {
                if(parent1.Connections[index].InnovNum != parent2.Connections[index].InnovNum)
                {
                    length++;
                }
                else
                {
                    index = networkSize;
                }
                index++;
            }
            return length;

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
                    int disjointEnd = lengthOfdisjoint(parent1, parent2, i, networkSize) + i;
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
