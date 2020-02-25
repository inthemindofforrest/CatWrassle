using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateManager : MonoBehaviour
{
    public static UltimateManager UM;

    public int P1Score;
    public int P2Score;

    private void Start()
    {
        if (UM == null) UM = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
