using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GateKeeper : MonoBehaviour
{
    // Следующие константы зависят от файла изображения по умолчанию DelverTiles.
    // Если вы переупорядочите спрайты в DelverTiles,
    // возможно, вам придется изменить эти константы!
    //...............Индексы плиток с запертыми дверьми 
    const int lockedR = 95;
    const int lockedUR = 81;
    const int lockedUL = 80;
    const int lockedL = 100;
    const int lockedDL = 101;
    const int lockedDR = 102;
    //.................Индексы плиток с открытыми дверьми
    const int openR = 48;
    const int openUR = 93;
    const int openUL = 92;
    const int openL = 51;
    const int openDL = 26;
    const int openDR = 27;
    private IKeyMaster keys;
    void Awake()
    {
        keys = GetComponent<IKeyMaster>();
    }
    void OnCollisionStay(Collision coll)
    { 
      // Если ключей нет, можно не продолжать
        if (keys.keyCount < 1) 
            return;
        // Интерес представляют только плитки
        Tile ti = coll.gameObject.GetComponent<Tile>();
        if (ti == null) 
            return;
        // Открывать, только если Дрей обращен лицом к двери
        // (предотвратить случайное использование ключа)
        int facing = keys.GetFacing();
        // Проверить, является ли плитка закрытой дверью
        Tile ti2;

        switch (ti.tileNum)
        { 
            case lockedR:
                if (facing != 0) return; 
                ti.SetTile(ti.x, ti.y, openR);
                break;
            case lockedUR:
                if (facing != 1)
                    return;
                ti.SetTile(ti.x, ti.y, openUR);
                ti2 = TileCamera.TILES[ti.x - 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openUL);
                break;
            case lockedUL:
                if (facing != 1) return;
                ti.SetTile(ti.x, ti.y, openUL);
                ti2 = TileCamera.TILES[ti.x + 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openUR);
                break;
            case lockedL:
                if (facing != 2) return;
                ti.SetTile(ti.x, ti.y, openL);
                break;
            case lockedDL:
                if (facing != 3) return;
                ti.SetTile(ti.x, ti.y, openDL);
                ti2 = TileCamera.TILES[ti.x + 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openDR);
                break;
            case lockedDR:
                if (facing != 3) return;
                ti.SetTile(ti.x, ti.y, openDR);
                ti2 = TileCamera.TILES[ti.x - 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openDL);
                break;
            default:
                return; // Выйти, чтобы исключить уменьшение счетчика ключей
        }

        keys.keyCount--;
    }
}
