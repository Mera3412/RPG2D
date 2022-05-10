using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private int moveSpeed;

    [SerializeField]
    private Animator playerAnim;

    public Rigidbody2D rb;

    [SerializeField]
    private Animator WeponAnim;

    [System.NonSerialized]
    public int currentHealth;　//現在のHP
    public int maxHealth;      //最大のHP


    public float totalstamina, recoveryspeed;　//maxスタミナ、スタミナ回復速度

    [System.NonSerialized]
    public float currentStamina; //現在のスタミナ

    [SerializeField]
    private float dashspeed, dashLength, dashCost;　//走るスピード、走る時間、走るのに消費するスタミナ
  
    private float dashCounter, activeMoveSpeed;


    private bool knockback;　//ノックバック
    private Vector2 knockdir; //ノックバックされる方向

    [SerializeField]
    private float knockbackTime, knockbackForce; //
    private float knockbackCounter;

    [SerializeField]
    private float invincivirityTime;
    private float invincivirityCounter;




    void Start()
    {
        currentHealth = maxHealth;　//ゲームが始まったときはHPが最大
        currentStamina = totalstamina;

        GameManager.instance.UpdateHealthUI();
        GameManager.instance.UpdateStaminaUI();


        activeMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if(invincivirityCounter > 0)
        {
            invincivirityCounter = invincivirityCounter - Time.deltaTime;
        }

        if(knockback)
        {
            knockbackCounter = knockbackCounter - Time.deltaTime;
            rb.velocity = knockdir * knockbackForce;

            if(knockbackCounter <= 0)
            {
                knockback = false;
            }
            else
            {
                return;
            }
        }
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * activeMoveSpeed;

        if (rb.velocity != Vector2.zero)  // rb.velocity(移動速度)が０でないとき
        {
            if (Input.GetAxisRaw("Horizontal") != 0)　//Horizontal(横軸)を受け取っているのか
            {
                if (Input.GetAxisRaw("Horizontal") > 0)　//右を向いているとき
                {
                    playerAnim.SetFloat("X", 1f);
                    playerAnim.SetFloat("Y", 0);

                    WeponAnim.SetFloat("X", 1f);
                    WeponAnim.SetFloat("Y", 0);
                }

                else // 左を向いているとき
                {
                    playerAnim.SetFloat("X", -1f);
                    playerAnim.SetFloat("Y", 0);

                    WeponAnim.SetFloat("X", -1f);
                    WeponAnim.SetFloat("Y", 0);
                }

            }
            else if (Input.GetAxisRaw("Vertical") > 0) //Vertical(縦軸)を受け取っているのか
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 1);

                WeponAnim.SetFloat("X", 0);
                WeponAnim.SetFloat("Y", 1);
            }
            else // 下
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", -1);

                WeponAnim.SetFloat("X", 0);
                WeponAnim.SetFloat("Y", -1);
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            WeponAnim.SetTrigger("Attack");
        }

        if (dashCounter <= 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && currentStamina > dashCost) //スタミナ無ければ×
            {
                activeMoveSpeed = dashspeed;
                dashCounter = dashLength;

                currentStamina = currentStamina - dashCost;

                GameManager.instance.UpdateStaminaUI();

            }
        }
        else 
        {
            dashCounter = dashCounter - Time.deltaTime;

            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
            }
        }


        currentStamina = Mathf.Clamp(currentStamina + recoveryspeed * Time.deltaTime, 0, totalstamina); //100を超えるスタミナ回復を止
        GameManager.instance.UpdateStaminaUI();
    }

    public void KnockBack(Vector3 position)
    {
        knockbackCounter = knockbackTime;
        knockback = true;

        knockdir = transform.position - position; //プレイヤーの現在地からエネミーの現在地を引く

        knockdir.Normalize();
    }

    public void DamagePlayer(int damage)
    {
        if(invincivirityCounter <= 0)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth); //０以下の場合０に収束

            invincivirityCounter = invincivirityTime;

            if(currentHealth == 0)
            {
                gameObject.SetActive(false);
            }
        }

        GameManager.instance.UpdateHealthUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.tag == "Item") && maxHealth != currentHealth && collision.GetComponent<Items>().waitTime <= 0)
        {
            Items items = collision.GetComponent<Items>();

            currentHealth = Mathf.Clamp(currentHealth + items.HealthItemRecoveryValue, 0, maxHealth);
            GameManager.instance.UpdateHealthUI();

            Destroy(collision.gameObject);
        }
    }
}
