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
            
            EditorGUI.DrawRect(rect, data.BackgroundColor);
            var style = new GUIStyle();
            style.font = data.Font;
            style.fontSize = data.FontSize;
            style.alignment = data.TextAlignment;
            EditorGUI.LabelField(rect, data.Reference.name, style);
        }
    }
}