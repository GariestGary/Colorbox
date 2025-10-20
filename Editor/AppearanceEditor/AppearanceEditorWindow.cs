using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VolumeBox.Colorbox.Editor.Elements;
using PopupWindow = UnityEditor.PopupWindow;

namespace VolumeBox.Colorbox.Editor.AppearanceEditor
{
    public class AppearanceEditorWindow: EditorWindow
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        private TemplateContainer _tree;
        
        public static void ShowWindow(GameObject target, Vector2 position)
        {
            var window = CreateInstance<AppearanceEditorWindow>();
            window.position = new Rect(position.x, position.y, 260, 180);
            window.ShowPopup();
        }

        private void CreateGUI()
        {
            _tree = _treeAsset.Instantiate();
            _tree.style.height = new StyleLength(Length.Percent(100));
            var root = rootVisualElement;
            var stack = _tree.Q<RadioButtonsStack>("buttons-stack");
            var textures = new Texture2D[]
            {
                LoadTex("Textures/upper_left"),
                LoadTex("Textures/upper_center"),
                LoadTex("Textures/upper_right"),
                LoadTex("Textures/middle_left"),
                LoadTex("Textures/middle_center"),
                LoadTex("Textures/middle_right"),
                LoadTex("Textures/lower_left"),
                LoadTex("Textures/lower_center"),
                LoadTex("Textures/lower_right"),
            };
            stack.SetButtons(textures);
            // var borderWidth = 3;
            // root.style.borderBottomWidth = borderWidth;
            // root.style.borderLeftWidth = borderWidth;
            // root.style.borderRightWidth = borderWidth;
            // root.style.borderTopWidth = borderWidth;
            // var borderColor = Color.gray2;
            // root.style.borderBottomColor = borderColor;
            // root.style.borderLeftColor = borderColor;
            // root.style.borderRightColor = borderColor;
            // root.style.borderTopColor = borderColor;
            // root.style.backgroundColor = Color.gray;
            root.Add(_tree);
        }

        private Texture2D LoadTex(string path)
        {
            return Resources.Load<Texture2D>(path);
        }

        private void OnGUI()
        {
            
        }

        private void OnLostFocus()
        {
            Close();
        }
    }
}