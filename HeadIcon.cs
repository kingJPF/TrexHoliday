using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HeadIcon : MonoBehaviourPun
{
    public Text nameText;
    public GameObject Dice ;

    private void Awake()
    {
        this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>(),false);


        if (photonView.IsMine)
        {
            nameText.text = "YOU";
        }
        else
        {
            nameText.text = photonView.Owner.NickName;
        }
    }
    public void SetCardFirst()
    {
        Dice = GameObject.Find("DICE");
        Debug.Log(this.name.Replace("(Clone)", "") + "Card");
        GameObject.Find(this.name.Replace("(Clone)","")+"Card").transform.SetSiblingIndex(Dice.transform.GetSiblingIndex()-1);
    }
}
