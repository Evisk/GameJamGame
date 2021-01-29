using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticPill : MonoBehaviour
{
    public void Active()
    {
        transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public void Deactive()
    {
        transform.GetChild(0).GetComponent<Image>().enabled = false;
    }
}
