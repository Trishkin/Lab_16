using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using System.IO;

namespace Lab_16
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task12();
            //Task34();
            //Thread.Sleep(2000);
            //Task56();
            //Task7();
            Task8();
            Console.ReadKey();
        }
        public static void Task12()
        {
            Console.WriteLine("Введите число n элементов :");
            int n = Int32.Parse(Console.ReadLine());
            int[] arr = new int[n];
            for (int i = 0; i < n; i++)
            {
                arr[i] = i;
            }
            Stopwatch sw = new Stopwatch();
            CancellationTokenSource token = new CancellationTokenSource();
            for (int i = 0; i < 5; i++)
            {
                Task task;
                sw.Start();
                if (i != 2)
                {
                    task = new Task(() => Program.sieve_of_Eratosthenes(arr));
                }
                else
                {
                    task = new Task(() => Program.sieve_of_Eratosthenes(arr), token.Token);
                }
                if (i == 3)
                {
                    token.Cancel();
                }
                task.Start();
                sw.Stop();
                Console.WriteLine($"Время выполнения:   {sw.ElapsedMilliseconds} мл");
                Console.WriteLine($"ID:     {task.Id}");
                if (task.IsCompleted)
                {
                    Console.WriteLine("Задача завершилась");
                }
                else
                    Console.WriteLine("Задача еще не завершилась");
                Console.WriteLine($"Текущий состояние:  {task.Status}");
            }
        }
        public static void sieve_of_Eratosthenes(int[] arr)
        {
            int size = arr.Length;
            for (int k = 2; Math.Pow(k, 2) <= size; k++)
            {
                for (int i = 0; i < size; i++)
                {

                    if (arr[i] % k == 0 && k != arr[i])         //делаем нулями простые числа
                    {
                        arr[i] = 0;
                    }
                }
            }
            var temp_list = new List<int>(arr);             
            for (int i = 0; i < size; i++)
            {
                if (temp_list[i] == 0)
                {
                    size--;
                    temp_list.RemoveAt(i);
                    i--;
                }
            }
            foreach (int i in temp_list)
            {
                Console.WriteLine(i);
            }
        }


        public static void Task34()
        {
            Task<int>[] tasks1 = new Task<int>[3]
            {
                new Task<int>(() => Num(12) ),
                new Task<int>(() => Num(4)),
                new Task<int>(() =>  Num(5))
            };
            foreach (var t in tasks1)
                t.Start();
            Task rezult = new Task(() => Console.WriteLine("Результаты: " + tasks1[0].Result + " " + tasks1[1].Result + " " + tasks1[2].Result));
            var await = rezult.GetAwaiter();
            rezult.Start();
            Task continuetask = rezult.ContinueWith(t => Console.WriteLine("Конец"));
            await.OnCompleted(() => Console.WriteLine("It's ok"));
        }
        public static int Num(int a)
        {
            return (a +15 -85)*64/100;
        }
        public static void Task56()
        {
            Parallel.For(1, 6, Factorial);
            ParallelLoopResult result = Parallel.ForEach<int>(new List<int>() { 12, 8, 9, 11 }, Factorial);
            //Parallel.Invoke(Task34, Task12);
        }
        public static void Factorial(int x)
        {
            int result = 1;

            for (int i = 1; i <= x; i++)
            {
                result *= i;
            }
            Console.WriteLine($"ID задачи {Task.CurrentId}");
            Console.WriteLine($"Факториал числа {x} = {result}");
            Thread.Sleep(5000);
        }
        public static void Task7()
        {
            BlockingCollection<string> blockcoll = new BlockingCollection<string>();
            Task[] tasks1 = new Task[5]
            {
                new Task(() => {for(int i=0;i<4;i++)
                    { 
                        Console.WriteLine("Поставщик 1 доставил телевизор");
                        blockcoll.Add("телевизор"); 
                        Thread.Sleep(2000); 
                    } 
                } ),
                new Task(() => {for(int i=0;i<4;i++)
                    {Console.WriteLine("Поставщик 2 доставил микроволновую печь");
                        blockcoll.Add("микроволновая печь"); 
                        Thread.Sleep(2000); 
                    } 
                }),
                new Task(() => {for(int i=0;i<4;i++)
                    {Console.WriteLine("Поставщик 3 доставил холодильник");
                        blockcoll.Add("холодильник"); 
                        Thread.Sleep(1000); 
                    } 
                }),
                new Task(() => {for(int i=0;i<4;i++)
                    {
                        Console.WriteLine("Поставщик 4 доставил посудомоечная машина");
                        blockcoll.Add("посудомоечная машина"); 
                        Thread.Sleep(3000); 
                    } 
                }),
                new Task(() => {for(int i=0;i<4;i++)
                    {
                        Console.WriteLine("Поставщик 5 доставил пылесос");
                        blockcoll.Add("пылесос"); 
                        Thread.Sleep(1000); 
                    } 
                })
            };
            Task[] tasks2 = new Task[10]
            {
                new Task(  () =>
                {
                foreach (var item in blockcoll.GetConsumingEnumerable())
                {
                    Console.WriteLine("Покупатель 1 купил  " + item);
                    Thread.Sleep(3000);
                }
                } ),
                new Task( () =>
                {
                foreach (var item in blockcoll.GetConsumingEnumerable())
                {
                    Console.WriteLine("Покупатель 2 купил  " + item);
                         Thread.Sleep(3000);
                }
                }),
                new Task( () =>
                {
                foreach (var item in blockcoll.GetConsumingEnumerable())
                {
                    Console.WriteLine("Покупатель 3 купил  " + item);
                         Thread.Sleep(3000);
                }
                }),
                new Task( () =>
                {
                foreach (var item in blockcoll.GetConsumingEnumerable())
                {
                    Console.WriteLine("Покупатель 4 купил  " + item);
                         Thread.Sleep(3000);
                }
                }),
                new Task( () =>
                {
                foreach (var item in blockcoll.GetConsumingEnumerable())
                {
                    Console.WriteLine("Покупатель 5 купил  " + item);
                         Thread.Sleep(3000);
                }
                }),
                new Task( () =>
                {
                foreach (var item in blockcoll.GetConsumingEnumerable())
                {
                    Console.WriteLine("Покупатель 6 купил  " + item);
                         Thread.Sleep(3000);
                }
                }),
                new Task( () =>
                {
                foreach (var item in blockcoll.GetConsumingEnumerable())
                {
                    Console.WriteLine("Покупатель 7 купил  " + item);
                         Thread.Sleep(3000);
                }
                }),
                new Task( () =>
                {
                foreach (var item in blockcoll.GetConsumingEnumerable())
                {
                    Console.WriteLine("Покупатель 8 купил  " + item);
                         Thread.Sleep(3000);
                }
                }),
                new Task( () =>
                {
                foreach (var item in blockcoll.GetConsumingEnumerable())
                {
                    Console.WriteLine("Покупатель 9 купил  " + item);
                         Thread.Sleep(3000);
                }
                }),
                new Task( () =>
                {
                foreach (var item in blockcoll.GetConsumingEnumerable())
                {
                    Console.WriteLine("Покупатель 10 купил  " + item);
                         Thread.Sleep(3000);
                }
                })
           };
            for (int producer = 0; producer < 5; producer++)
            {
                tasks1[producer].Start();
            }
            for (int consumer = 0; consumer < 10; consumer++)
            {
                tasks2[consumer].Start();
            }

        }
        public static async void Task8()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("1546.txt", false, System.Text.Encoding.Default))
                {
                    await sw.WriteLineAsync("Starting");
                    Console.WriteLine("End");
                }
            }
                catch (Exception e)
                {
                Console.WriteLine(e.Message);
                }
            
        }
    }
}
