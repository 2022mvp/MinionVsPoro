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
    private bool info = false;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), 3f);

    }

    public void OnMouseDown()
    {
        if(!preview)
        {
            ShowInfo();
            
            UnitMove(unit.GetComponent<UnitController>().status.attack_range);
        }
    }

    void OnMouseOver() //Ÿ�� �������� ���콺 �Է¿� ���� �̸������ ���� ����
    {
        if(GameManager.instance.selectCard != null)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                if(preview)
                {
                    ShowPreview(true);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                UnitInstance(GameManager.instance.selectCard.GetComponent<Card>().status.model);
            }
        }
    }

    private void OnMouseExit() //�̸����� ����
    {
        if (GameManager.instance.selectCard != null && preview)
        {
            ShowPreview(false);
        }
    }

    private void Setup()
    {
        originalMat = transform.GetComponent<MeshRenderer>().material;
        spawnPointOffset = new Vector3(0.0f, 0.6f, 0.0f);
    }

    public void ShowPreview(bool show) //�̸����� �޼ҵ�
    {

        Material mat;

        if (show)
        {
            mat = previewMat;
            if (unit == null)
            {
                GameObject obj = Instantiate(GameManager.instance.selectCard.GetComponent<Card>().status.model, transform.position + spawnPointOffset, transform.rotation);
                obj.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[1].color = GameManager.instance.selectCard.GetComponent<Card>().status.previewColor;
                unit = obj;
            }
        }
        else
        {
            mat = originalMat;
            GameObject.Destroy(unit);
        }

        ShowRange(mat, GameManager.instance.selectCard.GetComponent<Card>().status.model.GetComponent<UnitController>().status.attack_range);

    }

    public void ShowInfo() //���� ���� UI �޼ҵ�
    {
        Material mat;
        
        info = !info;
        
        if (info)
        {
            mat = previewMat; //�� ����
            //���� ���� UI ����
        }
        else
        {
            mat = originalMat;
            //���� ���� UI ����
        }
        ShowRange(mat, unit.GetComponent<UnitController>().status.attack_range);
    }

    public void ShowRange(Material mat, float range) //Ÿ�� �� ��Ÿ� ǥ�� �޼ҵ�
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f * range);


        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Tile" && collider != this)
            {
                collider.transform.GetComponent<MeshRenderer>().material = mat;
            }
        }
    }

    public void UnitInstance(GameObject model) //���� ���� �޼ҵ�
    {
        preview = false;
        ShowPreview(false);
        unit = Instantiate(model, transform.position + spawnPointOffset, transform.rotation);
        GameManager.instance.selectCard = null;
    }

    public void UnitMove(float range) //"�̵� ������ ����"�� �߰��Ͽ� ���� �ٸ��� �ϴ°͵� ������ �մϴ�.
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f * range);
        Transform tile = GameManager.instance.GetTile();

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Tile" && collider == tile.GetComponent<Collider>())
            {
                tile.GetComponent<HexTile>().UnitInstance(unit);
                GameObject.Destroy(unit);
            }
        }
    }
}