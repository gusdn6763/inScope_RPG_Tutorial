using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour
{
    public event KillConfirmed killConfirmedEvent;

    public static GameManager instance;

    [SerializeField] private Player player = null;

    private Enemy currentTarget;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        ClickTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())//If we click the left mouse button
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = hit.collider.GetComponent<Enemy>();
                player.Target = currentTarget.Select();
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
                player.Target = null;
            }
        }
        //마우스 오른쪽 클릭하고, UI가 아닐시  
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);


            Debug.Log(hit.collider);
            if (hit.collider != null && (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Interactable") && hit.collider.gameObject.GetComponent<IInteractable>() == player.MyInteractable))
            {
                player.Interact();
            }
        }
    }

    public void OnKillConfirmed(Character character)
    {
        if (killConfirmedEvent != null)
        {
            killConfirmedEvent(character);
        }
    }
}