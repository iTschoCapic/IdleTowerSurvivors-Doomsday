using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    // Cette méthode permet de changer de scène en fonction du nom de la scène passé en paramètre.
    public void SceneChange(string name)
    {
        // Charge la scène dont le nom est spécifié en paramètre
        SceneManager.LoadScene(name);
    }

    // Cette méthode permet de quitter le jeu
    public void QuitGame()
    {
        Application.Quit();
    }
}
