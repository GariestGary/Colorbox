using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VolumeBox.Colorbox.Core;

namespace VolumeBox.Colorbox.Editor.AppearanceEditor
{
    public class AppearanceEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        private TemplateContainer _tree;
        private ColoredGameObjectData _data;
        private GradientField _backgroundGradient;
        private ObjectField _backgroundTexture;
        private ColorField _backgroundTextureTint;
        private ColorField _backgroundColor;
        private static AppearanceEditorWindow _currentWindow;

        public static void ShowWindow(GameObject target, Vector2 position)
        {
            _currentWindow?.Close();
            var data = HierarchyDrawer.CurrentSceneData.GetOrAddGameObjectData(target);
            _currentWindow = CreateInstance<AppearanceEditorWindow>();
            _currentWindow.titleContent = new GUIContent($"\"{target.name}\" Appearance Settings");
            _currentWindow.SetData(data);
            _currentWindow.position = new Rect(position.x, position.y, 280, 440);
            _currentWindow.ShowPopup();
        }

        private void SetData(ColoredGameObjectData data)
        {
            _data = data;
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUI.DrawRect(position, Color.red);
            
            DrawContent();

            if (EditorGUI.EndChangeCheck())
            {
                EditorApplication.RepaintHierarchyWindow();
            }
        }

        private void DrawContent()
        {
            DrawHeader();
            EditorGUILayout.Space();

            _data.EnabledCustomization = EditorGUILayout.Toggle("Enable Styling", _data.EnabledCustomization);
            EditorGUILayout.Space();

            _data.Font = (Font)EditorGUILayout.ObjectField("Font", _data.Font, typeof(Font), false);
            EditorGUILayout.Space();

            DrawAlignmentButtons();
            EditorGUILayout.Space();

            DrawFontStyleButtons();
            EditorGUILayout.Space();

            _data.FontSize = EditorGUILayout.IntField("Font Size", _data.FontSize);
            EditorGUILayout.Space();

            _data.FontColor = EditorGUILayout.ColorField("Font Color", _data.FontColor);
            EditorGUILayout.Space();

            _data.FillType = (BackgroundFillType)EditorGUILayout.EnumPopup("Background Type", _data.FillType);
            EditorGUILayout.Space();

            DrawBackgroundFields();
        }
        
        private void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal(); // Start the horizontal row for header
            {
                // Header label on left
                EditorGUILayout.LabelField("Text Style Settings", EditorStyles.boldLabel);
        
                // Flexible space pushes the close button to the right
                GUILayout.FlexibleSpace();
        
                var prevColor = GUI.color;
                GUI.color = Color.brown;
                // Close button with fixed width and right-side constraint
                if (GUILayout.Button("X", GUILayout.Width(15),  GUILayout.Height(15)))
                {
                    this.Close();
                }
                GUI.color = prevColor;
            }
            EditorGUILayout.EndHorizontal(); // End the horizontal row
    
