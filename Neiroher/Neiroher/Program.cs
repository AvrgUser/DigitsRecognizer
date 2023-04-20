using System;
using System.Drawing;
using Microsoft.VisualBasic.FileIO;

namespace Neiroher
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("what to do?");
            var ans = Console.ReadLine();
            if (ans == "teach")
            {
                int[] used = new int[10];
                foreach (int i in used) used[i] = 0;
                Manager.Create(784, 16, 16, 10);
                string save = Console.ReadLine();
                if (save != "") if(File.Exists(Directory.GetCurrentDirectory()+@"\"+save+".txt"))
                {
                    Manager.LoadSave(save);
                }
                Console.WriteLine(Directory.GetCurrentDirectory() + @"\" + save);
                using (TextFieldParser parser = new TextFieldParser(@"C:\Users\Valera\Desktop\mnist_train.csv"))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int k = 0;
                    bool[] taken = new bool[60000];
                    string[] lines = new string[60000];
                    while (!parser.EndOfData)
                    {
                        
                        lines[k] = parser.ReadLine();
                        k++;
                    }
                    for (int ep = 0, am = k; ep < 10; ep++)
                    {
                        k = am;
                        Task networkJob = null;
                        while (k > 0)
                        {
                            k--;
                            int it = new Random().Next(0, k);
                            for (int i = 0, j = 0; i < it; i++)
                            {
                                if (!taken[i])
                                {
                                    if (j == it) it = i;
                                    j++;
                                }
                            }
                            taken[it] = true;
                            string[] pxs = lines[new Random().Next(0, lines.Length - 1)].Split(',');
                            float[] input = new float[pxs.Length - 1], answer = new float[10];
                            for (int j = 1; j < pxs.Length; j++)
                            {
                                input[j - 1] = Convert.ToSingle(pxs[j]) / 255;
                            }
                            for (int j = 0, a = Convert.ToInt32(pxs[0]); j < answer.Length; j++)
                            {
                                if (a == j)
                                {
                                    answer[j] = 1;
                                    used[j]++;
                                }
                                else answer[j] = 0;
                            }
                            if (networkJob != null) networkJob.Wait();
                            networkJob = Task.Run(() => { Manager.Teach(input, answer); });
                            if (k > 55000)
                            {
                                //Console.WriteLine("close");
                            }
                            if(k%5000==0) Console.WriteLine("here is " + ep + ", error is " + Manager.Network.avgEr);
                        }
                        
                    }
                    Manager.DoSave(save);
                }
            }
            else if (ans == "test")
            {
                Manager.Create(784, 16, 16, 10);
                string save = Console.ReadLine();
                Manager.LoadSave(save);
                using (TextFieldParser parser = new TextFieldParser(@"C:\Users\Valera\Desktop\mnist_test.csv"))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int r = 0, w = 0;
                    while (!parser.EndOfData)
                    {
                        string field = parser.ReadLine();
                        string[] pxs = field.Split(',');
                        float[] input = new float[pxs.Length - 1];
                        for (int i = 0; i < 784; i++)
                        {
                            input[i] = Convert.ToSingle(pxs[i+1]) / 255;
                        }
                        var answ = Manager.Get(input);
                        int biggest = 0;
                        for (int j = 0; j < answ.Length; j++)
                        {
                            if (answ[biggest] < answ[j]) biggest = j;
                        }
                        if (biggest == Convert.ToInt32(pxs[0]))
                        {
                            Console.WriteLine("right");
                            r++;
                        }
                        else
                        {
                            Console.WriteLine("wrong");
                            w++;
                        }
                    }
                    Console.WriteLine((float)r/(r+w)*100+"%");
                }
            }
            else if (ans == "work")
            {
                Manager.Create(784, 16, 16, 10);
                string save = Console.ReadLine();
                Manager.LoadSave(save);
                while (true)
                {
                    string path = Console.ReadLine();
                    Bitmap bitmap = new Bitmap(path);
                    float[] pixels = new float[bitmap.Width * bitmap.Height];
                    for (int x = 0, y = 0; y * 28 + x < pixels.Length; x++)
                    {
                        if (x > 27)
                        {
                            x = 0;
                            y++;
                        }
                        if (y > 27) break;
                        pixels[y * 28 + x] = bitmap.GetPixel(x, y).GetBrightness();
                    }
                    var a = Manager.Get(pixels);
                    int biggest = 0;
                    for(int j = 0;j < a.Length; j++)
                    {
                        if (a[biggest] < a[j]) biggest = j;
                        //Console.WriteLine(a[j].ToString()+';');
                    }
                    Console.WriteLine("answer is " + biggest);
                }
            }
        }
    }
}