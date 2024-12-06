using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    // Référence au MapController, qui contrôle la logique de la carte
    MapController mapController;
    // Référence à la carte cible, celle qui sera associée au joueur lorsqu'il entre dans la zone de déclenchement
    public GameObject targetMap;


    void Start()
    {
        // Trouve une instance du MapController dans la scène et l'associe à la variable mapController
        mapController = FindObjectOfType<MapController>();
    }

    // Cette méthode est appelée lorsque le collider du joueur entre en contact avec le collider 2D de ce gameObject
    private void OnTriggerStay2D(Collider2D col)
    {
        // Vérifie si le collider qui entre en contact avec la zone a le tag "Player"
        if (col.CompareTag("Player"))
        {
            // Si c'est le joueur, on met à jour la référence de la carte actuelle du mapController avec la carte cible
            mapController.currentChunk = targetMap;
        }
    }
    // Cette méthode est appelée lorsque le collider du joueur quitte la zone de déclenchement
    private void OnTriggerExit2D(Collider2D col)
    {
        // Vérifie si le collider qui quitte la zone a le tag "Player"
        if (col.CompareTag("Player"))
        {
            // Si le joueur quitte et que la carte actuelle est celle de targetMap, on la réinitialise à null
            if (mapController.currentChunk == targetMap)
            {
                mapController.currentChunk = null;
            }
        }
    }
}
