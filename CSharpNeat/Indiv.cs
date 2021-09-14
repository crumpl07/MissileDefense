using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpNeat
{
    class Indiv
    {
        private Random rand = new Random();
        private List<Node> nodes;
        private List<Connection> connections;
        private int numOutputNodes; 
        private int numInputNodes;
        private double fitness;
        
        //make new list of nodes everytime feed forward is called 

        public Indiv(int numInputNodes, int numOutputNodes)
        {
            this.NumInputNodes = numInputNodes;
            this.NumOutputNodes = numOutputNodes;
            connections = new List<Connection>();
            nodes = new List<Node>();

            for (int i = 0; i < numInputNodes; i++)
            {
                Node temp = new Node(i, NodeType.Sensor);
                nodes.Add(temp);
            }

            for (int i = 0; i < numOutputNodes; i++)
            {
                Node temp = new Node(i + numInputNodes,NodeType.Output);
                nodes.Add(temp);
            }
            
            for(int i = 0; i < numInputNodes; i++)
            {
                for(int j = 0; j < numOutputNodes; j++)
                {
                    Connection temp = new Connection(nodes[i], nodes[numInputNodes + j], 0.5 - rand.NextDouble(), i + j);
                    connections.Add(temp);
                }
            }
        }

        internal List<Node> Nodes { get => nodes; set => nodes = value; }
        internal List<Connection> Connections { get => connections; set => connections = value; }
        public double Fitness { get => fitness; set => fitness = value; }
        public int NumOutputNodes { get => numOutputNodes; set => numOutputNodes = value; }
        public int NumInputNodes { get => numInputNodes; set => numInputNodes = value; }

        public List<double> feedForward(double[] inputs) // This method assumes the input nodes take up the first n elements of the above list
        {
            nodes = nodes.OrderBy(o => o.NodeNum).ToList();
            assembleNetwork();
            List<double> output = computeNetwork(inputs);
            return output;

        }

        public List<Node> nodeOrder()
        {
            List<Node> ret = new List<Node>();
            for(int i = 0; i < connections.Count; i++)
            {
                if(connections[i].InNode.NodeType == NodeType.Hidden && indexOf(ret,connections[i].InNode) < 0)
                {
                    int index = indexOf(ret, connections[i].OutNode);
                    if (index > 0)
                    {
                        ret.Insert(index - 1, nodes[indexOfNode(connections[i].InNode)]);
                    }
                    else if(index < 0 && connections[i].OutNode.NodeType != NodeType.Output)
                    {
                        ret.Add(nodes[indexOfNode(connections[i].InNode)]);
                        ret.Add(nodes[indexOfNode(connections[i].OutNode)]);
                    }
                    else
                    {
                        ret.Add(nodes[indexOfNode(connections[i].InNode)]);
                    }
                }
            }
            return ret;
        }

        public int indexOf(List<Node> list, Node node)
        {
            int index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (node.NodeNum == list[i].NodeNum)
                    index = i;
            }
            return index;
        }

        public List<double> computeNetwork(double[] inputs)
        {
            
            List<double> outputs = new List<double>();

            for(int i = 0; i < numInputNodes; i++)
            {
                nodes[i].Value = inputs[i];
            }

            List<Node> hiddenNodes = nodeOrder();

            for(int i = 0; i < hiddenNodes.Count; i++)
            {
                hiddenNodes[i].commputeValue();
            }

            int j = 0;
            for(int i = numInputNodes; i < numInputNodes + numOutputNodes; i++)
            {
                outputs.Add(nodes[i].commputeValue());
                j++;
            }

            return outputs;
        }
        public void assembleNetwork()
        {
            //new plan go through all the connections to find the connections to the output node then add them to the node
            //Console.WriteLine("assembling network");

            for (int i = 0; i < nodes.Count; i++)
            {
                //Console.WriteLine("index of node " + i + " " + nodes[i].toString());
                List<Node> nodeConnections = new List<Node>();
                List<double> nodeConnectionsWeights = new List<double>();

                for (int j = 0; j < connections.Count; j++)
                {
                    //Console.WriteLine("index of connection " + j + " Connection " + connections[j].toString());
                    if (connections[j].OutNode.NodeNum == nodes[i].NodeNum && connections[j].IsEnabled)
                    {
                        //add the innode to the node connection and the connection weight to their respective lists
                        nodeConnections.Add(nodes[indexOfNode(connections[j].InNode)]);
                        //Console.WriteLine(nodes[indexOfNode(connections[j].InNode)].toString());
                        //Console.WriteLine(nodes[indexOfNode(connections[j].InNode)].toString());
                        nodeConnectionsWeights.Add(connections[j].Weight);
                    }
                }
                nodes[i].PreviousLayerNeurons = nodeConnections;
                nodes[i].Weights = nodeConnectionsWeights;

                //Console.WriteLine(nodes[i].inputsToString());
            }
            nodes = nodes.OrderBy(o => o.NodeNum).ToList();
        }

        public int indexOfConnection(int outputNodeNumber, int inputNodeNumber)
        {
            for(int i = 0; i < connections.Count; i++)
            {
                if(connections[i].OutNode.NodeNum == outputNodeNumber && connections[i].InNode.NodeNum == inputNodeNumber)
                {
                    return i;
                }
            }
            return -1;
        }

        //need to disable connections between two nodes when a middle node is added
        public int mutate(int innovNum)
        {
            //similar weight prob of .9 means there is a 90% chance the weight will be off by a small ammount
            //same num nodes prob of .8 means there is an 80% chance a connection will be added and 20% a nodes is added
            double similarWeightProb = .9;
            double sameNumNodesProb = .8;

            //selecting nodes to connect to
            //inputNodeNumber can be from 0 to numInputNodes and from numInputNodes + numOutputNodes to nodes.Count
            //outputNodeNumber can be from numInputNodes to nodes.Count
            int inputNodeNumber = 0;
            int outputNodeNumber = 0;

            double porportionInput = numInputNodes / (nodes.Count - numOutputNodes);
            nodes = nodes.OrderBy(o => o.NodeNum).ToList();

            if (porportionInput > rand.NextDouble())
            {
                inputNodeNumber = rand.Next(0, numInputNodes);
            }
            else
            {
                inputNodeNumber = rand.Next(numInputNodes + numOutputNodes, nodes.Count);
            }

            outputNodeNumber = rand.Next(NumInputNodes, nodes.Count);

            //connecting the nodes either with a new node or with a regular connection 
            if (rand.NextDouble() > sameNumNodesProb)
            {
                //add new node
                Node temp = new Node(nodes.Count, NodeType.Hidden);
                nodes.Add(temp);

                Connection temp1 = new Connection(nodes[inputNodeNumber], temp, 1, innovNum);
                innovNum++;
                Connection temp2 = new Connection(temp, nodes[outputNodeNumber], connections[indexOfConnection(outputNodeNumber, inputNodeNumber)].Weight, innovNum);
                innovNum++;

                
                connections.Add(temp1);
                connections.Add(temp2);
                  
                //disabling the connection
                connections[indexOfConnection(outputNodeNumber,inputNodeNumber)].IsEnabled = false;


            }
            else
            {
                //add new connection 
                //this is the same thing as updating a weigth if it happens to an existing connection

                if(rand.NextDouble() > similarWeightProb)
                {
                    Connection temp1 = new Connection(nodes[inputNodeNumber], nodes[outputNodeNumber], (0.5 - rand.NextDouble()) * 2, innovNum);
                    connections.Add(temp1);
                    innovNum++;
                }
                else
                {
                    for (int i = 0; i < connections.Count; i++)
                    {
                        if (connections[i].OutNode.NodeNum == outputNodeNumber && connections[i].InNode.NodeNum == inputNodeNumber)
                        {
                            double newWeight = connections[i].Weight + (0.5 - rand.NextDouble()) * 2;
                            Connection temp1 = new Connection(nodes[inputNodeNumber], nodes[outputNodeNumber], newWeight, innovNum);
                            connections.Add(temp1);
                            innovNum++;
                            break;
                        }

                    }
                    
                }


                
            }
            nodes = nodes.OrderBy(o => o.NodeNum).ToList();
            return innovNum;
        }

        public Boolean isInNetwork(Node nodeNum)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].NodeNum == nodeNum.NodeNum)
                    return true;
            }
            return false;
        }

        public int indexOfNode(Node node)
        {
            int index = 0;
            for(int i = 0; i < nodes.Count; i++)
            {
                if (node.NodeNum == nodes[i].NodeNum)
                    index = i;
            }
            return index;
        }
        
        public String toString()
        {
            String ret = "";

            for(int i = 0; i < connections.Count; i++)
            {
                ret += connections[i].toString() + "\n";
            }
            for(int i = 0; i < nodes.Count; i++)
            {
                ret += nodes[i].toString();
            }

            return ret;
        }

    }
}
