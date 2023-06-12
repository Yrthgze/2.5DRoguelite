using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnScreenTextFXFactory
{
    private GameObject m_go_to_apply_effect;
    private TextMeshProUGUI m_text_to_apply_effect;

    public void SetText(GameObject go_to_apply_effect, TextMeshProUGUI text_to_apply_effect)
    {
        m_go_to_apply_effect = go_to_apply_effect;
        m_text_to_apply_effect = text_to_apply_effect;
    }
    public OnScreenTextFX GetOSTFX()
    {
        return new OSTRandomBounce(m_go_to_apply_effect, m_text_to_apply_effect);
    }
}
