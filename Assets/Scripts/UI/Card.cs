using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public float speed;
    private Vector3 cardPosition;

    public CardBlueprint status;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ViewPrewview();
    }

    public void ViewPrewview()
    {
        if (Input.GetMouseButton(0) && GameManager.instance.selectCard == this.transform)
        {
            transform.GetComponent<Image>().enabled = false;
        }
        else
        {
            transform.GetComponent<Image>().enabled = true;
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
        iTween.MoveTo(gameObject, cardPosition, 0.5f);
    }
}
