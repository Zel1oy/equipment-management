using System;
using System.Collections.Generic;

namespace EquipmentManagement
{
    class Program
    {
        // Клас обладнання
        class Equipment
        {
            public int Id { get; set; }
            public string Назва { get; set; }
            public string ІнвентарнийНомер { get; set; }
            public string Категорія { get; set; }
            public string Статус { get; set; }
            public string Відповідальний { get; set; }
        }

        static void Main(string[] args)
        {
            List<Equipment> equipmentList = new List<Equipment>
            {
                new Equipment { Id = 1, Назва = "Ноутбук HP ProBook 450", ІнвентарнийНомер = "INV-00213", Категорія = "Комп’ютерна техніка", Статус = "У використанні", Відповідальний = "Іваненко І.І." },
                new Equipment { Id = 2, Назва = "Мікроскоп Leica", ІнвентарнийНомер = "INV-00487", Категорія = "Лабораторне обладнання", Статус = "На ремонті", Відповідальний = "Петренко П.П." },
                new Equipment { Id = 3, Назва = "Проектор Epson EB-X05", ІнвентарнийНомер = "INV-00145", Категорія = "Мультимедіа", Статус = "У резерві", Відповідальний = "-" }
            };

            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== СИСТЕМА УПРАВЛІННЯ ТА ОБЛІКУ ОБЛАДНАННЯ КАФЕДРИ ===");
                Console.WriteLine("1. Показати все обладнання (зі статусами)");
                Console.WriteLine("2. Додати нове");
                Console.WriteLine("3. Знайти за номером");
                Console.WriteLine("4. Змінити статус обладнання");
                Console.WriteLine("5. Вийти");
                Console.Write("Оберіть пункт меню (1-5): ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ПоказатиВсе(equipmentList);
                        break;

                    case "2":
                        ДодатиНове(equipmentList);
                        break;

                    case "3":
                        ЗнайтиЗаНомером(equipmentList);
                        break;

                    case "4":
                        ЗмінитиСтатус(equipmentList);
                        break;

                    case "5":
                        running = false;
                        Console.WriteLine("👋 Вихід із системи. До побачення!");
                        break;

                    default:
                        Console.WriteLine("⚠️ Невірний вибір, спробуйте ще раз.\n");
                        break;
                }
            }
        }

        static void ПоказатиВсе(List<Equipment> equipmentList)
        {
            Console.WriteLine("=== ВСЕ ОБЛАДНАННЯ ===");
            foreach (var item in equipmentList)
            {
                Console.WriteLine($"{item.Id}. {item.Назва} | {item.ІнвентарнийНомер} | {item.Категорія} | " +
                                  $"Статус: {item.Статус} | Відповідальний: {item.Відповідальний}");
            }
        }

        static void ДодатиНове(List<Equipment> equipmentList)
        {
            Console.WriteLine("=== ДОДАТИ НОВЕ ОБЛАДНАННЯ ===");

            Console.Write("Назва: ");
            string назва = Console.ReadLine();

            Console.Write("Інвентарний номер: ");
            string інвНомер = Console.ReadLine();

            Console.Write("Категорія: ");
            string категорія = Console.ReadLine();

            Console.Write("Статус (У використанні / У резерві / На ремонті): ");
            string статус = Console.ReadLine();

            Console.Write("Відповідальний: ");
            string відповідальний = Console.ReadLine();

            Equipment newItem = new Equipment
            {
                Id = equipmentList.Count + 1,
                Назва = назва,
                ІнвентарнийНомер = інвНомер,
                Категорія = категорія,
                Статус = статус,
                Відповідальний = відповідальний
            };

            equipmentList.Add(newItem);
            Console.WriteLine("✅ Обладнання додано успішно!");
        }

        static void ЗнайтиЗаНомером(List<Equipment> equipmentList)
        {
            Console.Write("Введіть інвентарний номер: ");
            string номер = Console.ReadLine();

            var item = equipmentList.Find(e => e.ІнвентарнийНомер.Equals(номер, StringComparison.OrdinalIgnoreCase));

            if (item != null)
            {
                Console.WriteLine($"\nЗНАЙДЕНО: {item.Назва} | {item.Категорія} | " +
                                  $"Статус: {item.Статус} | Відповідальний: {item.Відповідальний}\n");
            }
            else
            {
                Console.WriteLine("❌ Обладнання не знайдено.\n");
            }
        }

        static void ЗмінитиСтатус(List<Equipment> equipmentList)
        {
            Console.Write("Введіть інвентарний номер: ");
            string номер = Console.ReadLine();

            var item = equipmentList.Find(e => e.ІнвентарнийНомер.Equals(номер, StringComparison.OrdinalIgnoreCase));

            if (item != null)
            {
                Console.WriteLine($"Поточний статус: {item.Статус}");
                Console.Write("Новий статус: ");
                string newStatus = Console.ReadLine();
                item.Статус = newStatus;
                Console.WriteLine("✅ Статус оновлено!");
            }
            else
            {
                Console.WriteLine("❌ Обладнання не знайдено.");
            }
        }
    }
}
