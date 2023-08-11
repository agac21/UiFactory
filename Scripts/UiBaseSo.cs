using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI.UiFactorySystem.Scripts
{
    public class UiBaseSo : ScriptableObject
    {
        [AssetsOnly, SerializeField] private List<UiController> m_uiControllers;

#if UNITY_EDITOR
        public List<UiController> UIControllers => m_uiControllers;


        [Button]
        public void FillData()
        {
            var prefabGuids = AssetDatabase.FindAssets("t:Prefab");
            foreach (var guid in prefabGuids)
            {
                var prefabPath = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                var uiController = prefab.GetComponent<UiController>();
                if (uiController != null)
                {
                    m_uiControllers.Add(uiController);
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public static UiBaseSo GetOrCreateBaseSo()
        {
            UiBaseSo uiBaseSo;
            var guids = AssetDatabase.FindAssets("t:UiBaseSo");
            if (guids.Length == 0)
            {
                uiBaseSo = CreateInstance<UiBaseSo>();
                uiBaseSo.name = "UiBaseSo";
                AssetDatabase.CreateAsset(uiBaseSo, "Assets/UiBaseSo.asset");
                AssetDatabase.SaveAssets();
            }
            else
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                uiBaseSo = AssetDatabase.LoadAssetAtPath<UiBaseSo>(path);
            }

            return uiBaseSo;
        }

#endif

        public UiController GetPrefab<T>() where T : UiController
        {
            return m_uiControllers.FirstOrDefault(c => c.GetType() == typeof(T));
        }
    }
}