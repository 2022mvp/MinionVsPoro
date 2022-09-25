using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EHexTileDirection
{
    RightTop,
    Right,
    RightBottom,
    LeftBottom,
    Left,
    LeftTop,
    Max
}

public class CHexTile : MonoBehaviour
{
    public CHexTile[] arNearTiles;

    private float nSideLength;
    private OutlineAvgNormal outline;
    private CUnit cUnit;
    private Color colorBefore;

    private void Init()
    {
        nSideLength = 0.58f;
        cUnit = null;

        InitOutline();
        InitColor();
        UpdateNearTiles();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {

    }

    private void InitOutline()
    {
        outline = this.GetComponent<OutlineAvgNormal>();
    }

    // 주변 타일 업데이트, 맵 생성 후 반드시 한번 호출
    public void UpdateNearTiles()
    {
        arNearTiles = new CHexTile[(int)EHexTileDirection.Max];
        for (int nDir = 0; nDir < (int)EHexTileDirection.Max; nDir++)
        {
            arNearTiles[nDir] = SearchNearTile((EHexTileDirection)nDir);
        }
    }

    private CHexTile SearchNearTile(EHexTileDirection direction)
    {
        Vector3 rayOrigin = this.transform.position;
        float rayRad = (30.0f + 60.0f * (int)direction + 180.0f) * Mathf.Deg2Rad;
        Vector3 rayDirection = new Vector3(-Mathf.Sin(rayRad), 0.0f, -Mathf.Cos(rayRad));

        Ray raySearch = new Ray(rayOrigin, rayDirection * nSideLength);
        RaycastHit raycastHit;

        if (Physics.Raycast(raySearch, out raycastHit))
        {
            return raycastHit.collider.gameObject.GetComponent<CHexTile>();
        }

        return null;
    }

    public void SetOutlineColor(Color color)
    {
        if (outline == null)
        {
            return;
        }

        outline.SetOutlineColor(color);
    }

    public void SetColorBefore(Color color)
    {
        colorBefore = color;
    }

    public void ChangeColor(Color color)
    {
        SetOutlineColor(color);
        SetColorBefore(color);
    }

    public void InitColor()
    {
        SetOutlineColor(GameManager.Instance.arTileColors[(int)EHexTileColor.Default]);
        colorBefore = GameManager.Instance.arTileColors[(int)EHexTileColor.Default];
    }

    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            OnMouseExit();
            return;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PlayerController.Instance.ClickHexTile(this);
        }
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        SetOutlineColor(GameManager.Instance.arTileColors[(int)EHexTileColor.OnMouse]);
    }

    private void OnMouseExit()
    {
        SetOutlineColor(colorBefore);
    }

    public bool IsUnitOn()
    {
        if (cUnit != null)
        {
            return true;
        }

        return false;
    }

    public CUnit GetUnit()
    {
        return cUnit;
    }

    public void SetUnit(CUnit unit)
    {
        cUnit = unit;
    }

    public void SearchTileInRange(HashSet<CHexTile> hsetTilesInRange, int nRange)
    {
        if (nRange <= 0)
        {
            return;
        }

        for (int nDir = 0; nDir < (int)EHexTileDirection.Max; nDir++)
        {
            if (arNearTiles[nDir] != null)
            {
                hsetTilesInRange.Add(arNearTiles[nDir]);
                arNearTiles[nDir].SearchTileInRange(hsetTilesInRange, nRange - 1);
            }
        }
    }
}
