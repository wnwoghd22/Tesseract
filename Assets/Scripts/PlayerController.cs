using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const int LAYER_HOLD_TRIGGER = 14;

    // TODO: implement & complete portal system
    /*
     if mirror is close and in front of player, then that mirror is AVAILABLE.
    and user touches that mirror AVAILABLE, then mirror get LIT.
     */

    [SerializeField]
    private LayerMask mirror;
    private Mirror available;

    private Transform box;
    private Transform tempRoom;

    [SerializeField]
    private float moveSpeed = 3.0f;

    [SerializeField] private JoyStick moveStick;
    [SerializeField] private JoyButton holdButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        float h = Input.GetAxis("Horizontal");

        if (h > 0.5f)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;
        }
        else if (h < -0.5f) 
        {
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;
        }
        Vector3 pos = transform.position;
        pos.x += h * moveSpeed * Time.deltaTime;
        transform.position = pos;
#endif
#if UNITY_ANDROID
        switch (moveStick.State)
        {
            case eButtonState.None:
                break;
            case eButtonState.Down:
                break;
            case eButtonState.Pressed:
                Vector2 inputDir = moveStick.InputDir;

                float x = inputDir.x;

                if (x > 0.5f)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = 1f;
                    transform.localScale = scale;
                }
                else if (x < -0.5f)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = -1f;
                    transform.localScale = scale;
                }
                Vector3 posAndroid = transform.position;
                posAndroid.x += x * moveSpeed * Time.deltaTime;
                transform.position = posAndroid;
                break;
            case eButtonState.Up:
                break;
        }
#endif

        CheckMirror();
        Debug.DrawRay(transform.position, transform.right * transform.localScale.x, Color.white);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);

            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hitInfo)
            {
                Debug.Log(hitInfo.transform.name);

                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.gameObject.tag == "Mirror")
                    {
                        Mirror target = hitInfo.collider.gameObject.GetComponent<Mirror>();

                        if (target.IsPortal)
                            this.transform.position = target.transform.position;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (box)
            {
                tempRoom = box.parent; // get room transform
                box.SetParent(this.transform);
                Destroy(GameObject.Find("Laser Beam"));
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (box)
            {
                box.SetParent(tempRoom);
            }
        }
    }

    private void CheckMirror()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * transform.localScale.x, 1.5f, mirror);

        if (hit)
        {
            //Debug.Log("hit!");

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
            //Debug.Log("no hit");

            if (available != null)
            {
                Debug.Log("leave");
                available.Disable();
                available = null;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LAYER_HOLD_TRIGGER)
        {
            Debug.Log("can hold box");
            box = collision.gameObject.transform.parent;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LAYER_HOLD_TRIGGER)
        {
            Debug.Log("leave box");
            box = null;
        }
    }
}
