using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum ECorrectness
{
    EPerfect = 0,
    EGood = 1,
    EMidgood = 2,
    EBad = 3,
    ECount = 4
}

public class BeatController : MonoBehaviour
{
    private AudioSource c_as_this_audio;
    private int i_pulse_frequency;
    [SerializeField] private int i_bpm = 120;
    [SerializeField] private Interval[] _intervals;
    [SerializeField] private float f_maximum_pulse_delay = 0.8f;
    private float f_last_interval_time_samples = 0;
    private float m_f_wrong_pulse_distance, m_f_good_pulse_distance, m_f_perfect_pulse_distance;

    // Start is called before the first frame update
    void Start()
    {
        c_as_this_audio = GetComponent<AudioSource>();
        i_pulse_frequency = (int)(c_as_this_audio.clip.frequency * (60/(float)i_bpm));
        m_f_wrong_pulse_distance = f_maximum_pulse_delay / 2 * i_pulse_frequency;
        m_f_good_pulse_distance = m_f_wrong_pulse_distance / 2;
        m_f_perfect_pulse_distance = m_f_good_pulse_distance / 2;
        foreach (Interval c_interval in _intervals)
        {
            c_interval.CalculateSamplesPerPulse(i_bpm, c_as_this_audio.clip.frequency);
        }
    }

    void PulseDetection(int i_current_time_samples)
    {
        ECorrectness e_correctness = ECorrectness.EBad;
        int i_pulse_distance = i_current_time_samples >= i_pulse_frequency ?
            i_current_time_samples % i_pulse_frequency:
            -(Mathf.Abs(i_current_time_samples - i_pulse_frequency));
        if(i_pulse_distance <= m_f_perfect_pulse_distance)
        {
            Debug.Log("Perfect beat!");
            e_correctness = ECorrectness.EPerfect;
        }
        else if (i_pulse_distance <= m_f_good_pulse_distance)
        {
            Debug.Log("Good beat!");
            e_correctness = ECorrectness.EGood;
        }
        else if (i_pulse_distance <= m_f_wrong_pulse_distance)
        {
            Debug.Log("Need to hear the beat!");
            e_correctness = ECorrectness.EMidgood;
        }
        else
        {
            Debug.Log("Completely out of tempo!");
        }
        OnScreenTexts temp_text = gameObject.AddComponent<OnScreenTexts>();
        temp_text.SetCorrectness(e_correctness);
    }

    // Update is called once per frame
    void Update()
    {
        //! Only if an interval is active
        /*foreach (Interval c_interval in _intervals)
        {
            if(c_interval.IsActive())
            {
                if(c_interval.HasIntervalEnded(c_as_this_audio.timeSamples))
                {
                    f_last_interval_time_samples = c_as_this_audio.timeSamples;
                    GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
                    break;
                }
            }
        }*/
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PulseDetection(c_as_this_audio.timeSamples);
        }
    }
}

[System.Serializable]
public class Interval
{
    [SerializeField] private int idint = 1;
    [SerializeField] private int i_beats_per_pulse = 1;
    [SerializeField] private bool b_is_active = false;
    private int i_interval_freq_khz = 44100;
    private float f_check_frequency;
    private float f_samples_per_pulse;
    private int i_step = 0, i_last_step = 0;
    private int i_min_in_secs = 60;

    public bool IsActive()
    {
        return b_is_active;
    }

    public void CalculateSamplesPerPulse(int i_bpm, int i_clip_frequency)
    {
        i_interval_freq_khz = i_clip_frequency;
        f_check_frequency = (float)i_min_in_secs / ((float)i_bpm * (float)i_beats_per_pulse);
        f_samples_per_pulse = f_check_frequency * i_interval_freq_khz;
    }

