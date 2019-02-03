using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCNN
{
    // Connection between two nodes
    class Connection
    {
        public Connection(Node upstreamNode, Node downstreamNode)
        {
            UpstreamNode = upstreamNode;
            DownstreamNode = downstreamNode;
            Weight = BasicMath.RandomFromRange(-0.1, 0.1);
            Gradient = 0;
        }
        public void CalculateGradient()
        {
            Gradient = DownstreamNode.Delta * UpstreamNode.Output;
        }
        public void UpdateWeight(double rate)
        {
            CalculateGradient();
            Weight += rate * Gradient;
        }
        public double Weight { get; set; }
        public Node UpstreamNode { get; set; }
        public Node DownstreamNode { get; set; }
        public double Gradient { get; set; }
    }
}
