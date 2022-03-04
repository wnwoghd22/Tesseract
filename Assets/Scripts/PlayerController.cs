using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: implement & complete portal system
    /*
     if mirror is close and in front of player, then that mirror is AVAILABLE.
    and user touches that mirror AVAILABLE, then mirror get LIT.
     */

    [SerializeField]
    private LayerMask mirror;
    private Mirror available;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckMirror();
        Debug.DrawRay(transform.position, transform.right, Color.white);
    }

    private void CheckMirror()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right, 1.5f, mirror);

        if (hit)
        {
            Debug.Log("hit!");

            if (available != null)
            {
                if (available.gameObject != hit.transform.gameObject)
                {
                    available.Disable();
                    available = hit.transform.gameObject.GetComponent<Mirror>();
                    available.Enable();
                }
            }
            else
            {
                available = hit.transform.gameObject.GetComponent<Mirror>();
                available.Enable();
            }
        }
        else
        {
            Debug.Log("no hit");

            if (available != null)
            {
                available.Disable();
                available = null;
            }

        }
    }
}