using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid <T>
{
    private int width, height;
    private float cellSize;
    private Vector3 originPosition;
    public T[,] gridArray;
    //private TextMesh[,] textMeshArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<T> createObject) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new T[width, height];
        // text information for debugging purposes
        //textMeshArray = new TextMesh[width, height];

        for (int i = 0; i < gridArray.GetLength(0); i++) {
            for (int j = 0; j < gridArray.GetLength(1); j++) {
                gridArray[i, j] = createObject();
            }
        }

        for (int i = 0; i < gridArray.GetLength(0); i++) {
            for(int j = 0; j < gridArray.GetLength(1); j++) {
                //textMeshArray[i, j] = UtilsClass.CreateWorldText(gridArray[i, j].ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int i, int j) {
        return new Vector3(i, j) * cellSize + originPosition;
    }

    public void GetCoordinates(Vector3 worldPosition, out int i, out int j) {
        i = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        j = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int i, int j, T value) {
        if(i >= 0 && j >= 0 && i < width && j < height) {
            gridArray[i, j] = value;
            //textMeshArray[i, j].text = gridArray[i, j].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, T value) {
        int i, j;
        GetCoordinates(worldPosition, out i, out j);
        SetValue(i, j, value);
    }

    public T GetValue(int i, int j) {
        if (i >= 0 && j >= 0 && i < width && j < height) {
            return gridArray[i, j];
        }
        else {
            return default(T);
        }
    }

    public T GetValue(Vector3 worldPosition) {
        int i, j;
        GetCoordinates(worldPosition, out i, out j);
        return GetValue(i, j);
    }
}
