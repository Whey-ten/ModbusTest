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

            for (int i = 0; i < 10; i++)
            {
                modbusServer.holdingRegisters[i] = GetRandomValue();
            }

            modbusServer.HoldingRegistersChanged += (startAddress, quantity) =>
            {
                Console.WriteLine($"Client connected. Data was sent from register start address {startAddress} for {quantity} registers.");
            };

            modbusServer.CoilsChanged += (address, coilValue) =>
            {
                Console.WriteLine($"Client has written to coil at address {address}. New value: {coilValue}.");
            };

            modbusServer.Listen();
            Console.WriteLine("Simulated test Modbus server was started");

            timer = new System.Timers.Timer(30000);
            timer.Elapsed += TimerElapsed;
            timer.Start();

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            modbusServer.StopListening();
            timer.Stop();
        }

        static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                modbusServer.holdingRegisters[i] = GetRandomValue();
            }
            PrintRegisterValues();
        }

        static short GetRandomValue()
        {
            return (short)random.Next(10, 91);
        }

        static void PrintRegisterValues()
        {
            Console.WriteLine("################################");
            // Print Coil register values.
            Console.WriteLine("Current Coil register values:");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Coil {i}: {modbusServer.coils[i]}");
            }

            // Print Discrete Input register values.
            Console.WriteLine("Current Discrete Input register values:");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Discrete Input {i}: {modbusServer.discreteInputs[i]}");
            }

            // Print Input Register values.
            Console.WriteLine("Current Input Register values:");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Input Register {i}: {modbusServer.inputRegisters[i]}");
            }

            // Print Holding Register values.
            Console.WriteLine("Current Holding Register values:");
            for (int i = 0; i < 15; i++)
            {
                Console.WriteLine($"Holding Register {i}: {modbusServer.holdingRegisters[i]}");
            }
            Console.WriteLine("################################");
        }
    }
}
