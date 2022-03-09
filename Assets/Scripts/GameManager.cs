using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int LAYER_PLAYER = 3;
    private const int LAYER_MIRROR = 7;
    private const int LAYER_MIRROR_LIT = 8;
    private const int LAYER_BOX = 11;
    private const int LAYER_DOOR = 13;
    private const int LAYER_MIRROR_BOX = 15;

    [SerializeField]
    Transform room;
    [SerializeField]
    Transform player;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    Transform transformCamera;

    private bool fliped = false;

    Vector3 beganPos;

    [SerializeField]
    private LayerMask touchables;
    private bool touchingDoor = false;
    private Door doorOpen;

    private bool touchingMirror = false;
    private bool touchingPortal = false;
    private Mirror mirrorLit;
    private Mirror portal;

    private bool touchingBox = false;

    private bool touchingEmpty = false;

    private float touchingTime = 0f;
    [SerializeField]
    private float longTouchDelta = 0.5f;


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
            Flip();
        }
#if UNITY_EDITOR
        HandleMouse();
#endif
#if UNITY_ANDROID
        HandleTouch();
#endif
    }
    private void Flip()
    {
        Vector3 scale = room.localScale;
        scale.y *= -1;
        room.localScale = scale;

        Vector3 mapPos = room.position;
        Vector3 playerPos = player.position;
        playerPos.y = mapPos.y - (playerPos.y - mapPos.y);
        player.position = playerPos;
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
                touchingEmpty = true;
            }
            else
            {
                foreach (RaycastHit2D info in hitInfo)
                {
                    Debug.Log(info.collider.gameObject);

                    switch(info.collider.gameObject.layer)
                    {
                        case LAYER_BOX:
                        case LAYER_MIRROR_BOX:
                            Debug.Log("touching box");
                            if (info.collider.gameObject.GetComponent<Box>().Available)
                            {
                                Debug.Log("box available");
                                touchingBox = true;
                            }
                            else
                            {
                                Debug.Log("box unavailable");
                            }
                            break;
                        case LAYER_MIRROR:
                        case LAYER_MIRROR_LIT:
                            Debug.Log("touching mirror");

                            Mirror mirror = info.collider.gameObject.GetComponent<Mirror>();

                            if (mirror.Available)
                            {
                                touchingMirror = true;
                                mirrorLit = mirror;
                            }
                            else if (mirror.IsPortal)
                            {
                                touchingPortal = true;
                                portal = mirror;
                            }
                            break;
                        case LAYER_DOOR:
                            Debug.Log("touching door");

                            Door door = info.collider.gameObject.GetComponent<Door>();

                            if (door.Available)
                            {
                                touchingDoor = true;
                                doorOpen = door;
                            }
                            break;
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            touchingTime += Time.deltaTime;

            if (touchingEmpty)
            {
                MovePlayer(Input.mousePosition);
            }
            else
            {
                if (touchingMirror)
                {
                    Vector3 dir = Input.mousePosition - beganPos;
                    mirrorLit.Emit(dir.normalized);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(touchingTime);

            if (touchingEmpty)
            {
                if (Mathf.Abs(beganPos.x - Input.mousePosition.x) < 30f &&  beganPos.y - Input.mousePosition.y > 100f)
                {
                    Flip();
                }
            }
            else
            {
                if (touchingTime < longTouchDelta) // short touch
                {
                    if (touchingBox)
                    {
                        if (!playerController.IsHoldBox)
                            playerController.HoldBox();
                        else
                            playerController.UnholdBox();
                    }
                }
                else // long touch
                {
                    if (touchingDoor)
                    {
                        if (!touchingBox)
                        {
                            GoToTargetRoom(doorOpen.Target);
                        }
                    }
                    if (touchingPortal)
                    {
                        playerController.UnholdBox();
                        player.transform.position = portal.transform.position;
                    }
                }
            }

            touchingTime = 0;
            touchingBox = false;
            touchingDoor = false;
            touchingMirror = false;
            touchingPortal = false;
            touchingEmpty = false;

            mirrorLit = null;
            portal = null;
        }
    }
    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    beganPos = Input.mousePosition;

                    RaycastHit2D[] hitInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(beganPos), Vector2.zero, float.PositiveInfinity, touchables);

                    if (hitInfo.Length == 0)
                    {
                        // player move
                        Debug.Log("mouse click : no colliders");
                        touchingEmpty = true;
                    }
                    else
                    {
                        foreach (RaycastHit2D info in hitInfo)
                        {
                            Debug.Log(info.collider.gameObject);

                            switch (info.collider.gameObject.layer)
                            {
                                case LAYER_BOX:
                                case LAYER_MIRROR_BOX:
                                    Debug.Log("touching box");
                                    if (info.collider.gameObject.GetComponent<Box>().Available)
                                    {
                                        Debug.Log("box available");
                                        touchingBox = true;
                                    }
                                    else
                                    {
                                        Debug.Log("box unavailable");
                                    }
                                    break;
                                case LAYER_MIRROR:
                                case LAYER_MIRROR_LIT:
                                    Debug.Log("touching mirror");

                                    Mirror mirror = info.collider.gameObject.GetComponent<Mirror>();

                                    if (mirror.Available)
                                    {
                                        touchingMirror = true;
                                        mirrorLit = mirror;
                                    }
                                    else if (mirror.IsPortal)
                                    {
                                        touchingPortal = true;
                                        portal = mirror;
                                    }
                                    break;
                                case LAYER_DOOR:
                                    Debug.Log("touching door");

                                    Door door = info.collider.gameObject.GetComponent<Door>();

                                    if (door.Available)
                                    {
                                        touchingDoor = true;
                                        doorOpen = door;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    touchingTime += Time.deltaTime;

                    if (touchingEmpty)
                    {
                        MovePlayer(touch.position);
                    }
                    else
                    {
                        if (touchingMirror)
                        {
                            Vector3 dir = touch.position - (Vector2)beganPos;
                            mirrorLit.Emit(dir.normalized);
                        }
                    }
                    break;
                case TouchPhase.Stationary:
                    touchingTime += Time.deltaTime;

                    if (touchingEmpty)
                    {
                        MovePlayer(touch.position);
                    }
                    else
                    {
                        if (touchingMirror)
                        {
                            Vector3 dir = touch.position - (Vector2)beganPos;
                            mirrorLit.Emit(dir.normalized);
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    Debug.Log(touchingTime);

                    if (touchingEmpty)
                    {
                        if (Mathf.Abs(beganPos.x - touch.position.x) < 30f && beganPos.y - touch.position.y > 100f)
                        {
                            Flip();
                        }
                    }
                    else
                    {
                        if (touchingTime < longTouchDelta) // short touch
                        {
                            if (touchingBox)
                            {
                                if (!playerController.IsHoldBox)
                                    playerController.HoldBox();
                                else
                                    playerController.UnholdBox();
                            }
                        }
                        else // long touch
                        {
                            if (touchingDoor)
                            {
                                if (!touchingBox)
                                {
                                    GoToTargetRoom(doorOpen.Target);
                                }
                            }
                            if (touchingPortal)
                            {
                                playerController.UnholdBox();
                                player.transform.position = portal.transform.position;
                            }
                        }
                    }

                    touchingTime = 0;
                    touchingBox = false;
                    touchingDoor = false;
                    touchingMirror = false;
                    touchingPortal = false;
                    touchingEmpty = false;

                    mirrorLit = null;
                    portal = null;
                    break;
                case TouchPhase.Canceled:
                    break;
            }
        }
    }

    private void MovePlayer(Vector3 dir)
    {
        //Debug.Log(dir + ", " + Screen.width);

        // direction by position
        //if (dir.x / Screen.width > 0.5f)
        //{
        //    playerController.MoveRight();
        //}
        //else
        //{
        //    playerController.MoveLeft();
        //}

        // direction by drag
        Debug.Log(dir.x + ", " + beganPos.x);

        if (dir.x - beganPos.x > 50f)
        {
            playerController.MoveRight();
        }
        else if (dir.x - beganPos.x < -50f)
        {
            playerController.MoveLeft();
        }
    }
}
