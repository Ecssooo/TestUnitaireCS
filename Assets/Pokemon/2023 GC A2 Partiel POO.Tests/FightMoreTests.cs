using System.Security.Cryptography.X509Certificates;
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using UnityEditor.VersionControl;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer des features et les TU sur le reste du projet
        
        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
                                    // - un heal ne régénère pas plus que les HP Max
                                    // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
                                    // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
                                    // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
                                    // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
                                    // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type
        // - L'utilisation d'objets : Potion, SuperPotion, Vitess+, Attack+ etc.
                                    // - Gérer les PP (limite du nombre d'utilisation) d'une attaque,
                                        // si on selectionne une attack qui a 0 PP on inflige 0
        
        // Choisis ce que tu veux ajouter comme feature et fait en au max.
        // Les nouveaux TU doivent être dans ce fichier.
        // Modifiant des features il est possible que certaines valeurs
            // des TU précédentes ne matchent plus, tu as le droit de réadapter les valeurs
            // de ces anciens TU pour ta nouvelle situation.


            [Test]
            public void CharacterReceiveHeal()
            {
                var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
                var punch = new Punch();
                var oldHealth = pikachu.CurrentHealth;
                
                pikachu.ReceiveAttack(punch, pikachu);
                Assert.That(pikachu.CurrentHealth, Is.EqualTo(oldHealth - (punch.Power - pikachu.Defense)));
                oldHealth -= (punch.Power - pikachu.Defense);
                pikachu.ReceiveHealth(10);
                Assert.That(pikachu.CurrentHealth, Is.EqualTo(oldHealth + 10));
            }

            [Test]
            public void CharacterReceiveTooMuchHeal()
            {
                var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
                var oldHealth = pikachu.CurrentHealth;
                pikachu.ReceiveHealth(10);
                Assert.That(pikachu.CurrentHealth, Is.EqualTo(oldHealth));
            }

            [Test]
            public void ReduceCharacterMaxHealth()
            {
                var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
                var oldMaxHealth = pikachu.MaxHealth;
                var oldHealth = pikachu.CurrentHealth;
                
                pikachu.ReduceMaxHealth(50);
                
                Assert.That(pikachu.MaxHealth, Is.EqualTo(50));
                Assert.That(pikachu.CurrentHealth, Is.EqualTo(pikachu.MaxHealth));
            }

            [Test]
            public void CharacterReceiveDamage()
            {
                var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
                var oldHealth = pikachu.CurrentHealth;
                
                pikachu.ReceiveDamage(5);
                Assert.That(pikachu.CurrentHealth, Is.EqualTo(oldHealth - 5));
            }

            [Test]
            public void CharacterReceiveStatus()
            {
                var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
                var statu = new SleepStatus();
                
                pikachu.ReceiveStatus(statu);
                Assert.That(pikachu.CurrentStatus, Is.EqualTo(statu));
            }
            
            [Test]
            public void PriorityEquipment()
            {
                var pikachu = new Character(100, 5000, 30, 1, TYPE.NORMAL);
                var mewtwo = new Character(100, 5000, 30, 20, TYPE.NORMAL);
                var priorityEquipment = new Equipment(0, 0, 0, 100000, TYPE.NORMAL);
                pikachu.Equip(priorityEquipment);
                var fight = new Fight(pikachu, mewtwo);
                var punch = new Punch();
                var megaPunch = new MegaPunch();
                
                fight.ExecuteTurn(megaPunch, punch);
                Assert.That(pikachu.IsAlive, Is.EqualTo(true));
                Assert.That(mewtwo.IsAlive, Is.EqualTo(false));
            }

            [Test]
            public void FightWithStatus()
            {
                var pikachu = new Character(100, 50, 30, 21, TYPE.NORMAL);
                var mewtwo = new Character(100, 50, 30, 20, TYPE.NORMAL);
                var fight = new Fight(pikachu, mewtwo);

                var fireBall = new FireBall();
                var mewtwoOldHealth = mewtwo.CurrentHealth;
                var statusDamage = StatusEffect.GetNewStatusEffect(fireBall.Status).DamageEachTurn;
                
                var magicalGrass = new MagicalGrass();
                var statusRemainingTurn = StatusEffect.GetNewStatusEffect(magicalGrass.Status).RemainingTurn;
                
                fight.ExecuteTurn(fireBall, magicalGrass);

                Assert.That(pikachu.CurrentStatus.CanAttack, Is.EqualTo(false));
                Assert.That(pikachu.CurrentStatus.RemainingTurn, Is.EqualTo(statusRemainingTurn-1));
                Assert.That(mewtwo.CurrentHealth, Is.EqualTo(mewtwoOldHealth - (fireBall.Power - mewtwo.Defense)-statusDamage));
            }

            [Test]
            public void TestTypeFactor()
            {
                var attack = TYPE.WATER;
                var defense = TYPE.FIRE;
                
                Assert.That(TypeResolver.GetFactor(attack, defense), Is.EqualTo(1.2f));
            }

            [Test]
            public void FightWithTypeFactor()
            {
                var charmander = new Character(100, 50, 10, 21, TYPE.FIRE);
                var squirtle = new Character(100, 50, 10, 20, TYPE.WATER);

                var charmanderOldHealth = charmander.CurrentHealth;
                var squirtleOldHealth = squirtle.CurrentHealth;
                
                var fight = new Fight(charmander, squirtle);

                var water = new WaterBlouBlou();
                var fire = new FireBall();
                
                fight.ExecuteTurn(fire, water);
                squirtle.ReceiveHealth(10); //Burn damage
                Assert.That(charmander.CurrentHealth,
                    Is.EqualTo(charmanderOldHealth - ((water.Power - charmander.Defense) *
                                     TypeResolver.GetFactor(water.Type, charmander.BaseType))));
                Assert.That(squirtle.CurrentHealth,
                    Is.EqualTo(squirtleOldHealth - ((fire.Power - squirtle.Defense) *
                                                      TypeResolver.GetFactor(fire.Type, squirtle.BaseType))));
            }

            [Test]
            public void FightWithPP()
            {
                var charmander = new Character(100, 50, 10, 21, TYPE.FIRE);
                var squirtle = new Character(100, 50, 10, 20, TYPE.WATER);

                var charmanderOldHealth = charmander.CurrentHealth;
                var squirtleOldHealth = squirtle.CurrentHealth;
                
                var fight = new Fight(charmander, squirtle);

                var water = new WaterBlouBlou();
                var fire = new FireBall();
                
                fight.ExecuteTurn(fire, water);
                squirtle.ReceiveHealth(10); //Burn damage
                fight.ExecuteTurn(fire, water);
                squirtle.ReceiveHealth(10); //Burn damage
                
                Assert.That(charmander.CurrentHealth,
                    Is.EqualTo(charmanderOldHealth - ((water.Power - charmander.Defense) *
                                                      TypeResolver.GetFactor(water.Type, charmander.BaseType))));
                Assert.That(squirtle.CurrentHealth,
                    Is.EqualTo(squirtleOldHealth - ((fire.Power - squirtle.Defense) *
                                                    TypeResolver.GetFactor(fire.Type, squirtle.BaseType))));
                
                Assert.That(water.PP, Is.EqualTo(0));
                Assert.That(fire.PP, Is.EqualTo(0));
                
            }

            [Test]
            public void FightWithEquipementFactor()
            {
                var charmander = new Character(100, 50, 10, 21, TYPE.FIRE);
                var squirtle = new Character(100, 50, 10, 20, TYPE.WATER);

                var charmanderOldHealth = charmander.CurrentHealth;
                var squirtleOldHealth = squirtle.CurrentHealth;
                
                var fight = new Fight(charmander, squirtle);

                var water = new WaterBlouBlou();
                var fire = new FireBall();

                var equipement = new Equipment(0, 0, 0, 0, TYPE.WATER);
                
                charmander.Equip(equipement);
                
                fight.ExecuteTurn(fire, water);
                squirtle.ReceiveHealth(10); //Burn damage
                
                Assert.That(charmander.CurrentHealth,
                    Is.EqualTo(charmanderOldHealth - ((water.Power - charmander.Defense * TypeResolver.GetFactor(water.Type, charmander.CurrentEquipment.BonusResistance)) *
                                                      TypeResolver.GetFactor(water.Type, charmander.BaseType))));
                Assert.That(squirtle.CurrentHealth,
                    Is.EqualTo(squirtleOldHealth - ((fire.Power - squirtle.Defense) *
                                                    TypeResolver.GetFactor(fire.Type, squirtle.BaseType))));
            }
    }
}
