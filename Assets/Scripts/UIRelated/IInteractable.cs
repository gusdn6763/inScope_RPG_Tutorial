using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어와 상호작용이 가능한 오브젝트들, 상호작용 시작, 상호작용 금지
/// </summary>
public interface IInteractable
{
    void Interact();

    void StopInteract();

}
