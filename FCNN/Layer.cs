using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCNN
{
    // Layer class has mutiple nodes
    class Layer
    {
        public Layer(LayerTypeEnum layerType, int NodeCount)
        {
            LayerType = layerType;
            Nodes = new List<Node>();
            for (int i = 0; i < NodeCount; i++)
            {
                Nodes.Add(new GeneralNode(this));
            }
            Nodes.Add(new ConstantNode(this));
        }
        public void SetOutputData(List<double> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                Nodes[i].Output = data[i];
            }
        }
        public void CalculateOutput()
        {
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                Nodes[i].CalculateOutput();
            }
        }
        public enum LayerTypeEnum
        {
            InputLayer,
            HiddenLayer,
            OutputLayer
        }
        public LayerTypeEnum LayerType { get; set; }
        public List<Node> Nodes { get; set; }
    }
}
