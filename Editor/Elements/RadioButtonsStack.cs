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

        public event Action SelectionChanged;

        private int _selectedIndex = -1;

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

                if (i == _selectedIndex)
                {
                    //HOW I CAN MAKE BUTTON LOOK SELECTED????
                }

                button.clicked += OnButtonClicked;

                if (textures != null)
                {
                    button.iconImage = textures[i];
                }
                
                root.Add(button);
            }
            
            Add(root);
        }

        private void OnButtonClicked()
        {
            //HOW I CAN GET CLICKED BUTTON INDEX????
        }
    }
}