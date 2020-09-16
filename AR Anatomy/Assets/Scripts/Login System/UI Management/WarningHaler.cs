using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WarningHaler : MonoBehaviour
    {
        [Header("Warning Panel Object Reference")]
        [SerializeField] private GameObject WarningPanelObj;

        [Header("Reference Warning Panel Elements")]
        [SerializeField] private Image WarningSign;
        [SerializeField] private TMP_Text WarningSubject;
        [SerializeField] private TMP_Text WarningSolution;
        private void Awake()
        {
            Disable();
        }
        public void ShowWarning(Warning warning)
        {
            WarningPanelObj.SetActive(true);

            WarningSubject.text = warning.Subject;
            WarningSolution.text = warning.Solution;
            WarningSign.sprite = warning.Sign;
        }
        public void Disable() => WarningPanelObj.SetActive(false);
    }

    public class Warning
    {
        public string Subject;
        public string Solution;
        public Sprite Sign;
        public Warning(string Subject, string Solution, Sprite Sign)
        {
            this.Subject = Subject;
            this.Solution = Solution;
            this.Sign = Sign;
        }
    }
}