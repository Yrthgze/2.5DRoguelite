using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OSTWooble : OnScreenTextFX
{
    private Mesh m_mesh;
    private Vector3[] m_vertices;

    public OSTWooble(GameObject go_to_apply_effect, TextMeshProUGUI text_to_apply_effect) :
        base(go_to_apply_effect, text_to_apply_effect)
    {

    }

    public override void ApplyEffect()
    {
        Debug.Log("Updating");
        m_text_to_apply_effect.ForceMeshUpdate();
        m_mesh = m_text_to_apply_effect.mesh;
        m_vertices = m_mesh.vertices;/*

        for(int i = 0; i < m_vertices.Length; i++)
        {
            Vector3 v3_offset = Wooble(Time.deltaTime * (i+1));
            m_vertices[i] = m_vertices[i] + v3_offset;
        }

        */
        for (int i = 0; i < m_text_to_apply_effect.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo tmp_char_info = m_text_to_apply_effect.textInfo.characterInfo[i];
            int index = tmp_char_info.vertexIndex;

            Vector3 offset = Wooble(Time.time + i * Random.Range(0, 5)) * 2;
            m_vertices[index] += offset;
            m_vertices[index + 1] += offset;
            m_vertices[index + 2] += offset;
            m_vertices[index + 3] += offset;
        }

        m_mesh.vertices = m_vertices;
        m_text_to_apply_effect.canvasRenderer.SetMesh(m_mesh);
    }

    private Vector2 Wooble(float f_time)
    {
        return new Vector2(Mathf.Sin(f_time * 3.3f), Mathf.Cos(f_time * 2.8f));
    }
}
