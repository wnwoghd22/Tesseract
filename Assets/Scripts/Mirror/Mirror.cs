using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mirror : MonoBehaviour
{
    const int LAYER_MIRROR = 7;
    const int LAYER_MIRROR_LIT = 8;

    private bool isLit = false;

    [SerializeField]
    Material material;
    [SerializeField]
    ContactFilter2D filter;
    LaserBeam beam;

    Vector3 direction;

    private bool available = false;

    // Start is called before the first frame update
    void Start()
    {
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
        if (!available)
            return;

        Debug.Log("on click");
        gameObject.layer = LAYER_MIRROR_LIT;

        isLit = true;

        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        direction = p - transform.position; 
    }
    private void OnMouseDrag()
    {
        if (!available)
            return;

        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        direction = p - transform.position;
    }
    private void OnMouseUp()
    {
        //Unlit();
    }

    public void Unlit()
    {
        gameObject.layer = LAYER_MIRROR;
        isLit = false;
        beam = null;
        Destroy(GameObject.Find("Laser Beam"));
    }
    public void Enable()
    {
        available = true;
    }

    public void Disable()
    {
        available = false;
        Unlit();
    }
}
