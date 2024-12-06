using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks; // Liste des morceaux de terrain à instancier
    public GameObject player; // Le joueur du jeu
    public float checkerRadius; // Rayon de vérification autour des positions du terrain
    Vector3 noTerrainPos; // Position où un nouveau morceau de terrain sera généré
    public LayerMask terrainMask; // Masque de couche pour vérifier les collisions avec le terrain
    public GameObject currentChunk; // Référence au morceau de terrain actuel du joueur
    PlayerMovement PlayerMovement; // Référence au script de mouvement du joueur

    [Header("Optimization")]
    public List<GameObject> spawnedChunks; // Liste des morceaux de terrain déjà générés
    GameObject latestChunk; // Dernier morceau de terrain généré
    public float maxOpDist; // Distance maximale avant de désactiver un morceau de terrain
    float opDist; // Distance actuelle entre le joueur et un morceau de terrain
    float optimizerCooldown; // Temps d'attente avant de relancer l'optimisation
    public float optimizerCooldownDuration; // Durée du cooldown pour l'optimisation

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        chunkChecker(); // Vérifie la nécessité de générer un nouveau morceau de terrain
        chunkOptimizer(); // Optimise les morceaux de terrain générés en fonction de la distance
    }

    void chunkChecker()
    {
        if (!currentChunk) { return; } // Si le morceau actuel est nul, on arrête

        // Vérification des directions du mouvement du joueur
        if (PlayerMovement.moveDir.x > 0 && PlayerMovement.moveDir.y == 0) // moving right
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position,checkerRadius, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("Right").position;
                spawnChunks();
            }
        }
        // D'autres conditions de mouvement (gauche, haut, bas, diagonales)
        // Si l'espace dans la direction du mouvement est vide, un nouveau morceau de terrain est instancié
        else if (PlayerMovement.moveDir.x < 0 && PlayerMovement.moveDir.y == 0) // moving left
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("Left").position;
                spawnChunks();
            }
        }
        else if (PlayerMovement.moveDir.x == 0 && PlayerMovement.moveDir.y > 0) // moving up
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("Up").position;
                spawnChunks();
            }
        }
        else if (PlayerMovement.moveDir.x == 0 && PlayerMovement.moveDir.y < 0) // moving down
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask)) // + new Vector3(0, -18, 0)
            {
                noTerrainPos = currentChunk.transform.Find("Down").position;//player.transform.position + new Vector3(0, -18, 0);
                spawnChunks();
            }
        }
        else if (PlayerMovement.moveDir.x > 0 && PlayerMovement.moveDir.y > 0) // right up
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("RightUp").position, checkerRadius, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("RightUp").position;//player.transform.position + new Vector3(28, 18, 0);
                spawnChunks();
            }
        }
        else if (PlayerMovement.moveDir.x > 0 && PlayerMovement.moveDir.y < 0) // right down
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("RightDown").position, checkerRadius, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("RightDown").position;
                spawnChunks();
            }
        }
        else if (PlayerMovement.moveDir.x < 0 && PlayerMovement.moveDir.y > 0) // left up
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("LeftUp").position, checkerRadius, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("LeftUp").position;
                spawnChunks();
            }
        }
        else if (PlayerMovement.moveDir.x < 0 && PlayerMovement.moveDir.y < 0) // left down
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("LeftDown").position, checkerRadius, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("LeftDown").position;
                spawnChunks();
            }
        }
    }

    void spawnChunks()
    {
        int randomIndex = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[randomIndex], noTerrainPos, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    void chunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime; // Décrémenter le cooldown du gestionnaire d'optimisation
        if (optimizerCooldown <= 0f) { optimizerCooldown = optimizerCooldownDuration; }
        else { return; }

        // Vérification des distances et optimisation des morceaux générés
        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false); // Désactive les morceaux trop éloignés
            }
            else
            {
                chunk.SetActive(true); // Réactive les morceaux proches
            }
        }
    }
}
