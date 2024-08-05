using System;
using System.Reflection;

namespace RandomnessPrediction
{
    class Prediction
    {
        private readonly int[] SeedArray;
        private int INext;
        private int INextP;

        public Prediction(Random r)
        {
            FieldInfo seedarrayinfo = typeof(Random).GetField("SeedArray", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo inextinfo = typeof(Random).GetField("inext", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo inextpinfo = typeof(Random).GetField("inextp", BindingFlags.NonPublic | BindingFlags.Instance);           
            INext = (int)inextinfo.GetValue(r);
            INextP = (int)inextpinfo.GetValue(r);
            int[] seedarray = (int[])seedarrayinfo.GetValue(r);
            SeedArray = new int[seedarray.Length];
            for (int i = 0; i < seedarray.Length; i++)
                SeedArray[i] = seedarray[i];
        }

        private Prediction(Prediction p)
        {
            SeedArray = new int[p.SeedArray.Length];
            for (int i = 0; i < p.SeedArray.Length; i++)
                SeedArray[i] = p.SeedArray[i];
            INext = p.INext;
            INextP = p.INextP;
        }

        public int Next()
        {
            if (++INext >= SeedArray.Length)
                INext = 1;
            if (++INextP >= SeedArray.Length)
                INextP = 1;
            int result = SeedArray[INext] - SeedArray[INextP];
            if (result == int.MaxValue)
                result--;
            if (result < 0)
                result += int.MaxValue;
            SeedArray[INext] = result;
            return result;
        }

        public int Next(int max) => (int)(NextDouble() * max);

        public int Next(int min, int max)
        {
            long delta = max - min;
            if (delta <= int.MaxValue)
                return (int)(NextDouble() * delta) + min;
            int n = Next();
            if (Next() % 2 == 0)
                n = -n;
            return (int)(((n + 2147483646.0) / 4294967293.0 * delta) + min);
        }

        public double NextDouble() => 4.6566128752457969E-10 * Next();

        public void NextBytes(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (byte)(Next() % 0x100);
        }

        public void Print(int min = -1, int max = -1, int bytes = -1)
        {
            Console.WriteLine();
            Console.WriteLine("----------PREDICTION----------");
            Prediction temp = new Prediction(this);
            Console.WriteLine("Random.Next() = " + temp.Next());
            if (max != -1 || min != -1)
            {
                temp = new Prediction(this);
                Console.WriteLine("Random.Next(max) = " + temp.Next(min == -1 ? max : min));
            }
            if (min != -1 && max != -1)
            {
                temp = new Prediction(this);
                Console.WriteLine("Random.Next(min, max) = " + temp.Next(min, max));
            }
            temp = new Prediction(this);
            Console.WriteLine("Random.NextDouble() = " + temp.NextDouble());
            if (bytes != -1)
            {
                temp = new Prediction(this);
                byte[] data = new byte[bytes];
                temp.NextBytes(data);
                Console.Write("Random.NextBytes(bytes) =");
                foreach (byte b in data)
                    Console.Write(" " + b);
                Console.WriteLine();
            }
            Console.WriteLine("------------------------------");
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main()
        {
            Random r = new Random();
            Prediction p = new Prediction(r);

            p.Print(0, 100000);
            Console.WriteLine("Random value: " + r.Next(0, 100000));
            p.Next(0, 100000);

            p.Print(69696969);
            Console.WriteLine("Random value: " + r.Next(69696969));
            p.Next(69696969);

            p.Print();
            Console.WriteLine("Random value: " + r.Next());
            p.Next();

            p.Print();
            Console.WriteLine("Random value: " + r.NextDouble());
            p.NextDouble();

            p.Print(-1, -1, 10);
            Console.Write("Random value:");
            byte[] data = new byte[10];
            r.NextBytes(data);
            foreach (byte b in data)
                Console.Write(" " + b);
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}