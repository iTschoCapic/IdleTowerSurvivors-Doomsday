using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    // Liste des points de spawn où les objets (props) peuvent apparaître
    public List<GameObject> propSpawnPoints;
    // Liste des préfabriqués (props) qui peuvent être instanciés
    public List<GameObject> propPrefabs;

    void Start()
    {
        // Appelle la méthode pour générer des objets à des points de spawn aléatoires
        spawnProps();
    }

    // Méthode pour générer des objets à des points de spawn aléatoires
    void spawnProps()
    {
        // Parcours tous les points de spawn dans la liste propSpawnPoints
        foreach (GameObject spawnPoint in propSpawnPoints)
        {
            // Choisit un indice aléatoire dans la liste des préfabriqués (propPrefabs)
            int randomIndex = Random.Range(0, propPrefabs.Count);
            // Crée une instance d'un préfabriqué à la position du point de spawn
            GameObject prop = Instantiate(propPrefabs[randomIndex], spawnPoint.transform.position, Quaternion.identity);
            // Définit le parent du nouvel objet à l'objet de spawn, pour qu'il soit un enfant de celui-ci
            prop.transform.parent = spawnPoint.transform;
        }
    }
}
