using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCNN
{
    abstract class Node
    {
        protected Node(Layer nodeLayer)
        {
            NodeLayer = nodeLayer;
        }

        // The index of current node in associated layer and Layer Index
        public Layer NodeLayer { get; set; }
        public double Delta { get; set; }

        public double Output { get; set; }
        // All Connections from previous layer (Only in FCNN)
        public virtual List<Connection> UpstreamConnectionList { get; set; }
        // All Connections from next layer (Only in FCNN)
        public List<Connection> DownstreamConnectionList { get; set; }

        
        public virtual void CalculateOutput()
        {
            double TmpOutput = 0;
            foreach (Connection item in UpstreamConnectionList)
            {
                TmpOutput += item.Weight * item.UpstreamNode.Output;
            }
            Output = BasicMath.Sigmoid(TmpOutput);
        }
        // When node is in hidden layer, calculate delta
        public virtual void CalculateDelta()
        {
            double DownstreamDelta = 0;
            foreach (Connection item in DownstreamConnectionList)
            {
                DownstreamDelta += item.Weight * item.DownstreamNode.Delta;
            }
            Delta = Output * (1 - Output) * DownstreamDelta;
        }
        public virtual void CalculateDelta(double label)
        {
            Delta = Output * (1 - Output) * (label - Output);
        }
    }
    // The basic unit of the network
    class GeneralNode : Node
    {


        public GeneralNode(Layer layer) : base(layer)
        {
            Output = 0;
            Delta = 0;
            UpstreamConnectionList = new List<Connection>();
            DownstreamConnectionList = new List<Connection>();
        }

    }

    class ConstantNode : Node
    {
        public ConstantNode(Layer layer) : base(layer)
        {
            Output = 1;
            Delta = 0;
            DownstreamConnectionList = new List<Connection>();
        }
        public override void CalculateDelta(double label)
        {
            throw new NotSupportedException();
        }
        public override void CalculateOutput()
        {
            throw new NotSupportedException();
        }
    }
}
