using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target; // The character to follow
    public float smoothSpeed = 0.125f; // The speed of the camera's smooth movement
    public Vector3 offset = new Vector3(0, 1, -10); // Offset of the camera relative to the target
    public float minX; // Minimum X position for the camera
    public float maxX; // Maximum X position for the camera

    private void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow2D: No target set for the camera to follow.");
            return;
        }

        // Calculate the desired position with the offset
        Vector3 desiredPosition = target.position + offset;

        // Clamp the X position to the specified bounds
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);

        // Smoothly interpolate the camera's position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the new position to the camera
        transform.position = new Vector3(smoothedPosition.x, 0, smoothedPosition.z);
    }
}
