using System.Linq;
using UnityEditor;
using UnityEngine;
using VolumeBox.Colorbox.Core;

namespace VolumeBox.Colorbox.Editor
{
    [InitializeOnLoad]
    public static class HierarchyDrawer
    {
        private const string DataObjectName = "[Colorbox Scene Data]";

        private static ColorboxSceneData _currentSceneData;
        
        static HierarchyDrawer()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnDrawHierarchyItem;
        }

        private static void OnDrawHierarchyItem(int instanceID, Rect selectionRect)
        {
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
            DrawBackground(selectionRect, data.BackgroundColor);
        }
        
        private static void DrawBackground(Rect rect, Color color)
        {
            EditorGUI.DrawRect(rect, color);
        }
    }
}