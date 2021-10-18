using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace MainGame
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public bool GamePaused = false;
        [SerializeField] private Volume blurPostProcessVolume;
        [Header("UI")]
        [SerializeField] private Transform menuPanel;
        [SerializeField] private Transform pauseButton;
        [SerializeField] private TextMeshProUGUI levelTitleText;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private GameObject nextLevelButton;
        [SerializeField] private GameObject pausedOptionsPanel;
        [SerializeField] private GameObject finishedOptionsPanel;

        [Header("Sounds")]
        [SerializeField] private Image SoundButtonImage;
        [SerializeField] private Sprite soundOnImage;
        [SerializeField] private Sprite soundOffImage;

        private bool _soundOn;
        private CanvasGroup _menuPanelCG;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _menuPanelCG = menuPanel.GetComponent<CanvasGroup>();
        }

        void OpenPauseMenu(bool finishMenu = false)
        {
            GamePaused = true;
            pauseButton.gameObject.SetActive(false);
            _menuPanelCG.alpha = 1;
            _menuPanelCG.blocksRaycasts = true;
            blurPostProcessVolume.weight = 1;

            if (finishMenu)
            {
                LevelManager.SharedInstance.SaveProgress();
                if (LevelManager.SharedInstance.levelIndex == 24)
                {
                    titleText.text = "YOU BEAT THE GAME!";
                    pausedOptionsPanel.SetActive(true);
                    nextLevelButton.SetActive(false);
                }
                else
                {
                    finishedOptionsPanel.SetActive(true);
                }
                pausedOptionsPanel.SetActive(false);
            }
            else
            {
                // TODO
                // coreData.selectedLevelIndex = LevelManager.SharedInstance.coreData.selectedLevelIndex;
                pausedOptionsPanel.SetActive(true);
                finishedOptionsPanel.SetActive(false);
            }
        }

        public void OpenFinishMenu()
        {
            OpenPauseMenu(true);
        }

        public void OnPauseButton_Click()
        {
            AudioManager.SharedInstance.Play("UIButton_Sound");
            OpenPauseMenu();
        }

        public void OnContinueButton_Click()
        {
            AudioManager.SharedInstance.Play("UIButton_Sound");
            GamePaused = false;
            pauseButton.gameObject.SetActive(true);
            _menuPanelCG.alpha = 0;
            _menuPanelCG.blocksRaycasts = false;
            blurPostProcessVolume.weight = 0;
        }

        public void OnRestartButton_Click()
        {
            LevelManager.SharedInstance.RestartLevel();
        }

        public void OnNextLevelButton_Click()
        {
            AudioManager.SharedInstance.Play("UIButton_Sound");
            LevelManager.SharedInstance.GoNext();
        }

        public void OnExitToMenuButton_Click()
        {
            AudioManager.SharedInstance.Play("UIButton_Sound");
            LevelManager.SharedInstance.ExitToMenu();
        }

        public void ToggleSound()
        {
            _soundOn = !_soundOn;
            if (_soundOn)
                SoundButtonImage.sprite = soundOnImage;
            else SoundButtonImage.sprite = soundOffImage;

            GameManager.Instance.SetSound(_soundOn);
        }
    }
}