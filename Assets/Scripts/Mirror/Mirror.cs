using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Mirror : MonoBehaviour
{
    const int LAYER_MIRROR = 7;
    const int LAYER_MIRROR_LIT = 8;

    private bool isLit = false;

    [SerializeField]
    Material material;
    [SerializeField]
    LayerMask surface;
    [SerializeField]
    LayerMask maskLight;
    LaserBeam beam;

    Vector3 direction;

    public bool Available { get; private set; } = false;
    public bool IsPortal { get; private set; } = false;


    // Start is called before the first frame update
    void Start()
    {
        direction = gameObject.transform.right;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Unlit()
    {
        Debug.Log("unlit");
        gameObject.layer = LAYER_MIRROR;
        isLit = false;
        beam = null;
        Destroy(GameObject.Find("Laser Beam"));
    }
    public void Enable()
    {
        Debug.Log("available");
        Available = true;
    }

    public void Disable()
    {
        Available = false;
        Unlit();
    }

    public void Emit(Vector3 dir)
    {
        if (gameObject.layer != LAYER_MIRROR_LIT)
            gameObject.layer = LAYER_MIRROR_LIT;
        isLit = true;
        Destroy(GameObject.Find("Laser Beam"));
        beam = new LaserBeam(gameObject.transform.position, dir, material, surface);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Available && !IsPortal)
        {
            if ((1 << collision.gameObject.layer & maskLight.value) != 0)
            {
                Debug.Log("light hit!");
                IsPortal = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Available && !IsPortal)
        {
            if ((1 << collision.gameObject.layer & maskLight.value) != 0)
            {
                Debug.Log("trigger light!");
                IsPortal = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsPortal)
        {
            if ((1 << collision.gameObject.layer & maskLight.value) != 0)
            {
                Debug.Log("trigger light!");
                IsPortal = false;
            }
        }
    }

}
