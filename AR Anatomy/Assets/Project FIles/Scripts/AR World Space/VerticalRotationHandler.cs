using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalRotationHandler : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float Speed;
    public GameObject rootMaleAnatomy;
    [SerializeField] private bool isStanding = true;

    [Header("Anatomy Shadow Controller Reference")]
    [SerializeField] ShadowController Shadow;

    [Header("UI Reference")]
    [SerializeField] private Button RotateBtn;
    [SerializeField] private Sprite UpImg;
    [SerializeField] private Sprite DownImg;
#pragma warning restore 649
    

    private void Start()
    {
        RotateBtn.onClick.RemoveAllListeners();
        RotateBtn.onClick.AddListener(MaleAnatomyRotateToggle);

        isStanding = true;
        Shadow.SetMode(isStanding);
    }

    private void OnEnable()=>rootMaleAnatomy.transform.rotation = LookTo(rootMaleAnatomy.transform, Camera.main.transform);
    

    public static Quaternion LookTo(Transform A, Transform B)
    {
        Vector3 dir = A.position - B.position;
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg - 90;
        return Quaternion.Euler(new Vector3 (A.rotation.eulerAngles.x, -angle, A.rotation.eulerAngles.z));
    }


    // Update is called once per frame
    void Update()=>VerticalRotation();
    
    public void MaleAnatomyRotateToggle()
    { 
        isStanding = !isStanding;
        Shadow.SetMode(isStanding);
    }
    
    private void VerticalRotation()
    {
        Vector3 rot = rootMaleAnatomy.transform.localEulerAngles;
        rot.x = isStanding ? 0 : 90;
        RotateBtn.GetComponent<Image>().sprite = isStanding ? UpImg : DownImg;
        rootMaleAnatomy.transform.rotation = Quaternion.Slerp(rootMaleAnatomy.transform.rotation,Quaternion.Euler(rot), Speed * Time.deltaTime);
    }
}
