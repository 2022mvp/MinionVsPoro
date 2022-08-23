using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public float speed;
    private Vector3 cardPosition;

    private Image cardImage;
    public CardBlueprint status;
    public GameObject preview;

    // Start is called before the first frame update
    void Start()
    {
        cardImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewPreview()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Tile");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
        {
            if (preview == null)
            {
                preview = Instantiate(status.model, hit.transform.position + new Vector3(0.0f, 0.6f, 0.0f), hit.transform.rotation);
            }
            preview.transform.position = hit.transform.position + new Vector3(0.0f, 0.6f, 0.0f);
            cardImage.enabled = false;
        }
        else
        {
            cardImage.enabled = true;
            Destroy(preview);
            preview = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.selectCard = transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.instance.selectCard == this.transform)
        {
            ViewPreview();
            transform.position = eventData.position;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.instance.selectCard == this.transform)
        {
            cardPosition = transform.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cardImage.enabled = true; //ī�� ��� �� ���� �κ�(����� �ٽ� ���̵��� �����Ͽ���)
        iTween.MoveTo(gameObject, cardPosition, 0.5f);
        Destroy(preview);
        preview = null;
    }
}
