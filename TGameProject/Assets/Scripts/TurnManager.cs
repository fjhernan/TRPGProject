using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject Players;
    public GameObject Enemies;
    private PartyManager pComponent;
    private EnemyManager eComponent;


    private void Start()
    {
        pComponent = Players.GetComponent<PartyManager>();
        eComponent = Enemies.GetComponent<EnemyManager>();
    }

    public void UpdateCurrentTurn(bool value){
        if(value == false){
            Debug.Log("It is now enemies turn");
            pComponent.UpdateTurn(false);
            eComponent.UpdateTurn(true);
        }
        else if(value == true)
        {
            Debug.Log("It is not Players turn");
            pComponent.UpdateTurn(true);
            eComponent.UpdateTurn(false);
        }
    }
}
