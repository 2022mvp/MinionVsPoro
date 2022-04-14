using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eHexTileDirection
{
    RightTop,
    Right,
    RightBottom,
    LeftBottom,
    Left,
    LeftTop,
    Max
}

public class cHexTile : MonoBehaviour
{
    cHexTile[] nearHexTiles;

    private void Init()
    {
        InitNearHexTiles();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitNearHexTiles()
    {
        nearHexTiles = new cHexTile[(int)eHexTileDirection.Max];
        for (int i = 0; i < (int)eHexTileDirection.Max; i++)
        {
            nearHexTiles[i] = null;
        }
    }

    private void SearchNearHexTile(eHexTileDirection direction)
    {

    }
}
