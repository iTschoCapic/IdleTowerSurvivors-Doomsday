using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // La cible que la caméra doit suivre (généralement un joueur ou un objet spécifique).
    public Transform target;
    // Décalage entre la position de la caméra et celle de la cible.
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        // Met à jour la position de la caméra pour qu'elle suive la cible,
        // tout en appliquant un décalage défini par l'utilisateur.
        transform.position = target.position + offset;
    }
}
