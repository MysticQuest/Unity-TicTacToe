using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public int SetMove(string[] ticksArray)
    {
        for (int i = 0; i < ticksArray.Length; i++)
        {
            if (string.IsNullOrEmpty(ticksArray[i]))
            {
                return i;
            }
        }
        return 8;
    }
}
