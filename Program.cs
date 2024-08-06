using System;
using System.Reflection;

namespace RandomnessPrediction
{
    class Prediction
    {
        private int[] SeedArray;
        private int INext;
        private int INextP;
        private readonly Random Rand;

        public Prediction(Random r)
        {
            Rand = r;
            Init(r);
        }

        private Prediction(Prediction p)
        {
            SeedArray = new int[p.SeedArray.Length];
            for (int i = 0; i < p.SeedArray.Length; i++)
                SeedArray[i] = p.SeedArray[i];
            INext = p.INext;
            INextP = p.INextP;
        }

        private void Init(Random r)
        {
            FieldInfo seedarrayinfo = typeof(Random).GetField("SeedArray", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo inextinfo = typeof(Random).GetField("inext", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo inextpinfo = typeof(Random).GetField("inextp", BindingFlags.NonPublic | BindingFlags.Instance);
            INext = (int)inextinfo.GetValue(r);
            INextP = (int)inextpinfo.GetValue(r);
            SeedArray = (int[])seedarrayinfo.GetValue(r);
        }

        private int Next()
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

        private int Next(int max) => (int)(NextDouble() * max);

        private int Next(int min, int max)
        {
            long delta = max - min;
            if (delta <= int.MaxValue)
                return (int)(NextDouble() * delta) + min;
            int n = Next();
            if (Next() % 2 == 0)
                n = -n;
            return (int)(((n + 2147483646.0) / 4294967293.0 * delta) + min);
        }

        private double NextDouble() => 4.6566128752457969E-10 * Next();

        private void NextBytes(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (byte)(Next() % 0x100);
        }

        public void Predict()
        {
            Init(Rand);
            Console.WriteLine();
            Console.WriteLine("----------PREDICTION----------");
            Prediction temp = new Prediction(this);
            Console.WriteLine("Random.Next() = " + temp.Next());
            temp = new Prediction(this);
            Console.WriteLine("Random.NextDouble() = " + temp.NextDouble());
            Console.WriteLine("------------------------------");
            Console.WriteLine();
        }

        public void Predict(int max)
        {
            Init(Rand);
            Console.WriteLine();
            Console.WriteLine("----------PREDICTION----------");
            Prediction temp = new Prediction(this);
            Console.WriteLine("Random.Next() = " + temp.Next());
            temp = new Prediction(this);
            Console.WriteLine("Random.NextDouble() = " + temp.NextDouble());
            temp = new Prediction(this);
            Console.WriteLine("Random.Next(" + max + ") = " + temp.Next(max));
            Console.WriteLine("------------------------------");
            Console.WriteLine();
        }

        public void Predict(int min, int max)
        {
            Init(Rand);
            Console.WriteLine();
            Console.WriteLine("----------PREDICTION----------");
            Prediction temp = new Prediction(this);
            Console.WriteLine("Random.Next() = " + temp.Next());
            temp = new Prediction(this);
            Console.WriteLine("Random.NextDouble() = " + temp.NextDouble());
            temp = new Prediction(this);
            Console.WriteLine("Random.Next(" + max + ") = " + temp.Next(max));
            temp = new Prediction(this);
            Console.WriteLine("Random.Next(" + min + ", " + max + ") = " + temp.Next(min, max));
            Console.WriteLine("------------------------------");
            Console.WriteLine();
        }

        public void Predict(byte[] data)
        {
            Init(Rand);
            Console.WriteLine();
            Console.WriteLine("----------PREDICTION----------");
            Prediction temp = new Prediction(this);
            Console.WriteLine("Random.Next() = " + temp.Next());
            temp = new Prediction(this);
            Console.WriteLine("Random.NextDouble() = " + temp.NextDouble());
            temp = new Prediction(this);
            temp.NextBytes(data);
            Console.Write("Random.NextBytes(byte[" + data.Length + "]) =");
            foreach (byte b in data)
                Console.Write(" " + b);
            Console.WriteLine();
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

            p.Predict(0, 100000);
            Console.WriteLine("Random value: " + r.Next(0, 100000));

            p.Predict(69696969);
            Console.WriteLine("Random value: " + r.Next(69696969));

            p.Predict();
            Console.WriteLine("Random value: " + r.Next());

            p.Predict();
            Console.WriteLine("Random value: " + r.NextDouble());

            p.Predict(new byte[10]);
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
