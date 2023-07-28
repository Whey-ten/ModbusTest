using System;
using EasyModbus;

namespace ModbusTCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ModbusClient modbusClient = new ModbusClient("127.0.0.1", 502); // Replace with your server's IP address and port.

            try
            {
                modbusClient.Connect();
                
                if (modbusClient.Connected)
                {
                    Console.WriteLine("Successfully connected to the server.");
                }
                int[] values = modbusClient.ReadHoldingRegisters(0, 10);

                for (int i = 0; i < values.Length; i++)
                {
                    Console.WriteLine($"Register {i}: {values[i]}");
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
    }
}
