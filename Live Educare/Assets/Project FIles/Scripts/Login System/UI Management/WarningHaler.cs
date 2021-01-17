using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WarningHaler : MonoBehaviour
    {
#pragma warning disable 649
        [Header("Warning Panel Object Reference")]
        [SerializeField] private GameObject WarningPanelObj;

        [SerializeField] private Button OkayBtn;

        [Header("Reference Warning Panel Elements")]
        [SerializeField] private Image WarningSign;
        [SerializeField] private TMP_Text WarningSubject;
        [SerializeField] private TMP_Text WarningSolution;

        [Header("Warnings Data")]
        [SerializeField] private Warning connectionError;
        [SerializeField] private Warning passwordError;
        [SerializeField] private Warning registerError;
#pragma warning restore 649
        private void Awake()
        {
            OkayBtn.onClick.RemoveAllListeners();
            OkayBtn.onClick.AddListener(Disable);
            Disable();
        }
        public void ConnnectionError()
        {
            ShowWarning(connectionError);
        }
        public void PasswordError()
        {
            ShowWarning(passwordError);
        }
        public void RegisterError()
        {
            ShowWarning(registerError);
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

    [System.Serializable]
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