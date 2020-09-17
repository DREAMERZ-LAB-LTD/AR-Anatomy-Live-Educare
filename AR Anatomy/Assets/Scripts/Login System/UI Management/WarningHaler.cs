using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WarningHaler : MonoBehaviour
    {
        [Header("Warning Panel Object Reference")]
        [SerializeField] private GameObject WarningPanelObj;

        [SerializeField] private Button OkayBtn;

        [Header("Reference Warning Panel Elements")]
        [SerializeField] private Image WarningSign;
        [SerializeField] private TMP_Text WarningSubject;
        [SerializeField] private TMP_Text WarningSolution;
        private void Awake()
        {
            OkayBtn.onClick.RemoveAllListeners();
            OkayBtn.onClick.AddListener(Disable);
            Disable();
        }
        public void ShowWarning(Warning warning)
        {
            WarningPanelObj.SetActive(true);

            WarningSubject.text = warning.Subject;
            WarningSolution.text = warning.Suggestion;
            WarningSign.sprite = warning.Sign;
        }
        public void Disable() => WarningPanelObj.SetActive(false);
    }

    public class Warning 
    {
        public string Subject;
        public string Suggestion;
        public Sprite Sign;
        public Warning(string Subject, string Suggestion, Sprite Sign)
        {
            this.Subject = Subject;
            this.Suggestion = Suggestion;
            this.Sign = Sign;


        }
    }
}