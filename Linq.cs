using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace DJOUZE
{
    class Program {
        class Pessoa
        {
            public string Nome;
            public int Idade;
        }
        static void Main(string[] args)
        {
            List<Pessoa> dados = new List<Pessoa> {
                new Pessoa{Nome = "Jose",Idade = 20},
                new Pessoa{Nome = "Pedro",Idade = 20},
                new Pessoa{Nome = "Sandor",Idade = 21},
                new Pessoa{Nome = "Thiago",Idade = 20},
                new Pessoa{Nome = "Andre",Idade = 19},
                new Pessoa{Nome = "Maria",Idade = 24},
                new Pessoa{Nome = "Julia",Idade = 13},
                new Pessoa{Nome = "Humberto",Idade = 11},
                new Pessoa{Nome = "Otto",Idade = 9},
                new Pessoa{Nome = "Gabriela",Idade = 17},
                new Pessoa{Nome = "Nina",Idade = 21},
                new Pessoa{Nome = "Luis",Idade = 15},
                new Pessoa{Nome = "Rodrigo",Idade = 17},
                new Pessoa{Nome = "Neide",Idade = 46},
                new Pessoa{Nome = "Samanta",Idade = 32},
            };
            var query = from P in dados
                        where P.Idade > 17
                        select P;
            
            Console.WriteLine("Maiores de Idade: ");
            foreach(Pessoa p in query)
            {
                Console.WriteLine(p.Nome+"\t"+p.Idade);
            }
        }
    }
}
