using System;
using System.Timers;
using EasyModbus;

namespace ModbusTCPClient
{
    class Program
    {
        static ModbusClient modbusClient;
        static System.Timers.Timer timer;
        static Random random;

        static void Main(string[] args)
        {
            modbusClient = new ModbusClient("127.0.0.1", 502);
            random = new Random();

            try
            {
                modbusClient.Connect();

                if (modbusClient.Connected)
                {
                    Console.WriteLine("Successfully connected to the server.");
                }

                timer = new System.Timers.Timer(2000); // Change interval to 2 seconds
                timer.Elapsed += TimerElapsed;
                timer.Start();

                while (true)
                {
                    try
                    {
                        Console.WriteLine("Retrieving data from registers...");

                        int[] values = modbusClient.ReadHoldingRegisters(0, 4); // Read first 4 registers

                        for (int i = 0; i < values.Length; i++)
                        {
                            Console.WriteLine($"Register {i}: {values[i]}");
                        }

                        System.Threading.Thread.Sleep(2000); // Wait for 2 seconds
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"An error occurred: {e.Message}");
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
            finally
            {
                if (modbusClient.Connected)
                {
                    modbusClient.Disconnect();
                }
            }
        }

        static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                // Write a random value to each of the first 4 registers.
                for (int register = 0; register < 4; register++)
                {
                    modbusClient.WriteSingleRegister(register, GetRandomValue());
                    Console.WriteLine($"Random value written to Register {register}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static short GetRandomValue()
        {
            return (short)random.Next(1, 11); // Generate random value between 1 and 10
        }
    }
}
