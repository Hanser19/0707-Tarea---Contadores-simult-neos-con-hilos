using System;
using System.Threading;

namespace ContadoresSimultaneos
{
    class Program
    {
        static bool[] isRunning;
        static Thread[] threads;
        static int[] counters;
        static int numCounters = 3;

        static void Main(string[] args)
        {
            // Inicializar contadores, threads, y banderas de estado
            isRunning = new bool[numCounters];
            counters = new int[numCounters];
            threads = new Thread[numCounters];

            for (int i = 0; i < numCounters; i++)
            {
                int index = i;
                threads[i] = new Thread(() => RunCounter(index, 1000 * (index + 1)));  // Cada contador con un intervalo diferente
            }

            Menu();
        }

        static void RunCounter(int index, int interval)
        {
            while (isRunning[index])
            {
                counters[index]++;
                Console.WriteLine($"Contador {index + 1}: {counters[index]}");
                Thread.Sleep(interval); // Esperar el tiempo especificado antes de incrementar el contador
            }
        }

        static void StartCounter(int index)
        {
            if (!isRunning[index])
            {
                isRunning[index] = true;
                threads[index].Start();
                Console.WriteLine($"Contador {index + 1} iniciado.");
            }
            else
            {
                Console.WriteLine($"Contador {index + 1} ya está en ejecución.");
            }
        }

        static void StopCounter(int index)
        {
            if (isRunning[index])
            {
                isRunning[index] = false;
                threads[index] = new Thread(() => RunCounter(index, 1000 * (index + 1))); // Resetear el thread
                Console.WriteLine($"Contador {index + 1} detenido.");
            }
            else
            {
                Console.WriteLine($"Contador {index + 1} ya estaba detenido.");
            }
        }

        static void ShowStatus()
        {
            Console.WriteLine("Estado de los contadores:");
            for (int i = 0; i < numCounters; i++)
            {
                Console.WriteLine($"Contador {i + 1}: {(isRunning[i] ? "En ejecución" : "Detenido")} - Valor: {counters[i]}");
            }
        }

        static void Menu()
        {
            int option;
            do
            {
                Console.WriteLine("\nMenú:");
                Console.WriteLine("1. Iniciar contador");
                Console.WriteLine("2. Detener contador");
                Console.WriteLine("3. Mostrar estado de contadores");
                Console.WriteLine("4. Salir");
                Console.Write("Selecciona una opción: ");
                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.Write("¿Qué contador deseas iniciar (1-3)?: ");
                        int startIndex = int.Parse(Console.ReadLine()) - 1;
                        StartCounter(startIndex);
                        break;
                    case 2:
                        Console.Write("¿Qué contador deseas detener (1-3)?: ");
                        int stopIndex = int.Parse(Console.ReadLine()) - 1;
                        StopCounter(stopIndex);
                        break;
                    case 3:
                        ShowStatus();
                        break;
                    case 4:
                        StopAllCounters();
                        Console.WriteLine("Saliendo...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            } while (option != 4);
        }

        static void StopAllCounters()
        {
            for (int i = 0; i < numCounters; i++)
            {
                isRunning[i] = false;
            }
        }
    }
}
