using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    // Liste des points de spawn o� les objets (props) peuvent appara�tre
    public List<GameObject> propSpawnPoints;
    // Liste des pr�fabriqu�s (props) qui peuvent �tre instanci�s
    public List<GameObject> propPrefabs;

    void Start()
    {
        // Appelle la m�thode pour g�n�rer des objets � des points de spawn al�atoires
        spawnProps();
    }

    // M�thode pour g�n�rer des objets � des points de spawn al�atoires
    void spawnProps()
    {
        // Parcours tous les points de spawn dans la liste propSpawnPoints
        foreach (GameObject spawnPoint in propSpawnPoints)
        {
            // Choisit un indice al�atoire dans la liste des pr�fabriqu�s (propPrefabs)
            int randomIndex = Random.Range(0, propPrefabs.Count);
            // Cr�e une instance d'un pr�fabriqu� � la position du point de spawn
            GameObject prop = Instantiate(propPrefabs[randomIndex], spawnPoint.transform.position, Quaternion.identity);
            // D�finit le parent du nouvel objet � l'objet de spawn, pour qu'il soit un enfant de celui-ci
            prop.transform.parent = spawnPoint.transform;
        }
    }
}
