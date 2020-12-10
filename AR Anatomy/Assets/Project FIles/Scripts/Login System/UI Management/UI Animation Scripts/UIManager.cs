using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private float disableDelay = 0.25f;
        [SerializeField] List<UIContentAnimatorList> animatorLists = new List<UIContentAnimatorList>();
        [HideInInspector] public int currentScene = 0;
        [Space]
        [SerializeField] AudioSource _audioSource;
        [SerializeField] AudioClip _uiClickSound;
        public void ButtonClick() => _audioSource.PlayOneShot(_uiClickSound, Random.Range(0.5f, 0.8f));


        /// <summary>
        /// Call this method from button to switch menus
        /// </summary>
        /// <param name="newScene"></param>
        public void SwitchMenuScene(int newScene)
        {
            //Debug.Log("test");
            StopAllCoroutines(); //this is inefficient
            if (newScene != currentScene) DisappearScene(animatorLists[currentScene]);
            AppearScene(animatorLists[newScene]);

            currentScene = newScene;
        }
        /// <summary>
        /// Call this method from button to switch menus
        /// WITH
        /// clean up method called
        /// Clean up method checks for missing animator list items. This is due to dynamic loading unloading of car list
        /// </summary>
        /// <param name="newScene"></param>
        public void CleanSwitchMenu(int newScene)
        {
            animatorLists[newScene].gameObject.SetActive(true);
            DisappearScene(animatorLists[currentScene]);

            AppearScene(animatorLists[newScene]);

            currentScene = newScene;
        }
        public void SwitchMenuScene(int newScene, bool removeOldScene)
        {
            if (removeOldScene) DisappearScene(animatorLists[currentScene]);
            AppearScene(animatorLists[newScene]);
            currentScene = newScene;
        }

        public void SwitchMenuSceneWithDelay(int newScene)
        {
            DisappearScene(animatorLists[currentScene]);
            AppearScene(animatorLists[newScene]);

            currentScene = newScene;
        }

        public void SwitchMenuSceneAfterDelay(int newScene)
        {
            StartCoroutine(DisappearAfter());
            AppearScene(animatorLists[newScene]);

            currentScene = newScene;
        }

        private IEnumerator DisappearAfter()
        {
            yield return new WaitForSeconds(1);
            DisappearScene(animatorLists[currentScene]);
        }


        void AppearScene(UIContentAnimatorList uIContentAnimatorList)
        {
            uIContentAnimatorList.transform.SetAsLastSibling();
            uIContentAnimatorList.gameObject.SetActive(true);

            for (int i = 0; i < uIContentAnimatorList.uIContentAnimators.Count; i++)
            {
                if (uIContentAnimatorList.uIContentAnimators[i].gameObject.activeInHierarchy)
                    uIContentAnimatorList.uIContentAnimators[i].Appear();
            }
        }
        public void DisappearScene()
        {
            DisappearScene(animatorLists[currentScene]);
        }
        void DisappearScene(UIContentAnimatorList uIContentAnimatorList)
        {
            if (!uIContentAnimatorList)
                return;

            for (int i = 0; i < uIContentAnimatorList.uIContentAnimators.Count; i++)
            {
                uIContentAnimatorList.uIContentAnimators[i].Disappear();
            }

            StartCoroutine(DeActivateScene(uIContentAnimatorList.gameObject));
        }

        IEnumerator DeActivateScene(GameObject obj)
        {
            yield return new WaitForSeconds(disableDelay);
            obj.SetActive(false);
        }

        public void CleanUpReferences(UIContentAnimatorList uIContentAnimatorList)
        {
            for (int i = 0; i < uIContentAnimatorList.uIContentAnimators.Count; i++)
            {
                if (!uIContentAnimatorList.uIContentAnimators[i])
                    uIContentAnimatorList.uIContentAnimators.RemoveAt(i);
            }
        }

        public void DelayedDisappearScene()
        {
            DelayedDisappearScene(animatorLists[currentScene]);
        }
        void DelayedDisappearScene(UIContentAnimatorList uIContentAnimatorList)
        {
            for (int i = 0; i < uIContentAnimatorList.uIContentAnimators.Count; i++)
            {
                uIContentAnimatorList.uIContentAnimators[i].Disappear();
            }

            StartCoroutine(DelayedDeActivateScene(uIContentAnimatorList.gameObject));
        }

        IEnumerator DelayedDeActivateScene(GameObject obj) //Bad way to handle this
        {
            yield return new WaitForSeconds(3f);
            obj.SetActive(false);
        }

    }
}