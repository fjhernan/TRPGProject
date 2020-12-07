using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI TurnDisplay, EnterKey, SpaceKey,
        ArrowKeys, TabKey, BackspaceKey, PlayerText, CombatText;
    public Image TurnBackground, EnterBackground, SpaceBackground,
        AKBackground, TabBackground, BackspaceBackground, PlayerBackground, CombatBackground;
    public GameObject CombatButton;
    private bool hidden = false, combat = false, player_turn = true, player_hidden = false;
    private const string Turn = "Turn: ";
    private bool gameOver = false;

    private void Start(){
        HideEnterKey();
        PlayerText.enabled = false;
        PlayerBackground.enabled = false;
        CombatText.enabled = false;
        CombatBackground.enabled = false;
        CombatButton.SetActive(false);
    }

    private void Update(){
        if (gameOver == false)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                //Hide or Unhide Display
                if (player_turn == true)
                {
                    UpdateDisplay();
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (player_turn == true)
                {
                    ShowPlayerInfo();
                }
            }
        }
    }

    public void GameOver(){
        gameOver = true;
    }

    private void UpdateDisplay(){
        //Hide Background
        TurnBackground.enabled = hidden;
        AKBackground.enabled = hidden;
        SpaceBackground.enabled = hidden;
        TabBackground.enabled = hidden;
        BackspaceBackground.enabled = hidden;
        if (combat == true){
            HideEnterKey();
        }

        //Hide Display
        TurnDisplay.enabled = hidden;
        ArrowKeys.enabled = hidden;
        SpaceKey.enabled = hidden;
        TabKey.enabled = hidden;
        BackspaceKey.enabled = hidden;
        if(combat == true){
            ShowEnterKey();
        }

        if (hidden == false){
            hidden = true;
        }
        else{
            hidden = false;
        }
    }

    //Cannot enter combat when there is no enemy nearby
    public void HideEnterKey(){
        EnterBackground.enabled = false;
        EnterKey.enabled = false;
    }

    //Can enter combat when enemy is nearby
    public void ShowEnterKey(){
        EnterBackground.enabled = true;
        EnterKey.enabled = true;
        combat = true;
    }

    public void HideArrowKeys(){
        ArrowKeys.enabled = false;
        AKBackground.enabled = false;
    }

    //Hide proper displays when its enemy's turn.
    public void EnemyTurn(){
        Debug.Log("EnemyTurn called");
        AKBackground.enabled = false;
        SpaceBackground.enabled = false;
        TabBackground.enabled = false;
        BackspaceBackground.enabled = false;
        ArrowKeys.enabled = false;
        SpaceKey.enabled = false;
        TabKey.enabled = false;
        BackspaceKey.enabled = false;
        TurnDisplay.text = Turn + "Enemy";
        player_turn = false;
    }

    //Show proper displays when its player's turn.
    public void PlayerTurn(){
        Debug.Log("PlayerTurn called");
        AKBackground.enabled = true;
        SpaceBackground.enabled = true;
        TabBackground.enabled = true;
        BackspaceBackground.enabled = true;
        ArrowKeys.enabled = true;
        SpaceKey.enabled = true;
        TabKey.enabled = true;
        BackspaceKey.enabled = true;
        TurnDisplay.text = Turn + "Player";
        player_turn = true;
    }

    public void PlayerInfo(string name, int hp, int atk, int def){
        PlayerText.text = "" + name + "\nHP: " + hp + "\nATK: " + atk + "\nDEF: " + def;
    }

    public void Combat(string eName, string pName, int dmg, bool initiate){
        CombatText.enabled = true;
        CombatBackground.enabled = true;
        CombatButton.SetActive(true);
        if(initiate == true){
            //Player initiates combat
            CombatText.text = pName + " Attacks!\n" + eName + " takes " + dmg + " damage!";
        }
        if(initiate == false){
            //Enemy initiates combat
            CombatText.text = eName + " Attacks!\n" + pName + " takes " + dmg + " damage!";
        }
    }

    public void CombatMessage(){
        CombatText.enabled = false;
        CombatBackground.enabled = false;
        CombatButton.SetActive(false);
        GameObject.Find("GameManager").GetComponent<GameManager>().UnPause();
    }

    private void ShowPlayerInfo(){
        if (player_hidden == true) {
            player_hidden = false;
        }
        else{
            player_hidden = true;
        }

        PlayerBackground.enabled = player_hidden;
        PlayerText.enabled = player_hidden;
    }
}