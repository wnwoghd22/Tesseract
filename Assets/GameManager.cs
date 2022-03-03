using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform map;
    [SerializeField]
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 scale = map.localScale;
            scale.y *= -1;
            map.localScale = scale;

            Vector3 mapPos = map.position;
            Vector3 playerPos = player.position;
            playerPos.y = mapPos.y - (playerPos.y - mapPos.y);
            player.position = playerPos;
        }
    }
}
