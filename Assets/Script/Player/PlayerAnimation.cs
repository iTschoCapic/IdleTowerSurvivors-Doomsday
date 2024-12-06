using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //References
    Animator anim; // R�f�rence au composant Animator pour contr�ler les animations
    PlayerMovement playerMovement; // R�f�rence au script PlayerMovement pour acc�der aux informations de d�placement


    void Start()
    {
        // R�cup�re le composant Animator attach� � cet objet
        anim = GetComponent<Animator>();
        // R�cup�re le composant PlayerMovement attach� � cet objet
        playerMovement = GetComponent<PlayerMovement>();
    }


    void Update()
    {
        // Met � jour les param�tres "Horizontal" et "Vertical" dans l'Animator
        // Ces param�tres contr�lent les animations selon la direction du mouvement du joueur
        anim.SetFloat("Horizontal", playerMovement.moveDir.x);
        anim.SetFloat("Vertical", playerMovement.moveDir.y);

        // Met � jour le param�tre "Speed" dans l'Animator, bas� sur la magnitude du d�placement
        // Utilisation de sqrMagnitude pour �viter les calculs co�teux de la racine carr�e
        anim.SetFloat("Speed", playerMovement.moveDir.sqrMagnitude);
    }
}
