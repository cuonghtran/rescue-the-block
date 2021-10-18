using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndiCube : MonoBehaviour
{
    [SerializeField] private bool isGrounded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            isGrounded = true;
        }
    }

    public void ResetStatus()
    {
        isGrounded = false;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
