using System;
using System.IO;
using System.Collections.Generic;
namespace DJOUZE
{
    class Program {
        static void invert(List<int> lista)
        {
            int aux = 0;
            for(int i = 0; i < lista.Count / 2; i++)
            {
                aux = lista[i];
                lista[i] = lista[lista.Count - 1-i];
                lista[lista.Count - 1 - i] = aux;
            }
        }
        static void Main(string[] args)
        {
            List<int> lista = new List<int>();
            Console.WriteLine("Quantos elementos?");
            int num = int.Parse(Console.ReadLine());
            for (int i = 0; i < num; i++)
            {
                lista.Add(int.Parse(Console.ReadLine()));
            }
            Console.WriteLine("Lista antes da inversao:");
            for(int i = 0; i < lista.Count; i++)
            {
                Console.Write(lista[i]+" ");
            }
            Console.WriteLine("");
            invert(lista);
            Console.WriteLine("Lista apos a inversao:");
            for (int i = 0; i < lista.Count; i++)
            {
                Console.Write(lista[i] + " ");
            }
            Console.WriteLine("");
        }
    }
}
