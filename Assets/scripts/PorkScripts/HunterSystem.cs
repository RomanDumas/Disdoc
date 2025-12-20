using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterSystem : MonoBehaviour
{
    public Pork[] porks;
    public int Walkers;

    public void NightCheck()
    {
        foreach (Pork pork in porks)
        {
            if (pork.Funy == false)
            {
                pork.gameObject.SetActive(false);
                Walkers++;
            }
            pork.Funy = false;
        }
    }
}
