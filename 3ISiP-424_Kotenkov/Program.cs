using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueGame
{
    public static class Rng
    {
        private static readonly Random Random = new Random();

        public static int Next(int minInclusive, int maxInclusive)
        {
            return Random.Next(minInclusive, maxInclusive + 1);
        }

        public static double NextDouble()
        {
            return Random.NextDouble();
        }

        public static bool Chance(double probability)
        {
            return Random.NextDouble() < probability;
        }

        public static T Pick<T>(IReadOnlyList<T> items)
        {
            return items[Random.Next(items.Count)];
        }
    }

    public abstract class Item
    {
        public string Name { get; }

        protected Item(string name)
        {
            Name = name;
        }

        public abstract string Describe();
    }

    public class Weapon : Item
    {
        public int Attack { get; }

        public Weapon(string name, int attack) : base(name)
        {
            Attack = attack;
        }

        public override string Describe()
        {
            return $"{Name} (атака: {Attack})";
        }
    }

    public class Armor : Item
    {
        public int Defense { get; }

        public Armor(string name, int defense) : base(name)
        {
            Defense = defense;
        }

        public override string Describe()
        {
            return $"{Name} (защита: {Defense})";
        }
    }

    public static class ItemFactory
    {
        private static readonly string[] WeaponNames =
            { "Ржавый меч", "Стальной клинок", "Боевой топор", "Кинжал", "Копьё", "Булава", "Рапира" };

        private static readonly string[] ArmorNames =
            { "Кожаная броня", "Кольчуга", "Латы", "Стёганка", "Бронзовый нагрудник", "Мифриловая кираса" };

        public static Weapon RandomWeapon()
        {
            return new Weapon(Rng.Pick(WeaponNames), Rng.Next(4, 18));
        }

        public static Armor RandomArmor()
        {
            return new Armor(Rng.Pick(ArmorNames), Rng.Next(1, 10));
        }
    }

    public class Player
    {
        public const double DodgeChance = 0.4;
        public const double MinBlock = 0.7;
        public const double MaxBlock = 1.0;

        public int MaxHp { get; }
        public int Hp { get; private set; }
        public Weapon Weapon { get; private set; }
        public Armor Armor { get; private set; }
        public bool Frozen { get; set; }
        public bool IsAlive => Hp > 0;

        public Player(int maxHp, Weapon weapon, Armor armor)
        {
            MaxHp = maxHp;
            Hp = maxHp;
            Weapon = weapon;
            Armor = armor;
        }

        public void HealFully()
        {
            Hp = MaxHp;
        }

        public void EquipWeapon(Weapon weapon)
        {
            Weapon = weapon;
        }

        public void EquipArmor(Armor armor)
        {
            Armor = armor;
        }

        public int AttackPower => Weapon.Attack;

        public AttackOutcome ReceiveAttack(int rawDamage, bool ignoreArmor, bool defending)
        {
            if (defending && Rng.Chance(DodgeChance))
            {
                return new AttackOutcome(0, true, 0);
            }

            int blocked = 0;
            if (defending && !ignoreArmor)
            {
                double factor = MinBlock + Rng.NextDouble() * (MaxBlock - MinBlock);
                blocked = (int)Math.Round(Armor.Defense * factor);
            }

            int damage = Math.Max(1, rawDamage - blocked);
            Hp = Math.Max(0, Hp - damage);
            return new AttackOutcome(damage, false, blocked);
        }
    }

    public readonly struct AttackOutcome
    {
        public int Damage { get; }
        public bool Dodged { get; }
        public int Blocked { get; }

        public AttackOutcome(int damage, bool dodged, int blocked)
        {
            Damage = damage;
            Dodged = dodged;
            Blocked = blocked;
        }
    }

    public abstract class Enemy
    {
        public string Name { get; }
        public int MaxHp { get; }
        public int Hp { get; private set; }
        public int Attack { get; }
        public int Defense { get; }
        public bool IsAlive => Hp > 0;

        protected Enemy(string name, int hp, int attack, int defense)
        {
            Name = name;
            MaxHp = hp;
            Hp = hp;
            Attack = attack;
            Defense = defense;
        }

        protected static int Scale(int baseValue, double multiplier)
        {
            return Math.Max(1, (int)Math.Round(baseValue * multiplier));
        }

        public int TakeDamage(int playerAttack)
        {
            int damage = Math.Max(1, playerAttack - Defense);
            Hp = Math.Max(0, Hp - damage);
            return damage;
        }

        public abstract string Act(Player player, bool defending);

        public virtual string Describe()
        {
            return $"{Name} [HP: {Hp}/{MaxHp}, атака: {Attack}, защита: {Defense}]";
        }

        protected string Report(AttackOutcome outcome, string prefix)
        {
            if (outcome.Dodged)
            {
                return $"{prefix}Вы уклонились от атаки {Name}!";
            }

            string blockText = outcome.Blocked > 0 ? $" (блок поглотил {outcome.Blocked})" : "";
            return $"{prefix}{Name} наносит вам {outcome.Damage} урона{blockText}.";
        }
    }

    public class Goblin : Enemy
    {
        public const int BaseHp = 30;
        public const int BaseAttack = 8;
        public const int BaseDefense = 3;
        public const double BaseCritChance = 0.2;
        public const double CritMultiplier = 2.0;

        public double CritChance { get; }

        public Goblin() : this("Гоблин", 1.0, 1.0, 1.0, 0.0)
        {
        }

        protected Goblin(string name, double hpMul, double atkMul, double defMul, double critBonus)
            : base(name, Scale(BaseHp, hpMul), Scale(BaseAttack, atkMul), Scale(BaseDefense, defMul))
        {
            CritChance = BaseCritChance + critBonus;
        }

        public override string Act(Player player, bool defending)
        {
            bool crit = Rng.Chance(CritChance);
            int damage = crit ? (int)Math.Round(Attack * CritMultiplier) : Attack;
            AttackOutcome outcome = player.ReceiveAttack(damage, false, defending);
            return Report(outcome, crit ? "КРИТИЧЕСКИЙ УДАР! " : "");
        }
    }

    public class Skeleton : Enemy
    {
        public const int BaseHp = 25;
        public const int BaseAttack = 9;
        public const int BaseDefense = 2;

        public double FreezeChance { get; }

        public Skeleton() : this("Скелет", 1.0, 1.0, 1.0, 0.0)
        {
        }

        protected Skeleton(string name, double hpMul, double atkMul, double defMul, double freezeChance)
            : base(name, Scale(BaseHp, hpMul), Scale(BaseAttack, atkMul), Scale(BaseDefense, defMul))
        {
            FreezeChance = freezeChance;
        }

        public override string Act(Player player, bool defending)
        {
            AttackOutcome outcome = player.ReceiveAttack(Attack, true, defending);
            string result = Report(outcome, "Удар игнорирует броню! ");
            if (FreezeChance > 0 && Rng.Chance(FreezeChance))
            {
                player.Frozen = true;
                result += " Вы заморожены и пропустите следующий ход!";
            }
            return result;
        }
    }

    public class Mage : Enemy
    {
        public const int BaseHp = 20;
        public const int BaseAttack = 7;
        public const int BaseDefense = 1;
        public const double BaseFreezeChance = 0.25;

        public double FreezeChance { get; }

        public Mage() : this("Маг", 1.0, 1.0, 1.0, 0.0)
        {
        }

        protected Mage(string name, double hpMul, double atkMul, double defMul, double freezeBonus)
            : base(name, Scale(BaseHp, hpMul), Scale(BaseAttack, atkMul), Scale(BaseDefense, defMul))
        {
            FreezeChance = BaseFreezeChance + freezeBonus;
        }

        public override string Act(Player player, bool defending)
        {
            AttackOutcome outcome = player.ReceiveAttack(Attack, false, defending);
            string result = Report(outcome, "");
            if (Rng.Chance(FreezeChance))
            {
                player.Frozen = true;
                result += " Вы заморожены и пропустите следующий ход!";
            }
            return result;
        }
    }

    public class VVG : Goblin
    {
        public VVG() : base("БОСС ВВГ", 2.0, 1.5, 1.2, 0.10)
        {
        }
    }

    public class Kovalsky : Skeleton
    {
        public Kovalsky() : base("БОСС Ковальский", 2.5, 1.3, 1.4, 0.0)
        {
        }
    }

    public class ArchMageCpp : Mage
    {
        public ArchMageCpp() : base("БОСС Архимаг C++", 1.8, 1.6, 1.1, 0.10)
        {
        }
    }

    public class Pestov : Skeleton
    {
        public Pestov() : base("БОСС Пестов С--", 1.3, 1.8, 0.6, Mage.BaseFreezeChance + 0.15)
        {
        }
    }

    public static class EnemyFactory
    {
        private static readonly List<Func<Enemy>> Regular = new List<Func<Enemy>>
        {
            () => new Goblin(),
            () => new Skeleton(),
            () => new Mage()
        };

        private static readonly List<Func<Enemy>> Bosses = new List<Func<Enemy>>
        {
            () => new VVG(),
            () => new Kovalsky(),
            () => new ArchMageCpp(),
            () => new Pestov()
        };

        public static Enemy CreateRegular()
        {
            return Rng.Pick(Regular)();
        }

        public static Enemy CreateBoss()
        {
            return Rng.Pick(Bosses)();
        }
    }

    public enum PlayerAction
    {
        Attack,
        Defend
    }

    public class Game
    {
        private const int BossInterval = 10;

        private readonly Player _player;
        private int _turn;

        public Game()
        {
            _player = new Player(100, new Weapon("Деревянный меч", 7), new Armor("Тряпичная одежда", 2));
        }

        public void Run()
        {
            Console.WriteLine("=== ТЕКСТОВЫЙ РОГАЛИК ===");
            Console.WriteLine("Нажмите Enter для следующего хода или введите q для выхода.");

            while (_player.IsAlive)
            {
                Console.WriteLine();
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input == null || input.Trim().ToLower() == "q")
                {
                    Console.WriteLine("Вы покинули подземелье.");
                    return;
                }

                _turn++;
                Console.WriteLine($"\n----- Ход {_turn} -----");
                PrintPlayerStatus();

                if (_turn % BossInterval == 0)
                {
                    Console.WriteLine("Земля дрожит... Появляется БОСС!");
                    Fight(EnemyFactory.CreateBoss());
                }
                else if (Rng.Chance(0.5))
                {
                    Console.WriteLine("Вы встретили врага!");
                    Fight(EnemyFactory.CreateRegular());
                }
                else
                {
                    Console.WriteLine("Вы нашли сундук!");
                    OpenChest();
                }
            }

            Console.WriteLine($"\nВы погибли на ходу {_turn}. Игра окончена.");
        }

        private void PrintPlayerStatus()
        {
            Console.WriteLine($"Вы: HP {_player.Hp}/{_player.MaxHp} | Оружие: {_player.Weapon.Describe()} | Броня: {_player.Armor.Describe()}");
        }

        private void Fight(Enemy enemy)
        {
            Console.WriteLine($"Противник: {enemy.Describe()}");

            while (_player.IsAlive && enemy.IsAlive)
            {
                bool defending = false;

                if (_player.Frozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    _player.Frozen = false;
                }
                else
                {
                    PlayerAction action = AskAction();
                    if (action == PlayerAction.Attack)
                    {
                        int damage = enemy.TakeDamage(_player.AttackPower);
                        Console.WriteLine($"Вы наносите {enemy.Name} {damage} урона.");
                        if (!enemy.IsAlive)
                        {
                            Console.WriteLine($"{enemy.Name} повержен!");
                            return;
                        }
                    }
                    else
                    {
                        defending = true;
                        Console.WriteLine("Вы принимаете защитную стойку.");
                    }
                }

                Console.WriteLine(enemy.Act(_player, defending));
                Console.WriteLine($"Ваше HP: {_player.Hp}/{_player.MaxHp} | HP {enemy.Name}: {enemy.Hp}/{enemy.MaxHp}");
            }
        }

        private PlayerAction AskAction()
        {
            while (true)
            {
                Console.Write("Выберите действие: 1 - Атака, 2 - Защита: ");
                string input = Console.ReadLine()?.Trim();
                if (input == "1")
                {
                    return PlayerAction.Attack;
                }
                if (input == "2")
                {
                    return PlayerAction.Defend;
                }
                Console.WriteLine("Некорректный ввод. Введите 1 или 2.");
            }
        }

        private void OpenChest()
        {
            switch (Rng.Next(0, 2))
            {
                case 0:
                    _player.HealFully();
                    Console.WriteLine($"Внутри лечебное зелье! HP полностью восстановлено: {_player.Hp}/{_player.MaxHp}.");
                    break;
                case 1:
                    Weapon weapon = ItemFactory.RandomWeapon();
                    Console.WriteLine("Внутри оружие!");
                    Console.WriteLine($"Новое:  {weapon.Describe()}");
                    Console.WriteLine($"Текущее: {_player.Weapon.Describe()}");
                    if (AskTake())
                    {
                        _player.EquipWeapon(weapon);
                        Console.WriteLine("Вы взяли новое оружие.");
                    }
                    else
                    {
                        Console.WriteLine("Вы выбросили оружие.");
                    }
                    break;
                default:
                    Armor armor = ItemFactory.RandomArmor();
                    Console.WriteLine("Внутри доспех!");
                    Console.WriteLine($"Новый:  {armor.Describe()}");
                    Console.WriteLine($"Текущий: {_player.Armor.Describe()}");
                    if (AskTake())
                    {
                        _player.EquipArmor(armor);
                        Console.WriteLine("Вы надели новый доспех.");
                    }
                    else
                    {
                        Console.WriteLine("Вы выбросили доспех.");
                    }
                    break;
            }
        }

        private bool AskTake()
        {
            while (true)
            {
                Console.Write("1 - Взять новый предмет, 2 - Выбросить: ");
                string input = Console.ReadLine()?.Trim();
                if (input == "1")
                {
                    return true;
                }
                if (input == "2")
                {
                    return false;
                }
                Console.WriteLine("Некорректный ввод. Введите 1 или 2.");
            }
        }
    }

    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            new Game().Run();
        }
    }
}