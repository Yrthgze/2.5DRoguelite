using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChanger : MonoBehaviour
{
    public GameObject Brush;
    public GameObject MainCharacter;

    void Start()
    {
        MainCharacter = GameObject.Find("MainCharacter");
    }
    void Update()
    {
        Paint();
    }

    public void Paint()
    {
        Instantiate(Brush, new Vector3(MainCharacter.transform.position.x,
                                        0.1f,
                                        MainCharacter.transform.position.z), Quaternion.identity);
    }

}
