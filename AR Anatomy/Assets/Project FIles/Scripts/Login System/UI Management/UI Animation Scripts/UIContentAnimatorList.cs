using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DL.UI
{
    public class UIContentAnimatorList : MonoBehaviour
    {
        public List<UIContentAnimator> uIContentAnimators = new List<UIContentAnimator>();
        public void Appear()
        {
            foreach (var element in uIContentAnimators)
            {
                element.Appear();
            }
        }
        public void DisAppear()
        {
            foreach (var element in uIContentAnimators)
            {
                element.Disappear();
            }
        }
    }

}