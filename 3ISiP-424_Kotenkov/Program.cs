using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Data.Entity;

namespace _3ISiP_424_Kotenkov
{
    public class DatabaseManager
    {
        private static readonly Lazy<DatabaseManager> _instance =
            new Lazy<DatabaseManager>(() => new DatabaseManager());

        public static DatabaseManager Instance => _instance.Value;

        private readonly MarketplaceDatabaseEntities _context;

        private DatabaseManager()
        {
            _context = new MarketplaceDatabaseEntities();
        }

        public MarketplaceDatabaseEntities Context => _context;
    }

    internal class Program
    {
        static User currentUser;

        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== GMWOG||GG.MOW||WONGG =====");
                Console.WriteLine("1. Посмотреть товары");
                Console.WriteLine("2. Регистрация");
                Console.WriteLine("3. Войти в аккаунт");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ShowProductsAsync();
                        Pause();
                        break;

                    case "2":
                        await RegisterAsync();
                        Pause();
                        break;

                    case "3":
                        await LoginAsync();

                        if (currentUser != null)
                            await UserMenuAsync();

                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        Pause();
                        break;
                }
            }
        }


        static async Task ShowProductsAsync()
        {
            var db = DatabaseManager.Instance.Context;

            var products = await db.Product
                .Where(p => p.IsAvailable && p.StockQuantity > 0)
                .OrderBy(p => p.Name)
                .ToListAsync();

            Console.WriteLine("\n===== КАТАЛОГ ТОВАРОВ =====");

            if (products.Count == 0)
            {
                Console.WriteLine("Товаров нет.");
                return;
            }

            foreach (var p in products)
            {
                Console.WriteLine($"ID: {p.Id} | {p.Name} | " + $"{p.Price:F2} руб. | Остаток: {p.StockQuantity}");
            }
        }


        static async Task RegisterAsync()
        {
            var db = DatabaseManager.Instance.Context;

            Console.Write("Придумайте логин: ");
            string username = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Логин не может быть пустым.");
                return;
            }

            username = username.Trim();

            bool exists = await db.User
                .AnyAsync(u => u.Username == username);

            if (exists)
            {
                Console.WriteLine("Этот логин уже занят.");
                return;
            }

            Console.Write("Придумайте пароль: ");
            string password = Console.ReadLine();

            Console.Write("Повторите пароль: ");
            string confirmPassword = Console.ReadLine();

            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                Console.WriteLine("Пароль должен содержать минимум 6 символов.");
                return;
            }

            if (password != confirmPassword)
            {
                Console.WriteLine("Пароли не совпадают.");
                return;
            }

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                CreatedAt = DateTime.Now
            };

            db.User.Add(user);
            await db.SaveChangesAsync();

            Console.WriteLine("Регистрация успешна!");
        }


        static async Task LoginAsync()
        {
            var db = DatabaseManager.Instance.Context;

            Console.Write("Логин: ");
            string username = Console.ReadLine();

            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            var user = await db.User
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null ||
                !VerifyPassword(password, user.PasswordHash))
            {
                Console.WriteLine("Неверный логин или пароль.");
                Pause();
                return;
            }

            currentUser = user;
            Console.WriteLine("Вы успешно вошли в аккаунт!");
            Pause();
        }

        static async Task UserMenuAsync()
        {
            while (currentUser != null)
            {
                Console.Clear();

                Console.WriteLine($"===== GMWOG | {currentUser.Username} =====");

                Console.WriteLine("1. Каталог товаров");
                Console.WriteLine("2. Добавить товар в корзину");
                Console.WriteLine("3. Посмотреть корзину");
                Console.WriteLine("4. Купить один товар");
                Console.WriteLine("5. Купить всю корзину");
                Console.WriteLine("6. История заказов");
                Console.WriteLine("7. Выйти из аккаунта");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ShowProductsAsync();
                        Pause();
                        break;

                    case "2":
                        await AddToCartAsync();
                        Pause();
                        break;

                    case "3":
                        await ShowCartAsync();
                        Pause();
                        break;

                    case "4":
                        await BuyOneAsync();
                        Pause();
                        break;

                    case "5":
                        await BuyCartAsync();
                        Pause();
                        break;

                    case "6":
                        await ShowOrdersAsync();
                        Pause();
                        break;

                    case "7":
                        currentUser = null;
                        break;

                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        Pause();
                        break;
                }
            }
        }


        static async Task AddToCartAsync()
        {
            await ShowProductsAsync();

            int productId = ReadInt("Введите ID товара: ");
            int quantity = ReadInt("Количество: ");

            if (quantity <= 0)
            {
                Console.WriteLine("Количество должно быть больше нуля.");
                return;
            }

            var db = DatabaseManager.Instance.Context;

            var product = await db.Product
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null || !product.IsAvailable)
            {
                Console.WriteLine("Товар не найден или недоступен.");
                return;
            }

            var cartItem = await db.CartItem
                .FirstOrDefaultAsync(c =>
                    c.UserId == currentUser.Id &&
                    c.ProductId == productId);

            int oldQuantity = cartItem == null
                ? 0
                : cartItem.Quantity;

            if (oldQuantity + quantity > product.StockQuantity)
            {
                Console.WriteLine("Недостаточно товара на складе.");
                return;
            }

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    UserId = currentUser.Id,
                    ProductId = productId,
                    Quantity = quantity
                };

                db.CartItem.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await db.SaveChangesAsync();

            Console.WriteLine("Товар добавлен в корзину!");
        }

        static async Task ShowCartAsync()
        {
            var db = DatabaseManager.Instance.Context;

            var items = await db.CartItem
                .Where(c => c.UserId == currentUser.Id)
                .Include(c => c.Product)
                .ToListAsync();

            Console.WriteLine("===== КОРЗИНА =====");

            if (items.Count == 0)
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            decimal total = 0;

            foreach (var item in items)
            {
                decimal sum = item.Product.Price * item.Quantity;
                total += sum;

                Console.WriteLine($"{item.Product.Name} x {item.Quantity} = " + $"{sum:F2} руб.");
            }

            Console.WriteLine($"Итого: {total:F2} руб.");
        }


        static async Task BuyOneAsync()
        {
            await ShowProductsAsync();

            int productId = ReadInt("Введите ID товара: ");
            int quantity = ReadInt("Количество: ");

            if (quantity <= 0)
            {
                Console.WriteLine("Количество должно быть больше нуля.");
                return;
            }

            var db = DatabaseManager.Instance.Context;

            var product = await db.Product
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null ||
                !product.IsAvailable ||
                product.StockQuantity < quantity)
            {
                Console.WriteLine("Товар недоступен или его не хватает.");
                return;
            }

            var items = new List<OrderProduct>
            {
                new OrderProduct
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price
                }
            };

            await CreateOrderAsync(db, items, false);
        }


        static async Task BuyCartAsync()
        {
            var db = DatabaseManager.Instance.Context;

            var cart = await db.CartItem
                .Where(c => c.UserId == currentUser.Id)
                .ToListAsync();

            if (cart.Count == 0)
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            var items = new List<OrderProduct>();

            foreach (var cartItem in cart)
            {
                var product = await db.Product
                    .FirstOrDefaultAsync(p =>
                        p.Id == cartItem.ProductId);

                if (product == null)
                {
                    Console.WriteLine("Один из товаров не найден.");
                    return;
                }

                items.Add(new OrderProduct
                {
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price
                });
            }

            await CreateOrderAsync(db, items, true);
        }


        static async Task CreateOrderAsync(MarketplaceDatabaseEntities db, List<OrderProduct> items, bool clearCart)
        {
            var points = await db.PickupPoint
                .Where(p => p.IsActive)
                .OrderBy(p => p.Id)
                .ToListAsync();

            if (points.Count == 0)
            {
                Console.WriteLine("Нет доступных пунктов выдачи.");
                return;
            }

            Console.WriteLine("\n===== ВЫБЕРИТЕ ПВЗ =====");

            foreach (var point in points)
            {
                Console.WriteLine(
                    $"{point.Id}. {point.Name} — {point.Address}");
            }

            int pickupPointId = ReadInt("Введите ID ПВЗ: ");

            if (!points.Any(p => p.Id == pickupPointId))
            {
                Console.WriteLine("Неверный ID пункта выдачи.");
                return;
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    decimal total = 0;

                    foreach (var item in items)
                    {
                        var product = await db.Product
                            .FirstOrDefaultAsync(p =>
                                p.Id == item.ProductId);

                        if (product == null ||
                            !product.IsAvailable ||
                            item.Quantity <= 0 ||
                            product.StockQuantity < item.Quantity)
                        {
                            Console.WriteLine(
                                "Недостаточно товара на складе. Заказ отменён.");

                            transaction.Rollback();
                            return;
                        }

                        item.UnitPrice = product.Price;

                        total += product.Price * item.Quantity;
                    }

                    var order = new Order
                    {
                        UserId = currentUser.Id,
                        PickupPointId = pickupPointId,
                        CreatedAt = DateTime.Now,
                        TotalPrice = total
                    };

                    db.Order.Add(order);

                    await db.SaveChangesAsync();

                    foreach (var item in items)
                    {
                        item.OrderId = order.Id;
                        db.OrderProduct.Add(item);

                        var product = await db.Product
                            .FirstAsync(p => p.Id == item.ProductId);

                        product.StockQuantity -= item.Quantity;

                        if (product.StockQuantity == 0)
                            product.IsAvailable = false;
                    }

                    if (clearCart)
                    {
                        var cart = await db.CartItem
                            .Where(c => c.UserId == currentUser.Id)
                            .ToListAsync();

                        db.CartItem.RemoveRange(cart);
                    }

                    await db.SaveChangesAsync();

                    transaction.Commit();

                    Console.WriteLine($"Заказ №{order.Id} успешно оформлен!");

                    Console.WriteLine($"Сумма: {total:F2} руб.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    Console.WriteLine("Ошибка при оформлении заказа: " + ex.Message);
                }
            }
        }


        static async Task ShowOrdersAsync()
        {
            var db = DatabaseManager.Instance.Context;

            Console.WriteLine("1. Сначала новые");
            Console.WriteLine("2. Сначала старые");

            string sort = Console.ReadLine();

            var query = db.Order
                .Where(o => o.UserId == currentUser.Id)
                .Include(o => o.PickupPoint);

            var orders = sort == "2"
                ? await query.OrderBy(o => o.CreatedAt).ToListAsync()
                : await query.OrderByDescending(o => o.CreatedAt)
                    .ToListAsync();

            Console.WriteLine("===== ИСТОРИЯ ЗАКАЗОВ =====");

            if (orders.Count == 0)
            {
                Console.WriteLine("Заказов пока нет.");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"Заказ №{order.Id} | " + $"{order.CreatedAt:dd.MM.yyyy HH:mm}");

                Console.WriteLine(
                    $"ПВЗ: {order.PickupPoint.Name}");

                Console.WriteLine(
                    $"Сумма: {order.TotalPrice:F2} руб.");

                var products = await db.OrderProduct
                    .Where(p => p.OrderId == order.Id)
                    .Include(p => p.Product)
                    .ToListAsync();

                foreach (var item in products)
                {
                    Console.WriteLine(
                        $"  {item.Product.Name} x {item.Quantity} " +
                        $"по {item.UnitPrice:F2} руб.");
                }
            }
        }


        static string HashPassword(string password)
        {
            byte[] salt = new byte[16];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            using (var derive = new Rfc2898DeriveBytes(
                password, salt, 100000))
            {
                byte[] hash = derive.GetBytes(32);

                return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            }
        }


        static bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                string[] parts = storedHash.Split(':');

                if (parts.Length != 2)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] expected = Convert.FromBase64String(parts[1]);
                byte[] actual;

                using (var derive = new Rfc2898DeriveBytes(
                    password, salt, 100000))
                {
                    actual = derive.GetBytes(32);
                }

                if (actual.Length != expected.Length)
                    return false;

                int difference = 0;

                for (int i = 0; i < actual.Length; i++)
                    difference |= actual[i] ^ expected[i];

                return difference == 0;
            }
            catch
            {
                return false;
            }
        }


        static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                int number;

                if (int.TryParse(Console.ReadLine(), out number))
                    return number;

                Console.WriteLine("Введите целое число.");
            }
        }

        static void Pause()
        {
            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadLine();
        }
    }
}
