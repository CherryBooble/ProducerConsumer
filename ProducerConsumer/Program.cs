using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer
{
    class Program
    {
        const int size = 3;

        //test commit

        //test branch

        //static Semaphore s = new Semaphore(1, 1);

        static readonly object _locker = new object();

        static Semaphore n = new Semaphore(size, size);

        static Semaphore e = new Semaphore(0, size);

        static Queue<int> queue = new Queue<int>();

        static Random rand = new Random();

        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread produceThread = new Thread(Produce);
                produceThread.Name = $"Thread №{i + 1}";
                produceThread.Start();
            }

            for (int i = 0; i < 5; i++)
            {
                Thread consumeThread = new Thread(Consume);
                consumeThread.Name = $"Thread №{i + 1}";
                consumeThread.Start();
            }



            //for (int i = 0; i < 5; i++)
            //{
            //    ThreadPool.QueueUserWorkItem(Produce);
            //    ThreadPool.QueueUserWorkItem(Consume);
            //}


            Console.ReadLine();

        }

        public static void Produce(object state)
        {
            int count = 3;

            while (count > 0)
            {

                //produce time
                Thread.Sleep(1000);

                int res = rand.Next(1, 10);


                n.WaitOne();

                lock (_locker)
                {
                    //s.WaitOne();
                    queue.Enqueue(res);
                    Console.WriteLine();
                    Console.WriteLine($"{Thread.CurrentThread.Name} produce val {res}");
                    Console.WriteLine("Очередь: " + string.Join(" ", queue));
                    Console.WriteLine();
                    //s.Release();

                }
                e.Release();

                count--;
            }

        }

        public static void Consume(object state)
        {
            int count = 3;

            while (count > 0)
            {
                e.WaitOne();

                lock (_locker)
                {
                    //s.WaitOne();
                    var val = queue.Dequeue();

                    Console.WriteLine();
                    Console.WriteLine($"{Thread.CurrentThread.Name} consume val {val}");
                    Console.WriteLine("Очередь: " + string.Join(" ", queue));
                    Console.WriteLine();
                    //s.Release();
                }

                n.Release();

                count--;

                //consume time
                Thread.Sleep(1000);
            }
        }
    }
}
