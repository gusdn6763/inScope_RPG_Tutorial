using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocket : GearSocket
{
    private SpriteRenderer parentRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }
    private float currentY;

    //플레이어가 위를 바라볼시 지팡이가 가려져야 하기때문에 GearSocket를 상속받는 WeaponSocket를 작성함  
    public override void SetXAndY(float x, float y)
    {
        base.SetXAndY(x, y);

        if (currentY != y)
        {
            if (y == 1)
            {
                spriteRenderer.sortingOrder = parentRenderer.sortingOrder - 1;
            }
            else
            {
                spriteRenderer.sortingOrder = parentRenderer.sortingOrder + 5;
            }
        }
    }
}
