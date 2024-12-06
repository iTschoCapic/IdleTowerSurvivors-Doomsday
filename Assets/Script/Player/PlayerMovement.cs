using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Vitesse du joueur (définie dans l'inspecteur Unity)
    Rigidbody2D rb; // Référence au Rigidbody2D du joueur pour appliquer des mouvements physiques
    [HideInInspector]
    public float lastHorizontalVector; // Dernière valeur horizontale (x) de la direction du mouvement
    [HideInInspector]
    public float lastVerticalVector; // Dernière valeur verticale (y) de la direction du mouvement
    [HideInInspector]
    public Vector2 moveDir; // Vecteur directionnel qui représente le mouvement actuel du joueur

    void Start()
    {
        // On récupère le Rigidbody2D attaché au GameObject (le joueur)
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // On gère les entrées du joueur (déplacements) à chaque frame
        InputManagement();
    }

    private void FixedUpdate()
    {
        // Déplace le joueur en utilisant la physique, appelé à chaque "FixedUpdate"
        Move();
    }

    // Méthode de gestion des entrées du joueur
    void InputManagement()
    {
        // Récupère les entrées de l'utilisateur (flèches ou touches WASD) et les normalise
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        // Si une entrée horizontale (gauche/droite) a été détectée
        if (moveDir.x != 0)
        {
            // Met à jour la dernière direction horizontale avec la nouvelle valeur
            lastHorizontalVector = moveDir.x;
        }
        // Si une entrée verticale (haut/bas) a été détectée
        if (moveDir.y != 0)
        {
            // Met à jour la dernière direction verticale avec la nouvelle valeur
            lastVerticalVector = moveDir.y;
        }
    }

    // Méthode pour appliquer le mouvement du joueur en fonction des entrées
    private void Move()
    {
        // Applique la vitesse du joueur (moveSpeed) en fonction de la direction actuelle (moveDir)
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }
}
