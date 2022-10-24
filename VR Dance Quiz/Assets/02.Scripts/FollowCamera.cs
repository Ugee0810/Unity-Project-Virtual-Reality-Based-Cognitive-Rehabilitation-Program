using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public new Camera camera;

    [Range(0, 0.2f)]
    public float smoothFactor = 0.1f;

    private void LateUpdate()
    {
        // make the UI always face towards the camera
        transform.rotation = camera.transform.rotation;

        var currentPos = transform.position;

        var cameraCenter = camera.transform.position + camera.transform.forward;

        // in which direction from the center?
        var direction = currentPos - cameraCenter;

        // target is in the same direction but offsetRadius
        // from the center
        var targetPosition = cameraCenter + direction.normalized;

        // finally interpolate towards this position
        transform.position = Vector3.Lerp(currentPos, targetPosition, smoothFactor);
    }
}
