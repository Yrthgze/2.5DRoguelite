using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OSTRandomBounce : OnScreenTextFX
{
    private float f_rand_rotation_speed;
    private int i_rotation_orientation;
    private RectTransform m_rect_trans;

    public OSTRandomBounce(GameObject go_to_apply_effect, TextMeshProUGUI text_to_apply_effect) :
        base(go_to_apply_effect, text_to_apply_effect)
    {
        i_rotation_orientation = Random.Range(-1.1f, 1f) >= 0 ? 1 : -1;
        //! Rotation goes backwards (if we want to move to the right, the z change must be negative, thats why the "-")
        f_rand_rotation_speed = Random.Range(3f, 20f) * -i_rotation_orientation;
    }

    public override void ApplyEffect()
    {
        m_rect_trans = m_go_to_apply_effect.GetComponent<RectTransform>();
        float f_x = Mathf.Abs(m_rect_trans.localPosition.x) + Random.Range(10, 20);
        float f_y = 18 * Mathf.Sqrt(f_x);
        Debug.Log(f_x);
        //! Rotation orientation must be place into the position as well
        m_rect_trans.localPosition = new Vector3(f_x * i_rotation_orientation, f_y, 0);
        m_rect_trans.Rotate(new Vector3(0, 0, f_rand_rotation_speed));
    }
}
