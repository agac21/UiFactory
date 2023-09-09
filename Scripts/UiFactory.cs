using UnityEngine;
using Utilities;

namespace UI.UiFactorySystem.Scripts
{
    [RequireComponent(typeof(UiHub))]
    public class UiFactory : MonoSingleton<UiFactory>
    {
        [SerializeField] private UiHub m_uiHub;
        public UiBaseSo BaseSo => m_uiHub.BaseSo;

        public void Initialize()
        {
            m_uiHub.Initialize();
        }

        public static T Get<T>() where T : UiController
        {
            return Instance.m_uiHub.Get<T>();
        }

        public static void Hide<T>() where T : UiController
        {
            Instance.m_uiHub.Hide<T>();
        }
    }
}