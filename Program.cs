using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEncode
{
    partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter a string to encode:");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    throw new ArgumentException("Input string cannot be empty");
                }

                Console.WriteLine($"Original string: {input}\n");

                int strLength = input.Length;
                HashSet<char> uniqueChars = new HashSet<char>(input);
                List<char> alphabet = uniqueChars.ToList();
                alphabet.Sort();

                var bwtResult = BWT(input);
                Console.WriteLine($"BWT result: {bwtResult}");
                var mtfResult = MTF(bwtResult);
                Console.WriteLine($"Move-to-front result: {mtfResult}");

                var frequency = BuildFrequencyDictionary(mtfResult);
                var priorityQueue = BuildPriorityQueue(frequency);
                var root = BuildHuffmanTree(priorityQueue);
                var huffmanCodes = BuildHuffmanCodes(root);

                var encoded = Encode(mtfResult, huffmanCodes);
                Console.WriteLine($"Huffman encoding result in bytes:\n");
                foreach (byte b in encoded)
                {
                    string binaryByteString = Convert.ToString(b, 2).PadLeft(8, '0');
                    Console.WriteLine(binaryByteString);
                }

                Console.WriteLine($"\nCompressed size: {encoded.Length} bytes");

                int originalSize = Encoding.Unicode.GetByteCount(input);
                Console.WriteLine($"Original string size in UTF-16: {originalSize} bytes");
                
                double compressionRatio = (1 - (double)encoded.Length / originalSize) * 100;
                Console.WriteLine($"Compression ratio: {compressionRatio:F2}%\n");

                var decodedMtf = Decode(encoded, root, strLength);
                Console.WriteLine($"MTF decoding from Huffman: {decodedMtf}");

                var decodedBWT = DecodeMTF(decodedMtf, alphabet);
                Console.WriteLine($"BWT decoding result: {decodedBWT}\n");

                var decodedString = DecodeBWT(decodedBWT);
                Console.WriteLine($"Final decoding result: {decodedString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

