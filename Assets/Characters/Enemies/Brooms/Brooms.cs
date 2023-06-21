using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brooms : MonoBehaviour
{
    private GameObject go_player;
    private int i_attack_range = 10;
    private int i_sight_range = 50;
    private Rigidbody m_rg;
    public float f_force;
    // Start is called before the first frame update
    void Start()
    {
        go_player = GameObject.Find("MainCharacter");
        m_rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 48 == 0)
        {
            Attack();
            if (CanAttack())
            {
                Attack();
            }
            else if (CanClean())
            {
                CleanFloor();
            }
            else
            {
                Patrol();
            }
        }
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.transform.position.Set(transform.position.x, 0 , transform.position.z);
    }

    bool CanAttack()
    {
        bool b_res = false;
        if(Vector3.Distance(go_player.transform.position, transform.position) <= i_attack_range)
        {
            b_res = true;
        }
        return b_res;

    }

    bool CanClean()
    {
        bool b_res = false;
        if (Vector3.Distance(go_player.transform.position, transform.position) <= i_sight_range)
        {
            b_res = true;
        }
        return b_res;
    }

    void Patrol()
    {

    }

    void CleanFloor()
    {
        Vector3 v3 = (go_player.transform.position - transform.position).normalized * 20f;
        m_rg.AddForce(v3, ForceMode.Impulse);
    }

    void Attack()
    {
        float f_x_dir = go_player.transform.position.x - transform.position.x;
        float f_z_dir = go_player.transform.position.z - transform.position.z;
        /*int i_x_sign = f_x_dir >= 0 ? 1 : -1;
        int i_z_sign = f_z_dir >= 0 ? 1 : -1;
        f_x_dir = Mathf.Abs(f_x_dir) > 20 ? i_x_sign * 20 : f_x_dir;
        f_z_dir = Mathf.Abs(f_z_dir) > 20 ? i_z_sign * 20 : f_z_dir;*/
        Vector3 v3 = new Vector3(f_x_dir, 0,f_z_dir).normalized * f_force;
        m_rg.AddForce(v3, ForceMode.Impulse);

    }
}
