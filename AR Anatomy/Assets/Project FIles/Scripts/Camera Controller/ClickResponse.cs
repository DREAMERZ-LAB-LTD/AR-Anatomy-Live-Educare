using UnityEngine;


[System.Serializable]
public class ClickResponse
{
    [SerializeField] float doubleClickDelay = 0.3f;
    [SerializeField] float maxClickOffset = 2;

    float time = 0;
    bool isClickCanceled = true;

    /// <summary>
    /// return true if double click is detected
    /// </summary>
    public bool isDoubleClicked
    {
        get
        {
            if (!isClickCanceled)
            {
                time += Time.deltaTime;
                isClickCanceled = time > doubleClickDelay;
            }
            if (Input.touchCount != 1) return false;
            Touch touch = Input.GetTouch(0);


            switch (touch.phase)
            {
                case TouchPhase.Began:
                    //when double touch is detected
                    bool doubleClickDetected = !isClickCanceled;
                    if (doubleClickDetected)
                    {
                        isClickCanceled = true;
                        time = 0;
                        return true;
                    }

                    time = 0;
                    isClickCanceled = false;
                    return false;
         

                case TouchPhase.Ended:
                    float clickLength = touch.deltaPosition.magnitude;
                    if (clickLength > maxClickOffset)
                    { 
                        isClickCanceled = true;
                        time = 0;
                    }
                    return false;
            }
            return false;
        }
    }
}
