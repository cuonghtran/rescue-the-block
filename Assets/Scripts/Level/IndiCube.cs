using UnityEngine;

namespace MainGame
{
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
}