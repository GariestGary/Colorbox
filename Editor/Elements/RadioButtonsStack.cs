using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace VolumeBox.Colorbox.Editor.Elements
{
    [UxmlElement]
    public partial class RadioButtonsStack: VisualElement
    {
        private List<Button> _buttons = new();

        public event Action<int> SelectionChanged;

        private int _selectedIndex = -1;
        
        public int SelectedIndex 
        { 
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    UpdateButtonAppearance();
                    SelectionChanged?.Invoke(_selectedIndex);
                }
            }
        }
        
        private void UpdateButtonAppearance()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                var button = _buttons[i];
                
                if (i == _selectedIndex)
                {
                    // Selected button styling
                    button.AddToClassList("radio-button--selected");
                }
                else
                {
                    // Normal button styling
                    button.RemoveFromClassList("radio-button--selected");
                }
            }
        }

        public RadioButtonsStack()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("RadioButtonsStack"));
        }

        public void SetButtons(int count)
        {
            SetButtons(count, null, null);
        }

        public void SetButtons(params string[] names)
        {
            SetButtons(names.Length, names, null);
        }

        public void SetButtons(params Texture2D[] textures)
        {
            SetButtons(textures.Length, null, textures);
        }

        private void SetButtons(int count, string[] names, Texture2D[] textures)
        {
            var root = new VisualElement();
            var flex = root.style.flexDirection;
            flex.value = FlexDirection.Row;
            root.style.flexDirection = flex;
            root.style.flexGrow = 1f;
            root.style.flexShrink = 1f;
            
            for (int i = 0; i < count; i++)
            {
                var button = new Button();
                button.AddToClassList("radio-button");

                if (i == 0)
                {
                    button.AddToClassList("first-child");
                }

                if (i == count - 1)
                {
                    button.AddToClassList("last-child");
                }

                button.userData = i;
                button.clicked += () => OnButtonClicked(button);

                if (textures != null)
                {
                    button.iconImage = textures[i];
                }
                
                root.Add(button);
                _buttons.Add(button);
            }
            
            Add(root);
            UpdateButtonAppearance();
        }

        private void OnButtonClicked(Button clickedButton)
        {
            int clickedIndex = (int)clickedButton.userData;
            SelectedIndex = clickedIndex;
        }
    }
}