using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private Animator m_c_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_c_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {/*
        if(Input.GetKey(KeyCode.UpArrow))
        {
            m_c_animator.SetInteger("Walking", 1);
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            m_c_animator.SetInteger("Walking", 2);
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            m_c_animator.SetInteger("Walking", 3);
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_c_animator.SetInteger("Walking", 4);
        }
        else
        {
            m_c_animator.SetInteger("Walking", 0);
        }*/
    }
}
