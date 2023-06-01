using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    [SerializeField] private int i_bpm = 120;
    [SerializeField] private int i_freq_khz = 44100;
    private int i_min_in_secs = 60;
    private float f_samples_per_pulse;
    private float f_check_frequency;
    private int i_step = 0, i_last_step = 0;
    private GameObject go_main_camera;
    private AudioSource c_as_this_audio, c_as_main_audio;

    // Start is called before the first frame update
    void Start()
    {
        f_check_frequency = (float)i_min_in_secs / (float)i_bpm;
        f_samples_per_pulse = f_check_frequency * i_freq_khz;
        c_as_this_audio = GetComponent<AudioSource>();
        go_main_camera = GameObject.Find("Main Camera");
        c_as_main_audio = go_main_camera.GetComponent<AudioSource>();
    }

    void BeatDetection()
    {
        //c_as_this_audio.PlayOneShot(go_main_camera.GetComponent<InputController>().c_audio_clip_ka);
    }

    // Update is called once per frame
    void Update()
    {
        i_step = Mathf.FloorToInt(c_as_main_audio.timeSamples / f_samples_per_pulse);
        if (i_step == 0 && i_last_step > 0)
        {
            i_last_step = 0;
            BeatDetection();
        }
        if (i_step > i_last_step)
        {
            i_last_step++;
            BeatDetection();
        }
    }
}