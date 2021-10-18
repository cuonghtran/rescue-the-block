using System.Collections;
using UnityEngine;

namespace MainGame
{
    public class WallButton : MonoBehaviour
    {
        [SerializeField] private GameObject linkedWall;
        [SerializeField] private GameObject linkedBoulder;

        private float _yLoc = 0.28f;
        private float _wallLoc = -1f;
        private float _duration = 0.1f;
        private bool _isTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!_isTriggered)
                {
                    AudioManager.SharedInstance.Play("Wall_Button_Sound");
                    PressButton();
                }
            }
        }

        void PressButton()
        {
            _isTriggered = true;
            LeanTween.moveY(gameObject, _yLoc, _duration);
            LeanTween.moveY(linkedWall, _wallLoc, _duration + 0.25f);
            StartCoroutine(ReleaseBoulder());
        }

        IEnumerator ReleaseBoulder()
        {
            yield return new WaitForSeconds(0.4f);
            linkedBoulder.GetComponent<Rigidbody>().velocity = linkedBoulder.GetComponent<Rigidbody>().velocity * 2.5f;
            yield return new WaitForSeconds(3.5f);
            linkedBoulder.SetActive(false);
        }
    }
}