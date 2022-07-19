using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{

    public Material previewMat;
    public Material enemyMat;
    private Material originalMat;

    public GameObject unit = null;

    private Vector3 spawnPointOffset;
    private bool preview = true;

    private List<Transform> tiles = new List<Transform>();

    public bool info = false;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.selectTile != transform && info && Input.GetMouseButtonDown(0) && unit)
        {
            if (GetTileInRange(GameManager.instance.selectTile, info))
            {
                ShowInfo(false);
                if(GameManager.instance.selectTile.GetComponent<HexTile>().unit == null)
                {
                    ShowInfo(false);
                    GameManager.instance.selectTile.GetComponent<HexTile>().UnitMove(unit);
                    ResetTileState();
                }
                else if(GameManager.instance.selectTile.GetComponent<HexTile>().unit.tag == "Enemy")
                {
                    GameManager.instance.selectTile.GetComponent<HexTile>().ShowInfo(false);
                    GameManager.instance.selectTile.GetComponent<HexTile>().Attack(unit.GetComponent<UnitController>().status.damage);
                    unit.transform.GetComponent<UnitController>().status.hp -= GameManager.instance.selectTile.GetComponent<HexTile>().unit.transform.GetComponent<UnitController>().status.damage;
                }
            }
        }
        if(Input.GetMouseButtonDown(0) && GameManager.instance.selectTile != transform && info)
        {
            info = false;
            ShowInfo(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), 3f);

    }

    void OnMouseOver() //?¸?? ?§¿¡¼­?? ¸¶¿?½º ??·?¿¡ ??¶? ¹?¸®º¸±?¿? ??´? ??¼º
    {
        if (GameManager.instance.selectCard != null)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                if (preview)
                {
                    //ShowPreview(true);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                UnitInstance(GameManager.instance.selectCard.GetComponent<Card>().status.model);
            }
        }
    }

    private void OnMouseExit() //¹?¸®º¸±? ?¾·?
    {
        if (GameManager.instance.selectCard != null && preview)
        {
            //ShowPreview(false);
        }
    }

    private void OnMouseDown()
    {
        GameManager.instance.selectTile = transform;
        if (preview == false)
        {
            info = !info;
            ShowInfo(info);
        }
    }

    private void Setup()
    {
        originalMat = transform.GetComponent<MeshRenderer>().material;
        spawnPointOffset = new Vector3(0.0f, 0.6f, 0.0f);
    }

    public void ResetTileState()
    {
        unit = null;
        preview = true;
        info = false;
    }

    public void ShowPreview(bool show) //¹?¸®º¸±? ¸Þ¼???
    {
        Material mat;

        //TODO : ¾Æ·¡ ¹?¸®º¸±? ????¸? ¸¶¿?½º ?§ ?¸?? º?°æ½?, ¿?º??§Æ® Æ?±? ?? ´?½? ??¼º?? ¾Æ´? ?¹ ¹ø?° ??¼º ???? ¸¶¿?½º¸? ¶?´?¸? ??´? Æ?±?, ¾Æ´?¶?¸? ?§?¡¸¸ º?°æ??´?.
        if (show)
        {
            mat = previewMat;
            if (GameManager.instance.unit == null)
            {
                UpdateTileList(GameManager.instance.selectCard.GetComponent<Card>().status.attack_range);
                GameObject obj = Instantiate(GameManager.instance.selectCard.GetComponent<Card>().status.model, transform.position + spawnPointOffset, transform.rotation);
                obj.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[1].color = GameManager.instance.selectCard.GetComponent<Card>().status.previewColor;
                unit = obj;
            }
        }
        else
        {
            mat = originalMat;
            if (Input.GetMouseButtonUp(0))
            {
                ShowRange(mat);
                Destroy(unit);
            }
        }
        ShowRange(mat);
    }

    public void ShowInfo(bool info) //??´? ?¤º¸ UI ¸Þ¼???
    {
        Material mat;

        if (info)
        {
            mat = previewMat; //?? ¼³?¤
            //??´? ?¤º¸ UI ¶?¿?±?
        }
        else
        {
            mat = originalMat;
            //??´? ?¤º¸ UI ²?±?
        }
        ShowRange(mat);
    }

    public void ShowRange(Material mat) //?¸?? ?§ ??°?¸® ??½? ¸Þ¼???
    {
        foreach (Transform tile in tiles)
        {
            if (tile != this)
            {
                if(tile.GetComponent<HexTile>().unit != null && tile.GetComponent<HexTile>().unit.tag == "Enemy" && mat == previewMat)
                {
                    tile.GetComponent<HexTile>().SetColor(enemyMat);
                }
                else
                {
                    tile.GetComponent<HexTile>().SetColor(mat);
                }
            }
        }
    }

    public void SetColor(Material mat)
    {
        GetComponent<MeshRenderer>().material = mat;
    }

    public void UnitInstance(GameObject model) //??´? ??¼º ¸Þ¼???
    {
        UpdateTileList(model.GetComponent<UnitController>().status.attack_range);
        preview = false;
        //ShowPreview(false);
        unit = Instantiate(model, transform.position + spawnPointOffset, transform.rotation);
        GameManager.instance.selectCard = null;
    }

    public void UpdateTileList(int range)
    {
        tiles.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f * range);


        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Tile")
            {
                tiles.Add(collider.transform);
            }
        }
    }

    public bool GetTileInRange(Transform tile_, bool info)
    {
        foreach (Transform tile in tiles)
        {
            if (tile == tile_ && info)
            {
                return true;
            }
        }
        return false;
    }

    public void UnitMove(GameObject unit_) //"???¿ °¡´??? ¹??§"¸? ?ß°¡??¿? ???? ´?¸?°? ??´?°??? ?????? ??´?´?.
    {
        unit = unit_;
        unit.transform.position = transform.position + spawnPointOffset;
        preview = false;
        UpdateTileList(unit.GetComponent<UnitController>().status.attack_range);
    }

    public void Attack(int damage)
    {
        unit.GetComponent<UnitController>().status.hp -= damage;
        Debug.Log("남은 체력 : " + unit.GetComponent<UnitController>().status.hp);
    }
}