using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpNeat
{
    class Node
    {
        private double[] weights;
        private Node[] previousLayerNeurons;
        private double value;
        private NodeType type;

        public Node() { }

        public double Value { get => value; set => this.value = value; }
        public double[] Weights { get => weights; set => weights = value; }
        


        public double commputeValue()
        {
            double sum = 0;

            for (int i = 0; i < previousLayerNeurons.GetLength(0); i++)
            {
                sum += (weights[i] * previousLayerNeurons[i].value);
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
    }

    enum NodeType
    {
        Sensor,
        Output,
        Hidden
    }
}
