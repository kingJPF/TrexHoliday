using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDice : MonoBehaviour
{
    public GameObject YesButton;
    public int selectedDiceNum;
    public GameObject remind2DiceText;
    public GameObject UIObject;
    public GameObject Dice;

    // Start is called before the first frame update
    private void Awake()
    {
        this.transform.SetSiblingIndex(UIObject.transform.GetSiblingIndex()-1);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckRoomNumber();
    }
    public void CheckRoomNumber()
    {
       int num = 0;
       var list = GameObject.FindGameObjectsWithTag("dice");
       foreach (var t in list)
        {
            if( t.GetComponent<textUpdate>().isSelected == true)
            {
                num += 1;
            }
        }
        selectedDiceNum = num;
        if (num == 2 && Dice.GetComponentInChildren<textUpdate>().alreadyChoosed == false)
        {
            YesButton.SetActive(true);
            remind2DiceText.SetActive(false);
        }
        else
        {
            YesButton.SetActive(false);
            remind2DiceText.SetActive(true);
        }

    }
}
