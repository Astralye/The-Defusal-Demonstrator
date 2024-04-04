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
    private int[,] gridArray;
    RectTransform gridObject;

    public Grid(int width, int height, int cellSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new int[width, height];

        SetPosition();

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for(int y = 0; y < gridArray.GetLength(1); y++){
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

    private Vector2 getXY(Vector3 worldPos)
    {
        Vector2 localCoordinates;
        localCoordinates.x = Mathf.FloorToInt((worldPos - origin).x / cellSize);
        localCoordinates.y = Mathf.FloorToInt((worldPos - origin).y / cellSize);
        
        return localCoordinates;
    }

    public void SetPosition()
    {
        gridObject = GameObject.Find("Grid").GetComponent<RectTransform>();
        gridObject.sizeDelta = new Vector3(width * cellSize, height * cellSize, 0);
        gridObject.transform.position = origin + new Vector3((width * cellSize) / 2, (height * cellSize) / 2, 0);
    }
}
