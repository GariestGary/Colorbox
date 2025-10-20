using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VolumeBox.Colorbox.Core;
using VolumeBox.Colorbox.Editor;

public class AppearanceEditorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private static ColoredGameObjectWrapper _wrapper;
    private static Editor _cachedEditor;
    
    public static void Open(GameObject obj)
    {
        AppearanceEditorWindow wnd = GetWindow<AppearanceEditorWindow>();
        wnd.titleContent = new GUIContent("AppearanceEditorWindow");
        SetColoredObjectData(obj);
    }

    private static void SetColoredObjectData(GameObject obj)
    {
        _wrapper = CreateInstance<ColoredGameObjectWrapper>();
        var data = HierarchyDrawer.CurrentSceneData.GetOrAddGameObjectData(obj);
        _wrapper.Data = data;
        Selection.selectionChanged = SelectionChanged;
        ValidateEditor();
        _cachedEditor.Repaint();
    }

    private static void SelectionChanged()
    {
        var obj = Selection.activeObject;

        if (obj == null)
        {
            return;
        }
        
        if (obj is GameObject gObj && gObj.scene.IsValid())
        {
            SetColoredObjectData(gObj);
        }
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        var root = rootVisualElement;
        var tree = m_VisualTreeAsset.Instantiate();
        var rootContainer = tree.Q<IMGUIContainer>("root");
        rootContainer.onGUIHandler = DrawInspector;
        root.Add(tree);
    }

    private static void ValidateEditor()
    {
        Editor.CreateCachedEditor(_wrapper, null, ref _cachedEditor);
        
    }

    private void DrawInspector()
    {
        ValidateEditor();

        if (_cachedEditor == null)
        {
            return;
        }
        
        _cachedEditor.DrawDefaultInspector();
    }
}
