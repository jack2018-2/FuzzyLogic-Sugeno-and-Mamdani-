using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuzzy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Расчет по Мамдани");
            StreamReader inpValues = new StreamReader("../InputValues.txt");

            foreach (var line in inpValues.ReadToEnd().Split(new char[] {'\n' }))
            {
                string[] arrLine = File.ReadAllLines("../Rules_Mamdani.txt");
                arrLine[15] = line;
                File.WriteAllLines("../Rules_Mamdani.txt", arrLine);
                var calculator = new Mamdani();
                //считываем данные из файла
                StreamReader reader = new StreamReader("../Rules_Mamdani.txt");
                var values = Parser.Parse(calculator, reader);

                var results = calculator.Calculate(values).ToArray();

                for (int i = 0; i < calculator.OutputVariables.Count; i++)
                {
                    Console.Write($"[{values[0]}, {values[1]}, {results[i].ToString("0.00").Replace(',', '.')}], ");
                }
                reader.Close();
            }
            inpValues.Close();

            Console.WriteLine("\n\nРасчет по Сугено");
            inpValues = new StreamReader("../InputValues.txt");
            foreach (var line in inpValues.ReadToEnd().Split(new char[] {'\n' }))
            {
                string[] arrLine = File.ReadAllLines("../Rules_Sugeno.txt");
                arrLine[arrLine.Length-1] = line;
                File.WriteAllLines("../Rules_Sugeno.txt", arrLine);
                var calculator = new Sugeno();
                //считываем данные из файла
                StreamReader reader = new StreamReader("../Rules_Sugeno.txt");
                var values = Parser.Parse(calculator, reader);

                var results = calculator.Calculate(values).ToArray();
                for (int i = 0; i < calculator.OutputVariables.Count; i++)
                {
                    Console.Write($"[{values[0]}, {values[1]}, {results[i].ToString("0.00").Replace(',', '.')}], ");
                }
                reader.Close();
            }

            Console.ReadKey();
        }
    }
}
