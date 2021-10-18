using System.Collections;
using UnityEngine;

namespace MainGame
{
    public class MainPanelHandler : MonoBehaviour
    {
        [SerializeField] private GameObject playPanel;
        [SerializeField] private GameObject creditsPanel;
        public float fadeTime = 0.35f;

        private CanvasGroup _playPanelCG;
        private CanvasGroup _creditsPanelCG;

        // Start is called before the first frame update
        void Start()
        {
            _playPanelCG = playPanel.GetComponent<CanvasGroup>();
            _creditsPanelCG = creditsPanel.GetComponent<CanvasGroup>();
        }

        public void OnCreditsButton_Click()
        {
            AudioManager.SharedInstance.Play("UIButton_Sound");
            StartCoroutine(SwitchPanel(_playPanelCG, _creditsPanelCG));
        }

        public void OnBackButton_Click()
        {
            AudioManager.SharedInstance.Play("UIButton_Sound");
            StartCoroutine(SwitchPanel(_creditsPanelCG, _playPanelCG));
        }

        IEnumerator SwitchPanel(CanvasGroup fromPanel, CanvasGroup toPanel)
        {
            yield return StartCoroutine(FadeOut(fromPanel, fadeTime));

            StartCoroutine(FadeIn(toPanel, fadeTime));
        }

        IEnumerator FadeOut(CanvasGroup canvasGrp, float fadeTime)
        {
            float elapsedTime = 0;
            canvasGrp.blocksRaycasts = false;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                canvasGrp.alpha = 1.0f - Mathf.Clamp01(elapsedTime / fadeTime);
                yield return null;
            }
        }

        IEnumerator FadeIn(CanvasGroup canvasGrp, float fadeTime)
        {
            float elapsedTime = 0;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                canvasGrp.alpha = Mathf.Clamp01(elapsedTime / fadeTime);
                yield return null;
            }
            canvasGrp.blocksRaycasts = true;
        }
    }
}