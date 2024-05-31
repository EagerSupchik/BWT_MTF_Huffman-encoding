using System;
using System.Collections.Generic;

namespace StringEncode
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

    partial class Program
    {
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

        static byte[] Encode(string input, Dictionary<char, string> huffmanCodes)
        {
            var bits = new List<bool>();
            foreach (var ch in input)
            {
                if (huffmanCodes.TryGetValue(ch, out string code))
                {
                    foreach (var bit in code)
                    {
                        bits.Add(bit == '1');
                    }
                }
                else
                {
                    throw new KeyNotFoundException($"{ch} нет в словаре");
                }
            }

            int byteCount = (bits.Count + 7) / 8;
            byte[] encoded = new byte[byteCount];
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                {
                    encoded[i / 8] |= (byte)(1 << (7 - (i % 8)));
                }
            }

            return encoded;

        }

        static string Decode(byte[] encodedBytes, HuffmanNode root, ushort len)
        {
            var bits = new List<bool>();
            foreach (var b in encodedBytes)
            {
                for (int i = 0; i < 8; i++)
                {
                    bits.Add((b & (1 << (7 - i))) != 0);
                }
            }

            string result = "";
            HuffmanNode current = root;

            foreach (var bit in bits)
            {
                if (bit)
                {
                    current = current.Right;
                }
                else
                {
                    current = current.Left;
                }

                if (current.Character.HasValue)
                {
                    result += current.Character.Value;
                    current = root;
                }
            }

            result = result.Substring(0, len);

            return result;
        }
    }
}