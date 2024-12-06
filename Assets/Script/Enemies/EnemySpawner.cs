using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // D�finition d'une vague (Wave) d'ennemis
    [System.Serializable]
    public class Wave
    {
        public string waveName; // Nom de la vague
        public List<EnemyGroup> enemiesGroups; // Liste des groupes d'ennemis dans cette vague
        public int waveQuota; // Quota total d'ennemis � spawn dans cette vague
        public float spawnInterval; // Intervalle entre chaque spawn d'ennemis
        public int spawnCount; // Compteur d'ennemis d�j� spawnes dans cette vague
    }

    // D�finition d'un groupe d'ennemis dans une vague
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName; // Nom du type d'ennemi
        public int enemyCount; // Nombre d'ennemis � spawn pour ce groupe
        public int spawnCount; // Nombre d'ennemis d�j� spawnes pour ce groupe
        public GameObject enemyPrefab; // Pr�fabriqu� de l'ennemi � instancier
    }

    // Liste des vagues de la sc�ne
    public List<Wave> waves;
    public int currentWaveCount; // Indice de la vague actuelle en cours de spawn

    [Header("Spawner Attributes")]
    float _spawnTimer; // Timer pour g�rer le temps entre chaque spawn
    public int enemiesAlive; // Nombre d'ennemis actuellement vivants
    public int maxEnemiesAllowed; // Nombre maximum d'ennemis autoris�s dans la sc�ne
    public bool maxEnemiesReached = false; // Indicateur si le nombre maximum d'ennemis est atteint
    public float waveInterval; // Temps d'attente entre deux vagues

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; // Liste des points de spawn possibles

    public Transform _player; // R�f�rence au joueur (pour d�terminer la position du spawn autour du joueur)

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<gameManager>().player.transform; // On r�cup�re la r�f�rence du joueur via le gameManager
        CalculateWaveQuota(); // Calcule le quota total d'ennemis � spawn dans la vague actuelle
    }

    // Update is called once per frame
    void Update()
    {
        // Si la vague actuelle est termin�e (aucun ennemi n'a �t� spawn dans cette vague)
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0) 
        {
            // On commence la vague suivante apr�s un d�lai
            StartCoroutine(BeginNextWave());
        }
        // Mise � jour du timer de spawn
        _spawnTimer += Time.deltaTime;

        // Si l'intervalle de spawn est atteint, on spawn des ennemis
        if (_spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            _spawnTimer = 0; // R�initialiser le timer
            SpawnEnemies(); // Appeler la fonction pour spawn des ennemis
        }
    }

    // Coroutine pour attendre un certain temps avant de lancer la vague suivante
    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval); // Attendre "waveInterval" secondes avant de commencer la vague suivante

        // Si il y a encore des vagues � lancer, passer � la suivante
        if (currentWaveCount < waves.Count - 1) {
            currentWaveCount++; // On passe � la vague suivante
            CalculateWaveQuota(); // Recalculer le quota d'ennemis � spawn pour la nouvelle vague
        }
    }

    // Calcule le quota total d'ennemis � spawn pour la vague actuelle
    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (EnemyGroup enemiesGroup in waves[currentWaveCount].enemiesGroups)
        {
            currentWaveQuota += enemiesGroup.enemyCount; // Additionner le nombre d'ennemis de chaque groupe
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;// Mettre � jour le quota total d'ennemis pour la vague
    }

    // Fonction pour spawn des ennemis dans la sc�ne
    void SpawnEnemies()
    {
        // V�rifier si le nombre d'ennemis spawnes est inf�rieur au quota total de la vague et si le nombre max d'ennemis n'a pas �t� atteint
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota  && !maxEnemiesReached)
        {
            // Pour chaque groupe d'ennemis dans la vague actuelle
            foreach (EnemyGroup enemyGroup in waves[currentWaveCount].enemiesGroups)
            {
                // V�rifier si le nombre d'ennemis de ce groupe est inf�rieur au nombre requis
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    // Si le nombre maximum d'ennemis dans la sc�ne est atteint, arr�ter le spawn
                    if (enemiesAlive > maxEnemiesAllowed) {
                        maxEnemiesReached = true; // Marquer que le max est atteint
                        return; // Ne pas spawn d'autres ennemis
                    }

                    // Spawn d'un ennemi en utilisant un point de spawn al�atoire
                    Instantiate(enemyGroup.enemyPrefab,_player.position + relativeSpawnPoints[Random.Range(0,relativeSpawnPoints.Count)].position, Quaternion.identity);

                    // Mettre � jour les compteurs de spawn
                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++; // Ajouter un ennemi vivant
                }
            }
        }

        // Si le nombre d'ennemis vivants est inf�rieur au max, r�initialiser l'indicateur maxReached
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    // Fonction appel�e lorsqu'un ennemi est tu�
    public void OnEnemyKilled(int Money, int Exp)
    {
        enemiesAlive--; // R�duire le nombre d'ennemis vivants
        FindObjectOfType<gameManager>().AddMoney(Money); // Ajouter de l'argent au joueur
        FindObjectOfType<gameManager>().AddExp(Exp); // Ajouter de l'exp�rience au joueur
    }
}
