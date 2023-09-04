using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

namespace UI.UiFactorySystem.Scripts
{
    public class UiFactory : MonoSingleton<UiFactory>
    {
        [SerializeField] private UiBaseSo m_baseSo;

        private readonly List<UiController> _sceneUis = new();

        private void Awake()
        {
            CacheScene();
        }

        private void CacheScene()
        {
            var uiControllers = GetComponentsInChildren<UiController>();
            foreach (var ui in uiControllers)
            {
                _sceneUis.Add(ui);
                ui.DisableUi();
            }
        }

        public static T Get<T>() where T : UiController
        {
            var ui = GetSceneUi<T>();
            if (ui == null)
            {
                ui = Instance.InstantiateUi<T>();
            }

            ui.gameObject.SetActive(true);
            return ui as T;
        }

        public static bool TryGetFromScene<T>(out T ui) where T : UiController
        {
            ui = null;
            var uiController = GetSceneUi<T>();
            if (uiController == null) return false;
            ui = uiController as T;
            return true;
        }

        public static void Hide<T>() where T : UiController
        {
            var ui = GetSceneUi<T>();
            if (ui != null)
            {
                ui.Hide();
            }
        }


        private UiController InstantiateUi<T>() where T : UiController
        {
            var prefab = m_baseSo.GetPrefab<T>();
            var instance = Instantiate(prefab, transform);
            _sceneUis.Add(instance);
            return instance;
        }

        private static UiController GetSceneUi<T>() where T : UiController
        {
            var ui = Instance._sceneUis.FirstOrDefault(c => c.GetType() == typeof(T));
            return ui;
        }


#if UNITY_EDITOR
        public UiBaseSo BaseSo => m_baseSo;

        [Button]
        private void FindOrCreateUiBaseSo()
        {
            m_baseSo = UiBaseSo.GetOrCreateBaseSo();
        }

#endif
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}