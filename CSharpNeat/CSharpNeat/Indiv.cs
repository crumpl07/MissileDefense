using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpNeat
{
    class Indiv
    {

        private List<Node> nodes = new List<Node>();
        private List<Connection> connections = new List<Connection>();

        //make new list of nodes everytime feed forward is called 

        public Indiv(int numInputNodes, int numOutputNodes)
        {
            for(int i = 0; i < numInputNodes; i++)
            {
                for(int j = 0; j < numOutputNodes; j++)
                {
                    Connection temp = new Connection(i, j, 1.0, (i+j), NodeType.Output, NodeType.Sensor);//idk how good of an idea this is but the innovation nums are just the initial position in the connections List
                }
            }
        }

        internal List<Node> Nodes { get => nodes; set => nodes = value; }
        internal List<Connection> Connections { get => connections; set => connections = value; }

        public List<double> feedForward(double[] inputs) // This method assumes the input nodes take up the first n elements of the above list
        {
            Console.WriteLine("You are now feeding forward");
            assembleNetwork();
            List<double> output = computeNetwork(inputs);
            return output;

        }

        public List<double> computeNetwork(double[] inputs)
        {
            List<double> outputs = new List<double>();

            int j = 0;
            for(int i = 0; i < nodes.Count; i++)
            {
                if(nodes[i].NodeType == NodeType.Sensor)
                {
                    nodes[i].Value = inputs[j];
                    j++;
                }
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                if(nodes[i].NodeType == NodeType.Hidden)
                {
                    nodes[i].commputeValue();
                }
                
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].NodeType == NodeType.Output)
                {
                    nodes[i].commputeValue();
                    outputs.Add(nodes[i].Value);
                }
            }

            return outputs;
        }

        public void assembleNetwork()
        {
            //Network is rebuilt everytime this funciton is called
            //The clear is to ensure the last build is not messing anything up in the current build
            // It is the job of the connections List to hold the changes 
            nodes.Clear();
            for(int i = 0; i < connections.Count; i++)
            {
                //If neither node exists
                if (nodes[connections[i].OutNode] == null && nodes[connections[i].InNode] == null)
                {
                    Node tempOutNode = new Node(connections[i].OutNode);
                    Node tempInNode = new Node(connections[i].InNode);

                    tempOutNode.NodeType = connections[i].OutNodeType;
                    tempInNode.NodeType = connections[i].InNodeType;

                    tempOutNode.addNode(tempInNode, connections[i].Weight, tempInNode.NodeNum);

                    nodes.Insert(connections[i].OutNode, tempOutNode);
                    nodes.Insert(connections[i].InNode, tempInNode);
                }
                
                //If only the input node exists 
                if (nodes[connections[i].OutNode] == null && nodes[connections[i].InNode] != null)
                {
                    Node temp = new Node(connections[i].OutNode);

                    temp.NodeType = connections[i].OutNodeType;

                    temp.addNode(nodes[connections[i].InNode], connections[i].Weight, nodes[connections[i].InNode].NodeNum);

                    nodes.Insert(connections[i].OutNode, temp);
                }

                //If both nodes exist
                if (nodes[connections[i].OutNode] != null && nodes[connections[i].InNode] != null)
                {
                    nodes[connections[i].OutNode].updateWeight(connections[i].Weight, connections[i].InNode);
                }
                
                //dissables the connection if need be
                if (!connections[i].IsEnabled)
                {
                    nodes[connections[i].OutNode].updateWeight(0, connections[i].InNode);
                }

            }
        }

    }
}
