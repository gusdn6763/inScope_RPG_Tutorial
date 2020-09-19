using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Player player = null;
    private NPC currentTarget;

    void Update()
    {
        ClickTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.zero, Mathf.Infinity, 512);

            if (hit.collider != null)
            {
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = hit.collider.GetComponent<NPC>();
                player.MyTarget = currentTarget.Select();
                UIManager.instance.ShowTargetFrame(currentTarget);
            }
            else
            {
                UIManager.instance.HideTargetFrame();
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = null;
                player.MyTarget = null;
            }
        }
    }
}