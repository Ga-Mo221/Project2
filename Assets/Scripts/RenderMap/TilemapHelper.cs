using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapHelper : MonoBehaviour
{
    public List<Tilemap> tilemaps;

    public bool IsPointOverlappingTile(Vector3 point, float radius = 0.2f)
    {
        foreach (var tilemap in tilemaps)
        {
            if (IsCircleOverlappingTile(tilemap, point, radius))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsCircleOverlappingTile(Tilemap tilemap, Vector3 center, float radius)
    {
        if (tilemap == null || center == null)
            return false;

        Vector3Int minCell = tilemap.WorldToCell(center - new Vector3(radius, radius, 0));
        Vector3Int maxCell = tilemap.WorldToCell(center + new Vector3(radius, radius, 0));

        bool hit = false;

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(cellPos);
                if (tile == null) continue;

                Vector3 tilecenter = tilemap.GetCellCenterWorld(cellPos);
                float dist = Vector2.Distance(tilecenter, center);

                if (dist <= radius)
                {
                    hit = true;
                }
            }
        }

        return hit;
    }
}