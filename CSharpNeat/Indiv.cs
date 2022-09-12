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

            int innovcount = 0;
            for (int i = 0;i < numInputNodes; i++)
            {
                for(int j = 0; j < numOutputNodes; j++)
                {
                    connections.Add(new Connection(nodes[i], nodes[numInputNodes + j], rand.NextDouble(), innovcount));
                    innovcount++;
                }
            }
            //Console.WriteLine("after all the connections inlitilized" + innovcount);
           
        }

        internal List<Node> Nodes { get => nodes; set => nodes = value; }
        internal List<Connection> Connections { get => connections; set => connections = value; }
        public double Fitness { get => fitness; set => fitness = value; }
        public int NumOutputNodes { get => numOutputNodes; set => numOutputNodes = value; }
        public int NumInputNodes { get => numInputNodes; set => numInputNodes = value; }

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

        public void mutateWeights()
        {
            double weightChangeProb = .5;
            double weightChangeScalar = .25;
            foreach(Connection c in connections)
            {
                if(weightChangeProb > rand.NextDouble())
                {
                    c.Weight = c.Weight + weightChangeScalar * (rand.NextDouble()*2 - 1);
                }
            }
        }

        public List<Connection> mutateStructure(int innovNum)
        {
            double sameNumNodesProb = .8;
            List<Connection> output = new List<Connection>();

            if (rand.NextDouble() > sameNumNodesProb)
            {
                //add new node
                output.AddRange(addNodeToNetwork(innovNum));

            }
            else
            {
                if (canAddConnection())
                {
                    output.Add(addConnection(innovNum));
                }
                
            }

            return output;
        }

        public Boolean canAddConnection()
        {
            Boolean output = false;

            for(int i = 0; i < nodes.Count; i++)
            {
                for(int j = 0; j < nodes.Count; j++)
                {
                    if (nodes[i].NodeType != NodeType.Sensor && nodes[j].NodeType != NodeType.Output && !isInConnections(new Connection(nodes[i], nodes[j], 0, 0))){
                        output = true;
                    }
                }
            }

            return output;
        }

        public List<Connection> addNodeToNetwork(int innovNum)
        {

            Node temp = new Node(nodes.Count, NodeType.Hidden);
            nodes.Add(temp);

            Connection connectionToBeReplaced = connections[rand.Next(0, connections.Count)];
            connectionToBeReplaced.IsEnabled = false;

            Connection inputConnection = new Connection(connectionToBeReplaced.InNode, temp, 1, innovNum);
            innovNum++;
            Connection outputConnection = new Connection(temp, connectionToBeReplaced.OutNode, connectionToBeReplaced.Weight, innovNum);

            List<Connection> output = new List<Connection>();
            output.Add(inputConnection);
            output.Add(outputConnection);

            return output;          
        }

        public Connection addConnection(int innovNum)
        {
            Connection newConnection = new Connection(getValidConnectionInput(), getValidConnectionOutput(), rand.NextDouble() * 2 - 1, innovNum);

            while (isInConnections(newConnection))
            {
                newConnection = new Connection(getValidConnectionInput(), getValidConnectionOutput(), rand.NextDouble() * 2 - 1, innovNum);
            }

            connections.Add(newConnection);

            return newConnection;
     
        }
        
        public Boolean isInConnections(Connection c)
        {
            
            foreach(Connection con in connections)
            {
                if (c.equals(con))
                {
                    return true;
                }
                
            }
            return false;

        }

        public Node getValidConnectionInput()
        {
            

            Node validNode = nodes[rand.Next(0, nodes.Count)];

            while(validNode.NodeType == NodeType.Output)
            {
                validNode = nodes[rand.Next(0, nodes.Count)];
            }

            return validNode;
        }

        public Node getValidConnectionOutput()
        {

            Node validNode = nodes[rand.Next(0, nodes.Count)];

            while (validNode.NodeType == NodeType.Sensor)
            {
                validNode = nodes[rand.Next(0, nodes.Count)];
            }

            return validNode;
        }

        public int addNode(int innovNum)
        {

            Connection replacedConnection = connections[rand.Next(0, connections.Count)];

            Node newNode = new Node(nodes.Count,NodeType.Hidden);

            Connection inputConnection = new Connection(replacedConnection.InNode, newNode, 1, innovNum);
            Connection outputConnection = new Connection(newNode, replacedConnection.OutNode, replacedConnection.Weight, ++innovNum);

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
