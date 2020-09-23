using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    private SpriteRenderer parentRenderer;
    private List<Obstacle> obstacles = new List<Obstacle>();

    private void Awake()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeOut();
            SpriteRenderer oSpriteRenderer = o.spriteRenderer;
            if (obstacles.Count == 0 || oSpriteRenderer.sortingOrder - 1 < parentRenderer.sortingOrder)
            {
                parentRenderer.sortingOrder = oSpriteRenderer.sortingOrder - 1;
            }

            obstacles.Add(o);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeIn();
            obstacles.Remove(o);

            if (obstacles.Count == 0)
            {
                parentRenderer.sortingOrder = 200;
            }

            else
            {
                obstacles.Sort();
                parentRenderer.sortingOrder = obstacles[0].spriteRenderer.sortingOrder - 1;
            }
        }
    }
}