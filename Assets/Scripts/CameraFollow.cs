﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    float xMax, xMin, yMax, yMin;

    [SerializeField] Tilemap tilemap;

    private Player player;

    private void Start()
    {
        // Hierarchy 뷰에서 Tag가 Player 인 객체를 찾는다.
        target = GameObject.FindGameObjectWithTag("Player").transform;

        player = target.GetComponent<Player>();

        // 타일 좌표가 가장 낮은것과 가장 높은것의 Vector3 값을 찾는다.
        Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);
        Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);

        SetLimits(minTile, maxTile);
        player.SetLimits(minTile, maxTile);
    }

    private void LateUpdate()
    {
        float minClamp = Mathf.Clamp(target.position.x, xMin, xMax);
        float maxClamp = Mathf.Clamp(target.position.y, yMin, yMax);

        transform.position = new Vector3(minClamp, maxClamp, -10);
    }

    // 카메라의 이동범위를 정합니다.
    private void SetLimits(Vector3 minTile, Vector3 maxTile)
    {
        Camera cam = Camera.main;

        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        xMin = minTile.x + width / 2;
        xMax = maxTile.x - width / 2;

        yMin = minTile.y + height / 2;
        yMax = maxTile.y - height / 2;
    }

}
