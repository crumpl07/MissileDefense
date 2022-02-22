using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpNeat
{
    class Network
    {
        
        private List<Node> nodes;
        private Indiv indiv;
        private int numberOutputNodes;

        internal List<Node> Nodes { get => nodes; set => nodes = value; }
        internal Indiv Indiv { get => indiv; set => indiv = value; }
        public int NumberOutputNodes { get => numberOutputNodes; set => numberOutputNodes = value; }

        public Network(Indiv indiv)
        {
            this.Indiv = indiv;
            Nodes = new List<Node>(indiv.Nodes);
            NumberOutputNodes = indiv.NumOutputNodes;

            Nodes = Nodes.OrderBy(o => o.NodeNum).ToList();

            foreach (Connection c in indiv.Connections)
            {
                Nodes[c.OutNode.NodeNum].Weights = new List<double>();
                Nodes[c.OutNode.NodeNum].PreviousLayerNeurons = new List<Node>();
                Nodes[c.OutNode.NodeNum].Weights.Add(c.Weight);
                Nodes[c.OutNode.NodeNum].PreviousLayerNeurons.Add(c.InNode);
            }

        }

        public Network()
        {
            Nodes = new List<Node>();

        }

        private void initiliazeSensorNodes(double[] inputs)
        {
            //Console.WriteLine("initilizing the sensor nodes");
            int i = 0;
            foreach(Node n in nodes)
            {
                if(n.NodeType == NodeType.Sensor)
                {
                    //Console.WriteLine(inputs[i]);
                    n.Value = (double) inputs[i];
                    //Console.WriteLine("Value given to node: " + n.Value);
                    i++;
                }
            }
        }

        public double[] computeNetwork(double[] inputs)
        {
            double[] output = new double[countNodeTypes(NodeType.Output)];

            initiliazeSensorNodes(inputs);
            rankNodes();

            List<Node> temp = Nodes.OrderBy(o => o.NodeRank).ToList();

            foreach (Node n in temp)
            {
                //Console.WriteLine(n.NodeNum + " " + n.Value);
                n.commputeValue();
                //Console.WriteLine(n.NodeNum + " " + n.Value);
            }

            int i = 0;
            foreach(Node n in Nodes)
            {
                if(n.NodeType == NodeType.Output)
                {
                    output[i] = n.Value;
                    i++;
                }
            }

            return output;
        }

        public void rankNodes()
        {
            foreach(Node n in Nodes)
            {
                n.NodeRank = distanceFromSensor(n);
            }
        }

        public int distanceFromSensor(Node n)
        {
            return distanceFS(n, 0);
        }

        private int distanceFS(Node n, int count)
        {
            int maxCount = count;
            if(n.NodeType == NodeType.Sensor)
            {
                return count;
            }

            foreach(Node q in n.PreviousLayerNeurons)
            {
                int temp = distanceFS(q, ++count);
                if(temp > maxCount)
                {
                    maxCount = temp;
                }
            }

            return maxCount;
        }

        private int countNodeTypes(NodeType nodetype)
        {
            int count = 0;
            foreach(Node n in nodes)
            {
                if(n.NodeType == nodetype)
                {
                    count++;
                }
            }
            return count;
        }


    }
}
