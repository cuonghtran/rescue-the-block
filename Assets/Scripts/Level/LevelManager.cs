using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager SharedInstance;
    public bool GameStart;
    [Header("Data")]
    [SerializeField] private CoreData coreData;

    [Header("Information")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    public int levelIndex;
    [SerializeField] private string currentLevelString;
    [SerializeField] private string nextLevelString;

    private void Awake()
    {
        SharedInstance = this;

        Invoke(nameof(StartGame), 0.6f);
    }

    void StartGame()
    {
        GameStart = true;
    }

    public void SaveProgress()
    {
        if (levelIndex > coreData.progress)
            coreData.progress = levelIndex;

        SaveLoadSystem.SaveGame(coreData);
    }

    public void GoNext()
    {
        ChangeScene(nextLevelString);
    }

    public void ExitToMenu()
    {
        ChangeScene(ConstantsList.Scenes["OpeningScene"]);
    }

    public void RestartLevel()
    {
        SceneController.Instance.FadeAndLoadScene(currentLevelString);
    }

    public void UnfollowCamera()
    {
        virtualCamera.Follow = null;
    }

    void ChangeScene(string sceneName)
    {
        SceneController.Instance.FadeAndLoadScene(sceneName);
    }
}
