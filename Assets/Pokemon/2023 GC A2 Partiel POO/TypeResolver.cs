
using System;
using System.Collections.Generic;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition des types dans le jeu
    /// </summary>
    public enum TYPE { NORMAL, WATER, FIRE, GRASS}

    public class TypeResolver
    {
        private static float[,] tabType = { 
            { 1, 1, 1, 1 }, 
            { 1, 1, 1.2f, 0.8f },
            { 1, 0.8f, 1, 1.2f },
            {1, 0.8f, 0.8f, 1} }; 
        
        private static Dictionary<TYPE, int> typePosition = new( ) 
        {
            [TYPE.NORMAL] = 0 ,
            [TYPE.WATER] = 1 ,
            [TYPE.FIRE] = 2,
            [TYPE.GRASS] = 3,
        };
        /// <summary>
        /// Récupère le facteur multiplicateur pour la résolution des résistances/faiblesses
        /// WATER faible contre GRASS, resiste contre FIRE
        /// FIRE faible contre WATER, resiste contre GRASS
        /// GRASS faible contre FIRE, resiste contre WATER
        /// </summary>
        /// <param name="attacker">Type de l'attaque (le skill)</param>
        /// <param name="receiver">Type de la cible</param>
        /// <returns>
        /// Normal returns 1 if attacker or receiver
        /// 0.8 if resist
        /// 1.0 if same type
        /// 1.2 if vulnerable
        /// </returns>
        public static float GetFactor(TYPE attacker, TYPE receiver)
        {
            return tabType[typePosition[attacker], typePosition[receiver]];
        }

    }
}
