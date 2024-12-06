using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // La sant� du joueur, initialis�e � 100
    public float health = 100;

    // M�thode pour infliger des d�g�ts au joueur
    public void TakeDamage(float damage)
    {
        // R�duit la sant� du joueur en fonction des d�g�ts re�us
        health -= damage;
        // V�rifie si le joueur est mort apr�s avoir pris des d�g�ts
        isDead();
    }

    // M�thode priv�e pour v�rifier si la sant� du joueur est inf�rieure � z�ro
    private void isDead()
    {
        // Si la sant� du joueur est inf�rieure � z�ro, cela signifie que le joueur est mort
        if (health < 0)
        {
            // D�truit le GameObject auquel ce script est attach� (le joueur)
            Destroy(gameObject);
        }
    }
}
