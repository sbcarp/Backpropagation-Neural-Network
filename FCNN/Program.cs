using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCNN
{
    class Program
    {
        static void Main(string[] args)
        {
            // 784 is equal to the number of pixels of a single image
            // 100 nodes in hidden layer 1
            // 100 nodes in hidden layer 2
            // 10 nodes in output layer
            // Create a Backpropagation Network
            Network bpNetwrok = new Network(784, 100, 100, 10);
            // Read Mnist training data
            List<List<double>> trainingDataSet = MnistDataReader.Read(@"data\train-images.idx3-ubyte");
            List<List<double>> trainingLabelSet = MnistDataReader.Read(@"data\train-labels.idx1-ubyte");
            // Reat Mnist testing data
            List<List<double>> testDataSet = MnistDataReader.Read(@"data\t10k-images.idx3-ubyte");
            List<List<double>> testLabelSet = MnistDataReader.Read(@"data\t10k-labels.idx1-ubyte");

            Log("Finished reading data");
            // Check if Expected gradient and actual gradient are close enough.
            GradientCheckTest();
            Log("Finished GradientCheck");

            // Record current round number
            int Round = 1;
            while (true)
            {
                // Start Training with a fixed rate
                // TODO: use dynamic rate
                bpNetwrok.Train(trainingDataSet, trainingLabelSet, 1, 0.01);
                Log("Finished training Round #" + Round);
                Log("Testing error rate is " + Evaluate(bpNetwrok, testDataSet, testLabelSet) + "%");
                
                Round++;
            }

            //Console.ReadLine();
        }
        static private void Log(string description)
        {
            Console.WriteLine("{0}\t{1}", DateTime.Now.ToLongTimeString(), description);
        }
        // Find error rate of given test data set
        static private double Evaluate(Network bpNetwork, List<List<double>> testDataSet, List<List<double>> testLabelSet)
        {
            
            double errorCount = 0;
            double totalSmaple = testDataSet.Count;
            for (int i = 0; i < testDataSet.Count; i++)
            {
                int label = GetResult(testLabelSet[i]);
                int predict = GetResult(bpNetwork.Predict(testDataSet[i]));
                if (label != predict) errorCount++;
            }
            return (errorCount / totalSmaple) * 100;
        }
        // Find max value in the array, return it's index
        static private int GetResult(List<double> result)
        {
            int maxValueIndex = 0;
            double maxValue = result[0];
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] > maxValue)
                {
                    maxValueIndex = i;
                    maxValue = result[i];
                }
            }
            return maxValueIndex;
        }
        static private void GradientCheckTest()
        {
            Network BPNetwrok = new Network(2, 2, 2);
            List<double> trainingData = new List<double>();
            List<double> trainingLabel = new List<double>();
            trainingData.Add(0.9);
            trainingData.Add(0.1);
            trainingLabel.Add(0.9);
            trainingLabel.Add(0.1);
            GradientCheck(BPNetwrok, trainingData, trainingLabel);
        }

        // Validate the network, if the expected gradient is differ with actual gradient more than 0.001 
        static private void GradientCheck(Network BPNetwork, List<double> sampleData, List<double> sampleLabel)
        {
            BPNetwork.GetGradient(sampleLabel, sampleData);
            for (int i = 0; i < BPNetwork.Connections.ConnectionList.Count; i++)
            {
                double actualGradient = BPNetwork.Connections.ConnectionList[i].Gradient;
                double epsilon = 0.0001;
                BPNetwork.Connections.ConnectionList[i].Weight += epsilon;
                double error1 = GetNetworkError(BPNetwork.Predict(sampleData), sampleLabel);
                BPNetwork.Connections.ConnectionList[i].Weight -= 2 * epsilon;
                double error2 = GetNetworkError(BPNetwork.Predict(sampleData), sampleLabel);
                double expectedGradient = (error2 - error1) / (2 * epsilon);
                if (expectedGradient != 0 || actualGradient != 0)
                    Console.WriteLine("ExpectedGradient is {0}, ActualGradient is {1}", expectedGradient, actualGradient);
            }

        }
        static private double GetNetworkError(List<double> sampleLabel, List<double> perdiction)
        {
            double error = 0;
            for (int i = 0; i < sampleLabel.Count; i++)
            {
                error += (sampleLabel[i] - perdiction[i]) * (sampleLabel[i] - perdiction[i]);
            }
            error *= 0.5;
            return error;
        }
    }

}
