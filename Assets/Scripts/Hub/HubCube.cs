using System.Collections;
using UnityEngine;

public class HubCube : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 0.15f;
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private Vector3 positionA;
    [SerializeField] private Vector3 positionB;

    private Vector3 _targetPos;
    private bool _goingForward = true;

    private void Start()
    {
        _targetPos = positionB;   
    }

    // Update is called once per frame
    void Update()
    {
        // rotate around
        float x = Random.Range(-0.25f, 0.25f);
        float z = Random.Range(-0.15f, 0.15f);
        transform.RotateAround(transform.position, new Vector3(x, 1, z), rotateSpeed);

        if (positionA != Vector3.zero && positionB != Vector3.zero)
        {
            // float cube
            if (_goingForward)
                transform.position = Vector3.MoveTowards(transform.position, positionB, floatSpeed * Time.deltaTime);
            else if (!_goingForward)
                transform.position = Vector3.MoveTowards(transform.position, positionA, floatSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, positionB) <= 0.001f && _goingForward)
                StartCoroutine(SwitchDirection());

            if (Vector3.Distance(transform.position, positionA) <= 0.001f && !_goingForward)
                StartCoroutine(SwitchDirection());
        }
    }

    IEnumerator SwitchDirection()
    {
        yield return new WaitForSeconds(0.4f);
        _goingForward = !_goingForward;
    }
}
