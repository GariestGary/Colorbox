using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace VolumeBox.Colorbox.Core
{
    [Serializable]
    public class ColoredGameObjectData
    {
        public GameObject Reference;
        public bool EnabledCustomization;
        public Font Font;
        public int FontSize = 12;
        public BackgroundFillType FillType = BackgroundFillType.Color;
        public Gradient BackgroundGradient;
        public FontStyle FontStyle;
        public Color BackgroundColor = Color.black;
        public Texture2D BackgroundTexture;
        public Color BackgroundTextureTint = Color.white;
        public Color FontColor;
        public TextAnchor TextAlignment = TextAnchor.MiddleLeft;
        public ColorboxSceneData SceneData;
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