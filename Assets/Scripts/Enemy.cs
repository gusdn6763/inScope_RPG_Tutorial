using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup;

    public override Transform Select()
    {
        //Shows the health bar
        healthGroup.alpha = 1;
       return base.Select();
    }


    public override void DeSelect()
    {
        //Hides the healthbar
        healthGroup.alpha = 0;

        base.DeSelect();
    }
}