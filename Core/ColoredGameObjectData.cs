using System;
using UnityEngine;

namespace VolumeBox.Colorbox.Core
{
    [Serializable]
    public class ColoredGameObjectData
    {
        public GameObject Reference;
        public bool EnabledCustomization;
        public Font Font;
        public int FontSize;
        public Color BackgroundColor = Color.aliceBlue;
        public Color TextColor;
        public TextAnchor TextAlignment;
    }

    public class ColoredGameObjectWrapper : ScriptableObject
    {
        public ColoredGameObjectData Data;
    }
}