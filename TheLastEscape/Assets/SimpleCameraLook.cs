using UnityEngine;

public class SimpleCameraLook : MonoBehaviour
{
    public Vector2 cameraSensitivity = new Vector2(1.0f, 1.0f);
    public float cameraRotationLimit = 90.0f;
    private Vector2 cameraRotation = Vector2.zero;

    void Update()
    {
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseMovement = mouseMovement * cameraSensitivity;

        cameraRotation.x += mouseMovement.x;
        cameraRotation.y -= mouseMovement.y;

        // Clamp the vertical rotation
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -cameraRotationLimit, cameraRotationLimit);

        // Apply rotation to the camera
        transform.localRotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0);

        // Debug log to check rotation values
        Debug.Log("Simple Camera Rotation X: " + cameraRotation.x + ", Y: " + cameraRotation.y);
    }
}