using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApp5
{
    class HuffmanNode
    {
        public char? Character { get; set; }
        public int Frequency { get; set; }
        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }
    }

    class HuffmanComparer : IComparer<HuffmanNode>
    {
        public int Compare(HuffmanNode x, HuffmanNode y)
        {
            int result = x.Frequency.CompareTo(y.Frequency);
            if (result == 0)
            {
                if (x.Character.HasValue && y.Character.HasValue)
                    return x.Character.Value.CompareTo(y.Character.Value);
                else if (x.Character.HasValue)
                    return -1;
                else if (y.Character.HasValue)
                    return 1;
            }
            return result;
        }
    }

    internal class Program
    {

        public static string BWT(string s)
        {
            int length = s.Length;
            string[] rotations = new string[length];

            for (int i = 0; i < length; i++)
            {
                rotations[i] = s.Substring(i) + s.Substring(0, i);
            }
            Array.Sort(rotations);

            char[] last = new char[length];

            for (int i = 0; i < length; i++)
            {
                last[i] = rotations[i][length - 1];
            }

            return new string(last);
        }


        public static string MTF(string s) 
        {
            int length = s.Length;
            string[] rotations = new string[length];

            HashSet<char> uniq = new HashSet<char>(s);
            List<char> alphabet = uniq.ToList();
            alphabet.Sort();

            List<int> result = new List<int>();

            foreach (char c in s)
            {
                int index = alphabet.FindIndex(chr => chr == c);
                result.Add(index);

                alphabet.RemoveAt(index);
                alphabet.Insert(0, c);
            }

            return string.Join("", result);
        }

        static Dictionary<char, int> BuildFrequencyDictionary(string input)
        {
            var frequency = new Dictionary<char, int>();
            foreach (var ch in input)
            {
                if (!frequency.ContainsKey(ch))
                    frequency[ch] = 0;
                frequency[ch]++;
            }
            return frequency;
        }

        static SortedSet<HuffmanNode> BuildPriorityQueue(Dictionary<char, int> frequency)
        {
            var priorityQueue = new SortedSet<HuffmanNode>(new HuffmanComparer());
            foreach (var kvp in frequency)
            {
                priorityQueue.Add(new HuffmanNode { Character = kvp.Key, Frequency = kvp.Value });
            }
            return priorityQueue;
        }

        static HuffmanNode BuildHuffmanTree(SortedSet<HuffmanNode> priorityQueue)
        {
            while (priorityQueue.Count > 1)
            {
                var first = priorityQueue.Min;
                priorityQueue.Remove(first);
                var second = priorityQueue.Min;
                priorityQueue.Remove(second);

                var newNode = new HuffmanNode
                {
                    Frequency = first.Frequency + second.Frequency,
                    Left = first,
                    Right = second
                };

                priorityQueue.Add(newNode);
            }
            return priorityQueue.Min;
        }

        static Dictionary<char, string> BuildHuffmanCodes(HuffmanNode root)
        {
            var huffmanCodes = new Dictionary<char, string>();
            BuildHuffmanCodesHelper(root, "", huffmanCodes);
            return huffmanCodes;
        }

        static void BuildHuffmanCodesHelper(HuffmanNode node, string code, Dictionary<char, string> huffmanCodes)
        {
            if (node == null)
                return;

            if (node.Character.HasValue)
            {
                huffmanCodes[node.Character.Value] = code;
            }

            BuildHuffmanCodesHelper(node.Left, code + "0", huffmanCodes);
            BuildHuffmanCodesHelper(node.Right, code + "1", huffmanCodes);
        }

        static string Encode(string input, Dictionary<char, string> huffmanCodes)
        {
            string encoded = "";
            foreach (var ch in input)
            {
                if (huffmanCodes.TryGetValue(ch, out string code))
                {
                    encoded += code;
                }
                else
                {
                    throw new KeyNotFoundException($"{ch} нет в словаре");
                }
            }
            return encoded;
        }

        static void Main(string[] args)
        {
            string str = "ABACABA";

            var result = BWT(str);
            Console.WriteLine(result);
            var mtf = MTF(result);
            Console.WriteLine(mtf.ToString());


            var frequency = BuildFrequencyDictionary(mtf);
            var priorityQueue = BuildPriorityQueue(frequency);
            var root = BuildHuffmanTree(priorityQueue);
            var huffmanCodes = BuildHuffmanCodes(root);

            string encoded = Encode(mtf, huffmanCodes);
            Console.WriteLine($"Результат: {encoded}");
        }
    }
}