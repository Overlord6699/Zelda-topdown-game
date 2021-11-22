using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPanel : MonoBehaviour
{ 
    [Header("Set in Inspector")]
    public Dray dray;
    public Sprite healthEmpty;
    public Sprite healthHalf;
    public Sprite healthFull;
    Text keyCountText;

    List<Image> healthImages;



    void Start()
    {
        // Счетчик ключей
        Transform trans = transform.Find("Key Count");
        keyCountText = trans.GetComponent<Text>();
        // a
        // Индикатор уровня здоровья
        Transform healthPanel = transform.Find("Health Panel");
        healthImages = new List<Image>();
        if (healthPanel != null)
        { // b
            for (int i = 0; i < 20; i++)
            {
                trans = healthPanel.Find("H_"+i);
                if (trans == null) 
                    break;
                healthImages.Add(trans.GetComponent<Image>());
            }
        }

    }

    void Update()
    {
        // Показать количество ключей
        keyCountText.text = dray.numKeys.ToString(); //c
        // Показать уровень здоровья
        int health = dray.health;
        for (int i = 0; i < healthImages.Count; i++)
        { // d
            if (health > 1)
            {
                healthImages[i].sprite = healthFull;
            }
            else if (health == 1)
            {
                healthImages[i].sprite = healthHalf;
            }
            else
            {
                healthImages[i].sprite = healthEmpty;
            }

            health -= 2;
        }
    }
}
