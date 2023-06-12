using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Serializable]
public class OnScreenTextsManager : MonoBehaviour
{
    private string[] s_possible_strings = new string[(int)ECorrectness.ECount] { "Perfect", "Good", "Not bad", "Awful" };
    private const TextAnchor upperLeft = TextAnchor.UpperLeft;
    private TextMeshProUGUI m_s_text;
    private GameObject go_temp;
    [SerializeField] public OnScreenTextFXFactory m_os_text_factory;
    private OnScreenTextFX m_actual_text_fx;
    private GameObject m_go_canvas;
    private Canvas m_c_canvas;

    public void Awake()
    {

        if (GameObject.Find("Canvas") == null)
        {
            m_go_canvas = new GameObject();
            m_go_canvas.name = "Canvas";
            m_go_canvas.AddComponent<Canvas>();
        }

        m_c_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        m_c_canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        
    }

    public void GenerateText(ECorrectness e_correctness)
    {
        go_temp = new GameObject();
        go_temp.transform.parent = m_c_canvas.transform;
        go_temp.name = "TempGO";

        m_s_text = go_temp.AddComponent<TextMeshProUGUI>();
        m_s_text.font = (TMPro.TMP_FontAsset)Resources.Load("MyFont");
        m_s_text.fontSize = 50;
        m_s_text.alignment = TextAlignmentOptions.Center;
        m_s_text.verticalAlignment = VerticalAlignmentOptions.Middle;
        m_s_text.text = s_possible_strings[(int)e_correctness];
        go_temp.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

        m_os_text_factory = new OnScreenTextFXFactory();
        m_os_text_factory.SetText(this.gameObject, m_s_text);
        m_os_text_factory.count = 9;
        m_actual_text_fx = m_os_text_factory.GetOSTFX();

        StartCoroutine(AutoDestroy());
    }

    public void Update()
    {
        if(go_temp != null && m_actual_text_fx != null)
        {
            m_actual_text_fx.ApplyEffect();
        }
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Destroying");
        //! Do something
        Destroy(go_temp);
    }
}
