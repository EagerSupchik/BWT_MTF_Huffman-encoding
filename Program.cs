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
            string str = Console.ReadLine();
            Console.WriteLine($"Изначальная строка: {str}\n");

            HashSet<char> uniq = new HashSet<char>(str);
            List<char> alphabet = uniq.ToList();
            alphabet.Sort();


            var result = BWT(str);
            Console.WriteLine($"Результат BWT: {result}");
            var mtf = MTF(result);
            Console.WriteLine($"Результат move-to-forward: {mtf.ToString()}");


            var frequency = BuildFrequencyDictionary(mtf);
            var priorityQueue = BuildPriorityQueue(frequency);
            var root = BuildHuffmanTree(priorityQueue);
            var huffmanCodes = BuildHuffmanCodes(root);

            string encoded = Encode(mtf, huffmanCodes);
            Console.WriteLine($"Результат кодирования Хаффмана: {encoded}\n");

            var decodedMtf = Decode(encoded, root);
            Console.WriteLine($"Декодирование mtf из Хаффмана: {decodedMtf}");
            List<int> intDecodedMtf = decodedMtf.Select(c => int.Parse(c.ToString())).ToList();

            string decodedBWT = DecodeMTF(intDecodedMtf, alphabet);
            Console.WriteLine($"Результат декодирования MTF в BWT: {decodedBWT}\n");

            //string decodedString = DecodeBWT(decodedBWT);
            //Console.WriteLine($"Результат полного декодирования: {decodedString}");

        }
    }
}
