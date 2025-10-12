using PstInventory.Core.enums;
using PstInventory.Core.repository.impl;
using PstInventory.Core.service;

namespace PstInventory.ConsoleUI;

using Core;

public class Program
{
    public static void Main()
    {
        IEquipmentRepository repository = new FileEquipmentRepository();
        EquipmentService service = new EquipmentService(repository);

        var running = true;
        while (running)
        {
            Console.WriteLine("\n=== СИСТЕМА УПРАВЛІННЯ ТА ОБЛІКУ ОБЛАДНАННЯ КАФЕДРИ ===");
            Console.WriteLine("1. Показати все обладнання");
            Console.WriteLine("2. Додати нове обладнання");
            Console.WriteLine("3. Знайти за інвентарним номером");
            Console.WriteLine("4. Змінити статус обладнання");
            Console.WriteLine("5. Видалити обладнання");
            Console.WriteLine("9. Вийти");
            Console.Write("Оберіть пункт меню: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ShowAllEquipment(service);
                    break;
                case "2":
                    AddNewEquipment(service);
                    break;
                case "3":
                    FindEquipmentByNumber(service);
                    break;
                case "4":
                    ChangeEquipmentStatus(service);
                    break;
                case "5":
                    DeleteEquipment(service);
                    break;
                case "9":
                    running = false;
                    Console.WriteLine("👋 Вихід із системи. До побачення!");
                    break;
                default:
                    Console.WriteLine("⚠️ Невірний вибір, спробуйте ще раз.");
                    break;
            }
        }
    }

    private static void ShowAllEquipment(EquipmentService service)
    {
        var allItems = service.GetAllEquipment();

        if (!allItems.Any())
        {
            Console.WriteLine("ℹ️ Список обладнання порожній. Спробуйте додати новий елемент.");
            return;
        }
        
        Console.WriteLine("=== СПИСОК ОБЛАДНАННЯ ===");
        foreach (var item in allItems)
        {
            Console.WriteLine($"ID: {item.Id} | {item.Name} ({item.InventoryNumber})");
            Console.WriteLine($"\tСтатус: {item.Status} | Локація: {item.Location} | Відповідальний: {item.AssignedTo}\n");
        }
    }

    private static void AddNewEquipment(EquipmentService service)
    {
        try
        {
            Console.WriteLine("=== ДОДАВАННЯ НОВОГО ОБЛАДНАННЯ ===");
            Console.Write("Назва: ");
            string name = Console.ReadLine();

            Console.Write("Інвентарний номер: ");
            string invNumber = Console.ReadLine();

            Console.Write("Локація (аудиторія, кабінет): ");
            string location = Console.ReadLine();

            Console.Write("Відповідальний (ПІБ або 'N/A'): ");
            string assignedTo = Console.ReadLine();

            service.AddEquipment(name, invNumber, location, assignedTo);

            Console.WriteLine("\n✅ Обладнання успішно додано!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Помилка: {ex.Message}");
        }
    }

    private static void FindEquipmentByNumber(EquipmentService service)
    {
        Console.Write("Введіть інвентарний номер для пошуку: ");
        string number = Console.ReadLine();

        var item = service.FindEquipmentByInventoryNumber(number);

        Console.WriteLine(item != null
            ? $"\nЗНАЙДЕНО: {item.Name} | Статус: {item.Status} | Відповідальний: {item.AssignedTo}"
            : "\n❌ Обладнання з таким інвентарним номером не знайдено.");
    }

    private static void ChangeEquipmentStatus(EquipmentService service)
    {
        Console.Write("Введіть інвентарний номер обладнання, статус якого треба змінити: ");
        string number = Console.ReadLine();
        
        var item = service.FindEquipmentByInventoryNumber(number);
        if (item == null)
        {
            Console.WriteLine("\n❌ Обладнання з таким інвентарним номером не знайдено.");
            return;
        }

        Console.WriteLine($"\nОбрано: {item.Name}. Поточний статус: {item.Status}");
        Console.WriteLine("Оберіть новий статус:");
        
        var statuses = Enum.GetValues<EquipmentStatus>();
        for (int i = 0; i < statuses.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {statuses[i]}");
        }
        Console.Write("Ваш вибір: ");
        
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= statuses.Length)
        {
            var newStatus = statuses[choice - 1];
            service.UpdateEquipmentStatus(number, newStatus);
            Console.WriteLine("\n✅ Статус успішно оновлено!");
        }
        else
        {
            Console.WriteLine("\n❌ Невірний вибір статусу.");
        }
    }
    
    private static void DeleteEquipment(EquipmentService service)
    {
        Console.Write("Введіть інвентарний номер обладнання, яке потрібно видалити: ");
        string number = Console.ReadLine();
        
        var item = service.FindEquipmentByInventoryNumber(number);
        if (item == null)
        {
            Console.WriteLine("\n❌ Обладнання з таким інвентарним номером не знайдено.");
            return;
        }

        Console.Write($"Ви впевнені, що хочете видалити '{item.Name}'? (так/ні): ");
        string confirmation = Console.ReadLine();

        if (confirmation.Equals("так", StringComparison.OrdinalIgnoreCase))
        {
            if (service.DeleteEquipment(number))
            {
                Console.WriteLine("\n✅ Обладнання успішно видалено.");
            }
            else
            {
                Console.WriteLine("\n❌ Не вдалося видалити обладнання.");
            }
        }
        else
        {
            Console.WriteLine("\nℹ️ Видалення скасовано.");
        }
    }
}
