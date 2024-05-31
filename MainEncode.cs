using System.Collections.Generic;
using System;
using System.Linq;


namespace StringEncode
{
    partial class Program
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


        public static string DecodeMTF(string encoded, List<char> alphabet)
        {
            List<char> mtfList = new List<char>(alphabet);
            string result = "";

            foreach(char c in encoded)
            {
                char symbol = mtfList[(int)(c - '0')];
                result += symbol;

                mtfList.RemoveAt((int)(c - '0'));
                mtfList.Insert(0, symbol);
            }

            return result;
        }

        public static string DecodeBWT(string bwt)
        {
            int length = bwt.Length;

            List<(char, int)> lastColumn = bwt.Select((c, i) => (c, i)).ToList();
            List<(char, int)> firstColumn = lastColumn.OrderBy(x => x.Item1).ToList();

            int[] next = new int[length];
            for (int i = 0; i < length; i++)
            {
                next[i] = firstColumn.IndexOf(lastColumn[i]);
            }
            char[] original = new char[length];
            int index = 0; 
            for (int i = 0; i < length; i++)
            {
                original[length - 1 - i] = bwt[index];
                index = next[index];
            }

            return new string(original);
        }
    }
}