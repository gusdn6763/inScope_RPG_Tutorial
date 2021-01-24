using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event KillConfirmed killConfirmedEvent;

    [SerializeField] private Player player = null;
    [SerializeField] private LayerMask clickableLayer, groundLayer;

    private Enemy currentTarget;
    private Camera mainCamera;

    private int targetIndex;

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
        mainCamera = Camera.main;
    }

    void Update()
    {
        ClickTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                DeSelectTarget();

                SelectTarget(hit.collider.GetComponent<Enemy>());
            }
            else
            {
                UIManager.instance.HideTargetFrame();

                DeSelectTarget();

                currentTarget = null;
                player.Target = null;
            }
        }
        //마우스 오른쪽 클릭하고, UI가 아닐시  
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();
                if (hit.collider != null && (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Interactable") && player.MyInteractable.Contains(entity)))
                {
                    entity.Interact();
                }
            }
            else
            {
                hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, groundLayer);
            }
        }
    }


    private void DeSelectTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.DeSelect();
        }
    }

    private void SelectTarget(Enemy enemy)
    {
        currentTarget = enemy;
        player.MyTarget = currentTarget.Select();
        UIManager.instance.ShowTargetFrame(currentTarget);
    }

    /// <summary>
    /// 몹이 중쳡되어있을시 차례대로 몹을 클릭할 수 있게함
    /// </summary>
    private void NextTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeSelectTarget();
            if (Player.instance.MyAttackers.Count > 0)
            {
                if (targetIndex < Player.instance.MyAttackers.Count)
                {
                    SelectTarget(Player.instance.MyAttackers[targetIndex]);
                    targetIndex++;
                    if (targetIndex >= Player.instance.MyAttackers.Count)
                    {
                        targetIndex = 0;
                    }
                }
                else
                {
                    targetIndex = 0;
                }
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