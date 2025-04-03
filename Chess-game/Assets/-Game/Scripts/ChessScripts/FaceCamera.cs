using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;
    private bool isPlayerWhite;

    private void Start()
    {
        mainCamera = Camera.main;
        isPlayerWhite = PlayerPrefs.GetString("PlayerColor", "White") == "White";
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            float cameraRotation = mainCamera.transform.rotation.eulerAngles.z;

            if (!isPlayerWhite)
            {
                cameraRotation -= 0f;
            }
            ApplyRotation(cameraRotation);
        }
    }

    private void ApplyRotation(float cameraRotation)
    {
        float rotationAngle = cameraRotation;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
    }
}
