using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class DIce : MonoBehaviourPun
{
    public string num;
    // Start is called before the first frame update
    public void StartDice()
    {
        num = Random.Range(1, 6).ToString();
    }
}
