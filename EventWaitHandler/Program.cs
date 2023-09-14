namespace EventWaitHandler
{
    internal class Program
    {

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
                using StreamWriter sw = new(@"D:\Numbers.txt");
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
                if (!error) Console.WriteLine("Number generated");
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
                using StreamReader sr = new(@"D:\Numbers.txt");
                while (!sr.EndOfStream)
                    numbers.Add(int.Parse(sr.ReadLine()));

                using StreamWriter sw = new(@"D:\SumNumbers.txt");
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
                if (!error) Console.WriteLine("Number added");
            }
        }
        static void NumbersMul(object o)
        {
            bool error = false;
            ((ManualResetEvent)o).WaitOne();
            try
            {
                List<int> numbers = new();
                using StreamReader sr = new(@"D:\Numbers.txt");
                while (!sr.EndOfStream)
                    numbers.Add(int.Parse(sr.ReadLine()));
                using StreamWriter sw = new(@"D:\MulNumbers.txt");
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
                if (!error) Console.WriteLine("Number multiplied");
            }
        }
    }
}