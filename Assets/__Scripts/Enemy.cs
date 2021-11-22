using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    protected static Vector3[] directions = new Vector3[] {
        Vector3.right, Vector3.up, 
        Vector3.left, Vector3.down
    };

    [Header("Set in Inspector: Enemy")]
    public float maxHealth = 1;
    public float knockbackSpeed = 10;
    public float knockbackDuration = 0.25f;
    public float invincibleDuration = 0.5f;

    public GameObject[] randomltemDrops;

    [Header("Set Dynamically: Enemy")]
    public float health;
    public bool invincible;
    public bool knockback = false;

    private float invincibleDone = 0f;
    private float knockbackDone = 0f;
    private Vector3 knockbackVel;

    public GameObject guaranteedltemDrop = null;

    protected Animator anim; 
    protected Rigidbody rigid; 
    protected SpriteRenderer sRend;

    protected virtual void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        sRend = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    { 

      // Проверить состояние неуязвимости и необходимость выполнить отскок
        if (invincible && Time.time > invincibleDone)
            invincible = false;
        sRend.color = invincible ? Color.red : Color.white;
        if (knockback)
        {
            rigid.velocity = knockbackVel;
            if (Time.time < knockbackDone) return;
        }

        anim.speed = 1; 
        knockback = false;
    }

    void OnTriggerEnter(Collider colld)
    { 
        if (invincible) 
            return; // Выйти, если Дрей пока неуязвим

        DamageEffect dEf = colld.gameObject.GetComponent<DamageEffect>();
        if (dEf == null) 
            return; // Если компонент DamageEffect отсутствует - выйти
        health -= dEf.damage; // Вычесть величину ущерба из уровня здоровья
        
        if (health <= 0) 
            Die(); 

        invincible = true; // Сделать Дрея неуязвимым
        invincibleDone = Time.time + invincibleDuration;
        if (dEf.knockback)
        { // Выполнить отбрасывание
          // Определить направление отскока
            Vector3 delta = transform.position - colld.transform.root.position;
            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
            {
                // Отбрасывание по горизонтали
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            }
            else
            {
                // Отбрасывание по вертикали
                delta.x = 0;
                delta.y = (delta.y > 0) ? 1 : -1;
            }
            // Применить скорость отбрасывания к компоненту Rigidbody
            knockbackVel = delta * knockbackSpeed;
            rigid.velocity = knockbackVel;
            // Установить режим knockback и время прекращения отбрасывания
            knockback = true;
            knockbackDone = Time.time + knockbackDuration;
            anim.speed = 0;
        }
    }

    void Die()
    {
        GameObject go;
        if (guaranteedltemDrop != null)
        {
            go = Instantiate<GameObject>(guaranteedltemDrop);
            go.transform.position = transform.position;
        }
        else if (randomltemDrops.Length > 0)
        { 
            int n = Random.Range(0, randomltemDrops.Length);
            GameObject prefab = randomltemDrops[n];
            if (prefab != null)
            {
                go = Instantiate<GameObject>(prefab);
                go.transform.position = transform.position;
            }
        }
        Destroy(gameObject);
    }

}
