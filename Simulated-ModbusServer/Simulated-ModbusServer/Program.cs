using System;
using System.Timers;
using EasyModbus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModbusTCPServer
{
    class Program
    {
        static ModbusServer modbusServer;
        static Random random;

        static void Main(string[] args)
        {
            modbusServer = new ModbusServer();
            random = new Random();

            try
            {
                Console.WriteLine("Trying to load Modbus registers from json file...");
                loadRegisters();

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor(); // Reset color back to default

                Console.WriteLine("Filling first 10 holding registers with random values...");
                for (int i = 0; i < 11; i++)
                {
                    modbusServer.holdingRegisters[i] = GetRandomValue();
                }
            }

            PrintRegisterValues();

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

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            modbusServer.StopListening();
        }

        static short GetRandomValue()
        {
            return (short)random.Next(10, 91);
        }

        static void PrintRegisterValues()
        {
            Console.WriteLine("################################");
            // Print Coil register values.
            Console.WriteLine("\nCurrent Coil register values:");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Coil {i}: {modbusServer.coils[i]}");
            }

            // Print Discrete Input register values.
            Console.WriteLine("\nCurrent Discrete Input register values:");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Discrete Input {i}: {modbusServer.discreteInputs[i]}");
            }

            // Print Input Register values.
            Console.WriteLine("\nCurrent Input Register values:");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Input Register {i}: {modbusServer.inputRegisters[i]}");
            }

            // Print Holding Register values.
            Console.WriteLine("\nCurrent Holding Register values:");
            for (int i = 0; i < 15; i++)
            {
                Console.WriteLine($"Holding Register {i}: {modbusServer.holdingRegisters[i]}");
            }
            Console.WriteLine("################################");
        }

        static void loadRegisters()
        {
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;

            string dataFolderPath = Path.Combine(solutionDirectory, "Data");
            string jsonFilePath = Path.Combine(dataFolderPath, "registers.json");

            if (File.Exists(jsonFilePath))
            {
                Console.WriteLine($"JSON file path: {jsonFilePath}");

                try
                {
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    JObject jsonObject = JObject.Parse(jsonContent);

                    foreach (var property in jsonObject["coil"].Value<JObject>())
                    {
                        int address = int.Parse(property.Key);
                        bool value = (bool)property.Value;
                        modbusServer.coils[address] = value;
                    }

                    foreach (var property in jsonObject["discreteInput"].Value<JObject>())
                    {
                        int address = int.Parse(property.Key);
                        bool value = (bool)property.Value;
                        modbusServer.discreteInputs[address] = value;
                    }

                    foreach (var property in jsonObject["inputRegisters"].Value<JObject>())
                    {
                        int address = int.Parse(property.Key);
                        short value = (short)property.Value;
                        modbusServer.inputRegisters[address] = value;
                    }

                    foreach (var property in jsonObject["holdingRegisters"].Value<JObject>())
                    {
                        int address = int.Parse(property.Key);
                        short value = (short)property.Value;
                        modbusServer.holdingRegisters[address] = value;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                throw new FileNotFoundException("JSON file not found.", jsonFilePath);
            }
        }
    }
}
