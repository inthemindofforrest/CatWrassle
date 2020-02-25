using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfBounds : MonoBehaviour
{
    GameManager Manager;

    private void Start()
    {
        Manager = GameManager.FindManager();
    }

    private void OnCollisionEnter(Collision collision)
    {
        CharacterController Temp = collision.gameObject.GetComponent<CharacterController>();
        if(Temp)
        {
            if (Temp.CurrentPlayerNumber == 1) UltimateManager.UM.P1Score++;
            else UltimateManager.UM.P2Score++;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
