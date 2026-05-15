using System;
using UnityEngine;
using WheelOfFortune.Enums;

namespace WheelOfFortune.Config
{
    [CreateAssetMenu(fileName = "RankColorPalette", menuName = "WheelOfFortune/Config/RankColorPalette")]
    public class RankColorPalette : ScriptableObject
    {
        [Serializable]
        public struct RankColor
        {
            public Rank Rank;
            public Color Color;
        }

        public Color defaultColor = Color.white;
        public RankColor[] rankColors;

        public Color GetColor(Rank rank)
        {
            foreach (var item in rankColors)
            {
                if (item.Rank == rank)
                    return item.Color;
            }
            return defaultColor;
        }
    }
}