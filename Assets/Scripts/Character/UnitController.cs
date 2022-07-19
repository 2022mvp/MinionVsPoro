using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public UnitBlueprint status;
    [HideInInspector]
    public GameObject tile;

    private bool info = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(status.hp <= 0)
        {
            //tile.transform.GetComponent<HexTile>().ShowRange(tile.transform.GetComponent<HexTile>().originalMat);
            Destroy(gameObject);
        }
    }

}
