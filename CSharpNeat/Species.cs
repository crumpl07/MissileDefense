using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNeat
{
    class Species {
        private Indiv representative;
        private List<Indiv> members;
        private double totalfitness;
        double c1 = 1; //These coefficients are gonna have to be played with until we get something that feels right.
        double c2 = 1;
        double c3 = 1;
        public Species(Indiv rep, double sizeImportance, double structureImportance, double weightImportance) {
            representative = rep;
            c1 = sizeImportance;
            c2 = structureImportance;
            c3 = weightImportance;
        }

        public Species(Indiv rep)
        {
            representative = rep;
        }
        
        public void calcFitness()
        {
            totalfitness = 0;
            for (int i = 0; i < members.Count; i++)
            {
                totalfitness += members[i].Fitness;
            }
        }

        public void distFitness()
        {
            for (int i = 0; i < members.Count; i++)
            {
                members[i].addMembership(totalfitness, members.Count);
            }
        }

        public Boolean isCompatible(Indiv toComp, double tolerance)
        {
            if (compDistance(toComp) > tolerance)
            {
                return false;
            } else {
                return true;
            }
        }

        public double compDistance(Indiv toComp)
        {//Code dealing with compatibility distance
            double weightDiffTotal = 0;
            double numExcess = 0;
            double numDisjoint = 0;
            int largerConnection = 0;
            int innovationLimit = 0;

            //find the larger number of connections between the networks
            if (toComp.getNumConnections() > representative.getNumConnections())
            {
                largerConnection = toComp.getNumConnections();
            }
            else
            {
                largerConnection = representative.getNumConnections();
            }

            //Find the last innovation number that can be considered matching or disjoint instead of excess.
            if (toComp.networkSize() < representative.networkSize())
            {
                innovationLimit = toComp.networkSize();
                numExcess = representative.networkSize() - innovationLimit;
            }
            else
            {
                innovationLimit = representative.networkSize();
                numExcess = toComp.networkSize() - innovationLimit;
            }

            //Find the number of disjoint connections and average weight difference.
            int weightComp = 0;
            for (int i = 0; i < innovationLimit; i++)
            {
                if (toComp.conIsInNetwork(i) && representative.conIsInNetwork(i))
                {
                    weightComp++;
                    weightDiffTotal += Math.Abs(toComp.getWeightOfConNum(i) - representative.getWeightOfConNum(i));
                }
                else
                {
                    numDisjoint++;
                }
            }
            weightDiffTotal = weightDiffTotal / ((double)weightComp);

            //The actual compatibility distance calculation after all of the variables have been found
            double compDistance = (c1 * numExcess / largerConnection) + (c2 * numDisjoint / largerConnection) + (c3 * weightDiffTotal);
            return compDistance;
        }

        public int getSize()
        {
            return members.Count;
        }

        public double getFitness()
        {
            return totalfitness;
        }

        public void addMember(Indiv addThis)
        {
            members.Add(addThis);
        }

        public Boolean hasMember(Indiv compThis)
        {
            return members.Contains(compThis);
        }
    }
}
