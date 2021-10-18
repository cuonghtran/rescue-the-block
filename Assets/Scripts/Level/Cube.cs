using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityStandardAssets.CrossPlatformInput;

public class Cube : MonoBehaviour
{
    public static Cube Player;

    public enum CubeState { Standing, Laying };
    private enum LayingState { Vertical, Horizontal, None };

    [Header("Information")]
    [SerializeField] private CubeState cubeState = CubeState.Standing;
    [SerializeField] private LayingState layingState = LayingState.None;
    [SerializeField] private float rollSpeed = 6f;
    [SerializeField] private int stableBlockNumber = 2;

    [Header("References")]
    [SerializeField] private List<IndiCube> IndiCubes = new List<IndiCube>();
    [SerializeField] private LayerMask obstacleLayer;

    private Rigidbody _rb;
    private float _shortScale;
    private float _longScale;
    private bool _isMoving;
    private bool _isStable;
    private bool _isDead;
    private int _groundedCount = 0;
    //private float horizontalInput;
    //private float verticalInput;

    private void Awake()
    {
        if (Player == null)
            Player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _shortScale = transform.localScale.x / 2;
        _longScale = transform.localScale.y / 2;
        
        var childrenTransforms = transform.Cast<Transform>().ToList();
        childrenTransforms.ForEach(x => IndiCubes.Add(x.GetComponent<IndiCube>()));
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.GamePaused || _isDead)
            return;

        if (!_isMoving)
        {
            //horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
            //verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
            //if (horizontalInput < 0) StartCoroutine(Assemble(Vector3.left));
            //else if (horizontalInput > 0) StartCoroutine(Assemble(Vector3.right));
            //else if (verticalInput < 0) StartCoroutine(Assemble(Vector3.back));
            //else if (verticalInput > 0) StartCoroutine(Assemble(Vector3.forward));

            if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(Assemble(Vector3.left));
            else if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(Assemble(Vector3.right));
            else if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(Assemble(Vector3.back));
            else if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(Assemble(Vector3.forward));
        }
    }

    IEnumerator Assemble(Vector3 direction)
    {
        // check if there is an obstacle at the moving direction
        if (CheckObstacles(IndiCubes, direction))
        {
            yield break;
        }

        _isMoving = true;
        IndiCubes.ForEach(x => x.ResetStatus()); // reset indi cubes status

        var axis = Vector3.Cross(Vector3.up, direction);  // or Vector.forward;
        Vector3 anchor = Vector3.zero;

        if (cubeState == CubeState.Standing)
        {
            anchor = transform.position + ((Vector3.down * _longScale) + (direction * _shortScale));
            //yield return StartCoroutine(Roll(anchor, axis));
            if (direction == Vector3.forward || direction == Vector3.back)
            {
                cubeState = CubeState.Laying;
                layingState = LayingState.Vertical;
            }
            else if (direction == Vector3.left || direction == Vector3.right)
            {
                cubeState = CubeState.Laying;
                layingState = LayingState.Horizontal;
            }
        }
        else if (cubeState == CubeState.Laying)
        {
            if (layingState == LayingState.Vertical)
            {
                if (direction == Vector3.forward || direction == Vector3.back)
                    anchor = transform.position + ((Vector3.down * _shortScale) + (direction * _longScale));
                else anchor = transform.position + (Vector3.down + direction) * _shortScale;
                //yield return StartCoroutine(Roll(anchor, axis));

                if (direction == Vector3.forward || direction == Vector3.back)
                {
                    cubeState = CubeState.Standing;
                    layingState = LayingState.None;
                }
            }
            else if (layingState == LayingState.Horizontal)
            {
                if (direction == Vector3.left || direction == Vector3.right)
                    anchor = transform.position + ((Vector3.down * _shortScale) + (direction * _longScale));
                else anchor = transform.position + (Vector3.down + direction) * _shortScale;
                //yield return StartCoroutine(Roll(anchor, axis));

                if (direction == Vector3.left || direction == Vector3.right)
                {
                    cubeState = CubeState.Standing;
                    layingState = LayingState.None;
                }
            }

        }
        yield return StartCoroutine(Roll(anchor, axis));

        // check stable
        _isStable = CheckStable(IndiCubes);
        if (!_isStable)
        {
            StartCoroutine(FallSequence());
        }
    }

    bool CheckObstacles(List<IndiCube> indiCubes, Vector3 direction)
    {
        Ray ray = new Ray();
        RaycastHit hitInfo;

        foreach (IndiCube i in indiCubes)
        {
            ray.origin = i.transform.position;
            ray.direction = direction;

            if (Physics.Raycast(ray, out hitInfo, 0.6f, obstacleLayer))
            {
                return true;
            }
        }

        return false;
    }

    bool CheckEmpty(List<IndiCube> indiCubes, Vector3 direction)
    {
        Ray ray = new Ray();
        RaycastHit hitInfo;

        foreach (IndiCube i in indiCubes)
        {
            ray.origin = i.transform.position;
            ray.direction = new Vector3(direction.x, -1, direction.z);

            if (Physics.Raycast(ray, out hitInfo, 0.6f, obstacleLayer))
            {
                
            }
        }
        
        return true;
    }

    bool CheckStable(List<IndiCube> indiCubes)
    {
        _groundedCount = 0;
        foreach (IndiCube i in indiCubes)
        {
            if (i.IsGrounded())
                _groundedCount++;
        }
        
        if (cubeState == CubeState.Standing)
            return _groundedCount >= 1;
        else
            return _groundedCount >= stableBlockNumber;
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        AudioManager.SharedInstance.Play("Roll_Sound");

        for (int i = 0; i < 90 / rollSpeed; i++)
        {
            transform.RotateAround(anchor, axis, rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = new Vector3((float)Math.Round(transform.position.x, 1),
                                        (float)Math.Round(transform.position.y, 1),
                                        (float)Math.Round(transform.position.z, 1));
        yield return new WaitForSeconds(0.02f);
        _isMoving = false;
    }

    IEnumerator FallSequence()
    {
        LevelManager.SharedInstance.UnfollowCamera(); // detach follow camera

        _isDead = true;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        yield return new WaitForSeconds(0.8f);
        LevelManager.SharedInstance.RestartLevel();
    }

    IEnumerator VictorySequence()
    {
        AudioManager.SharedInstance.Play("Victory_Sound");
        LevelManager.SharedInstance.UnfollowCamera(); // detach follow camera

        _isMoving = true;
        float elapsed = 0;
        float upSpeed = 4.5f;
        float rotateSpeed = 3.5f;
        while(elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            // make cube go upward and spin around when winning
            transform.Translate(Vector3.up * upSpeed * Time.deltaTime, Space.World);
            transform.RotateAround(transform.position, new Vector3(0, 1, 0), rotateSpeed);
            yield return null;
        }

        UIManager.Instance.OpenFinishMenu();
    }

    public void TriggerVictory()
    {
        StartCoroutine(VictorySequence());
    }

    public void TriggerDead()
    {
        StartCoroutine(FallSequence());
    }

    public bool CheckDead()
    {
        return _isDead;
    }

    public CubeState GetCubeState()
    {
        return cubeState;
    }
}
