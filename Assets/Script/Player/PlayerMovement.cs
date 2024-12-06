using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Vitesse du joueur (d�finie dans l'inspecteur Unity)
    Rigidbody2D rb; // R�f�rence au Rigidbody2D du joueur pour appliquer des mouvements physiques
    [HideInInspector]
    public float lastHorizontalVector; // Derni�re valeur horizontale (x) de la direction du mouvement
    [HideInInspector]
    public float lastVerticalVector; // Derni�re valeur verticale (y) de la direction du mouvement
    [HideInInspector]
    public Vector2 moveDir; // Vecteur directionnel qui repr�sente le mouvement actuel du joueur

    void Start()
    {
        // On r�cup�re le Rigidbody2D attach� au GameObject (le joueur)
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // On g�re les entr�es du joueur (d�placements) � chaque frame
        InputManagement();
    }

    private void FixedUpdate()
    {
        // D�place le joueur en utilisant la physique, appel� � chaque "FixedUpdate"
        Move();
    }

    // M�thode de gestion des entr�es du joueur
    void InputManagement()
    {
        // R�cup�re les entr�es de l'utilisateur (fl�ches ou touches WASD) et les normalise
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        // Si une entr�e horizontale (gauche/droite) a �t� d�tect�e
        if (moveDir.x != 0)
        {
            // Met � jour la derni�re direction horizontale avec la nouvelle valeur
            lastHorizontalVector = moveDir.x;
        }
        // Si une entr�e verticale (haut/bas) a �t� d�tect�e
        if (moveDir.y != 0)
        {
            // Met � jour la derni�re direction verticale avec la nouvelle valeur
            lastVerticalVector = moveDir.y;
        }
    }

    // M�thode pour appliquer le mouvement du joueur en fonction des entr�es
    private void Move()
    {
        // Applique la vitesse du joueur (moveSpeed) en fonction de la direction actuelle (moveDir)
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }
}
