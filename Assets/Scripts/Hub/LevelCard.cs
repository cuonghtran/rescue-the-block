using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace MainGame
{
    public class LevelCard : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            AudioManager.SharedInstance.Play("UIButton_Sound");
            string selectedLevelIndex = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            GeneralUI.Instance.PlayLevel(selectedLevelIndex.Trim());
        }
    }
}

