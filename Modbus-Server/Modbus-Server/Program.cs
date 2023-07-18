using System;
using EasyModbus;

namespace ModbusTCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ModbusServer modbusServer = new ModbusServer();

            // Assign static values to the server's holding registers.
            // Assuming 10 registers here; adjust as needed.
            for (int i = 0; i < 10; i++)
            {
                modbusServer.holdingRegisters[i] = (short)(i * 10);
            }

            // Handle client connections and data retrieval.
            modbusServer.HoldingRegistersChanged += (startAddress, quantity) =>
            {
                Console.WriteLine($"Client connected. Data was sent from register start address {startAddress} for {quantity} registers.");
            };

            // Start the server.
            modbusServer.Listen();
            Console.WriteLine("Simulated test modbus server was started");

            // Keep the server running until the user decides to quit.
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            // Stop the server.
            modbusServer.StopListening();
        }
    }
}
