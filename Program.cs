using System;
using System.Globalization;
using SmartParkingSystem.Models;
using SmartParkingSystem.Repositories;
using SmartParkingSystem.Services;

class Program
{
    static void Main()
    {
        var repo = new FileRepository();
        var service = new ParkingService(repo);

        while (true)
        {
            Console.WriteLine("\n1. List");
            Console.WriteLine("2. Add");
            Console.WriteLine("3. Get by ID");
            Console.WriteLine("4. Delete");
            Console.WriteLine("5. Update");
            Console.WriteLine("6. Exit");

            Console.Write("Choose: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var list = service.List();
                    foreach (var s in list)
                        Console.WriteLine($"{s.Id} - {s.Name} - {s.PricePerHour}€ - Available: {s.IsAvailable}");
                    break;

                case "2":
                    try
                    {
                        Console.Write("Name: ");
                        var name = Console.ReadLine();

                        Console.Write("Price: ");
                        if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double price))
                        {
                            Console.WriteLine("Invalid price!");
                            break;
                        }

                        var spot = new ParkingSpot
                        {
                            Name = name,
                            PricePerHour = price,
                            IsAvailable = true
                        };

                        service.Add(spot);
                        Console.WriteLine("Added!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case "3":
                    Console.Write("ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("Invalid ID!");
                        break;
                    }

                    var found = service.GetById(id);
                    Console.WriteLine(found != null
                        ? $"{found.Id} - {found.Name} - {found.PricePerHour}€"
                        : "Not found");
                    break;

                case "4":
                    Console.Write("ID to delete: ");
                    if (!int.TryParse(Console.ReadLine(), out int delId))
                    {
                        Console.WriteLine("Invalid ID!");
                        break;
                    }

                    service.Delete(delId);
                    Console.WriteLine("Deleted!");
                    break;

                case "5":
                    Console.Write("ID to update: ");
                    if (!int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        Console.WriteLine("Invalid ID!");
                        break;
                    }

                    var existing = service.GetById(updateId);

                    if (existing == null)
                    {
                        Console.WriteLine("Not found!");
                        break;
                    }

                    Console.Write("New Name: ");
                    var newName = Console.ReadLine();

                    Console.Write("New Price: ");
                    if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double newPrice))
                    {
                        Console.WriteLine("Invalid price!");
                        break;
                    }

                    Console.Write("Is Available (true/false): ");
                    if (!bool.TryParse(Console.ReadLine(), out bool isAvailable))
                    {
                        Console.WriteLine("Invalid value!");
                        break;
                    }

                    var updatedSpot = new ParkingSpot
                    {
                        Id = updateId,
                        Name = newName,
                        PricePerHour = newPrice,
                        IsAvailable = isAvailable
                    };

                    service.Update(updatedSpot);

                    Console.WriteLine("Updated!");
                    break;

                case "6":
                    return;

                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }
    }
}