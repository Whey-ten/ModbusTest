using System;
using System.Timers;
using EasyModbus;

namespace ModbusTCPServer
{
    class Program
    {
        static ModbusServer modbusServer;
        static System.Timers.Timer timer;
        static Random random;

        static void Main(string[] args)
        {
            modbusServer = new ModbusServer();
            random = new Random();

            // Assign static values to the server's holding registers.
            // Assuming 10 registers here; adjust as needed.
            for (int i = 0; i < 10; i++)
            {
                modbusServer.holdingRegisters[i] = GetRandomValue();
            }

            // Handle client connections and data retrieval.
            modbusServer.HoldingRegistersChanged += (startAddress, quantity) =>
            {
                Console.WriteLine($"Client connected. Data was sent from register start address {startAddress} for {quantity} registers.");
            };

            // Start the server.
            modbusServer.Listen();
            Console.WriteLine("Simulated test Modbus server was started");

            // Start the timer to update register values every 30 seconds.
            timer = new System.Timers.Timer(30000);
            timer.Elapsed += TimerElapsed;
            timer.Start();

            // Keep the server running until the user decides to quit.
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            // Stop the server and timer.
            modbusServer.StopListening();
            timer.Stop();
        }

        static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Update the register values every 30 seconds.
            for (int i = 0; i < 10; i++)
            {
                modbusServer.holdingRegisters[i] = GetRandomValue();
            }
        }

        static short GetRandomValue()
        {
            return (short)random.Next(10, 91); // Generates a random value between 10 and 90 (inclusive).
        }
    }
}
