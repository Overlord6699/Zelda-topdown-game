using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFacingMover
{
    int GetFacing(); 
    bool moving { get; } 
    float GetSpeed();
    float gridMult { get; } //позволяет не обращаться напрямую к InRoom из других скриптов
    Vector2 roomPos { get; set; } 
    Vector2 roomNum { get; set; }

    //если передать -1, то будет использовать gridMult,
    //иначе будет использовать значение из класса
    Vector2 GetRoomPosOnGrid(float mult = -1);

}
