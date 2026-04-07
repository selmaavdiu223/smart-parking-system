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
            Console.WriteLine("6. Search by name");
            Console.WriteLine("7. Exit");

            Console.Write("Choose: ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        var list = service.List();
                        foreach (var s in list)
                            Console.WriteLine($"{s.Id} - {s.Name} - {s.PricePerHour}€ - Available: {s.IsAvailable}");
                        break;

                    case "2":
                        Console.Write("Name: ");
                        var name = Console.ReadLine();

                        Console.Write("Price: ");
                        if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double price))
                        {
                            Console.WriteLine("Ju lutem shkruani numër valid për çmim.");
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
                        break;

                    case "3":
                        Console.Write("ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            Console.WriteLine("ID jo valid!");
                            break;
                        }

                        var found = service.GetById(id);

                        if (found == null)
                        {
                            Console.WriteLine("Item nuk u gjet.");
                        }
                        else
                        {
                            Console.WriteLine($"{found.Id} - {found.Name} - {found.PricePerHour}€ - Available: {found.IsAvailable}");
                        }
                        break;

                    case "4":
                        Console.Write("ID to delete: ");
                        if (!int.TryParse(Console.ReadLine(), out int delId))
                        {
                            Console.WriteLine("ID jo valid!");
                            break;
                        }

                        service.Delete(delId);
                        Console.WriteLine("Deleted!");
                        break;

                    case "5":
                        Console.Write("ID to update: ");
                        if (!int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            Console.WriteLine("ID jo valid!");
                            break;
                        }

                        var existing = service.GetById(updateId);

                        if (existing == null)
                        {
                            Console.WriteLine("Item nuk u gjet!");
                            break;
                        }

                        Console.Write("New Name: ");
                        var newName = Console.ReadLine();

                        Console.Write("New Price: ");
                        if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double newPrice))
                        {
                            Console.WriteLine("Çmim jo valid!");
                            break;
                        }

                        Console.Write("Is Available (true/false): ");
                        if (!bool.TryParse(Console.ReadLine(), out bool isAvailable))
                        {
                            Console.WriteLine("Vlerë jo valide!");
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
                        Console.Write("Shkruaj emrin e parkingut: ");
                        var searchName = Console.ReadLine();

                        var results = service.SearchByName(searchName);

                        if (results.Count == 0)
                        {
                            Console.WriteLine("Asnje parking nuk u gjet.");
                        }
                        else
                        {
                            foreach (var p in results)
                            {
                                Console.WriteLine($"{p.Id} - {p.Name} - {p.PricePerHour}€ - Available: {p.IsAvailable}");
                            }
                        }
                        break;

                    case "7":
                        return;

                    default:
                        Console.WriteLine("Opsion i pavlefshëm!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gabim: " + ex.Message);
            }
        }
    }
}