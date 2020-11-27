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
            pComponent.UpdateTurn(false);
            eComponent.UpdateTurn(true);
        }
        else if(value == true)
        {
            pComponent.UpdateTurn(true);
            eComponent.UpdateTurn(false);
        }
    }
}
