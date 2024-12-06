using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UpgradeScreenLayout : MonoBehaviour
{
    // Liste des boutons pr�sents sur l'�cran de mise � niveau
    private List<GameObject> _buttons;

    // Prix des am�liorations pour diff�rents attributs
    public float damagePrice = 5;
    public float rangePrice = 5;
    public float speedPrice = 5;
    public float healthPrice = 5;

    // Indicateur de l'�tat de la tour (cass�e ou non)
    public bool broken = false;

    void Start()
    {
        // Initialisation de la liste des boutons
        _buttons = new List<GameObject>();
        // Ajout de tous les boutons enfants de cet objet dans la liste _buttons
        foreach (Transform Button in gameObject.transform) {
            _buttons.Add(Button.gameObject);
        }
    }

    void Update()
    {
        // Parcours de tous les boutons dans la liste _buttons
        foreach (GameObject Button in _buttons)
        {
            // On v�rifie le nom de chaque bouton pour adapter son comportement
            switch (Button.name)
            {
                // Si le nom du bouton est "Damage"
                case "Damage":
                    // Met � jour le texte du bouton pour afficher le prix de l'am�lioration
                    Button.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade\n" + Button.name + "\nfor " + damagePrice + " Gems";
                    
                    // Si la tour est cass�e, le bouton devient invisible
                    if (broken && Button.activeInHierarchy)
                    {
                        Button.SetActive(false);
                    }
                    else if (!broken && !Button.activeInHierarchy)
                    {
                        Button.SetActive(true);
                    }
                    break;
                // Si le nom du bouton est "Range"
                case "Range":
                    Button.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade\n" + Button.name + "\nfor " + rangePrice + " Gems";
                    // Si la tour est cass�e, le bouton devient invisible
                    if (broken && Button.activeInHierarchy)
                    {
                        Button.SetActive(false);
                    }
                    else if (!broken && !Button.activeInHierarchy)
                    {
                        Button.SetActive(true);
                    }
                    break;
                // Si le nom du bouton est "Speed"
                case "Speed":
                    Button.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade\n" + Button.name + "\nfor " + speedPrice + " Gems";
                    // Si la tour est cass�e, le bouton devient invisible
                    if (broken && Button.activeInHierarchy)
                    {
                        Button.SetActive(false);
                    }
                    else if (!broken && !Button.activeInHierarchy)
                    {
                        Button.SetActive(true);
                    }
                    break;
                // Si le nom du bouton est "Health"
                case "Health":
                    Button.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade\n" + Button.name + "\nfor " + healthPrice + " Gems";
                    // Si la tour est cass�e, le bouton devient invisible
                    if (broken && Button.activeInHierarchy)
                    {
                        Button.SetActive(false);
                    }
                    else if (!broken && !Button.activeInHierarchy)
                    {
                        Button.SetActive(true);
                    }
                    break;
                // Si le nom du bouton est "Repair"
                case "Repair":
                    // Met � jour le texte du bouton pour afficher le prix de la r�paration
                    Button.GetComponentInChildren<TextMeshProUGUI>().text = "Repair Tower for 10 Gems";
                    
                    // Si la tour est cass�e et que le bouton n'est pas d�j� visible, il devient visible
                    if (broken && !Button.activeInHierarchy)
                    {
                        Button.SetActive(true);
                    }
                    // Si la tour n'est pas cass�e et que le bouton est visible, il devient invisible
                    else if (!broken && Button.activeInHierarchy)
                    {
                        Button.SetActive(false);
                    }
                    break;
            }
        }
    }
}
