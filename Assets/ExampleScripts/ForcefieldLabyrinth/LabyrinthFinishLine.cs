using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthFinishLine : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject gameManager = GameObject.Find("Manager");
            gameManager.GetComponent<LabyrinthGameManager>().LevelCompleted();

        }
    }
}
