using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform room;
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform transformCamera;

    private bool fliped = false;

    Vector2 beganPos;

    [SerializeField]
    private JoyButton enterButton;
    public JoyButton EnterButton => enterButton;

    // Start is called before the first frame update
    void Start()
    {
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

                    foreach (RaycastHit2D info in hitInfo)
                    {

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
