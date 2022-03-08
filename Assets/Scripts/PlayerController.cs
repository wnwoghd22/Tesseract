using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const int LAYER_DOOR = 13;
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
    public bool IsHoldBox { get; private set; }
    private Transform tempRoom;

    [SerializeField]
    private float moveSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        IsHoldBox = false;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HoldBox();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            UnholdBox();
        }
#endif

        CheckMirror();

    }

    public void HoldBox()
    {
        if (box)
        {
            IsHoldBox = true;
            tempRoom = box.parent; // get room transform
            box.SetParent(this.transform);
            Destroy(GameObject.Find("Laser Beam"));
        }
    }
    public void UnholdBox()
    {
        if (box)
        {
            IsHoldBox = false;
            box.SetParent(tempRoom);
        }
    }
    public void HoldBox(Transform box)
    {
        tempRoom = box.parent; // get room transform
        box.SetParent(this.transform);
        Destroy(GameObject.Find("Laser Beam"));
    }
    public void UnholdBox(Transform box)
    {
        box.SetParent(tempRoom);
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
            box.GetComponent<Box>().Enable();
        }
        if (collision.gameObject.layer == LAYER_DOOR)
        {
            Door door = collision.gameObject.GetComponent<Door>();
            door.Enable();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LAYER_HOLD_TRIGGER)
        {
            Debug.Log("leave box");
            box.GetComponent<Box>().Disable();
            box = null;
        }
        if (collision.gameObject.layer == LAYER_DOOR)
        {
            Door door = collision.gameObject.GetComponent<Door>();
            door.Disable();
        }
    }

    public void MoveLeft()
    {
        Vector3 scale = transform.localScale;
        scale.x = -1f;
        transform.localScale = scale;

        Vector3 pos = transform.position;
        pos.x -= moveSpeed * Time.deltaTime;
        transform.position = pos;
    }
    public void MoveRight()
    {
        Vector3 scale = transform.localScale;
        scale.x = 1f;
        transform.localScale = scale;

        Vector3 pos = transform.position;
        pos.x += moveSpeed * Time.deltaTime;
        transform.position = pos;
    }
}
