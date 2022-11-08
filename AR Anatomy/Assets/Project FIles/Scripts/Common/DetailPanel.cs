using TMPro;
using UnityEngine;

public class DetailPanel : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Animator Detail_PanelAnim;
    [SerializeField] private TMP_Text TitleTxt, DetailTxt;
#pragma warning restore 649
    public void PopUp(bool show)=> Detail_PanelAnim.SetBool("Show", show);

    public void SetMessage(string Title, string Info)
    {
        TitleTxt.text = Title;
        DetailTxt.text = Info;
    }
}
