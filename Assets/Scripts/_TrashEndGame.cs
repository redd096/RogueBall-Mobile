using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TrashEndGame : MonoBehaviour
{
    public GameObject endGame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 || GameObject.FindGameObjectsWithTag("Player").Length <= 0)
        {
            endGame.SetActive(true);
        }
    }
}
