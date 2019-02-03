using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCNN
{

    static class MnistDataReader
    {
        static public List<List<double>> Read(string filename)
        {
            int lastIndexOfDot = filename.LastIndexOf('.');
            if (lastIndexOfDot == -1 || lastIndexOfDot == filename.Length - 1) return null;
            string extension = filename.Substring(lastIndexOfDot + 1);
            // If it's image data
            if (extension == "idx3-ubyte") return ReadImages(filename);
            // Or if it's label data
            if (extension == "idx1-ubyte") return ReadLabels(filename);

            return null;
        }
        static private List<List<double>> ReadImages(string filename)
        {
            BinaryReader images = new BinaryReader(new FileStream(filename, FileMode.Open));
            int magicNumber = ReadInt32BasedOnEndian(images);
            int numberOfItems = ReadInt32BasedOnEndian(images);
            int numberOfRows = ReadInt32BasedOnEndian(images);
            int numberOfColumns = ReadInt32BasedOnEndian(images);
            int numberOfPixelsOfItem = numberOfRows * numberOfColumns;

            List<List<double>> dataSet = new List<List<double>>();
            for (int i = 0; i < numberOfItems; i++)
            {
                List<double> data = new List<double>();
                byte[] pixels = images.ReadBytes(numberOfPixelsOfItem);
                foreach (byte item in pixels)
                {
                    data.Add((double)item);
                }
                dataSet.Add(data);
            }
            return dataSet;
        }
        static private List<List<double>> ReadLabels(string filename)
        {
            BinaryReader labels = new BinaryReader(new FileStream(filename, FileMode.Open));
            int magicNumber = ReadInt32BasedOnEndian(labels);
            int numberOfItems = ReadInt32BasedOnEndian(labels);

            List<List<double>> dataSet = new List<List<double>>();
            for (int i = 0; i < numberOfItems; i++)
            {
                List<double> data = new List<double>();
                int label = (int)labels.ReadByte();
                for (int j = 0; j < 10; j++)
                {
                    if (j == label) data.Add(0.9);
                    else data.Add(0.1);
                }
                dataSet.Add(data);
            }
            return dataSet;
        }
        static private int ReadInt32BasedOnEndian(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(4);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }

}
