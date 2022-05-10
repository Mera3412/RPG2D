using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator EnemyAnim;

    [SerializeField]
    private float moveSpeed, waitTime, walkTime; //動くスピード、待ち時間、歩く時間

    private float waitCounter, moveCounter; 

    private Vector2 moveDir;　//動く方向

    [SerializeField]
    private BoxCollider2D area; //移動できるエリア 

    [SerializeField]
    private bool chase;　

    private bool isChaseing;　
    [SerializeField]
    private float chaseSpeed, rangeToChase; //追跡速度、追跡範囲
    private Transform target; //エネミーの場所

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

        target = GameObject.FindGameObjectWithTag("Player").transform; //ゲーム開始のプレイヤーの開始位置
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
        if (!isChaseing) //falseの時
        {
            if (waitCounter > 0)
            {
                waitCounter = waitCounter - Time.deltaTime;
                rb.velocity = Vector2.zero;

                if (waitCounter <= 0)
                {
                    moveCounter = walkTime;

                    moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));//ランダムに移動
                    moveDir.Normalize(); //正規化
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

            if(chase) //trueの時
            {
                if(Vector3.Distance(transform.position,target.transform.position) < rangeToChase)//エネミーとプレイヤーの距離
                {
                    isChaseing = true; //プレイヤーとエネミーの距離が近くなったので追跡開始
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


            if (Vector3.Distance(transform.position, target.transform.position) > rangeToChase)//エネミーとプレイヤーの距離
            {
                isChaseing = false; //プレイヤーとエネミーの距離が遠くなったので追跡終了

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
