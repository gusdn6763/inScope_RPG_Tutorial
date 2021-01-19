using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public interface IClickable
{
    Image MyIcon
    {
        get;
        set;
    }

    int MyCount{ get; }

    Text MyStackText { get; }
}