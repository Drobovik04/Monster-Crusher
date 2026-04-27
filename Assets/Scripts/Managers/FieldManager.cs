using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Tile groundTile;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Vector2Int leftBottomCorner;

    public Vector2 CenterOfField { get; private set; }

    public void GenerateTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap.SetTile(new Vector3Int(leftBottomCorner.x + x, leftBottomCorner.y + y, 0), groundTile);
            }
        }

        CenterOfField = leftBottomCorner + new Vector2((float)width / 2, (float)height / 2);
    }

    public Vector3 ClampInsideField(Vector3 pos)
    {
        float minX = leftBottomCorner.x;
        float maxX = leftBottomCorner.x + width;

        float minY = leftBottomCorner.y;
        float maxY = leftBottomCorner.y + height;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        return pos;
    }

    public void BuildBorders(float thickness = 1f)
    {
        float left = leftBottomCorner.x;
        float bottom = leftBottomCorner.y;
        float right = leftBottomCorner.x + width;
        float top = leftBottomCorner.y + height;

        // ËÅÂÀß
        CreateWall(new Vector2(left - thickness / 2, bottom + height / 2), new Vector2(thickness, height));

        // ÏÐÀÂÀß
        CreateWall(new Vector2(right + thickness / 2, bottom + height / 2), new Vector2(thickness, height));

        // ÍÈÆÍßß
        CreateWall(new Vector2(left + width / 2, bottom - thickness / 2), new Vector2(width, thickness));

        // ÂÅÐÕÍßß
        CreateWall(new Vector2(left + width / 2, top + thickness / 2), new Vector2(width, thickness));
    }

    public PolygonCollider2D BuildCameraBounds()
    {
        GameObject camBoundsObj = new GameObject("CameraBounds");
        camBoundsObj.transform.parent = transform;

        var poly = camBoundsObj.AddComponent<PolygonCollider2D>();

        Vector2 lb = leftBottomCorner;
        Vector2 rt = leftBottomCorner + new Vector2(width, height);

        poly.points = new Vector2[]
        {
            new Vector2(lb.x, lb.y),
            new Vector2(lb.x, rt.y),
            new Vector2(rt.x, rt.y),
            new Vector2(rt.x, lb.y)
        };

        poly.isTrigger = true;

        return poly;
    }

    private void CreateWall(Vector2 pos, Vector2 size)
    {
        GameObject wall = new GameObject("BorderWall");
        wall.transform.parent = transform;
        wall.transform.position = pos;

        BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
        col.size = size;
    }

}
