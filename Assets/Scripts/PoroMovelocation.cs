using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoroMovelocation : MonoBehaviour
{
	Ray _ray;
	RaycastHit _hit;
	Vector3 _mousePoint;
	int _Clickcount;

	public MeshRenderer _OriginColor;
	public Material _changeMaterial;
	
	List<Collider> HitList = new List<Collider>();

	void Start()
	{
		_OriginColor = _OriginColor.GetComponentsInChildren<MeshRenderer>()[1].GetComponent<MeshRenderer>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			_Clickcount += 1;
			OnClick();
		}
	}

	public void OnClick()
	{
		Physics.Raycast(_ray, out _hit);

		Collider[] hitColliders;
		//_hit.transform.GetComponentsInChildren<MeshRenderer>()[1].GetComponent<MeshRenderer>().material.color = _changeMaterial.color;
		//ºÎµúÈù ¿ÀºêÁ§Æ®ÀÇ »ö±ò º¯°æ
		if (_hit.transform.CompareTag("Tile"))
		{
			hitColliders = Physics.OverlapSphere(_hit.transform.position, 3.0f);
			for (int i = 0; i < hitColliders.Length; i++)
			{
				if (hitColliders[i].name != _hit.transform.name)
				{
					hitColliders[i].GetComponentsInChildren<MeshRenderer>()[1].GetComponent<MeshRenderer>().material.color = _changeMaterial.color;
					HitList.Add(hitColliders[0]);
					HitList.Add(hitColliders[i]);
				}
				//if (!HitList.Count.Equals(0) && Input.GetMouseButtonDown(0)) return;
				//{
				//	_Clickcount += 1;
				//	if (HitList[0].Equals(hitColliders[0]) && _Clickcount.Equals(2))
				//	{
				//		HitList[i].transform.GetComponentsInChildren<MeshRenderer>()[1].GetComponent<MeshRenderer>().material.color = _OriginColor.material.color;
				//		_Clickcount = 0;
				//	}
				//}
			}
			Debug.Log(hitColliders);
			Debug.Log(HitList[0] + "aaaa");
			Debug.Log(hitColliders[0] + "bbbb");
		}
	}
}
