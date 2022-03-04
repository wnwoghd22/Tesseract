using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mirror : MonoBehaviour
{
    const int LAYER_MIRROR = 7;
    const int LAYER_MIRROR_LIT = 8;

    private bool isLit;

    [SerializeField]
    Material material;
    [SerializeField]
    ContactFilter2D filter;
    LaserBeam beam;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        isLit = false;
        direction = gameObject.transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLit)
        {
            Destroy(GameObject.Find("Laser Beam"));
            beam = new LaserBeam(gameObject.transform.position, direction, material, filter);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("on click");
        gameObject.layer = LAYER_MIRROR_LIT;

        isLit = true;

        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        direction = p - transform.position; 
    }
    private void OnMouseDrag()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        direction = p - transform.position;
    }

    public void Unlit()
    {
        isLit = false;
    }
}
