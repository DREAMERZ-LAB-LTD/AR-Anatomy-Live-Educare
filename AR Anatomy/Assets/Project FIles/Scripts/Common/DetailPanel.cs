using TMPro;
using UnityEngine;

public class DetailPanel : MonoBehaviour
{

    [SerializeField] private Animator Detail_PanelAnim;
    [SerializeField] private TMP_Text TitleTxt, DetailTxt;

    public void PopUp(bool show)=> Detail_PanelAnim.SetBool("Show", show);

    public void SetMessage(string Title, string Info)
    {
        TitleTxt.text = Title;
        DetailTxt.text = Info;
    }
}
