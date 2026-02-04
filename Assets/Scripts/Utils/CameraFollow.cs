using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, -5f);
    [SerializeField] private float smoothSpeed = 5f;

    void Start()
    {
        // Find player if not assigned
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Look at player
        transform.LookAt(target.position + Vector3.up);
    }
}
