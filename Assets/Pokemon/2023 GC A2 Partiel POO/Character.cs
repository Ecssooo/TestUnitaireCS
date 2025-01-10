using System;
using UnityEditor.UIElements;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition d'un personnage
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Stat de base, HP
        /// </summary>
        int _baseHealth;
        /// <summary>
        /// Stat de base, ATK
        /// </summary>
        int _baseAttack;
        /// <summary>
        /// Stat de base, DEF
        /// </summary>
        int _baseDefense;
        /// <summary>
        /// Stat de base, SPE
        /// </summary>
        int _baseSpeed;
        /// <summary>
        /// Type de base
        /// </summary>
        TYPE _baseType;

        public Character(int baseHealth, int baseAttack, int baseDefense, int baseSpeed, TYPE baseType)
        {
            _baseHealth = baseHealth;
            _baseAttack = baseAttack;
            _baseDefense = baseDefense;
            _baseSpeed = baseSpeed;
            _baseType = baseType;
            
            CurrentHealth = _baseHealth;
            CurrentEquipment = null;
            CurrentStatus = StatusEffect.GetNewStatusEffect(StatusPotential.NONE);
        }
        /// <summary>
        /// HP actuel du personnage
        /// </summary>
        public int CurrentHealth { get; private set; }
        public TYPE BaseType { get => _baseType;}
        /// <summary>
        /// HPMax, prendre en compte base et equipement potentiel
        /// </summary>
        public int MaxHealth
        {
            get
            {
                int healthReturn = _baseHealth;
                if (CurrentEquipment != null) healthReturn += CurrentEquipment.BonusHealth;  
                return healthReturn;
            }
        }
        /// <summary>
        /// ATK, prendre en compte base et equipement potentiel
        /// </summary>
        public int Attack
        {
            get
            {
                int attackReturn = _baseAttack;
                if (CurrentEquipment != null) attackReturn += CurrentEquipment.BonusAttack;  
                return attackReturn;

            }
        }
        /// <summary>
        /// DEF, prendre en compte base et equipement potentiel
        /// </summary>
        public int Defense
        {
            get
            {
                int defenseReturn = _baseDefense;
                if (CurrentEquipment != null) defenseReturn += CurrentEquipment.BonusDefense;
                return defenseReturn;
            }
        }
        /// <summary>
        /// SPE, prendre en compte base et equipement potentiel
        /// </summary>
        public int Speed
        {
            get
            {
                int speedReturn = _baseSpeed;
                if (CurrentEquipment != null) speedReturn += CurrentEquipment.BonusSpeed;  
                return speedReturn;

            }
        }
        /// <summary>
        /// Equipement unique du personnage
        /// </summary>
        public Equipment CurrentEquipment { get; private set; }
        /// <summary>
        /// null si pas de status
        /// </summary>
        public StatusEffect CurrentStatus { get; private set; }

        public bool IsAlive => CurrentHealth > 0;


        /// <summary>
        /// Application d'un skill contre le personnage
        /// On pourrait potentiellement avoir besoin de connaitre le personnage attaquant,
        /// Vous pouvez adapter au besoin
        /// </summary>
        /// <param name="s">skill attaquant</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ReceiveAttack(Skill s, Character attacker)
        {
            float typeFactor = TypeResolver.GetFactor(s.Type, _baseType);
            float equipementFactor = 1;
            if (CurrentEquipment != null)
            {
                equipementFactor = TypeResolver.GetFactor(s.Type, CurrentEquipment.BonusResistance);
            }
            CurrentHealth -= (int)((s.Power - this.Defense * equipementFactor) * typeFactor);
            ReceiveStatus(s.Status);
            if (CurrentHealth < 0) CurrentHealth = 0;
        }

        public void ReceiveDamage(int amount)
        {
            if (amount <= 0) return;
            CurrentHealth -= amount;
            if (CurrentHealth < 0) CurrentHealth -= 0;
        }
        
        public void ReceiveHealth(int amount)
        {
            if (amount <= 0) return;
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        }

        public void ReduceMaxHealth(int amount)
        {
            if (amount <= 0) return;
            _baseHealth = amount;
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        }

        public void ReceiveStatus(StatusPotential status)
        {
            CurrentStatus = StatusEffect.GetNewStatusEffect(status);
        }
        
        public void ReceiveStatus(StatusEffect status)
        {
            CurrentStatus = status;
        }
        /// <summary>
        /// Equipe un objet au personnage
        /// </summary>
        /// <param name="newEquipment">equipement a appliquer</param>
        /// <exception cref="ArgumentNullException">Si equipement est null</exception>
        public void Equip(Equipment newEquipment)
        {
            if (newEquipment == null) throw new ArgumentNullException();
            CurrentEquipment = newEquipment;
        }
        /// <summary>
        /// Desequipe l'objet en cours au personnage
        /// </summary>
        public void Unequip()
        {
            CurrentEquipment = null;
        }

    }
}
