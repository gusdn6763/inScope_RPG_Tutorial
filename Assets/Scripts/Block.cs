using System;
using UnityEngine;

[Serializable]
public class Block
{
    [SerializeField] private GameObject first = null, second = null;

    
    public void activate(bool activate)
    {
        first.SetActive(activate);
        second.SetActive(activate);
    }


}
