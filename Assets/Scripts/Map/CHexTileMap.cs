using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHexTileMap : MonoBehaviour
{
	public GameObject _HexTilePrefab;
	public int _col_Size = 8, _row_Size = 8;

	float _Hex_X = 3, _Hex_Z = 2.6f;
	float _NextCol = 1.5f;

	GameObject[,] _HexTile = new GameObject[100, 100];
	Vector3 _HexTileTransform;

	GameObject _ObjectTile;
	int[,] _ObjectTileArray = new int[10, 10];


	private void Awake()
	{
		//Debug.Log(_ObjectTileArray);
		//_HexTilePrefab.SetActive(false);
		for (int col = 0; col < _col_Size; col++)
		{
			for (int row = 0; row < _row_Size; row++)
			{
				_HexTile[col, row] = Instantiate(_HexTilePrefab, MoveCreateHexTransform(col, row), Quaternion.identity, this.transform);
				//_HexTile[col_rand, row_rand] = Instantiate(_ObjectTile);
			}
		}
	}

	Vector3 MoveCreateHexTransform(int col, int row)
	{
		_HexTileTransform.x += _Hex_X;

		if (row == 0)
		{
			if (col != 0)
				_HexTileTransform.z += _Hex_Z;
			if (col % 2 == 1)
				_HexTileTransform.x = _NextCol;
			else if (col % 2 == 0)
				_HexTileTransform.x = 0.0f;
		}
		return _HexTileTransform;
	}

	void RandomObjTile()
	{
		int col_rand = Random.Range(1, _col_Size - 1);
		int row_rand = Random.Range(0, _row_Size);
		//for (int i = 0; i < _ObjectTileArray.Length; i++)
		//{
		//	_ObjectTileArray

		//}
	}

	public void ResetAllTilesColor()
	{
		for (int col = 0; col < _col_Size; col++)
		{
			for (int row = 0; row < _row_Size; row++)
			{
				_HexTile[col, row].GetComponent<CHexTile>().InitColor();
			}
		}
	}
}
