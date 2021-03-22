using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpNeat
{
    class Indiv
    {

        private List<Node> nodes;
        private List<Connection> connections;
        private int numOutputNodes;
        private int numInputNodes;
        private double fitness;

        //make new list of nodes everytime feed forward is called 

        public Indiv(int numInputNodes, int numOutputNodes) 
        {
            this.numInputNodes = numInputNodes;
            this.numOutputNodes = numOutputNodes;
            connections = new List<Connection>();
            nodes = new List<Node>();
            Fitness = 0;
            for (int i = 0; i < numInputNodes; i++)
            {
                for(int j = 0; j < numOutputNodes; j++)
                {
                    
                    Connection temp = new Connection(i, (numInputNodes + j), 1.0, (i+j), NodeType.Output, NodeType.Sensor);
                    temp.IsEnabled = true;
                    connections.Add(temp);
                    //idk how good of an idea this is but the innovation nums are just the initial position in the connections List
                }
            }
            for(int i = 0; i < numInputNodes; i++)
            {
                Node temp = new Node(i);
                temp.NodeType = NodeType.Sensor;
                nodes.Add(temp);
            }
            for (int i = 0; i < numOutputNodes; i++)
            {
                Node temp = new Node(numInputNodes + i);
                temp.NodeType = NodeType.Output;
                nodes.Add(temp);
            }
        }

        internal List<Node> Nodes { get => nodes; set => nodes = value; }
        internal List<Connection> Connections { get => connections; set => connections = value; }
        public double Fitness { get => fitness; set => fitness = value; }

        public List<double> feedForward(double[] inputs) // This method assumes the input nodes take up the first n elements of the above list
        {
            
            assembleNetwork();
            List<double> output = computeNetwork(inputs);
            Console.WriteLine("You are now feeding forward");
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
            
            //The clear is to ensure the last build is not messing anything up in the current build
            // It is the job of the connections List to hold the changes 

            

            for (int i = 0; i < connections.Count; i++)
            {
                Console.WriteLine(i + " " + nodes.Count + " " + connections[i].toString());
                
                nodes.OrderBy(o => o.NodeNum).ToList();
                //If both nodes exist
                if (isInNetwork(connections[i].OutNode) && isInNetwork(connections[i].InNode))
                {
                    //if the connection exists 
                    if (nodes[connections[i].OutNode].hasAsInput(connections[i].InNode))
                    {
                        nodes[connections[i].OutNode].updateWeight(connections[i].Weight, connections[i].InNode);
                    }
                    else
                    {
                        nodes[connections[i].OutNode].addNode(nodes[connections[i].InNode], connections[i].Weight, connections[i].InNode);
                    }

                }
                //If only the input node exists 
                if (!isInNetwork(connections[i].OutNode) && isInNetwork(connections[i].InNode))
                {
                    Node temp = new Node(connections[i].OutNode);

                    temp.NodeType = connections[i].OutNodeType;

                    temp.addNode(nodes[connections[i].InNode], connections[i].Weight, nodes[connections[i].InNode].NodeNum);

                    nodes.Add(temp);

                }

                //If only the output node exists
                if(isInNetwork(connections[i].OutNode) && !isInNetwork(connections[i].InNode))
                {
                    Node temp = new Node(connections[i].InNode);
                    temp.NodeType = connections[i].InNodeType;
                    nodes.Add(temp);
                }

                //If neither node exists
                if (!isInNetwork(connections[i].OutNode) && !isInNetwork(connections[i].InNode))
                {
                    Node tempOutNode = new Node(connections[i].OutNode);
                    Node tempInNode = new Node(connections[i].InNode);

                    tempOutNode.NodeType = connections[i].OutNodeType;
                    tempInNode.NodeType = connections[i].InNodeType;

                    tempOutNode.addNode(tempInNode, connections[i].Weight, tempInNode.NodeNum);

                    nodes.Add(tempInNode);
                    nodes.Add(tempOutNode);
                    
                    
                    
                }

                //dissables the connection if need be
                if (!connections[i].IsEnabled)
                {
                    nodes[connections[i].OutNode].updateWeight(0, connections[i].InNode);
                }
            }
        }

        public Boolean isInNetwork(int nodeNum)
        {
            for(int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].NodeNum == nodeNum)
                    return true;
            }
            return false;
        }

        public int networkSize()
        {
            int largest = 0;
            for(int i = 0; i < connections.Count; i++)
            {
                if (connections[i].OutNode > largest)
                    largest = connections[i].OutNode;
                if (connections[i].InNode > largest)
                    largest = connections[i].InNode;
            }
            return largest;
        }

    }
}
