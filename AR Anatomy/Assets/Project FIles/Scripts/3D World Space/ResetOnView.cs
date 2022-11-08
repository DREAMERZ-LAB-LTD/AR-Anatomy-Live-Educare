using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anatomy.MoveingPart
{
    public class ResetOnView : MonoBehaviour
    {
        private Vector3 initialLocalPosition;
        private Quaternion initialLocalRotation;

        private void Awake()
        {
            initialLocalPosition = transform.localPosition;
            initialLocalRotation = transform.localRotation;
        }

        private void OnEnable()
        {
            transform.localPosition = initialLocalPosition;
            transform.localRotation = initialLocalRotation;
        }


    }
}