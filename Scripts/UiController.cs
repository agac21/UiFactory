using System.Linq;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI.UiFactorySystem.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UiController : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Sequence _seq;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected Sequence Show()
        {
            gameObject.SetActive(true);
            DisableCanvasGroup();

            InitSequence();
            ShowInner(_seq);
            _seq.AppendCallback(EnableCanvasGroup);
            return _seq;
        }

        public Sequence Hide()
        {
            InitSequence();
            HideInner(_seq);
            _seq.AppendCallback(DisableUi);
            return _seq;
        }


        private void InitSequence()
        {
            _seq?.Kill();
            _seq = DOTween.Sequence();
        }


        protected virtual void ShowInner(Sequence seq)
        {
            _canvasGroup.alpha = 0;
            seq.Append(_canvasGroup.DOFade(1, .5f));
        }

        protected virtual void HideInner(Sequence seq)
        {
            _canvasGroup.alpha = 1;
            seq.Append(_canvasGroup.DOFade(0, .5f));
        }

        private void EnableCanvasGroup()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void DisableCanvasGroup()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void DisableUi()
        {
            DisableCanvasGroup();
            gameObject.SetActive(false);
        }


#if UNITY_EDITOR
        [Button, HideIf(nameof(IsBaseSoContains))]
        private void AddToBaseSo(string path)
        {
            var isPrefab = PrefabUtility.IsAnyPrefabInstanceRoot(gameObject);
            var prefab = isPrefab
                ? PrefabUtility.GetCorrespondingObjectFromOriginalSource(this)
                : PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, path, InteractionMode.UserAction).GetComponent<UiController>();

            UiFactory.Instance.BaseSo.UIControllers.Add(prefab);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private bool IsBaseSoContains()
        {
            return UiFactory.Instance.BaseSo.UIControllers.Any(controller => controller.GetType() == GetType());
        }
#endif
    }
}