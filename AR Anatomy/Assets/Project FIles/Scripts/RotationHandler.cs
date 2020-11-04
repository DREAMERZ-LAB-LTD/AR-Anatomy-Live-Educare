using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationHandler : MonoBehaviour
{
    [SerializeField] private float Speed;
    public GameObject rootMaleAnatomy;
    private bool _rotate;

    [SerializeField] private Button RotateBtn;
    [SerializeField] private Sprite UpImg;
    [SerializeField] private Sprite DownImg;
    

    private void Start()
    {
        RotateBtn.onClick.RemoveAllListeners();
        RotateBtn.onClick.AddListener(MaleAnatomyRotateToggle);

    }

    private void OnEnable()
    {
        rootMaleAnatomy.transform.rotation = LookTo(rootMaleAnatomy.transform, Camera.main.transform);
    }

    public static Quaternion LookTo(Transform A, Transform B)
    {
        Vector3 dir = A.position - B.position;
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg - 90;
        return Quaternion.Euler(new Vector3 (A.rotation.eulerAngles.x, -angle, A.rotation.eulerAngles.z));
    }


    // Update is called once per frame
    void Update()
    {
        VerticalRotation();
        HorizontalRotation();
    }
    public void MaleAnatomyRotateToggle()
    { 
        _rotate = !_rotate;
    
    }
    

    private void VerticalRotation()
    {
        Vector3 rot = rootMaleAnatomy.transform.localEulerAngles;
        rot.x = _rotate ? 90 : 0;
        RotateBtn.GetComponent<Image>().sprite = _rotate ? UpImg : DownImg;
        rootMaleAnatomy.transform.rotation = Quaternion.Slerp(rootMaleAnatomy.transform.rotation,Quaternion.Euler(rot), Speed * Time.deltaTime);
    }

    private void HorizontalRotation()
    {
        int touchCount = Input.touchCount;
        if (touchCount < 2) return;// return from here when less than 2 touches on screen

        Touch touch = Input.GetTouch(touchCount - 1);

        float fraction = -touch.deltaPosition.x;

        Vector3 rot = rootMaleAnatomy.transform.rotation.eulerAngles;
        rot.y += fraction * Speed * Time.deltaTime;
        rootMaleAnatomy.transform.rotation = Quaternion.Euler(rot);
    }
}
