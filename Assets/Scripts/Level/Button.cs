using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private List<GameObject> linkedBlocks;

    private float _yPressedLoc = 0.28f;
    private float _duration = 0.1f;
    private bool _isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isTriggered)
            {
                AudioManager.SharedInstance.Play("Button_Sound");
                StartCoroutine(PressButton());
            }
        }
    }

    IEnumerator PressButton()
    {
        _isTriggered = true;
        LeanTween.moveY(gameObject, _yPressedLoc, _duration);
        yield return new WaitForSeconds(_duration);
        linkedBlocks.ForEach(x => x.SetActive(true));
    }
}
