
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
public class textUpdate : MonoBehaviourPun
{

    public string diceNum;
    public bool isSelected = false;
    public bool alreadyChoosed = false;
    public bool isTrexDie = true;
    public bool canBePut = true;
    public bool mustPutTrexDieThisTurn = false; //下一个trexDie必须放在Area11;
    public bool canNotPutExceptArea11;

    public bool onlyForTrexDie = false;
    public GameObject Area;
    public GameObject PreNum;
    public GameObject PreNumIn11;
    public GameObject card;
    public int endNum;
    public int size;
    public GameObject remind2DiceText;
    // Update is called once per frame
    private void Start()
    {
        diceNum = "?";
        canBePut = true;
        GameObject[] cardList = GameObject.FindGameObjectsWithTag("card");
        foreach(GameObject a in cardList){
            if(a.GetComponent<PhotonView>().OwnerActorNr == gameObject.GetComponent<PhotonView>().OwnerActorNr){
                card = a;
            }
        }
    }
    void Update()
    {
        checkCanBePut();
        CheckMustPutTrexDie();
        checkBottom3fieldsInArea11();
        if(card){
            canNotPutExceptArea11 = card.GetComponent<Card>().nextTrexDieMustPutInArea11;
        }

    }

    public void OnClickDiceButton()
    {

        diceNum = Random.Range(1, 6).ToString();
        GetComponent<Text>().text = diceNum;
        //remind2DiceText.SetActive(true);

    }
    public void OnClickOverButton()
    {
        if (isSelected)
        {
            GetComponent<PhotonView>().RPC("UpdateDice", RpcTarget.All, diceNum);
            GetComponent<PhotonView>().RPC("UpdateBoolTrexDie",RpcTarget.Others);
        }
        else
        {
            GetComponent<PhotonView>().RPC("UpdateDice", RpcTarget.Others, "?");
        }
    }
    public void OnClickSelectButton()
    {
        if(!alreadyChoosed){
            if (!isSelected)
            {
                if(GetComponent<Text>().text == "?") return;
                GetComponent<Text>().color = Color.green;
                isSelected = true;
                isTrexDie = false;
            }
            else
            {
                GetComponent<Text>().color = Color.white;
                isSelected = false;
                isTrexDie = true;
            }
        }
    }
    public void OnClickYesButton(){
        if(!alreadyChoosed){
            alreadyChoosed = true;
        }
    }

    private void checkCanBePut(){
        if(Area){
            size = Area.transform.childCount;
            for(int i =0;i<endNum;i++){
                if(Area.transform.GetChild(i).GetComponentInChildren<Text>().text==""){
                    canBePut = false;
                    return;
                }

                if(PreNumIn11&&PreNumIn11.GetComponentInChildren<Text>().text == ""){
                    canBePut = false;
                    return;
                }
                canBePut = true;
            }
        }
    }

    private void CheckMustPutTrexDie(){
        if(PreNum){
            if(PreNum.transform.GetComponentInChildren<Text>().text!=""&&gameObject.GetComponentInChildren<Text>().text==""){
                mustPutTrexDieThisTurn = true;
                return;
            }
            mustPutTrexDieThisTurn = false;
        }
    }
    private void checkBottom3fieldsInArea11(){
        if(PreNumIn11&&PreNumIn11.GetComponentInChildren<Text>().text==""){
            if(gameObject.GetComponentInChildren<Text>().text=="/") PreNumIn11.GetComponentInChildren<Text>().text="/";
        }
        if(PreNumIn11&&PreNumIn11.GetComponentInChildren<Text>().text=="/"){
            if(gameObject.GetComponentInChildren<Text>().text=="") PreNumIn11.GetComponentInChildren<Text>().text="";
        }
    }
   [PunRPC]
    void UpdateDice(string diceNum) {
        Debug.Log(diceNum);
        string curNum = diceNum;
        GetComponent<Text>().text =curNum;
    }
    [PunRPC]
    void ResetDice(){
        GetComponent<Text>().color = Color.white;
        this.alreadyChoosed = false;
        this.isSelected = false;
        this.isTrexDie = true;
    }
    [PunRPC]
    void UpdateBoolTrexDie(){
        this.isTrexDie = false;
    }
}
