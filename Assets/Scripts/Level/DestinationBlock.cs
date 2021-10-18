using UnityEngine;

public class DestinationBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Cube.Player.GetCubeState() == Cube.CubeState.Standing)
        {
            Cube.Player.TriggerVictory();
        }
    }
}
