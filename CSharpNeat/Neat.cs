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

        public Indiv crossOver(Indiv parent1, Indiv parent2)
        {
            int networkSize = 0;
            Indiv temp = parent1;
            List<Connection> connections = new List<Connection>();

            if(parent1.Fitness > parent2.Fitness)
            {
                networkSize = parent1.Connections.Count;
            }
            if (parent2.Fitness > parent1.Fitness)
            {
                networkSize = parent2.Connections.Count;
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
                    if (parent1.Fitness > parent2.Fitness)
                    {
                        connections.Add(parent1.Connections[i]);
                    }
                    if (parent2.Fitness > parent1.Fitness)
                    {
                        connections.Add(parent2.Connections[i]);
                    }
                }

            }
            temp.Connections = connections;
            return temp;
        }

    }
}
