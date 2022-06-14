using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform selectCard = null;

    public Transform tile = null;

    public GameObject unit = null;

    int money = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selectCard != null && Input.GetMouseButtonDown(0))
        {
            tile = GetTile();
        }
    }

    public Transform GetTile()
    {
        RaycastHit rayHit;
        Transform temp = null;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit))
        {
            if(rayHit.transform.tag == "Tile")
            {
                temp = rayHit.transform;
            }
        }
        return temp;
    }
}
