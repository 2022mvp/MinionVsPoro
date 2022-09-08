using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SCardStatus sCardStatus;
    public GameObject preview;
    public GameObject prefabUnit;

    private Vector3 cardPosition;
    private Image cardImage;
    private Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        cardImage = GetComponent<Image>();
        spawnPosition = new Vector3(0.0f, 0.6f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool RayCastTile(out RaycastHit hit)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Tile");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool IsTile = Physics.Raycast(ray, out RaycastHit rayHit, layerMask);
        hit = rayHit;
        return IsTile;
    }

    public void ViewPreview(RaycastHit hit)
    {
        if (preview == null)
        {
            preview = Instantiate(sCardStatus.model, hit.transform.position + spawnPosition, hit.transform.rotation);
        }
        preview.transform.position = hit.transform.position + spawnPosition;
        cardImage.enabled = false;
    }

    private void DestroyPreview()
    {
        cardImage.enabled = true;
        Destroy(preview);
        preview = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        cardPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RayCastTile(out RaycastHit hit))
        {
            ViewPreview(hit);
        }
        else
        {
            DestroyPreview();
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cardImage.enabled = true;
        transform.position = cardPosition;
        Destroy(preview);
        preview = null;

        if (RayCastTile(out RaycastHit hit))
        {
            // To do : 
            SpawnUnit(hit.collider.gameObject.GetComponent<CHexTile>());
        }
    }

    public void SpawnUnit(CHexTile tile)
    {
        GameObject unit = Instantiate(prefabUnit, tile.transform.position + spawnPosition, tile.transform.rotation);
        // To do : 유닛 능력치 세팅하기
        tile.SetUnit(unit.GetComponent<CUnit>());
    }
}