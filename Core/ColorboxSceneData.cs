using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VolumeBox.Colorbox.Core
{
    public class ColorboxSceneData: MonoBehaviour
    {
        public Scene Scene => gameObject.scene;

        [SerializeField] private List<ColoredGameObjectData> _gameObjectsData = new();

        public ColoredGameObjectData GetOrAddGameObjectData(GameObject obj)
        {
            var data = _gameObjectsData.FirstOrDefault(x => x.Reference == obj);

            if (data == null)
            {
                data = new();
                data.Reference = obj;
                data.SceneData = this;
                _gameObjectsData.Add(data);
            }

            return data;
        }

        public void RemoveGameObjectData()
        {
            
        }

        public void ValidateGameObjects()
        {
            var excessObjects = _gameObjectsData
                .Where(x => Scene.GetRootGameObjects().All(y => y.GetInstanceID() != x.Reference.GetInstanceID())).ToList();
            excessObjects.ForEach(x => _gameObjectsData.Remove(x));
        }

        private void OnValidate()
        {
            EditorApplication.RepaintHierarchyWindow();
        }
    }
}