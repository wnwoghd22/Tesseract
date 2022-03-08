using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Box : MonoBehaviour
{
    public bool Available { get; private set; }

    private void Start()
    {
        Available = false;
    }

    private void Update()
    {
        
    }

    public void Enable()
    {
        Available = true;
    }
    public void Disable()
    {
        Available = false;
    }
}
