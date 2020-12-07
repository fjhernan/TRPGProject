using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuObject : MonoBehaviour
{ 
    public void UnderstandButton(){
        SceneManager.LoadScene(1);
    }
}
