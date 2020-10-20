using UnityEngine;

public class ObjectScalar : MonoBehaviour
{
    [SerializeField] private Transform TargetObject;
    [SerializeField, Range (0.001f, 0.02f)] private float ZoomSpeed = 0.5f;
    [SerializeField] private float Offset;

    private Vector3 InitialSize;
    private float ScaleFactor = 1;
    float InitDis = 0;


    void Start()
    {
        InitialSize = TargetObject.localScale;
        ScaleFactor = 1;
    }

    private void OnGUI()
    {
        if (Input.touchCount < 2)
            return;

        Touch t1 = Input.GetTouch(0);
        Touch t2 = Input.GetTouch(1);

        switch (t2.phase)
        {
            case TouchPhase.Began:
                InitDis = Vector2.Distance(t1.position, t2.position);
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Ended:
                InitDis = 0;
                break;
        }
        float delta_fraction = GetDeltaFraction(t1, t2);
        float scale = delta_fraction * ZoomSpeed * Time.deltaTime;
        UpdateScaleFactor(scale);
        ApplyScale();
    }

    private float GetDeltaFraction(Touch t1, Touch t2)
    {
        float f = Vector2.Distance(t1.position, t2.position);
        bool rotate = (t1.deltaPosition.x < 0 && t2.deltaPosition.x < 0) || (t1.deltaPosition.x > 0 && t2.deltaPosition.x > 0);
        bool verticalDir = (t1.deltaPosition.x > t2.deltaPosition.x ?
                        t1.deltaPosition.x - t2.deltaPosition.x : 
                        t2.deltaPosition.x - t1.deltaPosition.x) < 10;
        //if (InitDis < f + Offset && InitDis > f - Offset )
        if (rotate && !verticalDir)
            return 0;


        Vector2 touchZeroPrevPos = t1.position - t1.deltaPosition;
        Vector2 touchOnePrevPos = t2.position - t2.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (t1.position - t2.position).magnitude;

        return prevTouchDeltaMag - touchDeltaMag;
    }
    

    private void UpdateScaleFactor(float deltaFraction)
    {
        ScaleFactor -= deltaFraction;
        ScaleFactor = Mathf.Clamp(ScaleFactor, 0.01f, 1.00f);
    }

    private void ApplyScale()
    {
        TargetObject.localScale = InitialSize * ScaleFactor;
    }
}
