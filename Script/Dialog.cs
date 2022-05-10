using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{

    [SerializeField, Header("âÔòbï∂èÕ"), Multiline(3)]
    private string[] lines;

    private bool Talk;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && Talk && !GameManager.instance.dialogBox.activeInHierarchy)
        {
            GameManager.instance.ShowDialog(lines);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Talk = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Talk = false;

            GameManager.instance.ShowDialogChenge(Talk);
        }
    }
}
