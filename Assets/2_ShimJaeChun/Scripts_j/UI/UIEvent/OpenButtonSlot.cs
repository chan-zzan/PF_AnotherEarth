using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenButtonSlot : MonoBehaviour
{
    [Header("리스트가 닫혀있는지? (체크:true)")]
    public bool isCloseList;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();    
    }


    private void OnDisable()
    {
        anim.SetBool("IsClicked", false);
        anim.SetBool("IsClosed", true);
    }

    public void OnClickSlotButton()
    {
        anim.SetBool("IsClicked", true);

        if(anim.GetBool("IsClosed"))
        {
            anim.SetBool("IsClosed", false);
        }
        else
        {
            anim.SetBool("IsClosed", true);
        }

    }
}
