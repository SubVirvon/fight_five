using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fight_five
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FightingArena arena = new FightingArena();

            arena.Work();
        }
    }

    class FightingArena
    {
        private Fighter _fighter1;
        private Fighter _fighter2;
        public void Work()
        {
            _fighter1 = CreateFighter("1");
            _fighter2 = CreateFighter("2");

            СhangeDuplicateNames(_fighter1, _fighter2);
            Fight();

            Console.ReadKey();
        }

        private Fighter CreateFighter(string FighterNumber)
        {
            const string CommandChooseHeavyFighter = "1";
            const string CommandChooseLightFighter = "2";
            const string CommandChooseMediumFighter  = "3";
            const string CommandChooseVampire = "4";
            const string CommandChooseRobot = "5";
            const string CommandShowPeculiarities = "6";
            bool isChosen = false;
            Fighter fighter = null;

            while(isChosen == false)
            {
                Console.Clear();
                Console.Write($"Выберите {FighterNumber} бойца:\n{CommandChooseHeavyFighter} Непробиваемый Том\n{CommandChooseLightFighter} Молния Джим\n{CommandChooseMediumFighter} Храбрый Рон\n{CommandChooseVampire} Кусака Майкл\n{CommandChooseRobot} Робот\n{CommandShowPeculiarities} Показать Особенности\nНомер: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case CommandChooseHeavyFighter:
                        fighter = new HeavyFighter(25, 370, 1, 12);
                        break;
                    case CommandChooseLightFighter:
                        fighter = new LightFighter(62, 125, 4, 25);
                        break;
                    case CommandChooseMediumFighter:
                        fighter = new MediumFighter(42, 195, 5, 3, 1.5f);
                        break;
                    case CommandChooseVampire:
                        fighter = new Vampire(56, 150, 3, 17);
                        break;
                    case CommandChooseRobot:
                        fighter = new Robot(65, 150, 10, 16, 68, 50);
                        break;
                    case CommandShowPeculiarities:
                        ShowPeculiarities();
                        break;
                    default:
                        break;
                }

                isChosen = fighter != null;
            }

            return fighter;
        }

        private void ShowPeculiarities()
        {
            Console.Clear();
            Console.WriteLine("Непробиваемый Том имеет шанс заблокировать весь входящий урон\n\nМолния Джим имеет шанс уклониться от удара соперника\n\nХрабрый Рон каждый 3 удар наносит повышенный урон\n\nКусака Майкл востанавливает часть нанесенного урона\n\nРобот имеет щит, но после потери половины здоровья деградирует");
            Console.ReadKey();
        }

        private void СhangeDuplicateNames(Fighter fighter1, Fighter fighter2)
        {
            if(fighter1.Name == fighter2.Name)
            {
                fighter1.ChangeColor("cиний");
                fighter2.ChangeColor("красный");
            }
        }

        private void Fight()
        {
            Random random = new Random();
            bool isFighter1Initiative = _fighter1.Initiative >= _fighter2.Initiative;
            bool isFinish = false;
            Fighter firstFighter;
            Fighter secondFighter;

            Console.Clear();

            if (isFighter1Initiative)
            {
                Console.WriteLine($"Первым бьет {_fighter1.Name}\n");
                firstFighter = _fighter1;
                secondFighter = _fighter2;
            }
            else
            {
                Console.WriteLine($"Первым бьет {_fighter2.Name}");
                firstFighter = _fighter2;
                secondFighter = _fighter1;
            }
            
            while(isFinish == false)
            {
                if (firstFighter.CheckDie() == false)
                {
                    firstFighter.Hit(secondFighter, random);
                }
                else
                {
                    isFinish = true;
                    ShowWinner(secondFighter);
                    break;
                }

                if (secondFighter.CheckDie() == false)
                {
                    secondFighter.Hit(firstFighter, random);
                }
                else
                {
                    isFinish = true;
                    ShowWinner(firstFighter);
                    break;
                }
            }
        }

        private void ShowWinner(Fighter winner)
        {
            Console.WriteLine($"\nПобедил {winner.Name}!");
        }
    }

    abstract class Fighter
    {
        protected int Health;
        protected int MaxDamageMultiplierPercentage;
        protected int Damage;
        public string Name { get; private set; }
        public int Initiative { get; private set; }
       
        public Fighter(int damage, int health, int initiative, string name)
        {
            Name = name;
            Damage = damage;
            Health = health;
            Initiative = initiative;
            MaxDamageMultiplierPercentage = 60;
        }

        public virtual void Hit(Fighter enemy, Random random)
        {
            int damage = (int)((float)Damage * (((float)random.Next(MaxDamageMultiplierPercentage + 1) / 100) + 1));

            Console.WriteLine($"\n{Name} бьет с силой {damage} урона");

            enemy.GetDamage(damage, random);
        }

        public virtual void GetDamage(int damage, Random random)
        {
            Health -= damage;

            if (Health < 0)
                Health = 0;

            Console.WriteLine($"{Name} теряет {damage} здоровья, осталось {Health} здоровья");
        }

        public bool CheckDie()
        {
            return Health <= 0;
        }

        public void ChangeColor(string color)
        {
            Name += $" ({color})";
        }
    }

    class HeavyFighter : Fighter
    {
        private int _chanceToBlockDamage;
        public HeavyFighter(int damage, int health, int initiative, int chanceToBliockDamage) : base(damage, health, initiative, "Непробиваемый Том") 
        {
            _chanceToBlockDamage = chanceToBliockDamage;
        }

        public override void GetDamage(int damage, Random random)
        {
            int chance = random.Next(101);

            if(chance > _chanceToBlockDamage)
                base.GetDamage(damage, random);
            else
                Console.WriteLine($"{Name} не почувствовал удара, осталось {Health} здоровья");
        }
    }

    class LightFighter : Fighter
    {
        private int _chanceToDodge;
        public LightFighter(int damage, int health, int initiative, int chanceToDodge) : base(damage, health, initiative, "Молния Джим")
        {
            _chanceToDodge = chanceToDodge;
        }

        public override void GetDamage(int damage, Random random)
        {
            int chance = random.Next(101);

            if (chance > _chanceToDodge)
                base.GetDamage(damage, random);
            else
                Console.WriteLine($"{Name} проворно уклонился от удара, осталось {Health} здоровья");
        }
    }

    class MediumFighter : Fighter
    {
        private int _hitCount;
        private int _hitCountToCrit;
        private float _critMultiplier;
        public MediumFighter(int damage, int health, int initiative, int hitCountToCrit, float critMultiplier) : base(damage, health, initiative, "Храбрый Рон")
        {
            _hitCount = 0;
            _hitCountToCrit = hitCountToCrit;
            _critMultiplier = critMultiplier;
        }

        public override void Hit(Fighter enemy, Random random)
        {
            _hitCount++;

            if(_hitCount == _hitCountToCrit)
            {
                _hitCount = 0;
                int damage = (int)((float)Damage * (((float)random.Next(MaxDamageMultiplierPercentage + 1) / 100) + 1) * _critMultiplier);

                Console.WriteLine($"\n{Name} бьет с увеличенной силой {damage} урона");

                enemy.GetDamage(damage, random);
            }
            else
            {
                base.Hit(enemy, random);
            }
        }
    }

    class Vampire : Fighter
    {
        private int _healthRecoveryPercentage;
        private int _maxHealth;
        public Vampire(int damage, int health, int initiative, int healthRecoveryPercent) : base(damage, health, initiative, "Кусака Майкл")
        {
            _healthRecoveryPercentage = healthRecoveryPercent;
            _maxHealth = health;
        }

        public override void Hit(Fighter enemy, Random random)
        {
            int damage = (int)((float)Damage * (((float)random.Next(MaxDamageMultiplierPercentage + 1) / 100) + 1));
            int healthRecovery = (int)((float)damage * ((float)_healthRecoveryPercentage / 100));

            if (_maxHealth - Health < healthRecovery)
                healthRecovery = _maxHealth - Health;

            Health += healthRecovery;

            Console.Write($"\n{Name} бьет с силой {damage} урона");

            if (healthRecovery != 0)
                Console.WriteLine($" и восстанавливает {healthRecovery} здоровья");
            else
                Console.WriteLine();

            enemy.GetDamage(damage, random);
        }
    }

    class Robot : Fighter
    {
        private int _degradationPercentage;
        private int _degradationBorder;
        private int _shield;
        private int _maxHealth;
        public Robot(int damage, int health, int initiative, int shield, int degradationPercentage, int degradationLimitPercentage) : base(damage, health, initiative, "Робот")
        {
            _shield = shield;
            _degradationPercentage = degradationPercentage;
            _maxHealth = health;
            _degradationBorder = (int)((float)_maxHealth * ((float)degradationLimitPercentage / 100));
        }

        public override void Hit(Fighter enemy, Random random)
        {

            if(Health <= _degradationBorder)
            {
                int damage = (int)((float)Damage * (((float)random.Next(MaxDamageMultiplierPercentage + 1) / 100) + 1) * (1 - ((float)_degradationPercentage / 100)));
                Console.WriteLine($"\n{Name} бьет с уменьшенной силой {damage} урона");

                enemy.GetDamage(damage, random);
            }
            else
            {
                base.Hit(enemy, random);
            }
        }

        public override void GetDamage(int damage, Random random)
        {
            if(Health <= _degradationBorder)
            {
                base.GetDamage(damage, random);
            }
            else
            {
                damage = (int)((float)damage * (1 - (float)_shield / 100));
                Health -= damage;

                if (Health < 0)
                    Health = 0;

                Console.WriteLine($"{Name} теряет {damage} здоровья, осталось {Health} здоровья");
            }
        }
    }
}

