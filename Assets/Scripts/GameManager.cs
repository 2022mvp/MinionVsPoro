using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton 패턴 적용
    private static GameManager _instance;
    public static GameManager Instance
    {
        get{ return _instance; }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        Init();
    }

    private CPlayer _cPlayer;
    public CPlayer cPlayer
    {
        get { return _cPlayer; }
        set { _cPlayer = value; }
    }

    private CHexTileMap _cHexTileMap;
    public CHexTileMap cHexTileMap
    {
        get { return _cHexTileMap; }
        set { _cHexTileMap = value; }
    }

    private Color[] _arTileColors;
    public Color[] arTileColors
    {
        get { return _arTileColors; }
        set { _arTileColors = value; }
    }

    private void Init()
    {
        InitPlayer();
        InitMap();
        InitTileColors();
    }

    private void InitPlayer()
    {
        if(_cPlayer == null)
        {
            _cPlayer = GameObject.Find("Player").GetComponent<CPlayer>();
        }
    }

    private void InitMap()
    {
        if (_cHexTileMap == null)
        {
            _cHexTileMap = GameObject.Find("HexagonTileMap").GetComponent<CHexTileMap>();
        }
    }

    private void InitTileColors()
    {
        arTileColors = new Color[(int)EHexTileColor.Max];
        arTileColors[(int)EHexTileColor.Default] = Color.white;
        arTileColors[(int)EHexTileColor.Move] = Color.yellow;
        arTileColors[(int)EHexTileColor.Enemy] = Color.red;
        arTileColors[(int)EHexTileColor.Ally] = Color.green;
        arTileColors[(int)EHexTileColor.OnMouse] = Color.cyan;
        arTileColors[(int)EHexTileColor.Selected] = Color.blue;
    }
}
