using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 클릭이 가능한 아이템의 대한 인터페이스, 아이콘과 갯수를 포함함
/// </summary>
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