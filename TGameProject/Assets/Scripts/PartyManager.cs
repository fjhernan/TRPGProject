﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public List<GameObject> PartyMembers;
    private int currentMember = 0;
    private bool currentTurn = true;

    public void GameStart(){
        for(int i = 0; i < PartyMembers.Count; i++){
            if (i != currentMember){
                PartyMembers[i].GetComponent<Player>().DisableHighlight();
            }
            GameObject.Find("GameManager").GetComponent<GameManager>().AddPlayer(PartyMembers[i]);
        }
        PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);
        UpdatePlayersOccupiedTiles();
    }

    private void Update(){
        if (currentTurn == true) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (currentMember < PartyMembers.Count){
                    PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(false);
                    currentMember++;
                    GameObject.Find("GameManager").GetComponent<GameManager>().UpdateEveryonesTiles();
                    //Debug.Log("Player currently" + currentMember);
                    if (currentMember < PartyMembers.Count){
                        PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);
                    }
                    else{
                        GameObject.Find("TurnManager").GetComponent<TurnManager>().UpdateCurrentTurn(false);
                        currentMember = 0;
                    }
                }
                GameObject.Find("GameManager").GetComponent<GameManager>().UpdateEveryonesTiles();
            }
        }
    }

    public void FinishedCharacterTurn(){
        if (currentTurn == true){
            if (currentMember < PartyMembers.Count){
                Debug.Log(currentMember);
                PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(false);
                currentMember++;
                Debug.Log(currentMember);
                GameObject.Find("GameManager").GetComponent<GameManager>().UpdateEveryonesTiles();
                    //Debug.Log("Player currently" + currentMember);
                if (currentMember < PartyMembers.Count){
                    PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);
                }
                else{
                    GameObject.Find("TurnManager").GetComponent<TurnManager>().UpdateCurrentTurn(false);
                    currentMember = 0;
                }
            }
            GameObject.Find("GameManager").GetComponent<GameManager>().UpdateEveryonesTiles();    
        }
    }

    public void UpdateTurn(bool value){
        //Debug.Log("UpdateTurn is called " + value);
        currentTurn = value;
        if(value == true){
            currentMember = 0;
            PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);
        }
    }

    public void UpdatePlayersOccupiedTiles(){
        for (int i = 0; i < PartyMembers.Count; i++){
            PartyMembers[i].GetComponent<Player>().UpdateOccupiedTiles();
        }
    }

    public void FillPlayerTiles(GameObject[,] arr2d)
    {
        for(int i = 0; i < PartyMembers.Count; i++){
            PartyMembers[i].GetComponent<Player>().SetAllTiles(arr2d);
        }
    }
}