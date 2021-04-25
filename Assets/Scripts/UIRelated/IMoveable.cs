using UnityEngine;
using UnityEditor;

/// <summary>
/// 드래그 할 수 있는 오브젝트를 자칭, 아이콘 필요
/// </summary>
public interface IMoveable
{
    Sprite MyIcon { get; }
}