using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{

    public int HealthItemRecoveryValue;

    [SerializeField]
    private float lifetime;

    public float waitTime;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if(waitTime > 0)
        {
            waitTime = waitTime - Time.deltaTime;
        }
    }
}
