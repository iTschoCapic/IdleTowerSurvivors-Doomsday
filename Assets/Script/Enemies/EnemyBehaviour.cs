using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // [Header("Stats")] permet de regrouper les variables dans une section dans l'inspecteur Unity.
    [Header("Stats")]
    public float health = 100;            // Points de vie de l'ennemi.
    public float MoveSpeed = 3;           // Vitesse de déplacement de l'ennemi.
    public float damage = 10;             // Dégâts infligés par l'ennemi.
    public float actionTime = 1.0f;       // Temps entre deux actions/attaques.
    public float attackRange = 2.0f;      // Portée d'attaque de l'ennemi.
    public int money = 10;                // Montant d'argent donné au joueur à la mort de l'ennemi.
    public int experience = 5;            // Points d'expérience donnés à la mort de l'ennemi.

    public GameObject target;             // Cible actuelle de l'ennemi (joueur ou autre).

    // Dictionnaire stockant les états de l'ennemi (ex: Feu, Glace) et leur durée.
    private Dictionary<State, Tuple<bool, float>> states = new Dictionary<State, Tuple<bool, float>>();

    private void Start()
    {
        // Initialise les états avec une valeur par défaut (aucun effet actif).
        states[State.None] = new Tuple<bool, float>(false, 0f);
        states[State.Fire] = new Tuple<bool, float>( false, 0f);
        states[State.Ice] = new Tuple<bool, float>(false, 0f);
        states[State.Poison] = new Tuple<bool, float>(false, 0f);
        states[State.Electricity] = new Tuple<bool, float>(false, 0f);
    }

    // Réduit les points de vie de l'ennemi en fonction des dégâts reçus.
    public void TakeDamage(float damage)
    {
        health -= damage;
        isDead();
    }

    // Définit un nouvel état pour l'ennemi (ex: feu, glace) avec une durée et des dégâts associés.
    public void SetState(State state, float stateTime, float stateDamage)
    {
        if (state == State.None) // Prevent from affecting an effect that doesn't exist
        {
            return;
        }

        // Active l'état avec sa durée.
        states[state] = new Tuple<bool, float>(true, stateTime);
        foreach (var item in states)
        {
            if (item.Key == state && item.Value.Item1)
            {
                StopCoroutine("stateSuffering");
                StartCoroutine(stateSuffering(state, stateTime, stateDamage));
            } else if (item.Key == state)
            {
                StopCoroutine("stateSuffering");
                StartCoroutine(stateSuffering(state, stateTime, stateDamage));
            }
        }
        
    }

    // Coroutine gérant les effets de l'état (dégâts sur la durée).
    private IEnumerator stateSuffering(State state, float stateTime, float stateDamage)
    {
        while (states[state].Item1 && stateTime > 0)
        {
            health -= stateDamage; // Inflige des dégâts à l'ennemi.
            stateTime -= 0.2f;     // Réduit le temps restant.
            isDead();              // Vérifie si l'ennemi est mort.
            print(health);         // Affiche les PV restants dans la console (utile pour le debug).
            yield return new WaitForSeconds(0.2f); // Attend avant la prochaine itération.
        }

        states[state] = new Tuple<bool, float>(false, 0f);
        yield return null;
    }

    // Vérifie si l'ennemi est mort et détruit l'objet si c'est le cas.
    private void isDead()
    {
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // Déplacement vers la cible : si aucune cible n'est définie, l'ennemi poursuit le joueur.
        if (target  == null)
        {
            // Détermine si l'ennemi doit se retourner en fonction de la position du joueur.
            bool flip = transform.position.x > FindObjectOfType<PlayerBehaviour>().transform.position.x;
            transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);
            // Déplace l'ennemi vers le joueur.
            transform.position = Vector2.MoveTowards(transform.position, FindObjectOfType<PlayerBehaviour>().transform.position, MoveSpeed * Time.deltaTime);
        }
        else
        {
            // Déplacement vers la cible définie (ex: une tour).
            bool flip = transform.position.x > target.transform.position.x;
            transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);
            // Déplace l'ennemi vers la cible.
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, MoveSpeed * Time.deltaTime);
        }
        
    }

    // Appelé lorsque l'objet est détruit.
    private void OnDestroy()
    {
        // Si le jeu est en pause, ne rien faire.
        if (FindObjectOfType<gameManager>().GetGameState() == gameManager.gameState.Paused) { return; }
        // Informe le EnemySpawner de la mort de l'ennemi pour donner des récompenses au joueur.
        EnemySpawner Es = FindObjectOfType<EnemySpawner>();
        Es.OnEnemyKilled(money, experience);
    }

    
}
