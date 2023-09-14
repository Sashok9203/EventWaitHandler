namespace EventWaitHandler
{
    internal class Program
    {
        private static readonly string numbersPath = @"D:\Numbers.txt";
        private static readonly string sumNumbersPath = @"D:\SumNumbers.txt";
        private static readonly string mulNumbersPath = @"D:\MulNumbers.txt";
        static void Main(string[] args)
        {
            ManualResetEvent resetEvent = new(false);
            ThreadPool.QueueUserWorkItem(GenerateNumbers, resetEvent);
            ThreadPool.QueueUserWorkItem(NumbersSum, resetEvent);
            ThreadPool.QueueUserWorkItem(NumbersMul, resetEvent);
            Console.ReadKey();
        }


        static void GenerateNumbers(object o)
        {
            bool error = false;
            try
            {
                Random rnd = new();
                using StreamWriter sw = new(numbersPath);
                for (int i = 0; i < 10; i++)
                    sw.WriteLine(rnd.Next(1, 101));

            }
            catch
            {
                Console.WriteLine("File error !!!");
                error = true;
            }
            finally
            {
                if (!error) Console.WriteLine("Numbers generated");
                ((ManualResetEvent)o).Set();
            }
        }

        static void NumbersSum(object o)
        {
            bool error = false;
            ((ManualResetEvent)o).WaitOne();
            try
            {
                List<int> numbers = new();
                using StreamReader sr = new(numbersPath);
                while (!sr.EndOfStream)
                    numbers.Add(int.Parse(sr.ReadLine()));

                using StreamWriter sw = new(sumNumbersPath);
                for (int i = 0; i < numbers.Count - 1; i += 2)
                    sw.WriteLine(numbers[i] + numbers[i + 1]);

            }
            catch
            {
                Console.WriteLine("File error !!!");
                error = true;
            }
            finally
            {
                if (!error) Console.WriteLine("Numberss added");
            }
        }
        static void NumbersMul(object o)
        {
            bool error = false;
            ((ManualResetEvent)o).WaitOne();
            try
            {
                List<int> numbers = new();
                using StreamReader sr = new(numbersPath);
                while (!sr.EndOfStream)
                    numbers.Add(int.Parse(sr.ReadLine()));
                using StreamWriter sw = new(mulNumbersPath);
                for (int i = 0; i < numbers.Count - 1; i += 2)
                    sw.WriteLine(numbers[i] * numbers[i + 1]);
            }
            catch
            {
                Console.WriteLine("File error !!!");
                error = true;
            }
            finally
            {
                if (!error) Console.WriteLine("Numbers multiplied");
            }
        }
    }
}