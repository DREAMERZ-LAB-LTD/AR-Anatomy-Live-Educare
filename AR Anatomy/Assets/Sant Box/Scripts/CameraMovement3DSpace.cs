using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraMovement3DSpace : MonoBehaviour
{
    public float minimumMovementSensitivity;
    public float speed = -0.05f;

    public float maxX = 1;
    public float maxY = 1;
    public float zPos = 0;

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if (Input.touchCount > 1)
        {
            Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                float xChange = -touch.deltaPosition.x;
                float yChange = -touch.deltaPosition.y;

                if (Mathf.Abs(xChange) < minimumMovementSensitivity || Mathf.Abs(yChange) < minimumMovementSensitivity)
                    return;
                Vector3 currentPos = transform.position;
                Vector3 targetX = (xChange * Time.deltaTime * speed * Camera.main.transform.forward);
                //if (targetX > maxX) targetX = maxX;
                //if (targetX < -maxX) targetX = -maxX;

                Vector3 targetY = (yChange * Time.deltaTime * speed * Camera.main.transform.forward);
                //float targetY = currentPos.y + (yChange * Time.deltaTime * speed * Camera.main.transform.forward);
                //if (targetY > maxY) targetY = maxY;
                //if (targetY < -maxY) targetY = -maxY;
        
                float targetZ = 0;

                Vector3 targetPos = new Vector3(targetX.x+ currentPos.x, targetY.y + currentPos.y, targetZ);

                transform.DOMove(targetPos, 0.1f);
            }
        }
    }
}
