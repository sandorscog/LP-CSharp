using System;
using System.IO;
using System.Collections.Generic;
namespace DJOUZE
{
    class Program {
        static void addAlphabet(Dictionary<string, string> lettertoMorse)
        {
            lettertoMorse.Add(" ", "/");
            lettertoMorse.Add("a", ".-");
            lettertoMorse.Add("b", "-...");
            lettertoMorse.Add("c", "-.-.");
            lettertoMorse.Add("d", "-..");
            lettertoMorse.Add("e", ".");
            lettertoMorse.Add("f", "..-.");
            lettertoMorse.Add("g", "--.");
            lettertoMorse.Add("h", "....");
            lettertoMorse.Add("i", "..");
            lettertoMorse.Add("j", ".---");
            lettertoMorse.Add("k", "-.-");
            lettertoMorse.Add("l", ".-..");
            lettertoMorse.Add("m", "--");
            lettertoMorse.Add("n", "-.");
            lettertoMorse.Add("o", "---");
            lettertoMorse.Add("p", ".--.");
            lettertoMorse.Add("q", "--.-");
            lettertoMorse.Add("r", ".-.");
            lettertoMorse.Add("s", "...");
            lettertoMorse.Add("t", "-");
            lettertoMorse.Add("u", "..-");
            lettertoMorse.Add("v", "...-");
            lettertoMorse.Add("w", ".--");
            lettertoMorse.Add("x", "-..-");
            lettertoMorse.Add("y", "-.--");
            lettertoMorse.Add("z", "--..");
            lettertoMorse.Add("A", ".-");
            lettertoMorse.Add("B", "-...");
            lettertoMorse.Add("C", "-.-.");
            lettertoMorse.Add("D", "-..");
            lettertoMorse.Add("E", ".");
            lettertoMorse.Add("F", "..-.");
            lettertoMorse.Add("G", "--.");
            lettertoMorse.Add("H", "....");
            lettertoMorse.Add("I", "..");
            lettertoMorse.Add("J", ".---");
            lettertoMorse.Add("K", "-.-");
            lettertoMorse.Add("L", ".-..");
            lettertoMorse.Add("M", "--");
            lettertoMorse.Add("N", "-.");
            lettertoMorse.Add("O", "---");
            lettertoMorse.Add("P", ".--.");
            lettertoMorse.Add("Q", "--.-");
            lettertoMorse.Add("R", ".-.");
            lettertoMorse.Add("S", "...");
            lettertoMorse.Add("T", "-");
            lettertoMorse.Add("U", "..-");
            lettertoMorse.Add("V", "...-");
            lettertoMorse.Add("W", ".--");
            lettertoMorse.Add("X", "-..-");
            lettertoMorse.Add("Y", "-.--");
            lettertoMorse.Add("Z", "--..");
            lettertoMorse.Add("1", ".----");
            lettertoMorse.Add("2", "..---");
            lettertoMorse.Add("3", "...--");
            lettertoMorse.Add("4", "....-");
            lettertoMorse.Add("5", ".....");
            lettertoMorse.Add("6", "-....");
            lettertoMorse.Add("7", "--...");
            lettertoMorse.Add("8", "---..");
            lettertoMorse.Add("9", "----.");
            lettertoMorse.Add("0", "-----");
        }
        static void Main(string[] args)
        {
            Dictionary<string, string> lettertoMorse = new Dictionary<string, string>();
            addAlphabet(lettertoMorse);
            string frase = Console.ReadLine();
            while(frase != "")
            {
                string morseFrase = "";
                for (int i = 0; i <  frase.Length;i++)
                {
                    morseFrase += lettertoMorse[(""+frase[i])];
                    morseFrase += " ";
                }
                Console.WriteLine(morseFrase);
                frase = Console.ReadLine();
            }
            
        }
    }
}
