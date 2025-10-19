using System;
using UnityEngine;

namespace VolumeBox.Colorbox.Core
{
    [Serializable]
    public class ColoredGameObjectData
    {
        public GameObject Reference;
        public int FontSize;
        public Color BackgroundColor = Color.aliceBlue;
        public Color TextColor;
    }
}