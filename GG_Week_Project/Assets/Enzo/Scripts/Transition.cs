using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Transition : MonoBehaviour
{
    public string Scene;

    [Range(1, 2)]
    public int ID;

    //ID 1 est �gale � joueur 1 qui doit aller � droite
    //ID 2 est �gale � joueur 2 qui doit aller � gauche


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((ID==1)&&(collision.transform == PlayerManager.instance.transformPlayer1))
        {
            SceneManager.LoadScene(Scene);
        }
        if ((ID == 2) && (collision.transform == PlayerManager.instance.transformPlayer2))
        {
            SceneManager.LoadScene(Scene);
        }
    }
}
