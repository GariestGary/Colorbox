using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VolumeBox.Colorbox.Core;
using VolumeBox.Colorbox.Editor.AppearanceEditor;

namespace VolumeBox.Colorbox.Editor
{
    [InitializeOnLoad]
    public static class HierarchyDrawer
    {
        private const string DataObjectName = "[Colorbox Scene Data]";
        private static ColorboxSceneData _currentSceneData;
        
        public static ColorboxSceneData CurrentSceneData => _currentSceneData;
        
        static HierarchyDrawer()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnDrawHierarchyItem;
        }

        private static void OnDrawHierarchyItem(int instanceID, Rect selectionRect)
        {
            bool isCtrlPressed = Application.platform == RuntimePlatform.OSXEditor ? Event.current.command : Event.current.control;
            
            if (Event.current.type == EventType.MouseDown && isCtrlPressed && selectionRect.Contains(Event.current.mousePosition))
            {
                GameObject clickedObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
                
                if (clickedObject != null)
                {
                    var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                    AppearanceEditorWindow.ShowWindow(clickedObject, mousePos + new Vector2(15, 15));
                    Event.current.Use();
                }
            }
            
            var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj == null)
            {
                return;
            }

            if (_currentSceneData == null || _currentSceneData.Scene != obj.scene)
            {
                foreach (var rootGameObject in obj.scene.GetRootGameObjects())
                {
                    if (rootGameObject.TryGetComponent(out _currentSceneData))
                    {
                        break;
                    }
                }
            }

            if (_currentSceneData == null)
            {
                return;
            }
            
            var data = _currentSceneData.GetOrAddGameObjectData(obj);
            DrawItem(selectionRect, data);
            _currentSceneData.ValidateGameObjects();
        }
        
        private static void DrawItem(Rect rect, ColoredGameObjectData data)
        {
            if (!data.EnabledCustomization)
            {
                return;
            }

            switch (data.FillType)
            {
                case BackgroundFillType.Color:
                    EditorGUI.DrawRect(rect, data.BackgroundColor);
                    break;
                case BackgroundFillType.Gradient:
                    DrawGradientBackground(rect, data.BackgroundGradient);
                    break;
                case BackgroundFillType.Texture:
                    DrawTiledTexture(rect, data.BackgroundTexture, data.BackgroundTextureTint);
                    break;
            }
            
            var style = new GUIStyle();
            style.font = data.Font;
            style.fontSize = data.FontSize;
            style.fontStyle = data.FontStyle;
            style.alignment = data.TextAlignment;
            style.normal.textColor = data.FontColor;
            EditorGUI.LabelField(rect, data.Reference.name, style);
        }
        
        private static void DrawTiledTexture(Rect rect, Texture2D patternTexture, Color tintColor)
        {
            if (patternTexture == null) return;
    
            // Set the texture to repeat
            patternTexture.wrapMode = TextureWrapMode.Repeat;
    
            // Calculate how many times the texture should repeat
            var tileCountX = rect.width / patternTexture.width;
            var tileCountY = rect.height / patternTexture.height;
    
            // Use tile counts for tex coords - this creates true repeating
            var texCoords = new Rect(0, 0, tileCountX, tileCountY);
            var prevColor = GUI.color;
            GUI.color = tintColor;
            GUI.DrawTextureWithTexCoords(rect, patternTexture, texCoords);
            GUI.color = prevColor;
            
        }
        
        private static void DrawGradientBackground(Rect rect, Gradient gradient, bool horizontal = true)
        {
            if (gradient == null) return;

            var textureWidth = horizontal ? 32 : 1;
            var textureHeight = horizontal ? 1 : 32;
    
            // Create texture with optimal size for the gradient direction
            var texture = new Texture2D(textureWidth, textureHeight)
            {
                wrapMode = TextureWrapMode.Clamp
            };

            // Fill texture with gradient colors
            var pixels = new Color[textureWidth * textureHeight];
    
            for (var i = 0; i < (horizontal ? textureWidth : textureHeight); i++)
            {
                var t = (float)i / (horizontal ? textureWidth - 1 : textureHeight - 1);
                var color = gradient.Evaluate(t);
        
                if (horizontal)
                {
                    // Horizontal gradient - same color for entire column
                    for (var j = 0; j < textureHeight; j++)
                    {
                        pixels[i + j * textureWidth] = color;
                    }
                }
                else
                {
                    // Vertical gradient - same color for entire row
                    for (var j = 0; j < textureWidth; j++)
                    {
                        pixels[j + i * textureWidth] = color;
                    }
                }
            }
    
            texture.SetPixels(pixels);
            texture.Apply();
    
            // Draw the gradient texture
            GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill, true);
    
            // Clean up
            UnityEngine.Object.DestroyImmediate(texture);
        }
    }
}