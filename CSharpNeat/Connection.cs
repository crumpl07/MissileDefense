using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpNeat
{
    class Connection
    {
        private int inNode; // number determened by position in arraylist in indiv class
        private int outNode; // number determened by position in arraylist in indiv class
        private double weight;
        private bool isEnabled;
        private int innovNum;
        private NodeType outNodeType;
        private NodeType inNodeType;

        public Connection(int inNode, int outNode, double weight, int innovNum, NodeType outNodeType, NodeType inNodeType)
        {
            this.inNode = inNode;
            this.outNode = outNode;
            this.weight = weight;
            this.innovNum = innovNum;
            this.inNodeType = inNodeType;
            this.outNodeType = outNodeType;
        }

        public int InNode { get => inNode; set => inNode = value; }
        public int OutNode { get => outNode; set => outNode = value; }
        public double Weight { get => weight; set => weight = value; }
        public bool IsEnabled { get => isEnabled; set => isEnabled = value; }
        public int InnovNum { get => innovNum; set => innovNum = value; }
        public NodeType OutNodeType { get => outNodeType; set => outNodeType = value; }
        public NodeType InNodeType { get => inNodeType; set => inNodeType = value; }

        public string toString()
        {
            return "In Node " + inNode + " Out Node " + outNode;
        }
    }
}
