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


        public static string DecodeMTF(List<int> encoded, List<char> alphabet)
        {
            List<char> mtfList = new List<char>(alphabet);
            string result = "";

            foreach (int position in encoded)
            {
                char symbol = mtfList[position];
                result += symbol;

                mtfList.RemoveAt(position);
                mtfList.Insert(0, symbol);
            }

            return result;
        }



        //public static string DecodeBWT(string encoded)
        //{
        //    int length = encoded.Length;
        //    string[] rotations = new string[length];

        //    for (int i = 0; i < length; i++)
        //    {
        //        rotations[i] = encoded.Substring(length - i) + encoded.Substring(0, length - i);
        //    }
        //    return rotations[length-1];
        //}
    }
}