using System;
using BootLoader.Base;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BootLoader.Managers
{
    public class LoadingScreen : SingletonDontDestroy<LoadingScreen>
    {
        [SerializeField] private TransitionType transitionType;
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private RectTransform loadingPanel;
        [SerializeField] private Image progressBar;
        [SerializeField] private Button continueButton;
        [SerializeField] private float inDuration, outDuration;
        [HideInInspector] public float target;
        private bool isLoading;

        #region Unity Methods
        private void Awake()
        {
            DOTween.Init();
            DontDestroyOnLoad(gameObject);
            canvasScaler ??= GetComponent<CanvasScaler>();
        }
        private void OnEnable()
        {
            continueButton.onClick.AddListener(SendOut);
        }
        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(SendOut);
        }
        private void Update()
        {
            if (!isLoading) return;
            progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, Time.deltaTime * 5);
        }
        #endregion

        #region API
        public void ResetProgress()
        {
            progressBar.fillAmount = 0f;
            isLoading = true;
        }
        public void FinishProgress()
        {
            isLoading = false;
            progressBar.fillAmount = 1f;
        }
        public void BringIn()
        {
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.anchoredPosition = transitionType switch
            {
                TransitionType.TopToTop => new Vector2(0f, canvasScaler.referenceResolution.y),
                TransitionType.TopToBottom => new Vector2(0f, canvasScaler.referenceResolution.y),
                TransitionType.LeftToRight => new Vector2(-canvasScaler.referenceResolution.x, 0f),
                _ => throw new ArgumentOutOfRangeException()
            };
            loadingPanel.DOAnchorPos(transitionType switch
            {
                TransitionType.TopToTop => Vector2.zero,
                TransitionType.TopToBottom => Vector2.zero,
                TransitionType.LeftToRight => Vector2.zero,
                _ => throw new ArgumentOutOfRangeException()
            }, inDuration);
        }

        public void SendOut()
        {
            loadingPanel.DOAnchorPos(transitionType switch
            {
                TransitionType.TopToBottom => new Vector2(0f, -canvasScaler.referenceResolution.y),
                TransitionType.LeftToRight => new Vector2(canvasScaler.referenceResolution.x, 0f),
                TransitionType.TopToTop => new Vector2(0f, canvasScaler.referenceResolution.y),
                _ => throw new ArgumentOutOfRangeException()
            }, outDuration).OnComplete(() =>
            {
                loadingPanel.gameObject.SetActive(false);
            });
        }
        #endregion
        private enum TransitionType
        {
            TopToBottom,
            TopToTop,
            LeftToRight
        }
    }
}
