using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class GeneralUI : MonoBehaviour
    {
        public static GeneralUI Instance;

        [Header("UI")]
        [SerializeField] private Transform openingPanel;
        [SerializeField] private Transform levelSelectPanel;
        [Header("Sounds")]
        [SerializeField] private Image SoundButtonImage;
        [SerializeField] private Sprite soundOnImage;
        [SerializeField] private Sprite soundOffImage;

        private CanvasGroup _openingPanelCG;
        private CanvasGroup _levelSelectPanelCG;
        private bool _soundOn;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _openingPanelCG = openingPanel.GetComponent<CanvasGroup>();
            _levelSelectPanelCG = levelSelectPanel.GetComponent<CanvasGroup>();

            Invoke(nameof(Theme), 0.65f);
        }

        void Theme()
        {
            AudioManager.SharedInstance.PlayTheme("Main_Theme");
        }

        public void OnPlayButton_Click()
        {
            AudioManager.SharedInstance.Play("UIButton_Sound");
            LeanTween.alphaCanvas(_openingPanelCG, 0, 0.25f).setOnComplete(DisplayLevels);
        }

        void DisplayLevels()
        {
            _openingPanelCG.blocksRaycasts = false;
            LeanTween.alphaCanvas(_levelSelectPanelCG, 1, 0.25f).setOnComplete(() => _levelSelectPanelCG.blocksRaycasts = true);
        }

        public void OnReturnButton_Click()
        {
            AudioManager.SharedInstance.Play("UIButton_Sound");
            LeanTween.alphaCanvas(_levelSelectPanelCG, 0, 0.25f).setOnComplete(DisplayMainPanel);
        }

        void DisplayMainPanel()
        {
            _levelSelectPanelCG.blocksRaycasts = false;
            LeanTween.alphaCanvas(_openingPanelCG, 1, 0.25f).setOnComplete(() => _openingPanelCG.blocksRaycasts = true);
        }

        public void ToggleSound()
        {
            _soundOn = !_soundOn;
            if (_soundOn)
                SoundButtonImage.sprite = soundOnImage;
            else SoundButtonImage.sprite = soundOffImage;

            GameManager.Instance.SetSound(_soundOn);
        }

        public void PlayLevel(string levelName)
        {
            SceneController.Instance.FadeAndLoadScene(ConstantsList.Scenes[levelName]);
        }
    }
}