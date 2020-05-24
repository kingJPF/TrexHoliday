using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
/// <summary>
/// 有关card内的各个area的判定都在这个script中
/// </summary>
public class Card : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    //public GameObject thiscard;
    public Text title;
    public int previousScore5;
    public int previousScore6;
    public int previousScore7;
    public int previousScore11;
    public GameObject Area1;
    public GameObject Area2;
    public GameObject Area3;
    public GameObject Area4;
    public GameObject Area5;
    public GameObject Area6;
    public GameObject Area7;
    public GameObject Area8;
    public GameObject Area9;
    public GameObject Area10;
    public GameObject Area11;
    public GameObject score;
    public GameObject biscuit;
    public List<GameObject> list; //Area11的前6个部分

    public int resultOf11 = 0;
    public GameObject Score1,Score2,Score3;
    public GameObject MustTrex1,MustTrex2,MustTrex3; //三个必须放trexdie的区域
    public bool nextTrexDieMustPutInArea11;
    public bool Score1IsShown =false; //Area11的第一个Eraser
    public bool Score2IsShown =false; //Area11的第二个Eraser
    public bool Score3IsShown =false; //Area11的第三个Eraser
    public bool isShown5=false;
    public bool isShown6=false;
    public bool isShown7=false; //5,6,7后的Eraser
    public int totalEmptyNumber;
    public bool Area12Full;//每个区域是否还能填进去Trex Die
    public bool Area34Full;
    public bool Area56Full;


    private void Awake()
    {
        //this.name = "Player" + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "Card";
        this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        this.transform.SetSiblingIndex(1);
    }
    void Start()
    {
        previousScore5 = 0;
        previousScore6 = 0;
        previousScore7 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        title.text = this.name;
        if (photonView.IsMine)
        {
            GetComponent<PhotonView>().RPC("UpdateName", RpcTarget.All, this.name);
        }
        nextTrexDieMustPutInArea11 = MustTrex1.GetComponentInChildren<textUpdate>().mustPutTrexDieThisTurn||
                                        MustTrex2.GetComponentInChildren<textUpdate>().mustPutTrexDieThisTurn||
                                        MustTrex3.GetComponentInChildren<textUpdate>().mustPutTrexDieThisTurn;
        CheckNumInArea2();
        CheckNumInArea3();
        CheckNumInArea4();
        previousScore5= CheckAndAddScoreInLineArea(previousScore5,GetScoreOfLineArea( CheckNumInLineArea(Area5,isShown5)));
        previousScore6= CheckAndAddScoreInLineArea(previousScore6,GetScoreOfLineArea( CheckNumInLineArea(Area6,isShown6)));
        previousScore7= CheckAndAddScoreInLineArea(previousScore7,GetScoreOfLineArea( CheckNumInLineArea(Area7,isShown7)));
        CheckNumInSumArea(Area8);
        CheckNumInSumArea(Area9);
        CheckNumInSumArea(Area10);
        CheckNumInArea11();
        int Score11Now = GetScoreOfArea11(resultOf11,ref Score1IsShown,ref Score2IsShown, ref Score3IsShown);
        previousScore11 = CheckAndAddScoreInLineArea(previousScore11,Score11Now);
        totalEmptyNumber = CheckEmptyNum(gameObject);
        Area12Full = (CheckEmptyNum(Area1)==0)&& (CheckEmptyNum(Area2)==0)&&(CheckEmptyNum(Area3)==0)&&(CheckEmptyNum(Area4)==0);
        Area34Full = (CheckEmptyNum(Area5)==0)&& (CheckEmptyNum(Area6)==0)&&(CheckEmptyNum(Area7)==0);
        Area56Full = (CheckEmptyNum(Area8)==0)&& (CheckEmptyNum(Area9)==0)&&(CheckEmptyNum(Area10)==0)&&(CheckEmptyNum(Area11)==0);
        if(totalEmptyNumber == 0){
            GameOver();
        }

    }
    public void CheckNumInArea1()
    {
        ArrayList Nums = new ArrayList();
        foreach(Transform Num in Area1.GetComponentsInChildren<Transform>())
        {
            if(Num.tag == "CardNum")
                Nums.Add(Num.GetComponent<Text>().text.ToString());
        }
        string tmp = Nums[0].ToString();
        for(int i = 0; i < Nums.Count; i++){
            if(Regex.Match(Nums[i].ToString(),@"\d").Success){
                tmp =  Nums[i].ToString();
                break;
            }
        }
        for(int i = 0; i < Nums.Count; i++)
        {
            if (Nums[i].ToString() != "" && (Nums[i].ToString() == tmp||Nums[i].ToString()=="☆"))
                continue;
            else
                return;
        }
        for(int i = 0; i<Nums.Count;i++){
            if(Nums[i].ToString()=="☆"){
                continue;
            }
            else{
                tmp = Nums[i].ToString();
                break;
            }
        }
        AddScore(tmp);

        foreach (Transform Num in Area1.GetComponentsInChildren<Transform>())
        {
            if (Num.tag == "CardNum")
                Num.GetComponentInChildren<Text>().text = "";
        }
    }
    public void CheckNumInArea2()
    {
        ArrayList Nums = new ArrayList();
        foreach(Transform Num in Area2.GetComponentsInChildren<Transform>())
        {
            if(Num.tag == "CardNum")
                Nums.Add(Num.GetComponent<Text>().text.ToString());
        }
        string tmp = Nums[0].ToString();
        for(int i = 0; i < Nums.Count; i++){
            if(Regex.Match(Nums[i].ToString(),@"\d").Success){
                tmp =  Nums[i].ToString();
                break;
            }
        }
        for(int i = 0; i < Nums.Count; i++)
        {
            if (Nums[i].ToString() != "" && (Nums[i].ToString() == tmp||Nums[i].ToString()=="☆"))
                continue;
            else
                return;
        }
        for(int i = 0; i<Nums.Count;i++){
            if(Nums[i].ToString()=="☆"){
                continue;
            }
            else{
                tmp = Nums[i].ToString();
                break;
            }
        }
        AddScore(tmp);
        AddScore("2");
        foreach (Transform Num in Area2.GetComponentsInChildren<Transform>())
        {
            if (Num.tag == "CardNum")
                Num.GetComponentInChildren<Text>().text = "";
        }
    }
    public void CheckNumInArea3()
    {
        {
        ArrayList Nums = new ArrayList();
        foreach(Transform Num in Area3.GetComponentsInChildren<Transform>())
        {
            if(Num.tag == "CardNum")
                Nums.Add(Num.GetComponent<Text>().text.ToString());
        }
        string tmp = Nums[0].ToString();
        for(int i = 0; i < Nums.Count; i++){
            if(Regex.Match(Nums[i].ToString(),@"\d").Success){
                tmp =  Nums[i].ToString();
                break;
            }
        }
        for(int i = 0; i < Nums.Count; i++)
        {
            if (Nums[i].ToString() != "" && (Nums[i].ToString() == tmp||Nums[i].ToString()=="☆"))
                continue;
            else
                return;
        }
        for(int i = 0; i<Nums.Count;i++){
            if(Nums[i].ToString()=="☆"){
                continue;
            }
            else{
                tmp = Nums[i].ToString();
                break;
            }
        }
        AddScore(tmp);
        biscuit.SetActive(true);
        foreach (Transform Num in Area3.GetComponentsInChildren<Transform>())
        {
            if (Num.tag == "CardNum")
                Num.GetComponentInChildren<Text>().text = "";
        }
    }
    }
    public void CheckNumInArea4()
    {
        ArrayList Nums = new ArrayList();
        foreach(Transform Num in Area4.GetComponentsInChildren<Transform>())
        {
            if(Num.tag == "CardNum")
                Nums.Add(Num.GetComponent<Text>().text.ToString());
        }
        string tmp = Nums[0].ToString();
        for(int i = 0; i < Nums.Count; i++){
            if(Regex.Match(Nums[i].ToString(),@"\d").Success){
                tmp =  Nums[i].ToString();
                break;
            }
        }
        for(int i = 0; i < Nums.Count; i++)
        {
            if (Nums[i].ToString() != "" && (Nums[i].ToString() == tmp||Nums[i].ToString()=="☆"))
                continue;
            else
                return;
        }
        for(int i = 0; i<Nums.Count;i++){
            if(Nums[i].ToString()=="☆"){
                continue;
            }
            else{
                tmp = Nums[i].ToString();
                break;
            }
        }
        AddScore(tmp);
        AddScore(tmp);
        foreach (Transform Num in Area4.GetComponentsInChildren<Transform>())
        {
            if (Num.tag == "CardNum")
                Num.GetComponentInChildren<Text>().text = "";
        }
    }
    public int CheckAndAddScoreInLineArea(int previousScore, int currentScore){
        if(previousScore!=currentScore){
            MinusScore(previousScore);
            AddScore(currentScore.ToString());
            previousScore = currentScore;
        }
        return previousScore;
    }
    public int CheckNumInLineArea(GameObject area,bool isShown){
        ArrayList Nums = new ArrayList();
        int max=0;
        int last =-1;
        int a=0;
        foreach(Transform Num in area.GetComponentsInChildren<Transform>())
        {
            if(Num.tag == "CardNum")
                Nums.Add(Num.GetComponent<Text>().text.ToString());
        }
        for(int i = 0; i < Nums.Count; i++){
            if(Nums[i].ToString()=="☆"){
                if(Nums[i+1].ToString()=="/"){
                    for(int j=i+1;j<Nums.Count;j++){
                    Nums[j] = "";
                }
                }
                else continue;
            }
            else if(Regex.Match(Nums[i].ToString(),@"\d").Success){
            if((int)float.Parse(Nums[i].ToString())==6){
                last = i;
                for(int j=i+1;j<Nums.Count;j++){
                    Nums[j] = "/";
                }
                 if(!isShown){
                    biscuit.SetActive(true);
                    if(area.name == "Area5")
                        isShown5=true;
                    else if(area.name == "Area6")
                        isShown6=true;
                    else if(area.name == "Area7")
                        isShown7=true;
                 }
                break;
            }
            else if((int)float.Parse(Nums[i].ToString())>max){
                max = (int)float.Parse(Nums[i].ToString());
                last = i;
                continue;
            }
            else if((int)float.Parse(Nums[i].ToString())<=max&&last!=-1){
                for(int j=i+1;j<Nums.Count;j++){
                    Nums[j] = "/";
                }
                last = -1;
                break;
            }
            }
            else{
                last = i-1;
                break;
            }
        }
        //Debug.Log("max"+max+"last"+last);
        foreach(Transform Num in area.GetComponentsInChildren<Transform>())
        {
            if(Num.tag == "CardNum"){
                Num.GetComponent<Text>().text = Nums[a].ToString();
                a+=1;
            }
        }
        return last;
    }
    public void CheckNumInSumArea(GameObject area){
        ArrayList Nums = new ArrayList();
        int sum = 0;
        foreach(Transform Num in area.GetComponentsInChildren<Transform>())
        {
            if(Num.tag == "CardNum")
                Nums.Add(Num.GetComponent<Text>().text.ToString());
        }
        for(int i = 0; i < Nums.Count; i++){
            if(Regex.Match(Nums[i].ToString(),@"\d").Success){
                sum+=(int)float.Parse(Nums[i].ToString());
            }
        }
        if(sum == 15){
            AddScore("2");
            foreach (Transform Num in area.GetComponentsInChildren<Transform>())
        {
            if (Num.tag == "CardNum")
                Num.GetComponentInChildren<Text>().text = "";
        }
        }

        else if(sum == 14){
            AddScore("5");
            foreach (Transform Num in area.GetComponentsInChildren<Transform>())
        {
            if (Num.tag == "CardNum")
                Num.GetComponentInChildren<Text>().text = "";
        }
        }
    }
    public void CheckNumInArea11(){
        ArrayList Nums = new ArrayList();
        resultOf11 =0;
        int min = 7;
        int a = 0;
        foreach(GameObject num in list)
        {
            foreach(Transform numChildern in num.GetComponentsInChildren<Transform>()){
                if(numChildern.GetComponentInChildren<Transform>().tag == "CardNum"){
                Nums.Add(numChildern.GetComponentInChildren<Text>().text.ToString());
            }
            }
        }
        //Debug.Log(Nums.Count);
        for(int i = 0; i < Nums.Count; i++){
            if(Nums[i].ToString()=="☆"){
                if(Nums[i+1].ToString()=="/"){
                    for(int j=i+1;j<Nums.Count;j++){
                        Nums[j]="";
                    }
                }
                resultOf11+=1;
                continue;
            }
            else if (Regex.Match(Nums[i].ToString(),@"\d").Success){
                int compareNum = (int)float.Parse(Nums[i].ToString());
                if(compareNum<min){
                    min = compareNum;
                    resultOf11+=1;
                }
                else{
                    for(int j = i+1;j<Nums.Count;j++){
                        Nums[j]="/";
                    }
                    resultOf11 = 0;
                    break;
                }
            }
        }
        foreach(GameObject num in list){
            num.GetComponentInChildren<Text>().text = Nums[a].ToString();
            a+=1;
        }
    }
    public void AddScore(string num)
    {
        int total;
        total = (int)float.Parse(score.GetComponentInChildren<Text>().text.ToString()) + (int)float.Parse(num);
        score.GetComponentInChildren<Text>().text = total.ToString();
    }
    public int GetScoreOfLineArea (int num){
        int score=0;

        switch(num){
            case 1:
                score=1;
                break;
            case 2:
                score=3;
                break;
            case 3:
                score=6;
                break;
            case 4:
                score=10;
                break;
            case 5:
                score=15;
                break;
        }
        return score;
    }
    public int GetScoreOfArea11(int result, ref bool isShown1, ref bool isShown2, ref bool isShown3){
        int score = 0;

        switch(result){
            case 0:
                score = 0;
                break;
            case 1:
                if(Regex.Match(Score1.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score1.GetComponentInChildren<Text>().text.ToString());
                }
                break;
            case 2:
                if(Regex.Match(Score1.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score1.GetComponentInChildren<Text>().text.ToString());
                }
                if(!isShown1){
                    biscuit.SetActive(true);
                    isShown1 = true;
                    Debug.Log("Eraser1 appeared");
                }
                break;
            case 3:
                if(Regex.Match(Score1.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score1.GetComponentInChildren<Text>().text.ToString());
                }
                if(Regex.Match(Score2.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score2.GetComponentInChildren<Text>().text.ToString());
                }
                break;
            case 4:
                if(Regex.Match(Score1.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score1.GetComponentInChildren<Text>().text.ToString());
                }
                if(Regex.Match(Score2.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score2.GetComponentInChildren<Text>().text.ToString());
                }
                if(!isShown2){
                    biscuit.SetActive(true);
                    isShown2 = true;
                }
                break;
            case 5:
                if(Regex.Match(Score1.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score1.GetComponentInChildren<Text>().text.ToString());
                }
                if(Regex.Match(Score2.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score2.GetComponentInChildren<Text>().text.ToString());
                }
                if(Regex.Match(Score3.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score3.GetComponentInChildren<Text>().text.ToString());
                }
                break;
            case 6:
                if(Regex.Match(Score1.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score1.GetComponentInChildren<Text>().text.ToString());
                }
                if(Regex.Match(Score2.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score2.GetComponentInChildren<Text>().text.ToString());
                }
                if(Regex.Match(Score3.GetComponentInChildren<Text>().text.ToString(),@"\d").Success){
                    score+= (int)float.Parse(Score3.GetComponentInChildren<Text>().text.ToString());
                }
                if(!isShown3){
                    biscuit.SetActive(true);
                    isShown3 = true;
                }
                break;
        }

        return score;
    }
    public void MinusScore(int num){
        int total;
        total = (int)float.Parse(score.GetComponentInChildren<Text>().text.ToString()) - num;
        score.GetComponentInChildren<Text>().text = total.ToString();
    }

    public int CheckEmptyNum(GameObject area){
        int emptyNum = 0;
        foreach(Transform a in area.GetComponentsInChildren<Transform>()){
            //Debug.Log(a.name);
            if(a.tag == "CardNum"&&a.GetComponent<Text>().text==""){
                emptyNum+=1;
            }
        }
        return emptyNum;
    }
    public void GameOver(){
        GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("GameOver",RpcTarget.All);
    }
    [PunRPC]
    public void UpdateName(string name1)
    {
        this.name = name1;
    }
}
