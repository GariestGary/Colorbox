using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VolumeBox.Colorbox.Core;
using VolumeBox.Colorbox.Editor.Elements;
using Object = UnityEngine.Object;

namespace VolumeBox.Colorbox.Editor.AppearanceEditor
{
    public class AppearanceEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        private TemplateContainer _tree;
        private ColoredGameObjectData _data;
        private GradientField _backgroundGradient;
        private ObjectField _backgroundTexture;
        private ColorField _backgroundColor;
        private static AppearanceEditorWindow _currentWindow;

        public static void ShowWindow(GameObject target, Vector2 position)
        {
            _currentWindow?.Close();
            var data = HierarchyDrawer.CurrentSceneData.GetOrAddGameObjectData(target);
            _currentWindow = CreateInstance<AppearanceEditorWindow>();
            _currentWindow.SetData(data);
            _currentWindow.position = new Rect(position.x, position.y, 260, 280);
            _currentWindow.ShowPopup();
        }

        private void SetData(ColoredGameObjectData data)
        {
            _data = data;
        }

        private void CreateGUI()
        {
            _tree = _treeAsset.Instantiate();
            _tree.style.height = new StyleLength(Length.Percent(100));
            var root = rootVisualElement;
            root.style.backgroundColor = Color.clear;
            
            //Customization switching
            var enableCustomization = _tree.Q<Toggle>("enable-customization");
            enableCustomization.value = _data.EnabledCustomization;
            enableCustomization.RegisterValueChangedCallback(OnCustomizationSwitched);
            
            //Close button
            var closeButton = _tree.Q<Button>("close-button");
            closeButton.clicked += Close;
            
            //Font
            var font = _tree.Q<ObjectField>("font");
            font.value = _data.Font;
            font.RegisterValueChangedCallback(OnFontChanged);
            
            //Text alignment
            var textAlignment = _tree.Q<RadioButtonsStack>("text-alignment");
            
            var textures = new[]
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

            textAlignment.SelectedIndex = (int)_data.TextAlignment;
            textAlignment.SetButtons(textures);
            textAlignment.SelectionChanged += OnTextAlignmentChanged;
            
            //Font size
            var fontSize = _tree.Q<IntegerField>("font-size");
            fontSize.value = _data.FontSize;
            fontSize.RegisterValueChangedCallback(OnFontSizeChanged);
            
            //Background type
            var backgroundType = _tree.Q<EnumField>("background-type");
            backgroundType.value = _data.FillType;
            backgroundType.RegisterValueChangedCallback(OnBackgroundTypeChanged);
            
            //Background texture
            _backgroundTexture = _tree.Q<ObjectField>("background-texture");
            _backgroundTexture.value = _data.BackgroundTexture;
            _backgroundTexture.RegisterValueChangedCallback(OnBackgroundTextureChanged);
            
            //Background color
            _backgroundColor = _tree.Q<ColorField>("background-color");
            _backgroundColor.value = _data.BackgroundColor;
            _backgroundColor.RegisterValueChangedCallback(OnBackgroundColorChanged);
            
            //Background gradient
            _backgroundGradient = _tree.Q<GradientField>("background-gradient");
            _backgroundGradient.value = _data.BackgroundGradient;
            _backgroundGradient.RegisterValueChangedCallback(OnBackgroundGradientChanged);
            
            ValidateBackgroundType();
            
            root.Add(_tree);
        }

        private void ValidateBackgroundType()
        {
            _backgroundGradient.style.display = _data.FillType == BackgroundFillType.Gradient ? DisplayStyle.Flex : DisplayStyle.None;
            _backgroundColor.style.display = _data.FillType == BackgroundFillType.Color ? DisplayStyle.Flex : DisplayStyle.None;
            _backgroundTexture.style.display = _data.FillType == BackgroundFillType.Texture ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnBackgroundGradientChanged(ChangeEvent<Gradient> evt)
        {
            _data.BackgroundGradient = evt.newValue;
            EditorApplication.RepaintHierarchyWindow();
        }

        private void OnBackgroundColorChanged(ChangeEvent<Color> evt)
        {
            _data.BackgroundColor = evt.newValue;
            EditorApplication.RepaintHierarchyWindow();
        }

        private void OnBackgroundTextureChanged(ChangeEvent<Object> evt)
        {
            _data.BackgroundTexture = evt.newValue as Texture2D;
            EditorApplication.RepaintHierarchyWindow();
        }

        private void OnBackgroundTypeChanged(ChangeEvent<Enum> evt)
        {
            _data.FillType = (BackgroundFillType)evt.newValue;
            ValidateBackgroundType();
            EditorApplication.RepaintHierarchyWindow();
        }

        private void OnFontSizeChanged(ChangeEvent<int> evt)
        {
            _data.FontSize = evt.newValue;
            EditorApplication.RepaintHierarchyWindow();
        }

        private void OnFontChanged(ChangeEvent<Object> evt)
        {
            _data.Font = evt.newValue as Font;
            EditorApplication.RepaintHierarchyWindow();
        }

        private void OnCustomizationSwitched(ChangeEvent<bool> evt)
        {
            _data.EnabledCustomization = evt.newValue;
            EditorApplication.RepaintHierarchyWindow();
        }

        private void OnTextAlignmentChanged(int obj)
        {
            _data.TextAlignment = (TextAnchor)obj;
            EditorApplication.RepaintHierarchyWindow();
        }

        private Texture2D LoadTex(string path)
        {
            return Resources.Load<Texture2D>(path);
        }
        
        private void MarkDataDirty()
        {
            // If your data is stored in another asset, mark that asset dirty
            // For example, if HierarchyDrawer.CurrentSceneData holds the reference:
            if (HierarchyDrawer.CurrentSceneData != null)
            {
                EditorUtility.SetDirty(HierarchyDrawer.CurrentSceneData);
            }
        }
    }
}