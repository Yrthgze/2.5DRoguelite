using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    private AudioSource c_as_this_audio;
    private int i_pulse_frequency;
    [SerializeField] private int i_bpm = 120;
    [SerializeField] private Interval[] _intervals;
    private float f_last_interval_time_samples = 0;

    // Start is called before the first frame update
    void Start()
    {
        c_as_this_audio = GetComponent<AudioSource>();
        i_pulse_frequency = c_as_this_audio.clip.frequency * (60/i_bpm);
        foreach (Interval c_interval in _intervals)
        {
            c_interval.CalculateSamplesPerPulse(i_bpm, c_as_this_audio.clip.frequency);
        }
    }

    void PulseDetection(int i_current_time_samples)
    {
        int i_pulse_distance = i_current_time_samples >= i_pulse_frequency ?
            i_current_time_samples % i_pulse_frequency:
            -(Mathf.Abs(i_current_time_samples - i_pulse_frequency));
        if(i_pulse_distance <= 5000)
        {
            Debug.Log("Beat Detection Corretc");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //! Only if an interval is active
        foreach (Interval c_interval in _intervals)
        {
            if(c_interval.IsActive())
            {
                if(c_interval.HasIntervalEnded(c_as_this_audio.timeSamples))
                {
                    f_last_interval_time_samples = c_as_this_audio.timeSamples;
                    break;
                }
            }
        }
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