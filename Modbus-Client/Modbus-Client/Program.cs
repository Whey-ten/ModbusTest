using System;
using EasyModbus;

namespace ModbusTCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Modbus TCP client.
            ModbusClient modbusClient = new ModbusClient("127.0.0.1", 502); // Replace with your server's IP address and port.

            try
            {
                // Connect to the server.
                modbusClient.Connect();

                // Check if the connection was successful.
                if (modbusClient.Connected)
                {
                    Console.WriteLine("Successfully connected to the server.");
                }

                // Read 10 holding registers starting at address 0.
                int[] values = modbusClient.ReadHoldingRegisters(0, 10);

                // Print the retrieved values.
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
                // Always disconnect when finished.
                if (modbusClient.Connected)
                {
                    modbusClient.Disconnect();
                }
            }
        }
    }
}
