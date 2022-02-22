using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpNeat
{
    class Node
    {
        private List<double> weights;
        private List<Node> previousLayerNeurons;
        private List<Node> nextLayerNeurons;
        private double value;
        private NodeType nodeType;
        private int nodeNum;
        private int nodeRank;

        public Node(int nodeNum, NodeType nodeType)
        {
            this.nodeType = nodeType;
            this.nodeNum = nodeNum;
            PreviousLayerNeurons = new List<Node>();
            weights = new List<double>();
            
        }

        public Node()
        {
            PreviousLayerNeurons = new List<Node>();
            weights = new List<double>();
        }

        public List<double> Weights { get => weights; set => weights = value; }
        public int NodeNum { get => nodeNum; set => nodeNum = value; }
        
        internal NodeType NodeType { get => nodeType; set => nodeType = value; }
        public double Value { get => value; set => this.value = value; }
        internal List<Node> PreviousLayerNeurons { get => previousLayerNeurons; set => previousLayerNeurons = value; }
        internal List<Node> NextLayerNeurons { get => nextLayerNeurons; set => nextLayerNeurons = value; }
        public int NodeRank { get => nodeRank; set => nodeRank = value; }

        public double commputeValue()
        {
            if(nodeType == NodeType.Sensor)
            {
                return value;
            }
            double sum = 0;

            for (int i = 0; i < PreviousLayerNeurons.Count; i++)
            {
                sum += (Weights[i] * PreviousLayerNeurons[i].Value);
            }

            value = LogSigmoid(sum);
            return value;
        }

        //this is no longer a sigmoid funciton it is now a leaky ReLu, I was just too lazy to rename it
        public double LogSigmoid(double x)
        {
            if (x < 0)
            {
                return x / 100;
            }
            else if (x > 1)
            {
                return 1;
            }
            else return x;
        }

        public Boolean hasAsInput(int nodeNum)
        {
            for (int i = 0; i < previousLayerNeurons.Count; i++)
            {
                if (previousLayerNeurons[i].nodeNum == nodeNum)
                    return true;
            }
            return false;
        }

        public String inputsToString()
        {
            String ret = "";

            ret += toString();

            for(int i = 0; i < previousLayerNeurons.Count; i++)
            {
                ret += "input node number: " + previousLayerNeurons[i].NodeNum + " Node weight: " + weights[i] + " \n";
            }
            return ret;
        }
        
        public String toString()
        {
            String ret = "";

            ret += "Node number: " + nodeNum + " Node type: " + nodeType + "\n";

            return ret;
        }

        public Boolean hasConnections()
        {
            return previousLayerNeurons.Count == 0 && nextLayerNeurons.Count == 0;
        }
    }

    public enum NodeType
    {
        Sensor,
        Output,
        Hidden
    }
}
