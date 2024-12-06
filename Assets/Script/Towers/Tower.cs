using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static gameManager;

public class Tower : MonoBehaviour
{
    // --- Statistiques de la tour ---
    [Header("Stats")]
    public float health = 100;
    public int maxHealth = 100;
    public float damage = 0;
    private int level = 1;
    private int tier = 0; // Niveau d'évolution de la tour
    public State state = State.None; // Type d'état actuel de la tour

    // --- Prix des améliorations ---
    [Header("Upgrades")]
    public float damagePrice = 5;
    public float rangePrice = 5;
    public float speedPrice = 5;
    public float healthPrice = 5;

    [Header("Other")]
    public int stateMachine = 0; // Machine d'état de la tour

    public Transform shootingPoint; // Point à partir duquel la tour tire les projectiles
    public GameObject[] projectilePrefabs; // Préfabriqués de projectiles pour différents états
    private GameObject projectilePrefab; // Projectile actuel sélectionné en fonction de l'état

    public List<GameObject> enemies = new List<GameObject>(); // Liste des ennemis à cibler par la tour

    private Object[] sprite; // Tableaux des sprites associés à la tour

    public float actionTime = 1.0f; // Temps entre chaque action de la tour (par exemple, tirer)
    public float baseActionTime = 1.0f; // Temps de base entre les actions, modifié par l'état

    private void Start()
    {
        // Redimensionner le point de tir pour l'adapter à la scène
        shootingPoint.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        // Charger les ressources de sprites pour la tour
        sprite = Resources.LoadAll("Towers/Tower1");

        // Définir un état aléatoire pour la tour
        state = GetRandomState();

        // Choisir le préfabriqué de projectile en fonction de l'état de la tour
        switch (state)
        {
            case State.None:
                projectilePrefab = projectilePrefabs[0];
                break;
            case State.Poison:
                projectilePrefab = projectilePrefabs[1];
                break;
            case State.Fire:
                projectilePrefab = projectilePrefabs[2];
                break;
            case State.Electricity:
                projectilePrefab = projectilePrefabs[3];
                break;
            case State.Radioactive:
                projectilePrefab = projectilePrefabs[4];
                break;
            case State.Ice:
                projectilePrefab = projectilePrefabs[5];
                break;
            case State.Plasma:
                projectilePrefab = projectilePrefabs[6];
                break;
            case State.Light:
                projectilePrefab = projectilePrefabs[7];
                break;
            case State.Sun:
                projectilePrefab = projectilePrefabs[8];
                break;
        }

        // Changer l'icône de la tour en fonction du projectile
        shootingPoint.gameObject.GetComponent<SpriteRenderer>().sprite = projectilePrefab.GetComponent<SpriteRenderer>().sprite;
    }

