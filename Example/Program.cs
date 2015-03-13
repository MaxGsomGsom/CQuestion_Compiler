using System;

namespace MathProg
{
    public class MathFunctions
    {

        static public int NumAverage(int n)
        {
            int x = 0;
            for (int i = 0; i < n; i = i + 1)
            {
                string cur_num_str = Console.ReadLine();
                int cur_num = Convert.ToInt32(cur_num_str);
                x = x + cur_num;
            }

            x = x / n;
            return x;
        }

        static public void QuadEquationTry(int a, int b, int c)
        {
            int d = b * b - 4 * a * c;
            if (d < 0)
            {
                Console.WriteLine("Нет действительных корней");
            }
            else if (d == 0)
            {
                Console.WriteLine("Существует 1 корень");
            }
            else
            {
                Console.WriteLine("Существует 2 различных корня");
            }

        }
    }




    class Program
    {
        static int Main()
        {
            Console.WriteLine("Выберите функцию:");
            Console.WriteLine("1. Определить среднее арифметическое нескольких чисел");
            Console.WriteLine("2. Проверка на существование корней квадратного уравнения");
            string variant_string = Console.ReadLine();
            int variant = Convert.ToInt32(variant_string);

            if (variant == 1)
            {
                Console.WriteLine("Введите количество чисел:");
                string num_string = Console.ReadLine();
                int num = Convert.ToInt32(num_string);
                Console.WriteLine("Введите числа:");

                int result = MathFunctions.NumAverage(num);

                string result_string = Convert.ToString(result);
                Console.WriteLine("Ответ:");
                Console.WriteLine(result_string);
            }
            else if (variant == 2)
            {
                Console.WriteLine("Введите 3 коэффицента:");
                string a_str, b_str, c_str;
                a_str = Console.ReadLine();
                b_str = Console.ReadLine();
                c_str = Console.ReadLine();
                int a, b, c;
                a = Convert.ToInt32(a_str);
                b = Convert.ToInt32(b_str);
                c = Convert.ToInt32(c_str);

                MathFunctions.QuadEquationTry(a, b, c);
            }
            Console.ReadLine();

            return 0;
        }
    }

}
