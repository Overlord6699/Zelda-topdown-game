using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CamFollowDray : MonoBehaviour
{
    static public bool TRANSITIONING = false;

    [Header("Set in inspector: start")]
    public InRoom drayInRoom;
    public float transTime = 0.5f;

    //позиции камеры(старая и новая)
    private Vector3 p0, p1;
    // камера тоже находится в какой-то комнате
    private InRoom inRoom;
    private float transStart;



    private void Awake()
    {
        inRoom = GetComponent<InRoom>();
    }


    void Update()
    {
        if (TRANSITIONING)
        {
            float num = (Time.time - transStart) / transTime;
            if(num >= 1)
            {
                num = 1;
                TRANSITIONING = false;
            }

            transform.position = (1 - num) * p0 + num * p1;
        }
        else
        {
            if(drayInRoom.roomNum != inRoom.roomNum)
            {
                TransitionTo(drayInRoom.roomNum);
            }
        }

    }

    void TransitionTo(Vector2 rm)
    { 
        p0 = transform.position;
        inRoom.roomNum = rm;
        p1 = transform.position + (Vector3.back * 10);

        transform.position = p0;

        transStart = Time.time;
        TRANSITIONING = true;
    }
}
