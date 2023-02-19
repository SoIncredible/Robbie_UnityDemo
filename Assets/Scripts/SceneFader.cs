using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    Animator anim;
    int FaderID;
    private void Start()
    {
        anim = GetComponent<Animator>();
        FaderID = Animator.StringToHash("Fade");
        //Debug.Log("SceneFader先执行的");
        // 只能说明这个函数没有执行成功
        GameManager.RegisteSceneFader(this);
        
    }

    public void FadeOut()
    {
        anim.SetTrigger(FaderID);
    }
}
