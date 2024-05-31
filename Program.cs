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

            ushort str_len = (ushort)str.Length;
            HashSet<char> uniq = new HashSet<char>(str);
            List<char> alphabet = uniq.ToList();
            alphabet.Sort();


            var result = BWT(str);
            Console.WriteLine($"Результат BWT: {result}");
            var mtf = MTF(result);
            Console.WriteLine($"Результат move-to-forward: {mtf}");


            var frequency = BuildFrequencyDictionary(mtf);
            var priorityQueue = BuildPriorityQueue(frequency);
            var root = BuildHuffmanTree(priorityQueue);
            var huffmanCodes = BuildHuffmanCodes(root);

            var encoded = Encode(mtf, huffmanCodes);
            Console.WriteLine($"Результат кодирования Хаффмана в массиве байт:\n");
            foreach (byte b in encoded)
            {
                string binaryByteString = Convert.ToString(b, 2);
                Console.WriteLine(binaryByteString);
            }

            Console.WriteLine($"\nЗанимаемое место в памяти {encoded.Length} байт");

            int oldSize = Encoding.Unicode.GetByteCount(str);

            Console.WriteLine($"Занимаемое место в памяти изначальной строчки в кодировке UTF-16: {oldSize} байт");
            double compressionRatio = (1 - (double)encoded.Length / oldSize) * 100;
            Console.WriteLine($"Процент сжатия: {compressionRatio:F2}%\n");

            var decodedMtf = Decode(encoded, root, str_len);
            Console.WriteLine($"Декодирование mtf из Хаффмана: {decodedMtf}");

            var decodedBWT = DecodeMTF(decodedMtf, alphabet);
            Console.WriteLine($"Результат декодирования MTF в BWT: {decodedBWT}\n");

            var decodedString = DecodeBWT(decodedBWT);
            Console.WriteLine($"Результат полного декодирования: {decodedString}");

        }
    }
}

