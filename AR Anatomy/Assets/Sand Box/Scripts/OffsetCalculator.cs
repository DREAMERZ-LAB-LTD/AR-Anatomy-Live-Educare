using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCalculator : MonoBehaviour
{
    [Header("Camera Field Of View Angle")]
    [SerializeField] int cameraFieldOfView = 60;
    [Header("Camera Max Zoom Out Point")]
    [SerializeField] Transform projectionPoint;
    [SerializeField] Transform center;
    [SerializeField, Range(0.00f, 100.00f)] float thresholdOffset = 0;


    /// <summary>
    /// return world space vector from left side to right side W.R.To screen
    /// </summary>
    /// <param name="time">should be 0 to 1</param>
    /// <returns></returns>
    public Vector3 GetPoint(float time)
    {
        time = Mathf.Clamp(time, 0.00f, 1.00f);
        Vector3 rightThreshold = GetRithtThreshold(center, projectionPoint.position, cameraFieldOfView);
        Vector3 leftThresholdos = -rightThreshold;
        return Vector3.Lerp(leftThresholdos, rightThreshold, time);
    }

    /// <summary>
    /// return world space vector from left side to right side W.R.To screen
    /// </summary>
    /// <param name="leftThreshold">Left side world space vector W.R.To Screen</param>
    /// <param name="rightThreshold">Right side world space vector W.R.To Screen</param>
    public void GetThresholds(out Vector3 leftThreshold, out Vector3 rightThreshold)
    {
        rightThreshold = GetRithtThreshold(center, projectionPoint.position, cameraFieldOfView);
        leftThreshold = -rightThreshold;
    }




    private Vector3 GetRithtThreshold(Transform center, Vector3 lookPoint, int cameraFieldOfView = 60)
    {

        float angleInDeg = cameraFieldOfView / 2;
        float angleInRad = angleInDeg * Mathf.Deg2Rad;

        float adjacent = Vector3.Distance(center.position, lookPoint);
        /*
         * formula :
         * opposite = adjacent * tan(theta)
         */
        float opposite = Mathf.Tan(angleInRad) * adjacent;

        float heightOffset = (opposite / 100) * thresholdOffset;
        float height = opposite - heightOffset;
        height /= 2;

        Vector3 localRight = center.right;
        Vector3 worldRight = localRight * height;

        Vector3 rightThreshold = center.position + worldRight;
        return rightThreshold;
    }


#if UNITY_EDITOR
    [Header("Draw Threshold Points Debug")]
    [SerializeField] bool showGizmos = true;
    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        GetThresholds(out Vector3 a, out Vector3 b);
        a.y = center.position.y;
        b.y = center.position.y;


        Gizmos.DrawSphere(a, 0.1f);
        Gizmos.DrawSphere(b, 0.1f);
    }
#endif
}
