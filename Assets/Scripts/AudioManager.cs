using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private enum EAudios
    {
        EMainAudio = 0,
        EBeatClip1 = 1,
        EBeatClip2 = 2,
        EBeatClip3 = 3,
        EBeatClip4 = 4,
        EBassClip1 = 5,
        EBassClip2 = 6,
        EBassClip3 = 7,
        EBassClip4 = 8,
        EMidClip1 = 9,
        EMidClip2 = 10,
        EMidClip3 = 11,
        EMidClip4 = 12,
        EHighClip1 = 13,
        EHighClip2 = 14,
        EHighClip3 = 15,
        EHighClip4 = 16
    }
    private AudioSourceManager m_c_audio_source_manager;
    [SerializeField] List<AudioClip> m_l_audio_clips;
    // Start is called before the first frame update
    void Start()
    {
        m_c_audio_source_manager = GetComponent<AudioSourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayClip(EAudios e_clip_index, bool b_loop = false)
    {
        m_c_audio_source_manager.PlayClip(m_l_audio_clips[(int)e_clip_index], 1, b_loop, 1, 0, null);
    }
}
