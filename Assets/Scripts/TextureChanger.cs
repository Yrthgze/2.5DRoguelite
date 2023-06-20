using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TextureChanger : MonoBehaviour
{
    public GameObject Brush;
    public GameObject MainCharacter;
    private GameObject go_brush;

    void Start()
    {
        MainCharacter = GameObject.Find("MainCharacter");

    }
    void Update()
    {
        Paint();
        if (Input.GetKeyDown(KeyCode.M))
        {
            CombineTextures();
        }
    }

    public void Paint()
    {
        go_brush =  Instantiate(Brush, new Vector3(MainCharacter.transform.position.x,
                                        0.1f,
                                        MainCharacter.transform.position.z), Quaternion.identity);
        go_brush.transform.parent = this.transform;
    }

    private void CombineTextures()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);

        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;
    }

}
