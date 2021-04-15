using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{

    public string victorySceneP1;
    public string victorySceneP2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (collision.GetComponent<Player>() == PlayerManager.instance.player1)
            {
                print("player1");
                SceneManager.LoadScene(victorySceneP1);
                return;
            }

            if (collision.GetComponent<Player>() == PlayerManager.instance.player2)
            {
                print("player2");
                SceneManager.LoadScene(victorySceneP2);
                return;
            }
        }
        
    }

}
