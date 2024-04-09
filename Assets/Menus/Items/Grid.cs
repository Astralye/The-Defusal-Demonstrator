using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 origin;
    private string name;
    public ItemList.Items[,] gridArray;

    RectTransform gridObject;

    public Grid(int width, int height, int cellSize, Vector3 origin, string objName)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        this.name = objName;

        gridArray = new ItemList.Items[width, height];

        SetPosition();

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for(int y = 0; y < gridArray.GetLength(1); y++){
                gridArray[x, y] = ItemList.Items.None;
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * cellSize + origin;
    }

    public Vector2Int getXY(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt((worldPos - origin).x / cellSize),
            Mathf.FloorToInt((worldPos - origin).y / cellSize));
    }

    public bool validPosition(Vector2 position)
    {
        if (position.x >= 0 && position.x <= ( width - 1) && position.y >= 0 && position.y <= ( height -1))
            return true;
        return false;
    }

    public void SetPosition()
    {
        gridObject = GameObject.Find(name).GetComponent<RectTransform>();
        gridObject.sizeDelta = new Vector3(width * cellSize, height * cellSize, 0);
        gridObject.transform.position = origin + new Vector3((width * cellSize) / 2, (height * cellSize) / 2, 0);
    }

    public float getWidth()
    {
        return width;
    }

    public float getHeight()
    {
        return height;
    }
}
