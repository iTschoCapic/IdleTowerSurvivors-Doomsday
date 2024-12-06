using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Définition d'une vague (Wave) d'ennemis
    [System.Serializable]
    public class Wave
    {
        public string waveName; // Nom de la vague
        public List<EnemyGroup> enemiesGroups; // Liste des groupes d'ennemis dans cette vague
        public int waveQuota; // Quota total d'ennemis à spawn dans cette vague
        public float spawnInterval; // Intervalle entre chaque spawn d'ennemis
        public int spawnCount; // Compteur d'ennemis déjà spawnes dans cette vague
    }

    // Définition d'un groupe d'ennemis dans une vague
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName; // Nom du type d'ennemi
        public int enemyCount; // Nombre d'ennemis à spawn pour ce groupe
        public int spawnCount; // Nombre d'ennemis déjà spawnes pour ce groupe
        public GameObject enemyPrefab; // Préfabriqué de l'ennemi à instancier
    }

    // Liste des vagues de la scène
    public List<Wave> waves;
    public int currentWaveCount; // Indice de la vague actuelle en cours de spawn

    [Header("Spawner Attributes")]
    float _spawnTimer; // Timer pour gérer le temps entre chaque spawn
    public int enemiesAlive; // Nombre d'ennemis actuellement vivants
    public int maxEnemiesAllowed; // Nombre maximum d'ennemis autorisés dans la scène
    public bool maxEnemiesReached = false; // Indicateur si le nombre maximum d'ennemis est atteint
    public float waveInterval; // Temps d'attente entre deux vagues

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; // Liste des points de spawn possibles

    public Transform _player; // Référence au joueur (pour déterminer la position du spawn autour du joueur)

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<gameManager>().player.transform; // On récupère la référence du joueur via le gameManager
        CalculateWaveQuota(); // Calcule le quota total d'ennemis à spawn dans la vague actuelle
    }

    // Update is called once per frame
    void Update()
    {
        // Si la vague actuelle est terminée (aucun ennemi n'a été spawn dans cette vague)
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0) 
        {
            // On commence la vague suivante après un délai
            StartCoroutine(BeginNextWave());
        }
        // Mise à jour du timer de spawn
        _spawnTimer += Time.deltaTime;

        // Si l'intervalle de spawn est atteint, on spawn des ennemis
        if (_spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            _spawnTimer = 0; // Réinitialiser le timer
            SpawnEnemies(); // Appeler la fonction pour spawn des ennemis
        }
    }

    // Coroutine pour attendre un certain temps avant de lancer la vague suivante
    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval); // Attendre "waveInterval" secondes avant de commencer la vague suivante

        // Si il y a encore des vagues à lancer, passer à la suivante
        if (currentWaveCount < waves.Count - 1) {
            currentWaveCount++; // On passe à la vague suivante
            CalculateWaveQuota(); // Recalculer le quota d'ennemis à spawn pour la nouvelle vague
        }
    }

    // Calcule le quota total d'ennemis à spawn pour la vague actuelle
    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (EnemyGroup enemiesGroup in waves[currentWaveCount].enemiesGroups)
        {
            currentWaveQuota += enemiesGroup.enemyCount; // Additionner le nombre d'ennemis de chaque groupe
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;// Mettre à jour le quota total d'ennemis pour la vague
    }

    // Fonction pour spawn des ennemis dans la scène
    void SpawnEnemies()
    {
        // Vérifier si le nombre d'ennemis spawnes est inférieur au quota total de la vague et si le nombre max d'ennemis n'a pas été atteint
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota  && !maxEnemiesReached)
        {
            // Pour chaque groupe d'ennemis dans la vague actuelle
            foreach (EnemyGroup enemyGroup in waves[currentWaveCount].enemiesGroups)
            {
                // Vérifier si le nombre d'ennemis de ce groupe est inférieur au nombre requis
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    // Si le nombre maximum d'ennemis dans la scène est atteint, arrêter le spawn
                    if (enemiesAlive > maxEnemiesAllowed) {
                        maxEnemiesReached = true; // Marquer que le max est atteint
                        return; // Ne pas spawn d'autres ennemis
                    }

                    // Spawn d'un ennemi en utilisant un point de spawn aléatoire
                    Instantiate(enemyGroup.enemyPrefab,_player.position + relativeSpawnPoints[Random.Range(0,relativeSpawnPoints.Count)].position, Quaternion.identity);

                    // Mettre à jour les compteurs de spawn
                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++; // Ajouter un ennemi vivant
                }
            }
        }

        // Si le nombre d'ennemis vivants est inférieur au max, réinitialiser l'indicateur maxReached
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    // Fonction appelée lorsqu'un ennemi est tué
    public void OnEnemyKilled(int Money, int Exp)
    {
        enemiesAlive--; // Réduire le nombre d'ennemis vivants
        FindObjectOfType<gameManager>().AddMoney(Money); // Ajouter de l'argent au joueur
        FindObjectOfType<gameManager>().AddExp(Exp); // Ajouter de l'expérience au joueur
    }
}
