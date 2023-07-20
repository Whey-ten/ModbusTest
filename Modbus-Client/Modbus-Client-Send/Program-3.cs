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

                timer = new System.Timers.Timer(20000);
                timer.Elapsed += TimerElapsed;
                timer.Start();

                while (true)
                {
                    try
                    {
                        Console.WriteLine("Retrieving data from registers...");

                        //changed 10 to 15
                        int[] values = modbusClient.ReadHoldingRegisters(0, 15);

                        for (int i = 0; i < values.Length; i++)
                        {
                            Console.WriteLine($"Register {i}: {values[i]}");
                        }

                        System.Threading.Thread.Sleep(10000);
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
                // Write a random value to a single register.
                //Changed 0 to 11 register
                modbusClient.WriteSingleRegister(10, GetRandomValue());
                Console.WriteLine("Random value written to the server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static short GetRandomValue()
        {
            return (short)random.Next(10, 91);
        }
    }
}
