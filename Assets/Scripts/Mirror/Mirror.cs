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

    private bool available = false;
    public bool IsPortal { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        direction = gameObject.transform.right;
        IsPortal = false;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        HandleTouch();
#endif
    }

#if UNITY_EDITOR
    private void OnMouseDown()
    {
        if (!available)
            return;

        Debug.Log("on click");
        gameObject.layer = LAYER_MIRROR_LIT;

        isLit = true;

        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        direction = p - transform.position;

        Destroy(GameObject.Find("Laser Beam"));
        beam = new LaserBeam(gameObject.transform.position, direction, material, surface);
    }
    private void OnMouseDrag()
    {
        if (!available)
            return;

        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        direction = p - transform.position;

        Destroy(GameObject.Find("Laser Beam"));
        beam = new LaserBeam(gameObject.transform.position, direction, material, surface);
    }
    private void OnMouseUp()
    {
        //Unlit();
    }
#endif
#if UNITY_ANDROID
    private void HandleTouch()
    {
        if (!available)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

                    if (hitInfo.collider)
                    {
                        if (hitInfo.collider.gameObject == this)
                        {
                            Debug.Log("touch mirror!");
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    break;
                case TouchPhase.Canceled:
                    break;
            }
        }
    }
#endif

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
        available = true;
    }

    public void Disable()
    {
        available = false;
        Unlit();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!available && !IsPortal)
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
        if (!available && !IsPortal)
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
