using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player = null;


    private void Update()
    {
        ClickTarget();
    }

    private void ClickTarget()
    {
       
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log(EventSystem.current.IsPointerOverGameObject());
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider.CompareTag("Enemy"))
            {
                player.MyTarget = hit.transform;
            }
        }
    }
}
