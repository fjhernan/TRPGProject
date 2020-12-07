using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public List<GameObject> PartyMembers;
    private int currentMember = 0;
    private bool currentTurn = true;
    private bool gameOver = false;
    private bool paused = false;
    private bool playerPausedAtStart = false;
    public void GameStart(){
        for(int i = 0; i < PartyMembers.Count; i++){
            if (i != currentMember){
                PartyMembers[i].GetComponent<Player>().DisableHighlight();
            }
            //GameObject.Find("GameManager").GetComponent<GameManager>().AddPlayer(PartyMembers[i]);
        }
        PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);
        UpdatePlayersOccupiedTiles();
    }

    private void Update(){
        if (gameOver == false)
        {
            if (paused == false)
            {
                if (currentTurn == true)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (currentMember < PartyMembers.Count)
                        {
                            PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(false);
                            currentMember++;
                            GameObject.Find("GameManager").GetComponent<GameManager>().UpdateEveryonesTiles();
                            //Debug.Log("Player currently" + currentMember);
                            if (currentMember < PartyMembers.Count)
                            {
                                PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);
                            }
                            else
                            {
                                GameObject.Find("TurnManager").GetComponent<TurnManager>().UpdateCurrentTurn(false);
                                currentMember = 0;
                            }
                        }
                        GameObject.Find("GameManager").GetComponent<GameManager>().UpdateEveryonesTiles();
                    }
                }
            }
        }
    }

    public void UpdateTurn(bool value){
        //Debug.Log("UpdateTurn is called " + value);
        currentTurn = value;
        if(value == true){
            currentMember = 0;
            if (paused == false)
                PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);
            else
                playerPausedAtStart = true;
        }
    }

    public void PlayerHasDied(GameObject player){
        PartyMembers.Remove(player);
        Debug.Log("There is this many party members left" + PartyMembers.Count);
        if(currentMember < PartyMembers.Count){
            PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);
            GameObject.Find("GameManager").GetComponent<GameManager>().UpdateEveryonesTiles();
        }
        else{
            GameObject.Find("GameManager").GetComponent<GameManager>().AllPlayersDead();
        }
        //FinishedCharacterTurn();
    }

    public void GameOver(){
        gameOver = true;
    }

    public int GetPartySize(){
        return PartyMembers.Count;
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

    public void Pause()
    {
        paused = true;
        if(currentTurn == true)
        {
            PartyMembers[currentMember].GetComponent<Player>().Pause();
        }
    }

    public void UnPause()
    {
        paused = false;
        if(currentTurn == true)
        {
            if (playerPausedAtStart == false)
                PartyMembers[currentMember].GetComponent<Player>().UnPause();
            else
            {
                PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);
                playerPausedAtStart = false; 
            }
        }
    }

}