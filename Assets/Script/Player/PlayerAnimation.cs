using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //References
    Animator anim; // Référence au composant Animator pour contrôler les animations
    PlayerMovement playerMovement; // Référence au script PlayerMovement pour accéder aux informations de déplacement


    void Start()
    {
        // Récupère le composant Animator attaché à cet objet
        anim = GetComponent<Animator>();
        // Récupère le composant PlayerMovement attaché à cet objet
        playerMovement = GetComponent<PlayerMovement>();
    }


    void Update()
    {
        // Met à jour les paramètres "Horizontal" et "Vertical" dans l'Animator
        // Ces paramètres contrôlent les animations selon la direction du mouvement du joueur
        anim.SetFloat("Horizontal", playerMovement.moveDir.x);
        anim.SetFloat("Vertical", playerMovement.moveDir.y);

        // Met à jour le paramètre "Speed" dans l'Animator, basé sur la magnitude du déplacement
        // Utilisation de sqrMagnitude pour éviter les calculs coûteux de la racine carrée
        anim.SetFloat("Speed", playerMovement.moveDir.sqrMagnitude);
    }
}
