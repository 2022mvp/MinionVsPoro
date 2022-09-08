using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton 패턴 적용
    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject newGameObj = new GameObject("_PlayerController");
                _instance = newGameObj.AddComponent<PlayerController>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public GameObject objSelected;

    public void ClickHexTile(CHexTile cClickedTile)
    {
        if(objSelected == null)
        {
            SelectHexTile(cClickedTile);
        }
        else
        {
            GameManager.Instance.cHexTileMap.ResetAllTilesColor();

            if(objSelected.GetType() == typeof(CHexTile))
            {

            }
            else if(objSelected.GetType() == typeof(CCard))
            {

            }

            objSelected = null;
        }
    }

    private void SelectHexTile(CHexTile cClickedTile)
    {
        objSelected = cClickedTile.gameObject;
        cClickedTile.ChangeColor(GameManager.Instance.arTileColors[(int)EHexTileColor.Selected]);

        if(cClickedTile.IsUnitOn())
        {
            int nAtkRange = cClickedTile.GetUnit().sStatus.attack_range;

        }
    }

    private void SelectCard(CCard cClickedCard)
    {
        objSelected = cClickedCard.gameObject;
    }
}