            // Optional: Add a separator line
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Height(5));
        }

        private static Color? DrawPastelColorPalette()
        {
            Color? selectedColor = null;
            
            // Define nice pastel colors
            var pastelColors = new Color[]
            {
                new(0.988f, 0.729f, 0.729f), // Soft Pink
                new(0.729f, 0.988f, 0.792f), // Mint Green
                new(0.729f, 0.847f, 0.988f), // Light Blue
                new(0.988f, 0.917f, 0.729f), // Pale Yellow
                new(0.847f, 0.729f, 0.988f), // Lavender
                new(0.729f, 0.988f, 0.965f)  // Aqua
            };
            
            var width = (_currentWindow.position.width - 21) / (pastelColors.Length);

            EditorGUILayout.BeginHorizontal();
            {
                for (var i = 0; i < pastelColors.Length; i++)
                {
                    // Save the current GUI color
                    var originalColor = GUI.color;
                    
                    // Set the button color
                    GUI.color = pastelColors[i];
                    
                    // Create the colored button
                    if (GUILayout.Button("", GUILayout.Width(width), GUILayout.Height(20)))
                    {
                        selectedColor = pastelColors[i];
                    }
                    
                    // Restore the original GUI color
                    GUI.color = originalColor;

                    // Add small space between buttons except for the last one
                    if (i < pastelColors.Length - 1)
                    {
                        //GUILayout.Space(1);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            return selectedColor;
        }

        private void DrawAlignmentButtons()
        {
            EditorGUILayout.LabelField("Text Alignment", EditorStyles.label);
            
            // Top row
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Toggle(_data.TextAlignment == TextAnchor.UpperLeft, "TL", "Button"))
                    _data.TextAlignment = TextAnchor.UpperLeft;
                if (GUILayout.Toggle(_data.TextAlignment == TextAnchor.UpperCenter, "TC", "Button"))
                    _data.TextAlignment = TextAnchor.UpperCenter;
                if (GUILayout.Toggle(_data.TextAlignment == TextAnchor.UpperRight, "TR", "Button"))
                    _data.TextAlignment = TextAnchor.UpperRight;
            }
            EditorGUILayout.EndHorizontal();

            // Middle row
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Toggle(_data.TextAlignment == TextAnchor.MiddleLeft, "ML", "Button"))
                    _data.TextAlignment = TextAnchor.MiddleLeft;
                if (GUILayout.Toggle(_data.TextAlignment == TextAnchor.MiddleCenter, "MC", "Button"))
                    _data.TextAlignment = TextAnchor.MiddleCenter;
                if (GUILayout.Toggle(_data.TextAlignment == TextAnchor.MiddleRight, "MR", "Button"))
                    _data.TextAlignment = TextAnchor.MiddleRight;
            }
            EditorGUILayout.EndHorizontal();

            // Bottom row
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Toggle(_data.TextAlignment == TextAnchor.LowerLeft, "BL", "Button"))
                    _data.TextAlignment = TextAnchor.LowerLeft;
                if (GUILayout.Toggle(_data.TextAlignment == TextAnchor.LowerCenter, "BC", "Button"))
                    _data.TextAlignment = TextAnchor.LowerCenter;
                if (GUILayout.Toggle(_data.TextAlignment == TextAnchor.LowerRight, "BR", "Button"))
                    _data.TextAlignment = TextAnchor.LowerRight;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFontStyleButtons()
        {
            EditorGUILayout.LabelField("Font Style", EditorStyles.label);
            
            EditorGUILayout.BeginHorizontal();
            {
                // Bold toggle
                var isBold = (_data.FontStyle & FontStyle.Bold) == FontStyle.Bold;
                var newBold = GUILayout.Toggle(isBold, "B", "Button");
                
                // Italic toggle
                var isItalic = (_data.FontStyle & FontStyle.Italic) == FontStyle.Italic;
                var newItalic = GUILayout.Toggle(isItalic, "I", "Button");
                
                // Update font style
                _data.FontStyle = FontStyle.Normal;
                if (newBold) _data.FontStyle |= FontStyle.Bold;
                if (newItalic) _data.FontStyle |= FontStyle.Italic;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawBackgroundFields()
        {
            switch (_data.FillType)
            {
                case BackgroundFillType.Color:
                    var color = DrawPastelColorPalette();

                    if (color != null)
                    {
                        _data.BackgroundColor = color.Value;
                    }
                    
                    EditorGUILayout.Space();
                    _data.BackgroundColor = EditorGUILayout.ColorField("Background Color", _data.BackgroundColor);
                    break;
                    
                case BackgroundFillType.Gradient:
                    // Custom property field for gradient since there's no built-in one
                    _data.BackgroundGradient = EditorGUILayout.GradientField("Gradient", _data.BackgroundGradient);
                    break;
                    
                case BackgroundFillType.Texture:
                    _data.BackgroundTexture = (Texture2D)EditorGUILayout.ObjectField("Background Texture", _data.BackgroundTexture, typeof(Texture2D), false);
                    EditorGUILayout.Space();
                    _data.BackgroundTextureTint = EditorGUILayout.ColorField("Texture Tint", _data.BackgroundTextureTint);
                    break;
            }
        }
    }
}