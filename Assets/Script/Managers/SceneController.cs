using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    // Cette m�thode permet de changer de sc�ne en fonction du nom de la sc�ne pass� en param�tre.
    public void SceneChange(string name)
    {
        // Charge la sc�ne dont le nom est sp�cifi� en param�tre
        SceneManager.LoadScene(name);
    }

    // Cette m�thode permet de quitter le jeu
    public void QuitGame()
    {
        Application.Quit();
    }
}
