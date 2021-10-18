using System.Collections;
using UnityEngine;

public class CrackBlock : MonoBehaviour
{
    public float dropSpeed = 5;

    private bool _isTriggered;
    private bool _dropFlag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isTriggered = true;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isTriggered && !Cube.Player.CheckDead())
            {
                _dropFlag = true;
                StartCoroutine(HideObject());
            }
        }
    }

    private void Update()
    {
        if (_dropFlag)
        {
            transform.Translate(Vector3.down * dropSpeed * Time.deltaTime);
        }
    }

    IEnumerator HideObject()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
