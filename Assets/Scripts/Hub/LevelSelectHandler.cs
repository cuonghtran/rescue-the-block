using UnityEngine;
using TMPro;

public class LevelSelectHandler : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CoreData coreData;

    [Header("References")]
    [SerializeField] private Transform levelPanel1;

    // Start is called before the first frame update
    void Start()
    {
        LoadLevelProgress();
    }

    void LoadLevelProgress()
    {
        foreach (Transform levelCard in levelPanel1)
        {
            TextMeshProUGUI lvlText = levelCard.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (int.Parse(lvlText.text) <= coreData.progress + 1)
            {
                LeanTween.alphaCanvas(levelCard.GetComponent<CanvasGroup>(), 1, 0);
            }
            else
            {
                LeanTween.alphaCanvas(levelCard.GetComponent<CanvasGroup>(), 0.3f, 0);
                levelCard.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }
}
