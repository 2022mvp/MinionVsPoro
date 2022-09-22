using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton 패턴 적용
    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public GameObject objSelected;
    public HashSet<CHexTile> hsetTilesInRange;

    private void Start()
    {
        hsetTilesInRange = new HashSet<CHexTile>();
    }

    public void ClickHexTile(CHexTile cClickedTile)
    {
        if (objSelected == null)                         // 이전에 아무것도 선택하지 않은 경우
        {
            if (cClickedTile.GetUnit() != null && cClickedTile.GetUnit().sStatus.group == 0)   // 아군인 경우 (임시지정 0 == Ally, 1 이상은 Enemy)
            {
                SelectHexTile(cClickedTile);
            }
        }
        else
        {
            GameManager.Instance.cHexTileMap.ResetAllTilesColor();

            if (objSelected.GetComponent<CHexTile>() != null)    // 이전에 타일을 선택한 경우
            {
                if (cClickedTile.IsUnitOn())                        // 선택한 타일에 유닛이 있을 경우
                {
                    CUnit cUnitOnClickedTile = cClickedTile.GetUnit();
                    if (cUnitOnClickedTile.sStatus.group == 0)                  // 아군인 경우 (임시지정 0 == Ally, 1 이상은 Enemy)
                    {
                        ResetSelect();
                        SelectHexTile(cClickedTile);
                        return;
                    }
                    else                                                // 적군인 경우
                    {
                        objSelected.GetComponent<CHexTile>().GetUnit().AttackUnit(cClickedTile.GetUnit());
                        if (cClickedTile.GetUnit() == null)
                        {
                            cClickedTile.SetUnit(null);
                        }
                    }
                }
                else                                                // 선택한 타일에 유닛이 없을 경우
                {
                    if (hsetTilesInRange.Contains(cClickedTile))
                    {
                        CHexTile cSelectedTile = objSelected.GetComponent<CHexTile>();
                        CUnit cUnit = cSelectedTile.GetUnit();
                        cSelectedTile.SetUnit(null);
                        cUnit.MoveUnit(cClickedTile.transform.position);
                        cClickedTile.SetUnit(cUnit);
                    }
                }
            }

            ResetSelect();
        }
    }

    private void SelectHexTile(CHexTile cClickedTile)
    {
        objSelected = cClickedTile.gameObject;
        cClickedTile.ChangeColor(GameManager.Instance.arTileColors[(int)EHexTileColor.Selected]);

        if (cClickedTile.IsUnitOn())
        {
            int nAtkRange = cClickedTile.GetUnit().sStatus.attack_range;
            cClickedTile.SearchTileInRange(hsetTilesInRange, nAtkRange);
            hsetTilesInRange.Remove(cClickedTile);
            ChangeTilesInRangeColor();
        }
    }

    private void ResetSelect()
    {
        objSelected = null;
        ResetTilesInRange();
    }

    private void ChangeTilesInRangeColor()
    {
        foreach (CHexTile cHexTile in hsetTilesInRange)
        {
            if (cHexTile.IsUnitOn())
            {
                if (cHexTile.GetUnit().sStatus.group == 0)
                {
                    cHexTile.ChangeColor(GameManager.Instance.arTileColors[(int)EHexTileColor.Ally]);
                }
                else
                {
                    cHexTile.ChangeColor(GameManager.Instance.arTileColors[(int)EHexTileColor.Enemy]);
                }
            }
            else
            {
                cHexTile.ChangeColor(GameManager.Instance.arTileColors[(int)EHexTileColor.Move]);
            }
        }
    }

    private void ResetTilesInRange()
    {
        foreach (CHexTile cHexTile in hsetTilesInRange)
        {
            cHexTile.InitColor();
        }
        hsetTilesInRange.Clear();
    }

    private void SelectCard(CCard cClickedCard)
    {
        objSelected = cClickedCard.gameObject;
    }
}