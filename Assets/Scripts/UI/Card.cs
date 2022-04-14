using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float speed;
    private Color originalColor;
    private Vector3 cardPosition;
    private GameObject unit;
    private GameObject unitPreview;
    private GameObject isPreviw = null;
    public Color prevewColor;

    Transform spawnTile;
    Vector3 spawnPositionOffset;

    public UnitBlueprint status;

    // Start is called before the first frame update
    void Awake()
    {
        Setup();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnTile = GetTile();
        if(spawnTile != null){
            if(Input.GetMouseButton(0) && isPreviw == null){
                isPreviw = Instantiate(unitPreview, spawnTile.position + spawnPositionOffset, spawnTile.rotation); //드래그 시 유닛 미리보기 생성
                transform.GetComponent<Image>().color =new Color(0, 0, 0, 0);
            }
            else if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonDown(0))){
                GameObject unit_temp;
                unit.SetActive(true);
                unit_temp = Instantiate(unit, spawnTile.position + spawnPositionOffset, spawnTile.rotation); //유닛 소환
                unit_temp.AddComponent<SampleUnitController>().status = status; //유닛 스테이터스 적용
                unit_temp.GetComponent<SampleUnitController>().status.model = null;
                GameObject.Destroy(isPreviw);
                GameObject.Destroy(gameObject); //카드 제거
			}
            else if(isPreviw != null && Input.GetMouseButton(0))
            {
                isPreviw.transform.position = spawnTile.position + spawnPositionOffset;
            }
            else{
                transform.GetComponent<Image>().color = originalColor;
                GameObject.Destroy(isPreviw);
            }
		}
        else
        {
            transform.GetComponent<Image>().color = originalColor;
            GameObject.Destroy(isPreviw);
        }
    }

    public void Setup()
    {
        originalColor = transform.GetComponent<Image>().color;
        unit = status.model;
        unitPreview = unit;
        unitPreview.transform.GetChild(0).GetComponent<Renderer>().material.color = prevewColor;
        spawnPositionOffset = new Vector3(0.0f, 1.0f, 0.0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        cardPosition = transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        iTween.MoveTo(gameObject, cardPosition, 0.7f); //ToDo : 카드가 드래그 될 때의 위치를 저장, 이동시키기
    }

    public Transform GetTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Input.mousePosition, Camera.main.transform.forward * 8, Color.red);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray.origin, ray.direction);

        Transform target = null;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Tile")
            {
                target = hit.transform;
            }
        }
        return target;
    }
}
