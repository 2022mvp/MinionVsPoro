using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineAvgNormal : MonoBehaviour
{
    private Material outlineMat;
    private GameObject outlineObj;

    private void Awake()
    {
        if(outlineMat == null)
        {
            outlineMat = new Material(Shader.Find("Custom/HexTileOutlineShader"));
            outlineMat.hideFlags = HideFlags.HideAndDontSave;
        }

        Transform tmpT = transform.Find("Outline");

        if(tmpT)
        {
            outlineObj = tmpT.gameObject;
            if(GetComponent<MeshFilter>())
            {
                outlineObj.GetComponent<MeshRenderer>().material = outlineMat;
            }
            if(GetComponent<SkinnedMeshRenderer>())
            {
                outlineObj.GetComponent<SkinnedMeshRenderer>().material = outlineMat;
            }
        }

        if(outlineObj == null)
        {
            outlineObj = new GameObject("Outline");
            outlineObj.transform.parent = transform;

            if(GetComponent<MeshFilter>())
            {
                outlineObj.AddComponent<MeshFilter>();
                //outlineObj.AddComponent<MeshRenderer>(GetComponent<MeshRenderer>());
                outlineObj.AddComponent<MeshRenderer>();
                Mesh tmpMesh = (Mesh)Instantiate(GetComponent<MeshFilter>().sharedMesh);
                OutlineExtension.MeshNormalAverage(tmpMesh);
                outlineObj.GetComponent<MeshFilter>().sharedMesh = tmpMesh;
                outlineObj.GetComponent<MeshRenderer>().material = outlineMat;
            }

            if(GetComponent<SkinnedMeshRenderer>())
            {
                outlineObj.AddComponent<SkinnedMeshRenderer>();
                Mesh tmpMesh = (Mesh)Instantiate(GetComponent<SkinnedMeshRenderer>().sharedMesh);
                OutlineExtension.MeshNormalAverage(tmpMesh);
                outlineObj.GetComponent<SkinnedMeshRenderer>().sharedMesh = tmpMesh;
                outlineObj.GetComponent<SkinnedMeshRenderer>().material = outlineMat;
            }

            outlineObj.transform.localPosition = Vector3.zero;
            outlineObj.transform.localRotation = Quaternion.identity;
            outlineObj.transform.localScale = Vector3.one;
        }
    }
}
