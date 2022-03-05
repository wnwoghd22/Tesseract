using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMesh : MonoBehaviour
{
    [SerializeField]
    LayerMask maskLight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit!");

        if ((1 << collision.gameObject.layer & maskLight.value) != 0)
        {
            Debug.Log("light hit!");
        }
    }
}
