using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // La cible que la cam�ra doit suivre (g�n�ralement un joueur ou un objet sp�cifique).
    public Transform target;
    // D�calage entre la position de la cam�ra et celle de la cible.
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        // Met � jour la position de la cam�ra pour qu'elle suive la cible,
        // tout en appliquant un d�calage d�fini par l'utilisateur.
        transform.position = target.position + offset;
    }
}
