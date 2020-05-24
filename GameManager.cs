using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject ReadyButton;
    public GameObject Dice;
    public GameObject Canvas;
    public GameObject EnterReadyButton;
    [Header("UI")]
    public GameObject UIObject;
    public GameObject TurnUI;
    public bool ConfirmButtonIsClicked = false;
    public bool ConfirmButtonCanShow = false;
    public GameObject turn;
    [Header("Three Dice")]
    public GameObject RollButton;
    public GameObject DiceUI;
    public GameObject Dice1;
    public GameObject Dice2;
    public GameObject Dice3;

    public GameObject Dice1postion;
    public GameObject Dice2postion;
    public GameObject Dice3postion;
    public GameObject ConfirmButton;
    public GameObject Player1;
    public  GameObject Player2;
    public  GameObject Player3;
    public GameObject yesButton;
    public GameObject GameOverText;
    public GameObject myCard;
    [Header("TurnController")]
    public int confirmNum;
    public int turnNum;
    public void ReadyToPlay()
    {
        ReadyButton.SetActive(false);
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Player1 = PhotonNetwork.Instantiate("Player1", new Vector3(350,150, 0), Quaternion.identity, 0);
            var clone = PhotonNetwork.Instantiate("Card", new Vector3(0, 0, 0), Quaternion.identity, 0);
            clone.name = "Player1Card";
            photonView.RPC("UpdatePlayer",RpcTarget.All);
            Debug.Log("Player1Coming");
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Player2 = PhotonNetwork.Instantiate("Player2", new Vector3(350, 100, 0), Quaternion.identity, 0);
            var clone = PhotonNetwork.Instantiate("Card", new Vector3(0, 0, 0), Quaternion.identity, 0);
            clone.name = "Player2Card";
            photonView.RPC("UpdatePlayer",RpcTarget.All);
            Debug.Log("Player2Coming");
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            Player3 = PhotonNetwork.Instantiate("Player3", new Vector3(350, 50, 0), Quaternion.identity, 0);
            PhotonNetwork.Instantiate("Card", new Vector3(0, 0, 0), Quaternion.identity, 0).name = "Player3Card";
            photonView.RPC("UpdatePlayer",RpcTarget.All);
            Debug.Log("Player3Coming");
        }
        //StartNextTurn();
    }
    public void ClickOnThisTurnOverButtton()
    {
        DiceUI.SetActive(false);
        GameObject[] cardList =  GameObject.FindGameObjectsWithTag("card");
        foreach(GameObject card in cardList)
        {
            if (card.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log(card.name);
                UpdateCardNumbers(card);
            }
        }
    }
    public void UpdateCardNumbers(GameObject card)
    {
        foreach (Transform numChild in card.GetComponentsInChildren<Transform>(true))
        {
            Debug.Log(numChild.name);
            if (numChild.tag == "CardNum")
            {
                numChild.GetComponent<PhotonView>().RPC("UpdateDice", RpcTarget.Others, numChild.GetComponent<Text>().text.ToString());
            }
        }
    }
    private void Awake()
    {
        UIObject.transform.SetAsLastSibling();
    }
    private void Start() {
    }
    private void Update()
    {

        if(myCard == null){
            GameObject[] cardList =  GameObject.FindGameObjectsWithTag("card");
            foreach(GameObject card in cardList)
            {
                if (card.GetComponent<PhotonView>().IsMine)
                {
                    myCard = card;
                }
            }
        }//获取属于自己的卡片
        CheckConfirmButton();
        turn.GetComponent<Text>().text = confirmNum.ToString()+"Players have confirmed"; //多少人确认
        TurnUI.GetComponent<Text>().text = "Player "+turnNum.ToString()+" turn"; //第几回合
        CheckTurnOver();
    }
    public void Player1First()
    {
        GameObject.Find("Player1Card").GetComponent<Transform>().SetSiblingIndex(5);
    }
    public void Player2First()
    {
        GameObject.Find("Player2Card").GetComponent<Transform>().SetSiblingIndex(5);
    }
    public void Player3First()
    {
        GameObject.Find("Player3Card").GetComponent<Transform>().SetSiblingIndex(5);
    }
    public void setBiscuitActiveFalse(){
        GameObject[] Biscuitlist=GameObject.FindGameObjectsWithTag("Eraser");
        foreach(GameObject a in Biscuitlist){
            if(a.GetComponentInChildren<PhotonView>().IsMine){
                a.SetActive(false);
            }
        }
    }
    public void PressConfirmButton()
    {
        if (!ConfirmButtonIsClicked)
        {
            ConfirmButtonIsClicked = true;
            GetComponent<PhotonView>().RPC("UpdateConfirmButton",RpcTarget.All);
        }
    }
    public void CheckTurnOver(){
        if(confirmNum == PhotonNetwork.CurrentRoom.PlayerCount){
            Debug.Log("turnOver");
            turnNum = turnNum%PhotonNetwork.CurrentRoom.PlayerCount+1;
            GetComponent<PhotonView>().RPC("setDiceActive",RpcTarget.All);
            ConfirmButtonIsClicked = false;
            confirmNum = 0;
            StartNextTurn();
        }
    }
    public void ResetDices(GameObject dice,GameObject dicePostion){
        dice.transform.position = dicePostion.transform.position;
        dice.SetActive(true);
        dice.GetComponentInChildren<PhotonView>().RPC("UpdateDice",RpcTarget.All,"?");
        dice.GetComponentInChildren<PhotonView>().RPC("ResetDice",RpcTarget.All);
    }
    public void GameOverTest(){
        gameObject.GetComponent<PhotonView>().RPC("GameOver",RpcTarget.All);
    }
    [PunRPC]
    public void GameOver(){
        GameObject[] scoreList = GameObject.FindGameObjectsWithTag("Score");
        int max = -1;
        GameObject maxCard = scoreList[0];
        foreach(GameObject score in scoreList){
            int sco  = (int)float.Parse(score.GetComponent<Text>().text);
            if(sco > max){
                max = sco;
                maxCard = score;
            }
        }
        Debug.Log(maxCard.GetComponent<PhotonView>().Controller.NickName);
        GameOverText.SetActive(true);
        GameOverText.transform.SetAsLastSibling();
        GameOverText.GetComponent<Text>().text = maxCard.GetComponent<PhotonView>().Controller.NickName+" Wins";
        AudioManager.PauseBgm();
    }
    public void StartNextTurn(){
        if(turnNum == Player1.GetComponent<PhotonView>().OwnerActorNr && Player1.GetComponent<PhotonView>().IsMine)
        {
            RollButton.SetActive(true);
        }
        else if(turnNum == Player2.GetComponent<PhotonView>().OwnerActorNr && Player2.GetComponent<PhotonView>().IsMine)
        {
            RollButton.SetActive(true);
        }
        else if(turnNum == Player3.GetComponent<PhotonView>().OwnerActorNr && Player3.GetComponent<PhotonView>().IsMine)
        {
            RollButton.SetActive(true);
        }
    }
    [PunRPC]
    public void setDiceActive()
    {
        Dice.SetActive(true);
        ResetDices(Dice1,Dice1postion);
        ResetDices(Dice2,Dice2postion);
        ResetDices(Dice3,Dice3postion);
        yesButton.SetActive(false);
    }
    [PunRPC]
    void UpdateConfirmButton(){
        confirmNum+=1;
    }
    public void UpdateName(string name1)
    {
        this.name = name1;
    }
    public void CheckConfirmButton()//检查确认键的出现情况
    {
        if (!EnterReadyButton.activeInHierarchy)
        {
            if (!Dice1.activeInHierarchy && !Dice2.activeInHierarchy && !Dice3.activeInHierarchy)
            {
                ConfirmButtonCanShow = true;
            }
            else if (!Dice1.activeInHierarchy && !Dice2.activeInHierarchy && Dice3.GetComponentInChildren<Text>().text == "?")
            {
                ConfirmButtonCanShow = true;
            }
            else if (!Dice2.activeInHierarchy && !Dice3.activeInHierarchy && Dice1.GetComponentInChildren<Text>().text == "?")
            {
                ConfirmButtonCanShow = true;
            }
            else if (!Dice1.activeInHierarchy && !Dice3.activeInHierarchy && Dice2.GetComponentInChildren<Text>().text == "?")
            {
                ConfirmButtonCanShow = true;
            }
            else if (!Dice1.activeInHierarchy && !Dice2.activeInHierarchy && Dice3.GetComponentInChildren<textUpdate>().isTrexDie ==true){
                if(Dice3.GetComponentInChildren<Text>().text =="1"||Dice3.GetComponentInChildren<Text>().text =="2"){
                    if(myCard.GetComponent<Card>().Area12Full==true) ConfirmButtonCanShow = true;
                }
                else if(Dice3.GetComponentInChildren<Text>().text =="3"||Dice3.GetComponentInChildren<Text>().text =="4"){
                    if(myCard.GetComponent<Card>().Area34Full==true) ConfirmButtonCanShow = true;
                }
                else if(Dice3.GetComponentInChildren<Text>().text =="5"||Dice3.GetComponentInChildren<Text>().text =="6"){
                    if(myCard.GetComponent<Card>().Area56Full==true) ConfirmButtonCanShow = true;
                }
            }
            else if (!Dice2.activeInHierarchy && !Dice3.activeInHierarchy && Dice1.GetComponentInChildren<textUpdate>().isTrexDie ==true){
                if(Dice1.GetComponentInChildren<Text>().text =="1"||Dice1.GetComponentInChildren<Text>().text =="2"){
                    if(myCard.GetComponent<Card>().Area12Full==true) ConfirmButtonCanShow = true;
                }
                else if(Dice1.GetComponentInChildren<Text>().text =="3"||Dice1.GetComponentInChildren<Text>().text =="4"){
                    if(myCard.GetComponent<Card>().Area34Full==true) ConfirmButtonCanShow = true;
                }
                else if(Dice1.GetComponentInChildren<Text>().text =="5"||Dice1.GetComponentInChildren<Text>().text =="6"){
                    if(myCard.GetComponent<Card>().Area56Full==true) ConfirmButtonCanShow = true;
                }
            }
            else if (!Dice1.activeInHierarchy && !Dice3.activeInHierarchy && Dice2.GetComponentInChildren<textUpdate>().isTrexDie ==true){
                if(Dice2.GetComponentInChildren<Text>().text =="1"||Dice2.GetComponentInChildren<Text>().text =="2"){
                    if(myCard.GetComponent<Card>().Area12Full==true) ConfirmButtonCanShow = true;
                }
                else if(Dice2.GetComponentInChildren<Text>().text =="3"||Dice2.GetComponentInChildren<Text>().text =="4"){
                    if(myCard.GetComponent<Card>().Area34Full==true) ConfirmButtonCanShow = true;
                }
                else if(Dice2.GetComponentInChildren<Text>().text =="5"||Dice2.GetComponentInChildren<Text>().text =="6"){
                    if(myCard.GetComponent<Card>().Area56Full==true) ConfirmButtonCanShow = true;
                }
            }
            else
            {
                ConfirmButtonCanShow = false;
            }
        }

        if (ConfirmButtonCanShow == true && ConfirmButtonIsClicked == false )
        {
            ConfirmButton.SetActive(true);
        }
        else
        {
            ConfirmButton.SetActive(false);
        }
    }
    [PunRPC]
    public void UpdatePlayer(){
        switch(PhotonNetwork.CurrentRoom.PlayerCount){
            case 1:
                this.Player1 = (GameObject.Find("Player1(Clone)"));
                break;
            case 2:
                this.Player1 = (GameObject.Find("Player1(Clone)"));
                this.Player2 = (GameObject.Find("Player2(Clone)"));
                break;
            case 3:
                this.Player1 = (GameObject.Find("Player1(Clone)"));
                this.Player2 = (GameObject.Find("Player2(Clone)"));
                this.Player3 = (GameObject.Find("Player3(Clone)"));
                break;
        }
    }
}
