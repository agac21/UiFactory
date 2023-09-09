using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UiFactorySystem.Scripts
{
    public class UiHub : MonoBehaviour
    {
        [SerializeField] private UiBaseSo m_baseSo;
        private readonly List<UiController> _sceneUis = new();

#if UNITY_EDITOR
        public UiBaseSo BaseSo => m_baseSo;

        [Button, ShowIf("@BaseSo == null")]
        private void CreateUiBaseSo()
        {
            m_baseSo = UiBaseSo.CreateBaseSo();
        }

#endif

        public void Initialize()
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

        public T Get<T>(Transform parent = null) where T : UiController
        {
            var ui = GetSceneUi<T>();
            if (ui == null)
            {
                ui = InstantiateUi<T>(parent);
            }

            ui.gameObject.SetActive(true);
            return ui as T;
        }

        public void Hide<T>() where T : UiController
        {
            var ui = GetSceneUi<T>();
            if (ui != null)
            {
                ui.Hide();
            }
        }


        private UiController GetSceneUi<T>() where T : UiController
        {
            var ui = _sceneUis.FirstOrDefault(c => c.GetType() == typeof(T));
            return ui;
        }

        private UiController InstantiateUi<T>(Transform parent) where T : UiController
        {
            var prefab = m_baseSo.GetPrefab<T>();
            if (parent == null) parent = transform;
            var instance = Instantiate(prefab, parent);
            _sceneUis.Add(instance);
            return instance;
        }

        public bool TryGetFromScene<T>(out T ui) where T : UiController
        {
            ui = null;
            var uiController = GetSceneUi<T>();
            if (uiController == null) return false;
            ui = uiController as T;
            return true;
        }
    }
}