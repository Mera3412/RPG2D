using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator EnemyAnim;

    [SerializeField]
    private float moveSpeed, waitTime, walkTime; //�����X�s�[�h�A�҂����ԁA��������

    private float waitCounter, moveCounter; 

    private Vector2 moveDir;�@//��������

    [SerializeField]
    private BoxCollider2D area; //�ړ��ł���G���A 

    [SerializeField]
    private bool chase;�@

    private bool isChaseing;�@
    [SerializeField]
    private float chaseSpeed, rangeToChase; //�ǐՑ��x�A�ǐՔ͈�
    private Transform target; //�G�l�~�[�̏ꏊ

    private float waitAfterHitting;
   
    [SerializeField]
    private int attackDamage;

    [SerializeField]
    private int currentHealth;

    private bool Knockback;
    
    [SerializeField]
    private float knockbackTime, knockbackForce;

    private float knockbackCounter;

    private Vector2 knockdir;

    [SerializeField]
    private GameObject porsion;

    [SerializeField]
    private float healthdrop;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        EnemyAnim = GetComponent<Animator>();

        waitCounter = waitTime;

        target = GameObject.FindGameObjectWithTag("Player").transform; //�Q�[���J�n�̃v���C���[�̊J�n�ʒu
    }

    void Update()
    {
        if (Knockback)
        {
            if(knockbackCounter > 0)
            {
                knockbackCounter = knockbackCounter - Time.deltaTime;
                rb.velocity = knockdir * knockbackForce;
            }
            else
            {
                rb.velocity = Vector2.zero;

                Knockback = false;
            }

            return;
        }
        if (!isChaseing) //false�̎�
        {
            if (waitCounter > 0)
            {
                waitCounter = waitCounter - Time.deltaTime;
                rb.velocity = Vector2.zero;

                if (waitCounter <= 0)
                {
                    moveCounter = walkTime;

                    moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));//�����_���Ɉړ�
                    moveDir.Normalize(); //���K��
                }
            }
            else
            {
                moveCounter = moveCounter - Time.deltaTime;

                rb.velocity = moveDir * moveSpeed;

                if (moveCounter <= 0)
                {

                    waitCounter = waitTime;
                }
            }

            if(chase) //true�̎�
            {
                if(Vector3.Distance(transform.position,target.transform.position) < rangeToChase)//�G�l�~�[�ƃv���C���[�̋���
                {
                    isChaseing = true; //�v���C���[�ƃG�l�~�[�̋������߂��Ȃ����̂ŒǐՊJ�n
                }                                                                              

            }
        }
        else
        {
            if(waitCounter > 0)
            {
                waitCounter = waitCounter - Time.deltaTime;
                rb.velocity = Vector2.zero;

            }
            else
            {
                moveDir = target.transform.position - transform.position;
                moveDir.Normalize();

                rb.velocity = moveDir * chaseSpeed;
            }


            if (Vector3.Distance(transform.position, target.transform.position) > rangeToChase)//�G�l�~�[�ƃv���C���[�̋���
            {
                isChaseing = false; //�v���C���[�ƃG�l�~�[�̋����������Ȃ����̂ŒǐՏI��

                waitCounter = waitTime;

            }
        }
       
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (isChaseing)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();

                player.KnockBack(transform.position);
                player.DamagePlayer(attackDamage);

                waitCounter = waitAfterHitting;

            }
        }
    }

    public void KnockBack(Vector3 position)
    {
        Knockback = true;
        knockbackCounter = knockbackTime;

        knockdir = transform.position - position;
        knockdir.Normalize();

       
    }

    public void TakeDamege(int damage,Vector3 position)
    {
        currentHealth = currentHealth - damage;

        if(currentHealth <= 0)
        {

            if (Random.Range(0,100) < healthdrop && porsion != null)
            {
                Instantiate(porsion, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }

        KnockBack(position);
    }

}
