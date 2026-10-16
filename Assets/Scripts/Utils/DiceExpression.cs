using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public struct DiceExpression
    {
        [SerializeField] private int diceNumber;
        [SerializeField] private Die die;
        public int DiceNumber => diceNumber;
        public Die Die => die;

        public DiceExpression(int diceNumber, Die die)
        {
            this.diceNumber = diceNumber;
            this.die = die;
        }
    }
}