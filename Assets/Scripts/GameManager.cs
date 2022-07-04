using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;

    public static GameManager instance;

    public Transform selectCard = null;

    public Transform selectTile = null;

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
        if (Input.GetKeyDown(KeyCode.A))
        {
            EnemySpawn();
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

    public void EnemySpawn()
    {
        if (selectTile.GetComponent<HexTile>().unit == null)
        {
            selectTile.GetComponent<HexTile>().UnitInstance(enemy);
        }
    }
}
