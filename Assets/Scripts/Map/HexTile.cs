using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{

    public Material previewMat;
    private Material originalMat;

    public GameObject unit = null;

    private Vector3 spawnPointOffset;
    private bool preview = true;

    private List<Transform> tiles = new List<Transform>();

    bool info = false;

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
                    GameManager.instance.selectTile.GetComponent<HexTile>().UnitMove(unit);
                    ResetTileState();
                }
                else if(GameManager.instance.selectTile.GetComponent<HexTile>().unit.tag == "Enemy")
                {
                    GameManager.instance.selectTile.GetComponent<HexTile>().Attack(unit.GetComponent<UnitController>().status.damage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), 3f);

    }

    void OnMouseOver() //?��?? ?�ע�������?? ���Ң�?���� ??��?���� ??��? ��?���稬����?��? ??��? ??����
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

    private void OnMouseExit() //��?���稬����? ?����?
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

    public void ShowPreview(bool show) //��?���稬����? ������???
    {
        Material mat;

        //TODO : �������� ��?���稬����? ????��? ���Ң�?���� ?�� ?��?? ��?�Ʃ���?, ��?��??�ר��� ��?��? ?? ��?��? ??����?? ������? ?�� ����?�� ??���� ???? ���Ң�?������? ��?��?��? ??��? ��?��?, ������?��?��? ?��?������ ��?�Ʃ�??��?.
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
                Destroy(unit);
            }
        }
        ShowRange(mat);
    }

    public void ShowInfo(bool info) //??��? ?������ UI ������???
    {
        Material mat;

        if (info)
        {
            mat = previewMat; //?? ����?��
            //??��? ?������ UI ��?��?��?
        }
        else
        {
            mat = originalMat;
            //??��? ?������ UI ��?��?
        }
        ShowRange(mat);
    }

    public void ShowRange(Material mat) //?��?? ?�� ??��?���� ??��? ������???
    {
        foreach (Transform tile in tiles)
        {
            if (tile != this)
            {
                tile.GetComponent<MeshRenderer>().material = mat;
            }
        }
    }

    public void UnitInstance(GameObject model) //??��? ??���� ������???
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
            if (collider.tag == "Tile" && collider != this)
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

    public void UnitMove(GameObject unit_) //"???�� �Ƣ���??? ��??��"��? ?���Ƣ�??��? ???? ��?��?��? ??��?��??? ?????? ??��?��?.
    {
        unit = unit_;
        unit.transform.position = transform.position + spawnPointOffset;
        UpdateTileList(unit.GetComponent<UnitController>().status.attack_range);
        preview = false;
    }

    public void Attack(int damage)
    {
        unit.GetComponent<UnitController>().status.hp -= damage;
    }
}