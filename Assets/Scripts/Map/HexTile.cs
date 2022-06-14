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

    void OnMouseOver() //타일 위에서의 마우스 입력에 따라 미리보기와 유닛 생성
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

    private void OnMouseExit() //미리보기 종료
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

    public void ShowPreview(bool show) //미리보기 메소드
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

    public void ShowInfo() //유닛 정보 UI 메소드
    {
        Material mat;
        
        info = !info;
        
        if (info)
        {
            mat = previewMat; //색 설정
            //유닛 정보 UI 띄우기
        }
        else
        {
            mat = originalMat;
            //유닛 정보 UI 끄기
        }
        ShowRange(mat, unit.GetComponent<UnitController>().status.attack_range);
    }

    public void ShowRange(Material mat, float range) //타일 위 사거리 표시 메소드
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

    public void UnitInstance(GameObject model) //유닛 생성 메소드
    {
        preview = false;
        ShowPreview(false);
        unit = Instantiate(model, transform.position + spawnPointOffset, transform.rotation);
        GameManager.instance.selectCard = null;
    }

    public void UnitMove(float range) //"이동 가능한 범위"를 추가하여 색을 다르게 하는것도 좋을듯 합니다.
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