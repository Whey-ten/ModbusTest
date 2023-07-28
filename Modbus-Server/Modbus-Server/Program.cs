using System;
using EasyModbus;

namespace ModbusTCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ModbusServer modbusServer = new ModbusServer();

            for (int i = 0; i < 10; i++)
            {
                modbusServer.holdingRegisters[i] = (short)(i * 10);
            }

            modbusServer.HoldingRegistersChanged += (startAddress, quantity) =>
            {
                Console.WriteLine($"Client connected. Data was sent from register start address {startAddress} for {quantity} registers.");
            };

            modbusServer.Listen();
            Console.WriteLine("Simulated test modbus server was started");

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            
            modbusServer.StopListening();
        }
    }
}
