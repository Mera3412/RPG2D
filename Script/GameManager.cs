using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public static GameManager instance; 

    [SerializeField]
    private Slider hpSlider;

    [SerializeField]
    private PlayerController Player;

    [SerializeField]
    private Slider StSlider;


    public GameObject dialogBox;
    public Text dialogText;

    private string[] dialogLines;

    private int currentLine;

    private bool justStarted;



    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;　　//instanceの中にGameManagerが入る
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (!justStarted)
                {
                    currentLine++;
                    if(currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                    }
                    else
                    {
                        dialogText.text = dialogLines[currentLine];
                    }
                }
                else
                {
                    justStarted = false; //最初の文字送りはしない
                }
            }
        }
    }

    public void UpdateHealthUI()
    {
        hpSlider.maxValue = Player.maxHealth; //HPスライダーにマックスのHPのUIが入る
        hpSlider.value = Player.currentHealth;
    }

    public void UpdateStaminaUI()
    {
        StSlider.maxValue = Player.totalstamina; //HPスライダーにマックスのHPのUIが入る
        StSlider.value = Player.currentStamina;
    }

    public void ShowDialog(string[] Lines)
    {
        dialogLines = Lines;

        currentLine = 0;

        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);

        justStarted = true;
    }

    public void ShowDialogChenge(bool x)
    {
        dialogBox.SetActive(x);
    }
}
