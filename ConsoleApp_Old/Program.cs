using System;
using System.Collections.Generic;

namespace ConsoleApp_Old
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var collect1 = new List<int>
            {
                5,6,7,2,4,3,4,5,6,8,9,10,11,12,13,14,15,16,17,
            };
            ShowALlElementsInCollectByIterations(collect1);
            ShowALlElementsInCollectByIndex(collect1);

            var collect2 = new int[]
            {
                5,6,7,2,4,3,4,5,6,8,9,10,11,12,13,14,15,16,17
            };
            ShowALlElementsInCollectByIterations(collect2);
            ShowALlElementsInCollectByIndex(collect2);
        }

        static void ShowALlElementsInCollectByIterations<T>(IEnumerable<T> elements)
        {
            foreach (T el in elements)
            {
                Console.WriteLine(el);
            }
        }

        static void ShowALlElementsInCollectByIndex<T>(IList<T> elements)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                Console.WriteLine(elements[i]);
            }
        }
    }
}
