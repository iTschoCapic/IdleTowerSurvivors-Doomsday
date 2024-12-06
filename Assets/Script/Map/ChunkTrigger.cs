using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    // R�f�rence au MapController, qui contr�le la logique de la carte
    MapController mapController;
    // R�f�rence � la carte cible, celle qui sera associ�e au joueur lorsqu'il entre dans la zone de d�clenchement
    public GameObject targetMap;


    void Start()
    {
        // Trouve une instance du MapController dans la sc�ne et l'associe � la variable mapController
        mapController = FindObjectOfType<MapController>();
    }

    // Cette m�thode est appel�e lorsque le collider du joueur entre en contact avec le collider 2D de ce gameObject
    private void OnTriggerStay2D(Collider2D col)
    {
        // V�rifie si le collider qui entre en contact avec la zone a le tag "Player"
        if (col.CompareTag("Player"))
        {
            // Si c'est le joueur, on met � jour la r�f�rence de la carte actuelle du mapController avec la carte cible
            mapController.currentChunk = targetMap;
        }
    }
    // Cette m�thode est appel�e lorsque le collider du joueur quitte la zone de d�clenchement
    private void OnTriggerExit2D(Collider2D col)
    {
        // V�rifie si le collider qui quitte la zone a le tag "Player"
        if (col.CompareTag("Player"))
        {
            // Si le joueur quitte et que la carte actuelle est celle de targetMap, on la r�initialise � null
            if (mapController.currentChunk == targetMap)
            {
                mapController.currentChunk = null;
            }
        }
    }
}
