using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RGBTitle : MonoBehaviour
{
    [SerializeField] Image titleImg;

    // Variables pour le cycle de couleur
    [SerializeField] private float cycleSpeed = 1f; // Vitesse du cycle de couleur
    private float cycleValue = 0f; // Valeur qui change au cours du temps

    void Update()
    {
        RGB_Cycling();
    }

    private void RGB_Cycling()
    {
        // La fonction Mathf.PingPong génère une valeur entre 0 et 1 en fonction du temps
        cycleValue += Time.deltaTime * cycleSpeed; // Augmente progressivement la valeur
        float red = Mathf.PingPong(cycleValue, 1f); // Valeur de rouge entre 0 et 1
        float green = Mathf.PingPong(cycleValue + 0.33f, 1f); // Décale légèrement pour le vert
        float blue = Mathf.PingPong(cycleValue + 0.66f, 1f); // Décale légèrement pour le bleu

        // Applique les nouvelles couleurs à l'image
        titleImg.color = new Color(red, green, blue);
    }
}