    void FixedUpdate()
    {
        // Retirer les ennemis détruits de la liste
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].IsDestroyed())
            {
                enemies.Remove(enemies[i]);
            }
        }
    }

    private void Update()
    {
        // Gérer le clic sur la tour pour afficher les options d'amélioration
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider.gameObject == gameObject)
            {
                Click();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        updateLife(); // Met à jour la santé et l'état de la tour
    }

    // --- Mise à jour de la vie et de l'état de la tour ---
    public void updateLife()
    {
        // Changer l'état de la tour en fonction de la santé restante
        if (health <= 0 && stateMachine != 2)
        {
            updateStateMachine(2); // Tour est brisée
        }
        if ((health < maxHealth / 2 && health > 0) && stateMachine != 1)
        {
            updateStateMachine(1); // Tour endommagée
        }
        if (health > maxHealth / 2 && stateMachine != 0)
        {
            updateStateMachine(0); // Tour en bon état
        }
    }

    // --- Mise à jour de la machine d'état et du sprite ---
    private void updateStateMachine(int newState) // Change of sprite
    {
        stateMachine = newState;

        // Logique de mise à jour de l'état de la tour et de ses actions
        if (stateMachine == 0)
        {
            actionTime = baseActionTime;
            if (IsInvoking("spawnProjectile"))
            {
                CancelInvoke("spawnProjectile");
                InvokeRepeating("spawnProjectile", 0f, actionTime);
            }
            sprite = Resources.LoadAll("Towers/Tower" + level);
        }
        if (stateMachine == 1)
        {
            actionTime = baseActionTime * 2; // Augmenter le temps d'action si la tour est endommagée
            if (IsInvoking("spawnProjectile"))
            {
                CancelInvoke("spawnProjectile");
                InvokeRepeating("spawnProjectile", 0f, actionTime);
            }
            sprite = Resources.LoadAll("Towers/Tower" + level + "_damaged");
        }
        if (stateMachine == 2)
        {
            actionTime = -1.0f; // Arrêter les actions si la tour est brisée
            if (IsInvoking("spawnProjectile"))
            {
                CancelInvoke("spawnProjectile");
            }
            sprite = Resources.LoadAll("Towers/Tower" + level + "_broken");
        }

        // Changer le sprite de la tour en fonction du niveau et de l'état
        ChangeSprite((Sprite)sprite[tier + 1]);
    }

    // --- Créer un projectile ---
    private void spawnProjectile()
    {
        if (enemies.Count > 0)
        {
            // Instancier un projectile et lui attribuer un ennemi cible
            GameObject p = Instantiate(projectilePrefab, shootingPoint.position, transform.rotation);
            p.GetComponent<Projectile>().setTarget(enemies[Random.Range(0, enemies.Count)]);
            p.GetComponent<Projectile>().damage+=damage;// Ajouter les dégâts de la tour

            // Ajouter l'effet de l'état si nécessaire
            if (state != State.None)
            {
                p.GetComponent<Projectile>().hasEffect = true;
                p.GetComponent<Projectile>().state = state;
            }
        }
    }

    // --- Mettre en surbrillance la tour ---
    public void HighlightTower()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }
    
    // --- Retirer la surbrillance de la tour ---
    public void UnHighlightTower()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // --- Changer l'apparence de la tour ---
    void ChangeSprite(Sprite NewSprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = NewSprite;

        Vector2 originalSize = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        Vector2 newSize = NewSprite.bounds.size;

        // Ajuster l'échelle de la tour pour garder une taille cohérente avec le sprite
        transform.localScale = new Vector3(
            transform.localScale.x * (originalSize.x / newSize.x),
            transform.localScale.y * (originalSize.y / newSize.y),
            transform.localScale.z
        );
    }

    // --- Gestion du clic pour la sélection de la tour ---
    private void Click()
    {
        // Si aucune tour n'est sélectionnée, sélectionner cette tour
        if (FindObjectOfType<gameManager>().selectedTower == null)
        {
            FindObjectOfType<gameManager>().selectedTower = gameObject;
            HighlightTower(); // Mettre en surbrillance la tour
            
            // Vérifier si la tour est brisée
            bool Broken = (stateMachine == 2);
            FindObjectOfType<gameManager>().upgradeScreen.GetComponent<UpgradeScreenLayout>().broken = Broken;
            FindObjectOfType<gameManager>().upgradeScreen.SetActive(true);// Afficher l'écran de mise à niveau
        }
        else
        {
            FindObjectOfType<gameManager>().selectedTower = null;
            UnHighlightTower();// Retirer la surbrillance de la tour
            FindObjectOfType<gameManager>().upgradeScreen.SetActive(false); // Fermer l'écran de mise à niveau
        }
    }

    // --- Récupérer un état aléatoire --
    State GetRandomState()
    {
        // Récupérer toutes les valeurs de l'énumération State
        State[] values = (State[])System.Enum.GetValues(typeof(State));

        // Générer un index aléatoire et retourner l'état correspondant
        return values[Random.Range(1, values.Length)];  // Commence à partir de 1 pour éviter l'état 'None' (optionnel)
    }

    // --- Améliorer le niveau de la tour ---
    public void upgradeLevel()
    {
        tier++; // Augmenter le tier de la tour
        if (tier >= 3) // Si la tour atteint le tier max
        {
            level++; // Passer au niveau suivant
            tier = 0; // Réinitialiser le tier
        }
        sprite = Resources.LoadAll("Towers/Tower" + level); // Charger les nouveaux sprites pour ce niveau
        ChangeSprite((Sprite)sprite[tier + 1]); // Appliquer le sprite approprié en fonction du niveau et du tier
    }
}
