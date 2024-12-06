using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    // Enumération représentant les différents états du jeu
    public enum gameState
    {
        Gameplay,
        Paused
    }

    [SerializeField] private gameState _currentState;
    [SerializeField] private gameState _previousState;

    [Header("UI")]
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] public GameObject upgradeScreen;

    [Header("Player")]
    public GameObject player;                          // Le joueur
    [SerializeField] private float _money = 0;          // Montant d'argent du joueur
    public GameObject moneyText;                       // Référence au texte affichant l'argent
    [SerializeField] private float _experience = 0;     // Expérience actuelle du joueur
    public GameObject experienceText;                  // Référence au texte affichant l'expérience
    [SerializeField] private int _level = 1;            // Niveau actuel du joueur
    public GameObject levelText;                       // Référence au texte affichant le niveau
    private int _experienceCap;                        // Capacité d'expérience pour passer au niveau suivant
    private int _numberOfTowers = 1;                   // Nombre de tours que le joueur peut poser

    [Header("Tower")]
    public GameObject selectedTower; // Tour actuellement sélectionnée
    public GameObject towerPrefab;   // Préfabriqué de la tour

    // Classe représentant les plages de niveaux et l'augmentation du cap d'expérience
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    public List<LevelRange> levelRanges; // Liste des plages de niveaux

    private void Awake()
    {
        DisableScreens(); // Désactive les écrans (pause et upgrade) au démarrage
        Time.timeScale = 1f; // Assure que le jeu tourne normalement
    }

    private void Start()
    {
        _experienceCap = levelRanges[0].experienceCapIncrease; // Initialise le cap d'expérience pour le premier niveau
    }

    void Update()
    {
        switch (_currentState)
        {
            case gameState.Gameplay:
                CheckForPauseAndResume(); // Vérifie si le jeu doit être mis en pause ou repris
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Obtient la position de la souris dans le monde
                    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, ~6); //Effectue un raycast pour détecter des objets sous le curseur

                    // Si le joueur a des tours à poser et que la souris est sur le terrain
                    if (_numberOfTowers > 0 && hit.collider.name.Contains("Terrain"))
                    {
                        GameObject t = Instantiate(towerPrefab); //Crée une nouvelle tour
                        t.transform.position = new Vector3(mousePosition.x, mousePosition.y, 1); //Positionne la tour sur le terrain
                        _numberOfTowers--; //Diminue le nombre de tours disponibles
                    }
                }
                break;
            case gameState.Paused:
                CheckForPauseAndResume(); // Vérifie si le jeu doit être mis en pause ou repris
                break;
            default:
                Debug.LogWarning("NO CURRENT STATE DEFINED"); // Alerte si un état inconnu est rencontré
                break;
        }
    }

    // Change l'état du jeu
    void changeState(gameState newState)
    {
        _currentState = newState;
    }

    // Met le jeu en pause
    void PauseGame()
    {
        if (_currentState != gameState.Paused) // Si le jeu n'est pas déjà en pause
        {
            _previousState = _currentState;  // Sauvegarde l'état actuel pour pouvoir revenir plus tard
            changeState(gameState.Paused);   // Change l'état du jeu en "Paused"
            Time.timeScale = 0;              // Met le temps du jeu en pause
            _pauseScreen.SetActive(true);    // Affiche l'écran de pause
            Debug.Log("Game Paused");
        }
    }

    // Reprend le jeu après une pause
    public void ResumeGame()
    {
        if (_currentState == gameState.Paused) // Si le jeu est en pause
        {
            changeState(_previousState); //Restaure l'état précédent
            Time.timeScale = 1f; //Reprend le temps normal du jeu
            _pauseScreen.SetActive(false); //Cache l'écran de pause
            Debug.Log("Game Resumed");
        }
    }

    // Vérifie si la touche Escape est pressée pour mettre en pause ou reprendre
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentState == gameState.Paused)
            {
                ResumeGame();
            }
            else if(selectedTower != null) // Si une tour est sélectionnée, on la désélectionne
            {
                selectedTower.GetComponent<Tower>().UnHighlightTower();
                selectedTower = null;
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Désactive les écrans de pause et d'upgrade
    void DisableScreens()
    {
        _pauseScreen.SetActive(false);
        upgradeScreen.SetActive(false);
    }

    // Méthodes pour ajouter de l'argent et de l'expérience au joueur
    public void AddMoney(int Money)
    {
        _money += Money; // Ajoute l'argent au total
        moneyText.GetComponent<TextMeshProUGUI>().text = _money.ToString(); // Met à jour l'affichage de l'argent
    }
    
    public void AddExp(int Exp)
    {
        _experience += Exp; // Ajoute l'expérience
        experienceText.GetComponent<TextMeshProUGUI>().text = _experience.ToString() + "/" + _experienceCap.ToString(); // Met à jour l'affichage de l'expérience

        LevelUpCheckUp(); // Vérifie si le joueur doit passer au niveau suivant
    }

    // Vérifie si le joueur doit passer au niveau suivant
    private void LevelUpCheckUp()
    {
        if (_experience >= _experienceCap) // Si l'expérience dépasse le cap
        {
            _level++; // Augmente le niveau
            levelText.GetComponent<TextMeshProUGUI>().text = _level.ToString(); // Met à jour l'affichage du niveau
            _experience -= _experienceCap; // Réinitialise l'expérience restante
            _numberOfTowers++; // Augmente le nombre de tours disponibles

            int expCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if(_level >= range.startLevel && _level <= range.endLevel) // Trouve la plage de niveaux correspondante et augmente le cap d'expérience
                {
                    expCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            _experienceCap += expCapIncrease; // Met à jour le cap d'expérience pour le niveau suivant
        }
    }

    // Retourne l'état actuel du jeu
    public gameState GetGameState()
    {
        return _currentState;
    }

    // Méthodes pour effectuer des améliorations sur la tour sélectionnée
    public void UpgradeTowerDamage()
    {
        if (selectedTower != null && _money >= selectedTower.GetComponent<Tower>().damagePrice)
        {
            selectedTower.GetComponent<Tower>().damage += 1;// Augmente les dégâts de la tour
            selectedTower.GetComponent<Tower>().damagePrice += 5;  // Augmente le prix de l'amélioration
            upgradeScreen.GetComponent<UpgradeScreenLayout>().damagePrice = selectedTower.GetComponent<Tower>().damagePrice;  // Met à jour l'UI d'amélioration
            _money -= selectedTower.GetComponent<Tower>().damagePrice;  // Retire l'argent du joueur
            selectedTower.GetComponent<Tower>().upgradeLevel();  // Met à jour le niveau de la tour
        }
    }

    // Amélioration de la portée de la tour
    public void UpgradeTowerRange()
    {
        if (selectedTower != null && _money >= selectedTower.GetComponent<Tower>().rangePrice)
        {
            //augmenter range tour

            selectedTower.GetComponent<Tower>().rangePrice += 5;
            upgradeScreen.GetComponent<UpgradeScreenLayout>().rangePrice = selectedTower.GetComponent<Tower>().rangePrice;

            selectedTower.GetComponentInChildren<CircleCollider2D>().radius += 0.01f;
            _money -= selectedTower.GetComponent<Tower>().rangePrice; // Retire l'argent du joueur

            selectedTower.GetComponent<Tower>().upgradeLevel();
        }
    }

    // Amélioration de la vitesse de la tour
    public void UpgradeTowerSpeed()
    {
        if (selectedTower != null && _money >= selectedTower.GetComponent<Tower>().speedPrice)
        {
            //augmenter speed tour
            selectedTower.GetComponent<Tower>().actionTime -= selectedTower.GetComponent<Tower>().actionTime/100;
            selectedTower.GetComponent<Tower>().baseActionTime -= selectedTower.GetComponent<Tower>().baseActionTime / 100;

            selectedTower.GetComponent<Tower>().speedPrice += 5;
            upgradeScreen.GetComponent<UpgradeScreenLayout>().speedPrice = selectedTower.GetComponent<Tower>().speedPrice;
            _money -= selectedTower.GetComponent<Tower>().speedPrice; // Retire l'argent du joueur

            selectedTower.GetComponent<Tower>().upgradeLevel();
        }
    }

    // Amélioration de la santé de la tour
    public void UpgradeTowerHealth()
    {
        if (selectedTower != null && _money >= selectedTower.GetComponent<Tower>().healthPrice)
        {
            //augmenter health tour
            selectedTower.GetComponent<Tower>().health += 10;
            selectedTower.GetComponent<Tower>().maxHealth += 10;

            selectedTower.GetComponent<Tower>().healthPrice += 5;
            upgradeScreen.GetComponent<UpgradeScreenLayout>().healthPrice = selectedTower.GetComponent<Tower>().healthPrice;
            selectedTower.GetComponent<Tower>().updateLife(); // Met à jour la vie affichée
            _money -= selectedTower.GetComponent<Tower>().healthPrice; // Retire l'argent du joueur

            selectedTower.GetComponent<Tower>().upgradeLevel();
        }
    }

    // Réparation de la tour
    public void RepairTower()
    {
        if (selectedTower != null && _money >= 20)
        {
            //augmenter health tour
            selectedTower.GetComponent<Tower>().health = selectedTower.GetComponent<Tower>().maxHealth; // Répare la tour
            selectedTower.GetComponent<Tower>().updateLife(); // Met à jour la vie affichée
            _money -= 20; // Retire l'argent du joueur
        }
    }

}
