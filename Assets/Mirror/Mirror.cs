using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mirror : MonoBehaviour
{
    private bool isLit;

    [SerializeField]
    Material material;
    LaserBeam beam;

    Vector3 touchPos;

    // Start is called before the first frame update
    void Start()
    {
        isLit = false;
        touchPos = gameObject.transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLit)
        {
            Destroy(GameObject.Find("Laser Beam"));
            //beam = new LaserBeam(gameObject.transform.position, touchPos, material);

            beam = new LaserBeam(gameObject.transform.position, gameObject.transform.right, material);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("on click");
        isLit = true;
    }

}
