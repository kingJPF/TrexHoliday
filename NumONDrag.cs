using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Photon.Pun;
using Photon.Realtime;

public class NumONDrag :MonoBehaviourPunCallbacks,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Transform DicePostion;
    public GameObject alreadyPutNum;
    private void Awake()
    {
        DicePostion.position = transform.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

        transform.position = eventData.position;
        if(GetComponentInChildren<Text>().text != "?")
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        //endDragObject.transform.parent.parent.gameObject.name 区域名字
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject endDragObject = eventData.pointerCurrentRaycast.gameObject;
        if(eventData.selectedObject.tag =="GameController"){
            if(eventData.selectedObject.GetComponentInChildren<textUpdate>().isTrexDie == false){
                if (endDragObject ==null||!Regex.IsMatch(endDragObject.transform.parent.gameObject.name, @"NumIcon\(?\d*\)?")
                || endDragObject.GetComponentInChildren<Text>().text!=""|| !endDragObject.GetComponent<PhotonView>().IsMine
                ||endDragObject.GetComponentInChildren<Text>().text=="☆"||endDragObject.GetComponentInChildren<Text>().text=="/"
                ||endDragObject.GetComponentInChildren<textUpdate>().canBePut==false
                ||endDragObject.GetComponentInChildren<textUpdate>().onlyForTrexDie==true)
                {
                    transform.position = DicePostion.position;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                else
                {
                    alreadyPutNum = endDragObject;
                    endDragObject.GetComponentInChildren<Text>().text = transform.GetComponentInChildren<Text>().text;
                    AudioManager.PlayPutNumberAudio();
                    gameObject.SetActive(false);
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }
            // 拖动的数字是trexDie
            else{
                if(!endDragObject.transform.parent.parent||endDragObject.GetComponentInChildren<textUpdate>().canBePut==false
                || !endDragObject.GetComponent<PhotonView>().IsMine
                || (endDragObject.GetComponentInChildren<textUpdate>().canNotPutExceptArea11 == true
                && endDragObject.transform.parent.parent.gameObject.name != "Area11"))
                {
                    transform.position = DicePostion.position;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Debug.Log(endDragObject.transform.parent.name);
                    return;
                }
                string AreaName = endDragObject.transform.parent.parent.gameObject.name;
                string diceNum = eventData.selectedObject.GetComponentInChildren<Text>().text;
                if(endDragObject.GetComponentInChildren<textUpdate>().canNotPutExceptArea11){
                    if(AreaName=="Area11"&&endDragObject.GetComponentInChildren<textUpdate>().mustPutTrexDieThisTurn){
                        alreadyPutNum = endDragObject;
                        endDragObject.GetComponentInChildren<Text>().text = transform.GetComponentInChildren<Text>().text;
                        AudioManager.PlayPutNumberAudio();
                        gameObject.SetActive(false);
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        return;
                    }
                    else{
                         transform.position = DicePostion.position;
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        Debug.Log(endDragObject.transform.parent.name);
                        return;
                    }
                }
                if(diceNum == "1" || diceNum == "2"){
                    if(endDragObject.GetComponentInChildren<Text>().text==""&& (AreaName == "Area1"||AreaName == "Area2"
                    ||AreaName == "Area3" || AreaName == "Area4")){
                        alreadyPutNum = endDragObject;
                        endDragObject.GetComponentInChildren<Text>().text = transform.GetComponentInChildren<Text>().text;
                        AudioManager.PlayPutNumberAudio();
                        gameObject.SetActive(false);
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                    else{
                        transform.position = DicePostion.position;
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                }
                if(diceNum == "3" || diceNum == "4"){
                    if(endDragObject.GetComponentInChildren<Text>().text==""&& (AreaName == "Area5"||AreaName == "Area6"
                    ||AreaName == "Area7")&&endDragObject.GetComponentInChildren<textUpdate>().canBePut==true)
                    {
                        alreadyPutNum = endDragObject;
                        endDragObject.GetComponentInChildren<Text>().text = transform.GetComponentInChildren<Text>().text;
                        AudioManager.PlayPutNumberAudio();
                        gameObject.SetActive(false);
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                    else{
                        transform.position = DicePostion.position;
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                }
                if(diceNum == "5" || diceNum == "6"){
                    if(endDragObject.GetComponentInChildren<Text>().text==""&& (AreaName == "Area8"||AreaName == "Area9"
                    ||AreaName == "Area10"||AreaName == "Area11")){
                        alreadyPutNum = endDragObject;
                        endDragObject.GetComponentInChildren<Text>().text = transform.GetComponentInChildren<Text>().text;
                        AudioManager.PlayPutNumberAudio();
                        gameObject.SetActive(false);
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                    else{
                        transform.position = DicePostion.position;
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                }
                if(diceNum == "?"){
                    transform.position = DicePostion.position;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }

        }
        else if (eventData.selectedObject.tag == "Eraser"){
            if (endDragObject ==null||!Regex.IsMatch(endDragObject.transform.parent.gameObject.name, @"NumIcon\(?\d*\)?")
            || endDragObject.GetComponentInChildren<Text>().text==""|| !endDragObject.GetComponent<PhotonView>().IsMine||
            endDragObject.GetComponentInChildren<Text>().text=="☆"||endDragObject.GetComponentInChildren<Text>().text=="/"){
                transform.position = DicePostion.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else{
                endDragObject.GetComponentInChildren<Text>().text = "☆";
                transform.position = DicePostion.position;
                gameObject.SetActive(false);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
    }
    public void Undo()
    {
        alreadyPutNum.gameObject.GetComponentInChildren<Text>().text = "";
        transform.position = DicePostion.position;
        gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
