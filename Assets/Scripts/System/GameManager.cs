using System.IO;
using UnityEngine;
using UnityEngine.Audio;

namespace MainGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private CoreData coreData;
        [SerializeField] private AudioMixerGroup mainMixerGroup;
        private readonly string firstPlay = "FirstPlay";
        private readonly string soundTogglePref = "SoundToggle";

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            Application.targetFrameRate = 60;
        }

        // Start is called before the first frame update
        void Start()
        {
            InitDataOnOpeningGame();
        }

        void InitDataOnOpeningGame()
        {
            try
            {
                MusicSettings();
                SaveLoadSystem.LoadGame(coreData);
            }
            catch (FileNotFoundException e)
            {
                InitPlayerData();
                FirstPlayMusicSettings();
            }
        }

        void InitPlayerData()
        {
            coreData.progress = 0;
            coreData.SoundOn = true;
        }

        public void FirstPlayMusicSettings()
        {
            coreData.SoundOn = true;
            mainMixerGroup.audioMixer.SetFloat("Volume", 0);
            PlayerPrefs.SetInt(soundTogglePref, 1);
        }

        public void MusicSettings()
        {
            var soundValue = coreData.SoundOn;
            if (soundValue)
            {
                mainMixerGroup.audioMixer.SetFloat("Volume", 0);
                coreData.SoundOn = true;
            }
            else
            {
                mainMixerGroup.audioMixer.SetFloat("Volume", -80);
                coreData.SoundOn = false;
            }
        }

        public void SetSound(bool soundOn)
        {
            if (soundOn)
            {
                mainMixerGroup.audioMixer.SetFloat("Volume", 0);
                coreData.SoundOn = true;
            }
            else
            {
                mainMixerGroup.audioMixer.SetFloat("Volume", -80);
                coreData.SoundOn = false;
            }
        }
    }
}