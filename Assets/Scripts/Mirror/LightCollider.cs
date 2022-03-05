using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollider : MonoBehaviour
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((maskLight.value & 1 << collision.gameObject.layer) != 0)
        {
            Debug.Log("light hit!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("light hit!");

        if ((maskLight.value & 1 << collision.gameObject.layer) != 0)
        {
            Debug.Log("light hit!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger hit!");

        if ((maskLight.value & 1 << collision.gameObject.layer) != 0)
        {
            Debug.Log("triggerlight hit!");
        }
    }
}