    public bool HasIntervalEnded(int i_ellapsed_samples)
    {
        bool b_ret = false;
        i_step = Mathf.FloorToInt(i_ellapsed_samples / f_samples_per_pulse);
        if (i_step == 0 && i_last_step > 0)
        {
            i_last_step = 0;
            b_ret = true;
        }
        if (i_step > i_last_step)
        {
            i_last_step++;
            b_ret = true;
        }
        return b_ret;
    }

}

[System.Serializable]
public class OnScreenTexts : MonoBehaviour
{
    private string[] s_possible_strings = new string[(int)ECorrectness.ECount] {"Perfect", "Good", "Not bad", "Awful" };
    private const TextAnchor upperLeft = TextAnchor.UpperLeft;
    private TextMeshProUGUI m_s_text;
    private GameObject go_temp;
    private Mesh m_mesh;
    private Vector3[] m_vertices;
    private RectTransform m_rect_trans;

    private float f_rand_rotation_speed;
    private int i_rotation_orientation;
    public void Awake()
    {
        GameObject go_canvas;
        Canvas c_canvas;

        if (GameObject.Find("Canvas") == null)
        {
            go_canvas = new GameObject();
            go_canvas.name = "Canvas";
            go_canvas.AddComponent<Canvas>();
        }

        c_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        c_canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Text
        go_temp = new GameObject();
        go_temp.transform.parent = c_canvas.transform;
        go_temp.name = "TempGO";

        m_s_text = go_temp.AddComponent<TextMeshProUGUI>();
        m_s_text.font = (TMPro.TMP_FontAsset)Resources.Load("MyFont");
        m_s_text.fontSize = 50;
        m_s_text.alignment = TextAlignmentOptions.Center;
        m_s_text.verticalAlignment = VerticalAlignmentOptions.Middle;

        m_rect_trans = m_s_text.GetComponent<RectTransform>();

        i_rotation_orientation = Random.Range(-1.1f, 1f) >= 0 ? 1 : -1;
        //! Rotation goes backwards (if we want to move to the right, the z change must be negative, thats why the "-")
        f_rand_rotation_speed = Random.Range(3f, 20f) * -i_rotation_orientation;
        StartCoroutine(AutoDestroy());
    }

    private void MovingCurve()
    {
        float f_x = Mathf.Abs(m_rect_trans.localPosition.x) + Random.Range(10,20);
        float f_y = 18 * Mathf.Sqrt(f_x);
        Debug.Log(f_x);
        //! Rotation orientation must be place into the position as well
        m_rect_trans.localPosition = new Vector3(f_x * i_rotation_orientation, f_y,0);
        m_rect_trans.Rotate(new Vector3(0, 0, f_rand_rotation_speed));
    }

    private Vector2 Wooble(float f_time)
    {
        return new Vector2(Mathf.Sin(f_time * 3.3f), Mathf.Cos(f_time  * 2.8f));
    }

    public void Update()
    {
        Debug.Log("Updating");
        m_s_text.ForceMeshUpdate();
        m_mesh = m_s_text.mesh;
        m_vertices = m_mesh.vertices;/*

        for(int i = 0; i < m_vertices.Length; i++)
        {
            Vector3 v3_offset = Wooble(Time.deltaTime * (i+1));
            m_vertices[i] = m_vertices[i] + v3_offset;
        }

        */
        for (int i = 0; i < m_s_text.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo tmp_char_info = m_s_text.textInfo.characterInfo[i];
            int index = tmp_char_info.vertexIndex;

            Vector3 offset = Wooble(Time.time + i*Random.Range(0,5)) * 2;
            m_vertices[index] += offset;
            m_vertices[index + 1] += offset;
            m_vertices[index + 2] += offset;
            m_vertices[index + 3] += offset;
        }

        m_mesh.vertices = m_vertices;
        m_s_text.canvasRenderer.SetMesh(m_mesh);
        MovingCurve();
    }

    public void SetCorrectness(ECorrectness e_correctness)
    {
        m_s_text.text = s_possible_strings[(int)e_correctness];
        go_temp.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Destroying");
        //! Do something
        Destroy(go_temp);
        Destroy(this);
    }
}