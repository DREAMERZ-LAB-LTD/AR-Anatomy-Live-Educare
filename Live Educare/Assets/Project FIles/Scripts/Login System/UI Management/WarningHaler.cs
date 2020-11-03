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

        [Header("Warning Sign")]
        [SerializeField] private Sprite Connnection_Sign;
        [SerializeField] private Sprite PasswordIncorrect_Sign;
        [SerializeField] private Sprite CanNotRegister_Sign;
#pragma warning restore 649
        private void Awake()
        {
            OkayBtn.onClick.RemoveAllListeners();
            OkayBtn.onClick.AddListener(Disable);
            Disable();
        }
        public void ConnnectionError()
        {
            Warning warniing = new Warning("No internet connection", "Please make sure that wifi or mobile data", Connnection_Sign);
            ShowWarning(warniing);
        }
        public void PasswordError()
        {
            Warning warniing = new Warning("Your username or passrword does not match",
                                          "Please try again with proper credential",
                                          PasswordIncorrect_Sign);
            ShowWarning(warniing);
        }

        public void RegisterError(string message)
        {
            Warning warniing = new Warning("Can not register",message, CanNotRegister_Sign);
            ShowWarning(warniing);
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