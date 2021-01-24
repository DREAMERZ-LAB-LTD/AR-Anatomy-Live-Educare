using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anatomy.Body
{
    public class AnatomySystem : MonoBehaviour
    {
        [SerializeField] protected GameObject[] bodyParts;


        public virtual void ShowBodyLayer(int currentIndx)
        {

            bodyParts[currentIndx].SetActive(true);

            for (int i = 0; i < currentIndx; i++)
            {
                bodyParts[i].SetActive(false);
            }

            bool isOnlySkeletnLayer = currentIndx == 3;
            for (int i = currentIndx + 1; i < bodyParts.Length; i++)
            {
                bodyParts[i].SetActive(!isOnlySkeletnLayer);
            }

        }

        protected void SetActive_AllLayers(bool show)
        {
            for (int i =0; i < bodyParts.Length; i++)
            {
                bodyParts[i].SetActive(show);
            }
        }

    }
}