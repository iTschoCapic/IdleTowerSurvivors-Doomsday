using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    // Enum�ration repr�sentant les diff�rents �tats du jeu
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
    public GameObject moneyText;                       // R�f�rence au texte affichant l'argent
    [SerializeField] private float _experience = 0;     // Exp�rience actuelle du joueur
    public GameObject experienceText;                  // R�f�rence au texte affichant l'exp�rience
    [SerializeField] private int _level = 1;            // Niveau actuel du joueur
    public GameObject levelText;                       // R�f�rence au texte affichant le niveau
    private int _experienceCap;                        // Capacit� d'exp�rience pour passer au niveau suivant
    private int _numberOfTowers = 1;                   // Nombre de tours que le joueur peut poser

    [Header("Tower")]
    public GameObject selectedTower; // Tour actuellement s�lectionn�e
    public GameObject towerPrefab;   // Pr�fabriqu� de la tour

    // Classe repr�sentant les plages de niveaux et l'augmentation du cap d'exp�rience
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
        DisableScreens(); // D�sactive les �crans (pause et upgrade) au d�marrage
        Time.timeScale = 1f; // Assure que le jeu tourne normalement
    }

    private void Start()
    {
        _experienceCap = levelRanges[0].experienceCapIncrease; // Initialise le cap d'exp�rience pour le premier niveau
    }

    void Update()
    {
        switch (_currentState)
        {
            case gameState.Gameplay:
                CheckForPauseAndResume(); // V�rifie si le jeu doit �tre mis en pause ou repris
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Obtient la position de la souris dans le monde
                    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, ~6); //Effectue un raycast pour d�tecter des objets sous le curseur

                    // Si le joueur a des tours � poser et que la souris est sur le terrain
                    if (_numberOfTowers > 0 && hit.collider.name.Contains("Terrain"))
                    {
                        GameObject t = Instantiate(towerPrefab); //Cr�e une nouvelle tour
                        t.transform.position = new Vector3(mousePosition.x, mousePosition.y, 1); //Positionne la tour sur le terrain
                        _numberOfTowers--; //Diminue le nombre de tours disponibles
                    }
                }
                break;
            case gameState.Paused:
                CheckForPauseAndResume(); // V�rifie si le jeu doit �tre mis en pause ou repris
                break;
            default:
                Debug.LogWarning("NO CURRENT STATE DEFINED"); // Alerte si un �tat inconnu est rencontr�
                break;
        }
    }

    // Change l'�tat du jeu
    void changeState(gameState newState)
    {
        _currentState = newState;
    }

    // Met le jeu en pause
    void PauseGame()
    {
        if (_currentState != gameState.Paused) // Si le jeu n'est pas d�j� en pause
        {
            _previousState = _currentState;  // Sauvegarde l'�tat actuel pour pouvoir revenir plus tard
            changeState(gameState.Paused);   // Change l'�tat du jeu en "Paused"
            Time.timeScale = 0;              // Met le temps du jeu en pause
            _pauseScreen.SetActive(true);    // Affiche l'�cran de pause
            Debug.Log("Game Paused");
        }
    }

    // Reprend le jeu apr�s une pause
    public void ResumeGame()
    {
        if (_currentState == gameState.Paused) // Si le jeu est en pause
        {
            changeState(_previousState); //Restaure l'�tat pr�c�dent
            Time.timeScale = 1f; //Reprend le temps normal du jeu
            _pauseScreen.SetActive(false); //Cache l'�cran de pause
            Debug.Log("Game Resumed");
        }
    }

    // V�rifie si la touche Escape est press�e pour mettre en pause ou reprendre
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentState == gameState.Paused)
            {
                ResumeGame();
            }
            else if(selectedTower != null) // Si une tour est s�lectionn�e, on la d�s�lectionne
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

    // D�sactive les �crans de pause et d'upgrade
    void DisableScreens()
    {
        _pauseScreen.SetActive(false);
        upgradeScreen.SetActive(false);
    }

    // M�thodes pour ajouter de l'argent et de l'exp�rience au joueur
    public void AddMoney(int Money)
    {
        _money += Money; // Ajoute l'argent au total
        moneyText.GetComponent<TextMeshProUGUI>().text = _money.ToString(); // Met � jour l'affichage de l'argent
    }
    
    public void AddExp(int Exp)
    {
        _experience += Exp; // Ajoute l'exp�rience
        experienceText.GetComponent<TextMeshProUGUI>().text = _experience.ToString() + "/" + _experienceCap.ToString(); // Met � jour l'affichage de l'exp�rience

        LevelUpCheckUp(); // V�rifie si le joueur doit passer au niveau suivant
    }

    // V�rifie si le joueur doit passer au niveau suivant
    private void LevelUpCheckUp()
    {
        if (_experience >= _experienceCap) // Si l'exp�rience d�passe le cap
        {
            _level++; // Augmente le niveau
            levelText.GetComponent<TextMeshProUGUI>().text = _level.ToString(); // Met � jour l'affichage du niveau
            _experience -= _experienceCap; // R�initialise l'exp�rience restante
            _numberOfTowers++; // Augmente le nombre de tours disponibles

            int expCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if(_level >= range.startLevel && _level <= range.endLevel) // Trouve la plage de niveaux correspondante et augmente le cap d'exp�rience
                {
                    expCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            _experienceCap += expCapIncrease; // Met � jour le cap d'exp�rience pour le niveau suivant
        }
    }

    // Retourne l'�tat actuel du jeu
    public gameState GetGameState()
    {
        return _currentState;
    }

    // M�thodes pour effectuer des am�liorations sur la tour s�lectionn�e
    public void UpgradeTowerDamage()
    {
        if (selectedTower != null && _money >= selectedTower.GetComponent<Tower>().damagePrice)
        {
            selectedTower.GetComponent<Tower>().damage += 1;// Augmente les d�g�ts de la tour
            selectedTower.GetComponent<Tower>().damagePrice += 5;  // Augmente le prix de l'am�lioration
            upgradeScreen.GetComponent<UpgradeScreenLayout>().damagePrice = selectedTower.GetComponent<Tower>().damagePrice;  // Met � jour l'UI d'am�lioration
            _money -= selectedTower.GetComponent<Tower>().damagePrice;  // Retire l'argent du joueur
            selectedTower.GetComponent<Tower>().upgradeLevel();  // Met � jour le niveau de la tour
        }
    }

    // Am�lioration de la port�e de la tour
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

    // Am�lioration de la vitesse de la tour
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

    // Am�lioration de la sant� de la tour
    public void UpgradeTowerHealth()
    {
        if (selectedTower != null && _money >= selectedTower.GetComponent<Tower>().healthPrice)
        {
            //augmenter health tour
            selectedTower.GetComponent<Tower>().health += 10;
            selectedTower.GetComponent<Tower>().maxHealth += 10;

            selectedTower.GetComponent<Tower>().healthPrice += 5;
            upgradeScreen.GetComponent<UpgradeScreenLayout>().healthPrice = selectedTower.GetComponent<Tower>().healthPrice;
            selectedTower.GetComponent<Tower>().updateLife(); // Met � jour la vie affich�e
            _money -= selectedTower.GetComponent<Tower>().healthPrice; // Retire l'argent du joueur

            selectedTower.GetComponent<Tower>().upgradeLevel();
        }
    }

    // R�paration de la tour
    public void RepairTower()
    {
        if (selectedTower != null && _money >= 20)
        {
            //augmenter health tour
            selectedTower.GetComponent<Tower>().health = selectedTower.GetComponent<Tower>().maxHealth; // R�pare la tour
            selectedTower.GetComponent<Tower>().updateLife(); // Met � jour la vie affich�e
            _money -= 20; // Retire l'argent du joueur
        }
    }

}
