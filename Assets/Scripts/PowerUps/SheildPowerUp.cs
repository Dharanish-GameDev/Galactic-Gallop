using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheildPowerUp : MonoBehaviour
{
    public void DisableSheild()
    {
        PowerUpManager.instance.DisableSheildAfterTime();
    }
}
