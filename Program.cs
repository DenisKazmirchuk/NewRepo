using System;
using System.Text;
using System.IO;

namespace Person1
{
    class Person
    {
        string name;
        int birth_year;
        double pay;

        public Person() // конструктор без параметрів
        {
            name = "Anonimous";
            birth_year = 0;
            pay = 0;
        }

        public Person(string s) // конструктор з параметром (розбиття рядка)
        {
            string[] parts = s.Split(' '); // Розбиття рядка на частини
            if (parts.Length != 3)
                throw new FormatException("Невірний формат рядка");

            name = parts[0];
            birth_year = Convert.ToInt32(parts[1]);
            pay = Convert.ToDouble(parts[2]);

            if (birth_year < 0) throw new FormatException("Невірний рік народження");
            if (pay < 0) throw new FormatException("Невірний оклад");
        }

        public override string ToString() // перевизначений метод
        {
            return string.Format("Name: {0,30} birth: {1} pay: {2:F2}", name, birth_year, pay);
        }

        public int Compare(string name) // порівняння прізвища
        {
            return (string.Compare(this.name, 0, name + " ", 0, name.Length + 1, StringComparison.OrdinalIgnoreCase));
        }

        // Властивості класу
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Birth_year
        {
            get { return birth_year; }
            set
            {
                if (value > 0) birth_year = value;
                else throw new FormatException();
            }
        }

        public double Pay
        {
            get { return pay; }
            set
            {
                if (value > 0) pay = value;
                else throw new FormatException();
            }
        }

        // Операції класу
        public static double operator +(Person pers, double a)
        {
            pers.pay += a;
            return pers.pay;
        }

        public static double operator +(double a, Person pers)
        {
            pers.pay += a;
            return pers.pay;
        }

        public static double operator -(Person pers, double a)
        {
            pers.pay -= a;
            if (pers.pay < 0) throw new FormatException();
            return pers.pay;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person[] dbase = new Person[100];
            int n = 0;
            try
            {
                StreamReader f = new StreamReader("D:\\vcom\\project\\123.txt"); // зчитування даних з файлу
                string s;
                int i = 0;

                while ((s = f.ReadLine()) != null) // обробка кожного рядка
                {
                    dbase[i] = new Person(s); // створення об'єкта Person з рядка
                    Console.WriteLine(dbase[i]);
                    ++i;
                }
                n = i;
                f.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Перевірте правильність імені і шляху до файлу!");
                return;
            }

            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Дуже великий файл!");
                return;
            }
            catch (FormatException)
            {
                Console.WriteLine("Неприпустима дата народження або оклад");
                return;
            }

            catch (Exception e)
            {
                Console.WriteLine("Помилка: " + e.Message);
                return;
            }

            int n_pers = 0;
            double mean_pay = 0;
            Console.WriteLine("Введіть прізвище співробітника");
            string name;
            while ((name = Console.ReadLine()) != "") // пошук співробітника за прізвищем
            {
                bool not_found = true;
                for (int k = 0; k < n; ++k)
                {
                    Person pers = dbase[k];
                    if (pers.Compare(name) == 0)
                    {
                        Console.WriteLine(pers);
                        ++n_pers;
                        mean_pay += pers.Pay;
                        not_found = false;
                    }
                }
                if (not_found) Console.WriteLine("Такого співробітника немає");
                Console.WriteLine("Введіть прізвище співробітника або Enter для завершення");
            }
            if (n_pers > 0)
                Console.WriteLine("Середній оклад: {0:F2}", mean_pay / n_pers);
            Console.ReadKey();
        }
    }
}
