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
    public int jj = 0;

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
        if(jj < 9000)
        {

            jj++;
        }
        if (jj % 1 == 0)
        {
            go_brush = Instantiate(Brush, new Vector3(MainCharacter.transform.position.x,
                                            0.01f,
                                            MainCharacter.transform.position.z), Brush.transform.rotation);
            go_brush.transform.parent = this.transform;
        }
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

        //AutoWeld(mesh, 0.1f);
    }

    private void AutoWeld(Mesh mesh, float threshold)
    {
        Vector3[] verts = mesh.vertices;

        // Build new vertex buffer and remove "duplicate" verticies
        // that are within the given threshold.
        List<Vector3> newVerts = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();
        int[] newTris = new int[mesh.triangles.Length];

        int k = 0;
        int i_new_vert_index = 0;

        foreach (Vector3 vert in verts)
        {
            // Has vertex already been added to newVerts list?
            foreach (Vector3 newVert in newVerts)
                if (Vector3.Distance(newVert, vert) <= threshold)
                    goto skipToNext;

            // Accept new vertex!
            newVerts.Add(vert);
            newUVs.Add(mesh.uv[k]);
        skipToNext:;
            ++k;
        }

        // Rebuild triangles using new vertices
        int[] tris = mesh.triangles;
        for (int i = 0; i < tris.Length; ++i)
        {
            // Find new vertex point from buffer
            for (int j = 0; j < newVerts.Count; ++j)
            {
                if (Vector3.Distance(newVerts[j], verts[tris[i]]) <= threshold)
                {
                    tris[i] = j; 
                    break; 
                }
             }
        }

        // Update mesh!
        mesh.Clear(); 
        mesh.vertices = newVerts.ToArray(); 
        mesh.triangles = tris; 
        mesh.uv = newUVs.ToArray(); 
        mesh.RecalculateBounds(); 
     }
}
