using UnityEngine;

public class ObjectScalar : MonoBehaviour
{
    [SerializeField] private Transform TargetObject;
    [SerializeField, Range (0.001f, 0.02f)] private float ZoomSpeed = 0.5f;
    [SerializeField] private float Offset;

    private Vector3 InitialSize;
    private float ScaleFactor = 1;



    void Start()
    {
        InitialSize = TargetObject.localScale;
        ScaleFactor = 1;
    }

    private void OnGUI()
    {
        if (Input.touchCount < 2) return;
        

        Touch t1 = Input.GetTouch(0);
        Touch t2 = Input.GetTouch(1);

        if (t2.phase == TouchPhase.Moved)
        {
            bool rotate = MaskScaleInput(t1, t2);
            if (rotate) return;

            float delta_fraction = GetDeltaFraction(t1, t2);
            
            UpdateScaleFactor(delta_fraction);
            ApplyScale();
        }
    }
    private bool MaskScaleInput(Touch t1, Touch t2)
    {
        //result will be true when finger swipe to horizontal direction
        bool isRotating = (t1.deltaPosition.x < 0 && t2.deltaPosition.x < 0) ||
                          (t1.deltaPosition.x > 0 && t2.deltaPosition.x > 0);

        return isRotating;
    }

    private float GetDeltaFraction(Touch t1, Touch t2)
    {
        Vector2 touchZeroPrevPos = t1.position - t1.deltaPosition;
        Vector2 touchOnePrevPos = t2.position - t2.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (t1.position - t2.position).magnitude;

        return prevTouchDeltaMag - touchDeltaMag;
    }
    

    private void UpdateScaleFactor(float deltaFraction)
    {
        float scaleRatio = deltaFraction * ZoomSpeed * Time.deltaTime;
        ScaleFactor -= scaleRatio;

        ScaleFactor = Mathf.Clamp(ScaleFactor, 0.3f, 1.00f);
    }

    private void ApplyScale()
    {
        TargetObject.localScale = InitialSize * ScaleFactor;
    }
}
