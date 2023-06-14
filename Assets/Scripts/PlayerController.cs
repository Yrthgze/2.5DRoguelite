using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody m_rb;
    public float f_speed, f_force;

    private Vector2 m_v2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_v2.x = Input.GetAxis("Horizontal");
        m_v2.y = Input.GetAxis("Vertical");
        //m_v2.Normalize();
        m_rb.velocity = new Vector3(m_v2.x * f_speed, m_rb.velocity.y, m_v2.y * f_speed);

        this.transform.rotation = new Quaternion(0,0,0,0);
    }
}
