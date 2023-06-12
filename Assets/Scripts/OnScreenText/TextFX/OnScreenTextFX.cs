using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class OnScreenTextFX
{
    public float f_duration = 6;
    protected GameObject m_go_to_apply_effect;
    protected TextMeshProUGUI m_text_to_apply_effect;

    public OnScreenTextFX(GameObject go_to_apply_effect, TextMeshProUGUI text_to_apply_effect)
    {
        m_go_to_apply_effect = go_to_apply_effect;
        m_text_to_apply_effect = text_to_apply_effect;
    }
    public virtual void ApplyEffect()
    {
        if (f_duration > 0)
        {
            _ApplyEffect();
        }
        f_duration = f_duration - Time.deltaTime;
    }

    protected abstract void _ApplyEffect();
}
