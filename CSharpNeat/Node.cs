using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpNeat
{
    class Node
    {
        private List<double> weights;
        private List<Node> previousLayerNeurons;
        private List<int> inputNodeNums;
        private double value;
        private NodeType nodeType;
        private int nodeNum;
         
        public Node(int nodeNum)
        {
            this.nodeNum = nodeNum;
            previousLayerNeurons = new List<Node>();
            weights = new List<double>();
            inputNodeNums = new List<int>();
        }

        public Node()
        {
            previousLayerNeurons = new List<Node>();
            weights = new List<double>();
        }
        
        public List<double> Weights { get => weights; set => weights = value; }
        public int NodeNum { get => nodeNum; set => nodeNum = value; }
        public List<int> InputNodeNums { get => inputNodeNums; set => inputNodeNums = value; }
        internal NodeType NodeType { get => nodeType; set => nodeType = value; }
        public double Value { get => value; set => this.value = value; }

        public void addNode(Node input, double weight, int inputNodeNum)
        {
            previousLayerNeurons.Add(input);
            weights.Add(weight);
            inputNodeNums.Add(inputNodeNum);
        }
        public void updateWeight(double weight, int inputNodeNum)
        {
            weights[inputNodeNums.IndexOf(inputNodeNum)] = weight;
        }
        public double commputeValue()
        {
            double sum = 0;

            for (int i = 0; i < previousLayerNeurons.Count; i++)
            {
                sum += (Weights[i] * previousLayerNeurons[i].Value);
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
            for (int i = 0; i < inputNodeNums.Count; i++)
            {
                if (inputNodeNums[i] == nodeNum)
                    return true;
            }
            return false;
        }
    }

    public enum NodeType
    {
        Sensor,
        Output,
        Hidden
    }
}
