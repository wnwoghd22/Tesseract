using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour
{
    [SerializeField]
    private Door target;
    public Door Target => target;

    private GameManager gm;
    [SerializeField]
    private GameObject room;
    public GameObject Room => room;

    public bool Available { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Available)
            {
                GoToTarget();
            }
        }
#endif
    }

    public void GoToTarget()
    {
        gm.GoToTargetRoom(target);
    }

    public void Enable()
    {
        Debug.Log("door open");
        Available = true;
    }
    public void Disable()
    {
        Available = false;
    }
}
