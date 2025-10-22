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
        public BackgroundFillType FillType;
        public Gradient BackgroundGradient;
        public Color BackgroundColor;
        public Texture2D BackgroundTexture;
        public Color BackgroundTextureTint;
        public Color TextColor;
        public TextAnchor TextAlignment;
    }

    public class ColoredGameObjectWrapper : ScriptableObject
    {
        public ColoredGameObjectData Data;
    }

    public enum BackgroundFillType
    {
        Color,
        Gradient,
        Texture,
    }
}