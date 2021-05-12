using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
namespace DJOUZE
{
    class Program {
        class HashD
        {
            int profGlobal;
            int maxEntradas;
            public int tamNotes;
            string defaultNotes = "                                                            ";
            const string pathDiretorio = @"D:\C#\diretorio.txt";
            const string pathIndice = @"D:\C#\indice.txt";
            const string pathMestre = @"D:\C#\mestre.txt";
            public FileStream fsDir, fsIndice, fsMestre;
            public BinaryWriter bwDir, bwIndice, bwMestre;
            public BinaryReader brDir, brIndice, brMestre;
            public HashD(int prof,int maxEn,int tamNotas)
            {
                
                maxEntradas = maxEn;
                tamNotes = tamNotas;
                if (!File.Exists(pathDiretorio))
                {
                    profGlobal = prof;
                    bwDir = new BinaryWriter(File.Open(pathDiretorio, FileMode.Create));
                    bwIndice = new BinaryWriter(File.Open(pathIndice, FileMode.Create));
                    bwIndice.Close();
                    bwDir.Write(profGlobal);

                    for(int i = 0; i < (int)Math.Pow(2, profGlobal); i++)
                    {
                        bwDir.Write(i);
                        bwDir.Write(createBucket(1));
                    }
                    bwDir.Close();
                }
                else
                {
                    brDir = new BinaryReader(File.Open(pathDiretorio, FileMode.Open));
                    profGlobal = brDir.ReadInt32();
                    brDir.Close();
                }
            }
            public int createBucket(int profundidade)
            {
                fsIndice = new FileStream(pathIndice,FileMode.Open,FileAccess.Write);
                bwIndice = new BinaryWriter(fsIndice);
                int endereco = (int)fsIndice.Seek(0,SeekOrigin.End);
                bwIndice.Write(profundidade);
                for(int i = 0; i < maxEntradas; i++)
                {
                    bwIndice.Write(-1);
                    bwIndice.Write(-1);
                }
                fsIndice.Close();
                bwIndice.Close();
                return endereco;
            }
            public void splitBucket(int profLocal, int enderecoBucket,int cpf,int enderecoMestre)
            {
                int pointingToMe = (int)Math.Pow(2,profGlobal-profLocal);
                int enderecoNewBucket = createBucket(profLocal + 1);
                fsDir = new FileStream(pathDiretorio, FileMode.Open, FileAccess.ReadWrite);
                brDir = new BinaryReader(fsDir);
                bwDir = new BinaryWriter(fsDir);
                fsDir.Seek(4,SeekOrigin.Begin);
                int counter = 0,path = 0;
                while (path < (int)Math.Pow(2,profGlobal))
                {
                    brDir.ReadInt32();
                    if (brDir.ReadInt32() == enderecoBucket)
                    {
                        counter++;
                        if(counter > pointingToMe / 2)
                        {
                            fsDir.Seek(-4,SeekOrigin.Current);
                            bwDir.Write(enderecoNewBucket);
                        }
                    }
                    
                    path++;
                }
                bwDir.Close();
                brDir.Close();
                fsDir.Close();
                fsIndice = new FileStream(pathIndice, FileMode.Open, FileAccess.ReadWrite);
                bwIndice = new BinaryWriter(fsIndice);
                brIndice = new BinaryReader(fsIndice);
                fsIndice.Seek(enderecoBucket, SeekOrigin.Begin);
                bwIndice.Write(profLocal + 1);
                List<int> bucketContents = new List<int>();
                for(int i = 0; i < maxEntradas; i++)
                {
                    bucketContents.Add(brIndice.ReadInt32());
                    bucketContents.Add(brIndice.ReadInt32());
                }
                bucketContents.Add(cpf);
                bucketContents.Add(enderecoMestre);
                fsIndice.Seek(enderecoBucket+4, SeekOrigin.Begin);
                for (int i = 0; i < maxEntradas; i++)
                {
                    bwIndice.Write(-1);
                    bwIndice.Write(-1);
                }
                fsIndice.Close();
                bwIndice.Close();
                brIndice.Close();
                for(int i = 0; i < bucketContents.Count-1; i+=2)
                {
                    reinserir(bucketContents[i],bucketContents[i+1]);
                }
            }
            public void reinserir(int cpf, int enderecoMestre)
            {
                int i;
                bool bucketFull;
                fsDir = new FileStream(pathDiretorio, FileMode.Open, FileAccess.ReadWrite);
                brDir = new BinaryReader(fsDir);
                fsDir.Seek(4, SeekOrigin.Begin);
                int hash = cpf % (int)Math.Pow(2, profGlobal);
                while (brDir.ReadInt32() != hash) brDir.ReadInt32();
                int enderecoBucket = brDir.ReadInt32();
                brDir.Close();
                fsDir.Close();
                fsIndice = new FileStream(pathIndice, FileMode.Open, FileAccess.ReadWrite);
                bwIndice = new BinaryWriter(fsIndice);
                brIndice = new BinaryReader(fsIndice);
                fsIndice.Seek(enderecoBucket, SeekOrigin.Begin);
                int profLocal = brIndice.ReadInt32();
                for (i = 0; i < maxEntradas; i++)
                {
                    if (brIndice.ReadInt32() == -1)
                    {
                        fsIndice.Seek(-4, SeekOrigin.Current);
                        bwIndice.Write(cpf);
                        bwIndice.Write(enderecoMestre);
                        i = maxEntradas + 1;
                        fsIndice.Seek(0,SeekOrigin.Begin);
                    }
                    brIndice.ReadInt32();
                }
                bwIndice.Close();
                brIndice.Close();
                fsIndice.Close();
                if (i == maxEntradas) bucketFull = true;
                else bucketFull = false;
                if (bucketFull)
                {
                    if (profLocal < profGlobal)
                    {
                        splitBucket(profLocal, enderecoBucket, cpf, enderecoMestre);
                    }
                    else
                    {
                        fsDir = new FileStream(pathDiretorio, FileMode.Open, FileAccess.ReadWrite);
                        brDir = new BinaryReader(fsDir);
                        bwDir = new BinaryWriter(fsDir);
                        fsDir.Seek(4, SeekOrigin.Begin);
                        List<int> enderecos = new List<int>();
                        for (int j = 0; j < (int)Math.Pow(2, profGlobal); j++)
                        {
                            brDir.ReadInt32();
                            enderecos.Add(brDir.ReadInt32());
                        }
                        int index = 0;
                        for (int j = (int)Math.Pow(2, profGlobal); j < (int)Math.Pow(2, profGlobal + 1); j++)
                        {
                            bwDir.Write(j);
                            bwDir.Write(enderecos[index]);
                            index++;
                        }
                        profGlobal++;
                        fsDir.Seek(0, SeekOrigin.Begin);
                        bwDir.Write(profGlobal);
                        bwDir.Close();
                        brDir.Close();
                        fsDir.Close();
                        splitBucket(profLocal, enderecoBucket, cpf, enderecoMestre);
                    }
                }
            }
            public void insert(int cpf,string nome,string data,char sexo)
            {
                int i = 0;
                bool bucketFull = false;
                int enderecoMestre;
                if (!File.Exists(pathMestre))
                {
                    fsMestre = new FileStream(pathMestre, FileMode.Create, FileAccess.ReadWrite);
                    bwMestre = new BinaryWriter(fsMestre);
                }
                else
                {
                    fsMestre = new FileStream(pathMestre, FileMode.Open, FileAccess.ReadWrite);
                    bwMestre = new BinaryWriter(fsMestre);
                }
                enderecoMestre = (int)fsMestre.Seek(0, SeekOrigin.End);
                //registro enviado e inserido no arquivo mestre
                bwMestre.Write(nome);
                bwMestre.Write(data);
                bwMestre.Write(sexo);
                bwMestre.Write(defaultNotes);
                bwMestre.Close();
                fsMestre.Close();
                //endereco do registro e guardado nos arquivos de indice
                fsDir = new FileStream(pathDiretorio, FileMode.Open, FileAccess.ReadWrite);
                brDir = new BinaryReader(fsDir);
                fsDir.Seek(4,SeekOrigin.Begin);
                int hash = cpf % (int)Math.Pow(2, profGlobal);
                while (brDir.ReadInt32() != hash) brDir.ReadInt32();
                int enderecoBucket = brDir.ReadInt32();
                brDir.Close();
                fsDir.Close();
                fsIndice = new FileStream(pathIndice,FileMode.Open,FileAccess.ReadWrite);
                bwIndice = new BinaryWriter(fsIndice);
                brIndice = new BinaryReader(fsIndice);
                fsIndice.Seek(enderecoBucket, SeekOrigin.Begin);
                int profLocal = brIndice.ReadInt32();
                for (i = 0; i < maxEntradas; i++)
                {
                    if(brIndice.ReadInt32() == -1)
                    {
                        fsIndice.Seek(-4,SeekOrigin.Current);
                        bwIndice.Write(cpf);
                        bwIndice.Write(enderecoMestre);
                        i = maxEntradas + 1;
                        fsIndice.Seek(0, SeekOrigin.Begin);
                    }
                    brIndice.ReadInt32();
                }
                bwIndice.Close();
                brIndice.Close();
                fsIndice.Close();
                if (i == maxEntradas) bucketFull = true;
                //se o bucket nao tem espaco, ou sera necessario somente duplicar o bucket ou tambem sera necessario duplicar o diretorio
                if (bucketFull)
                {
                    if(profLocal < profGlobal)
                    {
                        splitBucket(profLocal,enderecoBucket, cpf, enderecoMestre);
                    }
                    else
                    {
                        fsDir = new FileStream(pathDiretorio, FileMode.Open, FileAccess.ReadWrite);
                        brDir = new BinaryReader(fsDir);
                        bwDir = new BinaryWriter(fsDir);
                        fsDir.Seek(4, SeekOrigin.Begin);
                        List<int> enderecos = new List<int>();
                        for (int j = 0; j < (int)Math.Pow(2, profGlobal); j++)
                        {
                            brDir.ReadInt32();
                            enderecos.Add(brDir.ReadInt32());
                        }
                        int index = 0;
                        for (int j = (int)Math.Pow(2,profGlobal); j < (int)Math.Pow(2, profGlobal+1); j++)
                        {
                            bwDir.Write(j);
                            bwDir.Write(enderecos[index]);
                            index++;
                        }
                        profGlobal++;
                        fsDir.Seek(0, SeekOrigin.Begin);
                        bwDir.Write(profGlobal);
                        bwDir.Close();
                        brDir.Close();
                        fsDir.Close();
                        splitBucket(profLocal,enderecoBucket,cpf,enderecoMestre);
                    }
                }
            }
            public int search(int cpf)
            {
                int enderecoNotes = -1;
                fsDir = new FileStream(pathDiretorio,FileMode.Open,FileAccess.Read);
                brDir = new BinaryReader(fsDir);
                int hash = cpf % (int)Math.Pow(2,profGlobal);
                fsDir.Seek(8 + (8 * hash), SeekOrigin.Begin);
                int enderecoBucket = brDir.ReadInt32();
                brDir.Close();
                fsDir.Close();

                fsIndice = new FileStream(pathIndice, FileMode.Open, FileAccess.Read);
                brIndice = new BinaryReader(fsIndice);
                fsIndice.Seek(enderecoBucket+4,SeekOrigin.Begin);
                int i = 0,enderecoMestre = 0;
                bool found = false;
                while(i < maxEntradas && !found)
                {
                    if(brIndice.ReadInt32() == cpf)
                    {
                        enderecoMestre = brIndice.ReadInt32();
                        found = true;
                    } else brIndice.ReadInt32();
                    i++;
                }
                brIndice.Close();
                fsIndice.Close();
                if (!found) Console.WriteLine("Registro nao existe!");
                else
                {
                    fsMestre = new FileStream(pathMestre,FileMode.Open,FileAccess.ReadWrite);
                    brMestre = new BinaryReader(fsMestre);
                    fsMestre.Seek(enderecoMestre,SeekOrigin.Begin);
                    Console.WriteLine("Registro encontrado:\n"+
                        "Nome: "+brMestre.ReadString()+"\n"+
                        "Aniversario: " + brMestre.ReadString() + "\n"+
                        "Sexo: " + brMestre.ReadChar());
                    enderecoNotes = (int)fsMestre.Position;
                    string notes;
                    if ((notes = brMestre.ReadString()) == defaultNotes)
                        Console.WriteLine("Notas: VAZIO");
                    else Console.WriteLine("Notas: " + notes);
                }
                brMestre.Close();
                fsMestre.Close();
                return enderecoNotes;
            }
            public void editNotes(int enderecoNotas,string newNotes)
            {
                fsMestre = new FileStream(pathMestre, FileMode.Open, FileAccess.ReadWrite);
                bwMestre = new BinaryWriter(fsMestre);
                fsMestre.Seek(enderecoNotas,SeekOrigin.Begin);
                int resto = tamNotes - newNotes.Length;
                for (int i = 0; i < resto; i++) newNotes += " ";
                bwMestre.Write(newNotes);
                bwMestre.Close();
                fsMestre.Close();
                return;
            }
            public void delete(int cpf)
            {
                fsDir = new FileStream(pathDiretorio, FileMode.Open, FileAccess.Read);
                brDir = new BinaryReader(fsDir);
                int hash = cpf % (int)Math.Pow(2, profGlobal);
                fsDir.Seek(8 + (8 * hash), SeekOrigin.Begin);
                int enderecoBucket = brDir.ReadInt32();
                brDir.Close();
                fsDir.Close();

                fsIndice = new FileStream(pathIndice, FileMode.Open, FileAccess.ReadWrite);
                brIndice = new BinaryReader(fsIndice);
                bwIndice = new BinaryWriter(fsIndice);
                fsIndice.Seek(enderecoBucket + 4, SeekOrigin.Begin);
                int i = 0;
                bool found = false;
                while (i < maxEntradas && !found)
                {
                    if (brIndice.ReadInt32() == cpf)
                    {
                        fsIndice.Seek(fsIndice.Position - 4, SeekOrigin.Begin);
                        bwIndice.Write(-1);
                        bwIndice.Write(-1);
                        found = true;
                    }
                    else brIndice.ReadInt32();
                    i++;
                }
                brIndice.Close();
                fsIndice.Close();
                if (!found) Console.WriteLine("O registro nao foi encontrado/nao existe");
            }
            public void status()
            {
                fsDir = new FileStream(pathDiretorio,FileMode.Open,FileAccess.Read);
                brDir = new BinaryReader(fsDir);
                fsIndice = new FileStream(pathIndice, FileMode.Open, FileAccess.Read);
                brIndice = new BinaryReader(fsIndice);
                Console.WriteLine("Arquivo Diretorio:\nPROFUNDIDADE GLOBAL: "+brDir.ReadInt32()+"\n\tCHAVE\t\tENDERECO");
                for(int i = 0; i < (int)Math.Pow(2,profGlobal); i++)
                {
                    Console.WriteLine("\t"+brDir.ReadInt32()+"\t"+brDir.ReadInt32());
                }
                Console.WriteLine("Arquivo Indice:\n");
                int endFim = (int)fsIndice.Seek(0, SeekOrigin.End);
                fsIndice.Seek(0, SeekOrigin.Begin);
                int k = 0;
                while ((int)fsIndice.Position != endFim)
                {
                    Console.WriteLine("BUCKET " + k);
                    Console.WriteLine("\t" + "PROFUNDIDADE LOCAL: " + brIndice.ReadInt32() + "\n\tCPF\tENDERECO");
                    for (int j = 0; j < maxEntradas; j++)
                    {
                        Console.WriteLine("\t" + brIndice.ReadInt32() + "\t" + brIndice.ReadInt32());
                    }
                    k++;
                }
                fsDir.Close();
                brDir.Close();
                fsIndice.Close();
                brIndice.Close();

            }
        }
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            int cpf;
            string nome, data;
            char sexo;
            HashD tabela = new HashD(1,2,60);
            Console.WriteLine("1-Inserir\n2-Remover\n3-Editar notas\n4-Mostrar status dos arquivos\n0-Sair");
            int option = int.Parse(Console.ReadLine());
            while (option != 0)
            {
                switch (option)
                {
                    case 1:
                        Console.WriteLine("Insira cpf(primeiros 9 digitos):");
                        cpf = int.Parse(Console.ReadLine());
                        Console.WriteLine("Insira nome:");
                        nome = Console.ReadLine();
                        Console.WriteLine("Insira data de nascimento(dd/MM/AAAA):");
                        data = Console.ReadLine();
                        Console.WriteLine("Insira sexo(F ou M):");
                        sexo = Console.ReadLine()[0];
                        stopwatch.Restart();
                        stopwatch.Start();
                        tabela.insert(cpf, nome, data, sexo);
                        stopwatch.Stop();
                        Console.WriteLine("Inserido, com tempo para inserção de "+stopwatch.ElapsedMilliseconds+"ms");
                        break;
                    case 2:
                        Console.WriteLine("Insira cpf para pesquisa(primeiros 9 digitos): ");
                        cpf = int.Parse(Console.ReadLine());
                        stopwatch.Restart();
                        stopwatch.Start();
                        tabela.delete(cpf);
                        stopwatch.Stop();
                        Console.WriteLine("Registro removido com sucesso em "+ stopwatch.ElapsedMilliseconds+"ms");
                        break;
                    case 3:
                        Console.WriteLine("Insira cpf para pesquisa(primeiros 9 digitos): ");
                        cpf = int.Parse(Console.ReadLine());
                        stopwatch.Restart();
                        stopwatch.Start();
                        int enderecoNotes = tabela.search(cpf);
                        stopwatch.Stop();
                        Console.WriteLine("Escreva novas anotacoes(Maximo de "+tabela.tamNotes+" caracteres): ");
                        string notas = Console.ReadLine();
                        while(notas.Length > tabela.tamNotes)
                        {
                            Console.WriteLine("Limite de caracteres excedido, favor re-escrever");
                            notas = Console.ReadLine();
                        }
                        stopwatch.Start();
                        tabela.editNotes(enderecoNotes,notas);
                        stopwatch.Stop();
                        Console.WriteLine("Terminado, com tempo de "+stopwatch.ElapsedMilliseconds+"ms");
                        break;
                    case 4:
                        tabela.status();
                        break;
                }
                Console.WriteLine("1-Inserir\n2-Remover\n3-Editar notas\n4-Mostrar status dos arquivos\n0-Sair");
                option = int.Parse(Console.ReadLine());
            }
        }
    }
}
