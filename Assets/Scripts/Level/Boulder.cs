using UnityEngine;

public class Boulder : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            var rb = collision.transform.GetComponent<Rigidbody>();
            if (rb)
            {
                var direction = rb.position - transform.position;
                Cube.Player.TriggerDead();
                AudioManager.SharedInstance.Play("Boulder_Collide_Sound");
                rb.AddForceAtPosition(direction * 15, collision.contacts[0].point, ForceMode.Impulse);
            }
        }
    }
}
