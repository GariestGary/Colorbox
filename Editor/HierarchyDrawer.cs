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
                    // Use your custom window here
                    AppearanceEditorWindow.ShowWindow(clickedObject, Event.current.mousePosition + new Vector2(15, 150));
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
            
            DrawGradientBackground(rect, data.BackgroundGradient);
            var style = new GUIStyle();
            style.font = data.Font;
            style.fontSize = data.FontSize;
            style.alignment = data.TextAlignment;
            EditorGUI.LabelField(rect, data.Reference.name, style);
        }
        
        private static void DrawGradientBackground(Rect rect, Gradient gradient, bool horizontal = true)
        {
            if (gradient == null) return;

            int textureWidth = horizontal ? 32 : 1;
            int textureHeight = horizontal ? 1 : 32;
    
            // Create texture with optimal size for the gradient direction
            Texture2D texture = new Texture2D(textureWidth, textureHeight);
            texture.wrapMode = TextureWrapMode.Clamp;
    
            // Fill texture with gradient colors
            Color[] pixels = new Color[textureWidth * textureHeight];
    
            for (int i = 0; i < (horizontal ? textureWidth : textureHeight); i++)
            {
                float t = (float)i / (horizontal ? textureWidth - 1 : textureHeight - 1);
                Color color = gradient.Evaluate(t);
        
                if (horizontal)
                {
                    // Horizontal gradient - same color for entire column
                    for (int j = 0; j < textureHeight; j++)
                    {
                        pixels[i + j * textureWidth] = color;
                    }
                }
                else
                {
                    // Vertical gradient - same color for entire row
                    for (int j = 0; j < textureWidth; j++)
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