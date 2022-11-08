using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;
/// <summary>
/// Image position change scrapped due to repositioning stuff like slider background was creating weird positions
/// </summary>
namespace DL.UI
{
    public class UIContentAnimator : MonoBehaviour
    {
        [SerializeField] float delay = 0;
        [SerializeField] float fadeInTime = 0.25f;
        [SerializeField] float fadeOutTime = 0.25f;
        [Space]
        public List<Image> images = new List<Image>();
        public List<TextMeshProUGUI> text = new List<TextMeshProUGUI>();
        List<Vector3> textsOriginalPosition = new List<Vector3>();


        private void Awake()
        {
            CleanUP();
        }

        void CleanUP()
        {
            for (int k = 0; k < images.Count; k++)
            {
                if (!images[k]) return;

                Color c = images[k].color;
                c.a = 0;
                images[k].color = c;
                images[k].raycastTarget = false;
            }
            for (int k = 0; k < text.Count; k++)
            {
                if (!text[k]) return;

                Color c = text[k].color;
                c.a = 0;
                text[k].color = c;
            }
        }

        public void Appear()
        {
            StopAllCoroutines();
            StartCoroutine(AppearRoutine(delay));
        }

        private IEnumerator AppearRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i])
                {
                    images[i].DOFade(1, fadeInTime);
                    images[i].raycastTarget = true;
                }
            }
            for (int i = 0; i < text.Count; i++)
            {
                if (text[i])
                {
                    text[i].DOFade(1, fadeInTime * 2);
                }
            }
        }

        public void Disappear()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(DisapppearRoutine());
        }

        private IEnumerator DisapppearRoutine()
        {
            yield return new WaitForSeconds(delay / 4);
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i])
                {
                    images[i].DOKill(false);
                    images[i].DOFade(0, fadeOutTime);
                    images[i].raycastTarget = false;
                }
            }
            for (int i = 0; i < text.Count; i++)
            {
                if (text[i])
                {
                    text[i].DOKill(false);
                    text[i].DOFade(0, fadeOutTime * 2);
                }
            }
        }
    }
}