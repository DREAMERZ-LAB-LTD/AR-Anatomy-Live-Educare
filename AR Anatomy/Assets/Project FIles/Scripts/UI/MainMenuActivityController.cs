using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.UI;

public class MainMenuActivityController : UIManager
{


    [Header("Which Page Will Enable As Default")]
    [SerializeField] private int DefaultActivityIndx = -1;
    
    [Header("Which Page Will Enable After Back From AR scene")]
    [SerializeField] private int BackFromARpageNo = -1;

    public static bool backFromARscene = false;



    private void OnEnable()
    {
        if (backFromARscene)
        {
            //this page will active when we will back from ar sceen
            SwitchMenuScene(BackFromARpageNo);
            backFromARscene = false;
        }
        else
        {
            //this page will active as default page of the menu scene
            SwitchMenuScene(DefaultActivityIndx);
           
        }
     
    }
}


