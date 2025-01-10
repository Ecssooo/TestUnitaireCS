
namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition abstraite d'une compétence.
    /// </summary>
    public abstract class Skill
    {
        public Skill(TYPE type, int power, StatusPotential status)
        {
            Type = type;
            Power = power;
            Status = status;
        }

        /// <summary>
        /// Le type de l'attaque, à prendre en compte lors de la résolution des résistance/faiblesses
        /// </summary>
        public TYPE Type { get; private set; }
        /// <summary>
        /// La puissance du coup, à prendre en compte lors de la résolution des dégâts
        /// </summary>
        public int Power { get; private set; }
        /// <summary>
        /// Le statut infligé à la cible à la suite de l'attaque
        /// </summary>
        public StatusPotential Status { get; private set; }

    }

}
