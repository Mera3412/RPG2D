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
    public int currentHealth;�@//���݂�HP
    public int maxHealth;      //�ő��HP


    public float totalstamina, recoveryspeed;�@//max�X�^�~�i�A�X�^�~�i�񕜑��x

    [System.NonSerialized]
    public float currentStamina; //���݂̃X�^�~�i

    [SerializeField]
    private float dashspeed, dashLength, dashCost;�@//����X�s�[�h�A���鎞�ԁA����̂ɏ����X�^�~�i
  
    private float dashCounter, activeMoveSpeed;


    private bool knockback;�@//�m�b�N�o�b�N
    private Vector2 knockdir; //�m�b�N�o�b�N��������

    [SerializeField]
    private float knockbackTime, knockbackForce; //
    private float knockbackCounter;

    [SerializeField]
    private float invincivirityTime;
    private float invincivirityCounter;




    void Start()
    {
        currentHealth = maxHealth;�@//�Q�[�����n�܂����Ƃ���HP���ő�
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

        if (rb.velocity != Vector2.zero)  // rb.velocity(�ړ����x)���O�łȂ��Ƃ�
        {
            if (Input.GetAxisRaw("Horizontal") != 0)�@//Horizontal(����)���󂯎���Ă���̂�
            {
                if (Input.GetAxisRaw("Horizontal") > 0)�@//�E�������Ă���Ƃ�
                {
                    playerAnim.SetFloat("X", 1f);
                    playerAnim.SetFloat("Y", 0);

                    WeponAnim.SetFloat("X", 1f);
                    WeponAnim.SetFloat("Y", 0);
                }

                else // ���������Ă���Ƃ�
                {
                    playerAnim.SetFloat("X", -1f);
                    playerAnim.SetFloat("Y", 0);

                    WeponAnim.SetFloat("X", -1f);
                    WeponAnim.SetFloat("Y", 0);
                }

            }
            else if (Input.GetAxisRaw("Vertical") > 0) //Vertical(�c��)���󂯎���Ă���̂�
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 1);

                WeponAnim.SetFloat("X", 0);
                WeponAnim.SetFloat("Y", 1);
            }
            else // ��
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
            if (Input.GetKeyDown(KeyCode.LeftShift) && currentStamina > dashCost) //�X�^�~�i������΁~
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


        currentStamina = Mathf.Clamp(currentStamina + recoveryspeed * Time.deltaTime, 0, totalstamina); //100�𒴂���X�^�~�i�񕜂��~
        GameManager.instance.UpdateStaminaUI();
    }

    public void KnockBack(Vector3 position)
    {
        knockbackCounter = knockbackTime;
        knockback = true;

        knockdir = transform.position - position; //�v���C���[�̌��ݒn����G�l�~�[�̌��ݒn������

        knockdir.Normalize();
    }

    public void DamagePlayer(int damage)
    {
        if(invincivirityCounter <= 0)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth); //�O�ȉ��̏ꍇ�O�Ɏ���

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
