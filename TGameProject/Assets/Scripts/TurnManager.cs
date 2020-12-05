using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject Players;
    public GameObject Enemies;
    public GameObject UIManager;
    private PartyManager pComponent;
    private EnemyManager eComponent;
    private UIManager uComponent;

    private void Start(){
        pComponent = Players.GetComponent<PartyManager>();
        eComponent = Enemies.GetComponent<EnemyManager>();
        uComponent = UIManager.GetComponent<UIManager>();
    }

    public void UpdateCurrentTurn(bool value){
        if(value == false){
            //Enemy Turn Starts
            Debug.Log("It is enemies turn");
            uComponent.EnemyTurn();
            pComponent.UpdateTurn(false);
            eComponent.UpdateTurn(true);
        }
        else if(value == true){
            //Player Turn Starts
            Debug.Log("It is players turn");
            //uComponent.PlayerTurn();
            pComponent.UpdateTurn(true);
            eComponent.UpdateTurn(false);
        }
    }
}
