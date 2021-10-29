using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay;
    [SerializeField] private float respawnDelay;

    private Rigidbody2D ballRigidbody;
    private SpringJoint2D ballSpringJoint;
    private Camera mainCamera;
    private bool isDragging;

    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    void Update()
    {
        if (ballRigidbody == null)
        {
            return;
        }

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging == true)
            {
                LaunchBall();
            }

            isDragging = false;
            return;
        }

        isDragging = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        ballRigidbody.position = worldPosition;
        ballRigidbody.isKinematic = true;
    }

    void LaunchBall()
    {
        ballRigidbody.isKinematic = false;
        ballRigidbody = null;

        Invoke(nameof(DetachBall), detachDelay);
    }

    void DetachBall()
    {
        ballSpringJoint.enabled = false;
        ballSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }

    void SpawnNewBall()
    {
        GameObject newBall = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        ballRigidbody = newBall.GetComponent<Rigidbody2D>();
        ballSpringJoint = newBall.GetComponent<SpringJoint2D>();

        ballSpringJoint.connectedBody = pivot;
    }
}
