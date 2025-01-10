using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using System;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class CharacterTests
    {
        [Test]
        public void CharacterConstructor()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);

            Assert.That(c.MaxHealth, Is.EqualTo(100));
            Assert.That(c.Attack, Is.EqualTo(50));
            Assert.That(c.Defense, Is.EqualTo(30));
            Assert.That(c.Speed, Is.EqualTo(20));
            Assert.That(c.BaseType, Is.EqualTo(TYPE.NORMAL));

            // Character starts full life
            Assert.That(c.CurrentHealth, Is.EqualTo(100));
        }

        [Test]
        public void EquipmentConstructor()
        {
            var e = new Equipment(100, 90, 70, 12);
            Assert.That(e.BonusHealth, Is.EqualTo(100));
            Assert.That(e.BonusAttack, Is.EqualTo(90));
            Assert.That(e.BonusDefense, Is.EqualTo(70));
            Assert.That(e.BonusSpeed, Is.EqualTo(12));
        }

        [Test]
        public void CharacterEquipped()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);
            var e = new Equipment(100, 90, 70, 12);

            // Equip character
            c.Equip(e);
            Assert.That(c.CurrentEquipment, Is.EqualTo(e));
            Assert.That(c.MaxHealth, Is.EqualTo(200));
            Assert.That(c.Attack, Is.EqualTo(140));
            Assert.That(c.Defense, Is.EqualTo(100));
            Assert.That(c.Speed, Is.EqualTo(32));

            // Increase MaxHealth doesn't increase CurrentHealth
            Assert.That(c.CurrentHealth, Is.EqualTo(100));  

            // Then remove equipment
            c.Unequip();
            Assert.That(c.CurrentEquipment, Is.EqualTo(null));
            Assert.That(c.MaxHealth, Is.EqualTo(100));
            Assert.That(c.Attack, Is.EqualTo(50));
            Assert.That(c.Defense, Is.EqualTo(30));
            Assert.That(c.Speed, Is.EqualTo(20));
            Assert.That(c.BaseType, Is.EqualTo(TYPE.NORMAL));
        }

        [Test]
        public void CharacterEquippedNullCheck()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Equip character
                c.Equip(null);
            });
        }

        [Test]
        public void CharacterReceivePunch()
        {
            var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            var punch = new Punch();
            var oldHealth = pikachu.CurrentHealth;

            pikachu.ReceiveAttack(punch); // hp : 100 => 60
            Assert.That(pikachu.CurrentHealth, 
                Is.EqualTo(oldHealth - (punch.Power - pikachu.Defense))); // 100 - (70-30)
            Assert.That(pikachu.CurrentStatus, Is.EqualTo(null));
            Assert.That(pikachu.IsAlive, Is.EqualTo(true));
            
            pikachu.ReceiveAttack(punch); // hp : 60 => 20
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(20));
            Assert.That(pikachu.IsAlive, Is.EqualTo(true));
            
            pikachu.ReceiveAttack(punch); // hp : 20 => 0
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(0));
            Assert.That(pikachu.IsAlive, Is.EqualTo(false));
            // RIP Pikachu
        }

        [Test]
        public void CharacterEquippedReceivePunch()
        {
            var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            var shield = new Equipment(0, 0, 10, 0);
            pikachu.Equip(shield);

            var punch = new Punch();
            var oldHealth = pikachu.CurrentHealth;

            pikachu.ReceiveAttack(punch); // hp : 100 => 70
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(70)); 
            Assert.That(pikachu.CurrentStatus, Is.EqualTo(null));
            Assert.That(pikachu.IsAlive, Is.EqualTo(true));

            pikachu.ReceiveAttack(punch); // hp : 70 => 40
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(40)); 
            Assert.That(pikachu.IsAlive, Is.EqualTo(true));

            pikachu.ReceiveAttack(punch); // hp : 40 => 10
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(10));
            Assert.That(pikachu.IsAlive, Is.EqualTo(true));

            pikachu.ReceiveAttack(punch); // hp : 10 => 0
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(0));
            Assert.That(pikachu.IsAlive, Is.EqualTo(false));
            // RIP Pikachu
        }

        [Test]
        public void FightConstructor()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character mewtwo = new Character(1000, 500, 300, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, mewtwo);

            Assert.That(f.Character1, Is.EqualTo(pikachu));
            Assert.That(f.Character2, Is.EqualTo(mewtwo));
            Assert.That(f.IsFightFinished, Is.EqualTo(false));
        }

        [Test]
        public void CreateFightNullCheck()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character mewtwo = new Character(1000, 500, 300, 200, TYPE.NORMAL);

            Assert.Throws<ArgumentNullException>(() =>
            {
                Fight f = new Fight(pikachu, null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                Fight f = new Fight(null, mewtwo);
            });
        }

        [Test]
        public void FightWithOneTurn()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character bulbizarre = new Character(90, 60, 10, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, bulbizarre);
            Punch p = new Punch();

            // Both uses punch
            f.ExecuteTurn(p, p);

            Assert.That(pikachu.IsAlive, Is.EqualTo(true));
            Assert.That(bulbizarre.IsAlive, Is.EqualTo(true));
            Assert.That(f.IsFightFinished, Is.EqualTo(false));
        }

        [Test]
        public void FightWithOneShotTurn()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character mewtwo = new Character(1000, 5000, 0, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, mewtwo);
            Punch p = new Punch();
            MegaPunch mp = new MegaPunch();

            // mewtwo attacks first, oneshot pikachu, so pikachu doesn't attack
            f.ExecuteTurn(p, mp);

            Assert.That(pikachu.IsAlive, Is.EqualTo(false));
            Assert.That(mewtwo.IsAlive, Is.EqualTo(true));
            Assert.That(mewtwo.CurrentHealth, Is.EqualTo(mewtwo.MaxHealth));
            Assert.That(f.IsFightFinished, Is.EqualTo(true));
        }

    }
}
