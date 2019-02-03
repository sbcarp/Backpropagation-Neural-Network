using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCNN
{
    // The Network Contains mutiple layers and one connection collection
    class Network
    {
        public List<Layer> LayerList { get; set; }
        public Connections Connections;

        public Network(params int[] layerNodesNumber)
        {
            // Generate layers
            LayerList = new List<Layer>();
            LayerList.Add(new Layer(Layer.LayerTypeEnum.InputLayer, layerNodesNumber[0]));
            for (int i = 1; i < layerNodesNumber.Length - 1; i++)
            {
                LayerList.Add(new Layer(Layer.LayerTypeEnum.HiddenLayer, layerNodesNumber[i]));
            }
            LayerList.Add(new Layer(Layer.LayerTypeEnum.OutputLayer, layerNodesNumber[layerNodesNumber.Length - 1]));

            Connections = new Connections();
            for (int i = 0; i < LayerList.Count - 1; i++)
            {
                Connections TmpConnections = new Connections();
                for (int j = 0; j < LayerList[i].Nodes.Count; j++)
                {
                    for (int k = 0; k < LayerList[i+1].Nodes.Count - 1; k++)
                    {
                        Connection TmpConnection = new Connection(LayerList[i].Nodes[j], LayerList[i + 1].Nodes[k]);
                        TmpConnections.ConnectionList.Add(TmpConnection);
                    }
                }
                foreach (Connection item in TmpConnections.ConnectionList)
                {
                    Connections.ConnectionList.Add(item);
                    item.DownstreamNode.UpstreamConnectionList.Add(item);
                    item.UpstreamNode.DownstreamConnectionList.Add(item);
                }

            }
        }
        public void Train(List<List<double>> data, List<List<double>> labels, int iteration, double rate)
        {
            Console.Write("Training {0} samples... ", data.Count);
            using (var progress = new ProgressBar())
            {
                for (int i = 0; i < iteration; i++)
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        progress.Report((double)(j+1) / data.Count);
                        SingleIteration(data[j], labels[j], rate);
                    }
                }
            }
            Console.WriteLine("Done.");
        }
        private void SingleIteration(List<double> data, List<double> label, double rate)
        {
            Predict(data);
            CalculateDelta(label);
            UpdateWeight(rate);
        }
        private void CalculateDelta(List<double> label)
        {
            List<Node> OutputLayerNodes = LayerList[LayerList.Count - 1].Nodes;
            for (int i = 0; i < label.Count; i++)
            {
                OutputLayerNodes[i].CalculateDelta(label[i]);
            }
            for (int i = LayerList.Count - 2; i >= 0; i--)
            {
                foreach (Node item in LayerList[i].Nodes)
                {
                    item.CalculateDelta();
                }
            }
        }
        private void UpdateWeight(double rate)
        {
            for (int i = 0; i < LayerList.Count - 1; i++)
            {
                foreach (Node node in LayerList[i].Nodes)
                {
                    foreach (Connection connection in node.DownstreamConnectionList)
                    {
                        connection.UpdateWeight(rate);
                    }
                }
            }
        }
        public List<double> Predict(List<double> data)
        {
            LayerList[0].SetOutputData(data);
            for (int i = 1; i < LayerList.Count; i++)
            {
                LayerList[i].CalculateOutput();
            }
            List<double> result = new List<double>();
            for (int i = 0; i < LayerList[LayerList.Count - 1].Nodes.Count - 1; i++)
            {
                result.Add(LayerList[LayerList.Count - 1].Nodes[i].Output);
            }
            return result;
        }
        public void CalculateGradient()
        {
            for (int i = 0; i < LayerList.Count - 1; i++)
            {
                foreach (Node node in LayerList[i].Nodes)
                {
                    foreach (Connection connection in node.DownstreamConnectionList)
                    {
                        connection.CalculateGradient();
                    }
                }
            }
        }
        public void GetGradient(List<double> label, List<double> sample)
        {
            Predict(sample);
            CalculateDelta(label);
            CalculateGradient();
        }

    }
}
