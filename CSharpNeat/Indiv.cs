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
            Fitness = 0;
            for (int i = 0; i < numInputNodes; i++)
            {
                for (int j = 0; j < numOutputNodes; j++)
                {
                    Node temp1 = new Node(i, NodeType.Sensor);
                    Node temp2 = new Node(j + numInputNodes, NodeType.Output);
                    

                    Connection temp = new Connection(temp1, temp2, 1.0, (i + j));
                    temp.IsEnabled = true;
                    //Console.WriteLine(temp.toString());
                    connections.Add(temp);
                    //idk how good of an idea this is but the innovation nums are just the initial position in the connections List
                }
            }
            for (int i = 0; i < numInputNodes; i++)
            {
                Node temp = new Node(i, NodeType.Sensor);
                nodes.Add(temp);
            }
            for (int i = 0; i < numOutputNodes; i++)
            {                
                Node temp2 = new Node(numInputNodes + i, NodeType.Output);                
                nodes.Add(temp2);
            }
        }

        internal List<Node> Nodes { get => nodes; set => nodes = value; }
        internal List<Connection> Connections { get => connections; set => connections = value; }
        public double Fitness { get => fitness; set => fitness = value; }
        public int NumOutputNodes { get => numOutputNodes; set => numOutputNodes = value; }
        public int NumInputNodes { get => numInputNodes; set => numInputNodes = value; }

        public List<double> feedForward(double[] inputs) // This method assumes the input nodes take up the first n elements of the above list
        {

            assembleNetwork();
            List<double> output = computeNetwork(inputs);
            return output;

        }

        public List<double> computeNetwork(double[] inputs)
        {
            List<double> outputs = new List<double>();

            int j = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].NodeType == NodeType.Sensor)
                {
                    //Console.WriteLine(nodes[i].toString());
                    //Console.WriteLine("i: " + i + " j: " + j);

                    nodes[i].Value = inputs[j];
                   

                    j++;
                }
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].NodeType == NodeType.Hidden)
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

        //need to disable connections between two nodes when a middle node is added
        public int mutate(int innovNum)//uuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuhhhh idk how often we want these mutations to occur
        {
            double mutType = rand.NextDouble();

            //selecting nodes to connect to

            //this selection process is dum
            int inputNodeNumber = 0;
            if(rand.NextDouble() < NumInputNodes / nodes.Count)
            {
                inputNodeNumber = rand.Next(0, NumInputNodes - 1);
            }
            else
            {
                inputNodeNumber = rand.Next(NumInputNodes + NumOutputNodes - 2, nodes.Count - 1);
            }

            int outputNodeNumber = rand.Next(NumInputNodes, nodes.Count - 1);


            

            Node inputNode = nodes[inputNodeNumber];
            Node outputNode = nodes[outputNodeNumber];

            //connecting the nodes either with a new node or with a regular connection 
            if(mutType > .5)
            {
                //add new node
                Node temp = new Node(nodes.Count, NodeType.Hidden);

                Connection temp1 = new Connection(inputNode, temp, rand.NextDouble(), innovNum);
                innovNum++;
                Connection temp2 = new Connection(temp, outputNode, rand.NextDouble(), innovNum);
                innovNum++;

                connections.Add(temp1);
                connections.Add(temp2);

                for(int i = 0; i < connections.Count; i++)
                {
                    if(connections[i].OutNode.NodeNum == outputNodeNumber && connections[i].InNode.NodeNum == inputNodeNumber)
                    {
                        connections[i].IsEnabled = false;
                    }

                }


            }
            else
            {
                //add new connection 
                //this is the same thing as updating a weigth if it happens to an existing connection

                Connection temp1 = new Connection(inputNode, outputNode,  100 * rand.NextDouble(), innovNum);
                connections.Add(temp1);
                innovNum++;
            }
            return innovNum;
        }

        public void assembleNetwork()
        {

            //The clear is to ensure the last build is not messing anything up in the current build
            // It is the job of the connections List to hold the changes 
            


            for (int i = 0; i < connections.Count; i++)
            {
                //Console.WriteLine(i + " " + nodes.Count + " " + connections[i].toString());

                nodes.OrderBy(o => o.NodeNum).ToList();
                //If both nodes exist
                if (isInNetwork(connections[i].OutNode.NodeNum) && isInNetwork(connections[i].InNode.NodeNum))
                {
                    //if the connection exists 
                    if (nodes[connections[i].OutNode.NodeNum].hasAsInput(connections[i].InNode.NodeNum))
                    {
                        nodes[connections[i].OutNode.NodeNum].updateWeight(connections[i].Weight, connections[i].InNode.NodeNum);
                    }
                    else
                    {
                        nodes[connections[i].OutNode.NodeNum].addNode(nodes[connections[i].InNode.NodeNum], connections[i].Weight, connections[i].InNode.NodeNum);
                    }

                }
                //If only the input node exists 
                if (!isInNetwork(connections[i].OutNode.NodeNum) && isInNetwork(connections[i].InNode.NodeNum))
                {
                    Node temp = new Node(connections[i].OutNode.NodeNum, NodeType.Hidden);

                    temp.NodeType = connections[i].OutNode.NodeType;

                    temp.addNode(nodes[connections[i].InNode.NodeNum], connections[i].Weight, connections[i].InNode.NodeNum);

                    nodes.Add(temp);

                }

                //If only the output node exists
                if (isInNetwork(connections[i].OutNode.NodeNum) && !isInNetwork(connections[i].InNode.NodeNum))
                {
                    Node temp = new Node(connections[i].InNode.NodeNum, NodeType.Hidden);
                    temp.NodeType = connections[i].InNode.NodeType;
                    nodes.Add(temp);
                }

                //If neither node exists
                if (!isInNetwork(connections[i].OutNode.NodeNum) && !isInNetwork(connections[i].InNode.NodeNum))
                {
                    Node tempOutNode = new Node(connections[i].OutNode.NodeNum, NodeType.Hidden);
                    Node tempInNode = new Node(connections[i].InNode.NodeNum, NodeType.Hidden);

                    tempOutNode.NodeType = connections[i].OutNode.NodeType;
                    tempInNode.NodeType = connections[i].InNode.NodeType;

                    tempOutNode.addNode(tempInNode, connections[i].Weight, tempInNode.NodeNum);

                    nodes.Add(tempInNode);
                    nodes.Add(tempOutNode);



                }

                //dissables the connection if need be
                if (!connections[i].IsEnabled)
                {
                    nodes[connections[i].OutNode.NodeNum].updateWeight(0, connections[i].InNode.NodeNum);
                }
            }
        }

        public Boolean isInNetwork(int nodeNum)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].NodeNum == nodeNum)
                    return true;
            }
            return false;
        }

        public int networkSize()
        {
            int largest = 0;
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].OutNode.NodeNum > largest)
                    largest = connections[i].OutNode.NodeNum;
            }
            return largest;
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
