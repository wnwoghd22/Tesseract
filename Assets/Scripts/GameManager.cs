using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int LAYER_PLAYER = 3;
    private const int LAYER_MIRROR = 7;
    private const int LAYER_BOX = 11;

    [SerializeField]
    Transform room;
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform transformCamera;

    private bool fliped = false;

    Vector2 beganPos;

    [SerializeField]
    private LayerMask touchables;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(LAYER_BOX);
        SetCamerePos(room.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 scale = room.localScale;
            scale.y *= -1;
            room.localScale = scale;

            Vector3 mapPos = room.position;
            Vector3 playerPos = player.position;
            playerPos.y = mapPos.y - (playerPos.y - mapPos.y);
            player.position = playerPos;
        }

        HandleMouse();
    }

    public void GoToTargetRoom(Door door)
    {
        room = door.Room.transform;
        SetCamerePos(room.position);
        player.position = door.transform.position;
    }

    private void SetCamerePos(Vector3 pos)
    {
        Vector3 targetPos = new Vector3(pos.x, pos.y, -10f);
        transformCamera.position = targetPos;
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            beganPos = Input.mousePosition;
            
            RaycastHit2D[] hitInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(beganPos), Vector2.zero, float.PositiveInfinity, touchables);

            if (hitInfo.Length == 0)
            {
                // player move
                Debug.Log("mouse click : no colliders");
            }
            else
            {
                foreach (RaycastHit2D info in hitInfo)
                {
                    Debug.Log(info.collider.gameObject);

                    switch(info.collider.gameObject.layer)
                    {
                        case LAYER_BOX:
                            Debug.Log("touching box");
                            break;
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {

        }
        else if (Input.GetMouseButtonUp(0))
        {

        }
    }
    // TODO : implement non-virtual-button UI
    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    beganPos = touch.position;

                    RaycastHit2D[] hitInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(beganPos), Vector2.zero);

                    if (hitInfo.Length == 0)
                    {
                        // player move
                        Debug.Log("no colliders");
                    } 
                    else
                    {
                        foreach (RaycastHit2D info in hitInfo)
                        {

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
}
