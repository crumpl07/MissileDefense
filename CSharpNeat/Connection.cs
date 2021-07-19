using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpNeat
{
    class Connection
    {
        private Node inNode; // number determened by position in arraylist in indiv class
        private Node outNode; // number determened by position in arraylist in indiv class
        private double weight;
        private bool isEnabled;
        private int innovNum;
        

        public Connection(Node inNode, Node outNode, double weight, int innovNum)
        {
            this.inNode = inNode;
            this.outNode = outNode;
            this.weight = weight;
            this.innovNum = innovNum;
        }

        public Node InNode { get => inNode; set => inNode = value; }
        public Node OutNode { get => outNode; set => outNode = value; }
        public double Weight { get => weight; set => weight = value; }
        public bool IsEnabled { get => isEnabled; set => isEnabled = value; }
        public int InnovNum { get => innovNum; set => innovNum = value; }


        public string toString()
        {
            return "In Node " + inNode.NodeNum + " Out Node " + outNode.NodeNum;
        }
    }
}
