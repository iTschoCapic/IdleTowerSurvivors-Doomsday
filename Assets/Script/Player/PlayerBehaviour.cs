using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // La santé du joueur, initialisée à 100
    public float health = 100;

    // Méthode pour infliger des dégâts au joueur
    public void TakeDamage(float damage)
    {
        // Réduit la santé du joueur en fonction des dégâts reçus
        health -= damage;
        // Vérifie si le joueur est mort après avoir pris des dégâts
        isDead();
    }

    // Méthode privée pour vérifier si la santé du joueur est inférieure à zéro
    private void isDead()
    {
        // Si la santé du joueur est inférieure à zéro, cela signifie que le joueur est mort
        if (health < 0)
        {
            // Détruit le GameObject auquel ce script est attaché (le joueur)
            Destroy(gameObject);
        }
    }
}
