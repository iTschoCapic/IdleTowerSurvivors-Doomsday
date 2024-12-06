using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // [Header("Stats")] permet de regrouper les variables dans une section dans l'inspecteur Unity.
    [Header("Stats")]
    public float health = 100;            // Points de vie de l'ennemi.
    public float MoveSpeed = 3;           // Vitesse de d�placement de l'ennemi.
    public float damage = 10;             // D�g�ts inflig�s par l'ennemi.
    public float actionTime = 1.0f;       // Temps entre deux actions/attaques.
    public float attackRange = 2.0f;      // Port�e d'attaque de l'ennemi.
    public int money = 10;                // Montant d'argent donn� au joueur � la mort de l'ennemi.
    public int experience = 5;            // Points d'exp�rience donn�s � la mort de l'ennemi.

    public GameObject target;             // Cible actuelle de l'ennemi (joueur ou autre).

    // Dictionnaire stockant les �tats de l'ennemi (ex: Feu, Glace) et leur dur�e.
    private Dictionary<State, Tuple<bool, float>> states = new Dictionary<State, Tuple<bool, float>>();

    private void Start()
    {
        // Initialise les �tats avec une valeur par d�faut (aucun effet actif).
        states[State.None] = new Tuple<bool, float>(false, 0f);
        states[State.Fire] = new Tuple<bool, float>( false, 0f);
        states[State.Ice] = new Tuple<bool, float>(false, 0f);
        states[State.Poison] = new Tuple<bool, float>(false, 0f);
        states[State.Electricity] = new Tuple<bool, float>(false, 0f);
    }

    // R�duit les points de vie de l'ennemi en fonction des d�g�ts re�us.
    public void TakeDamage(float damage)
    {
        health -= damage;
        isDead();
    }

    // D�finit un nouvel �tat pour l'ennemi (ex: feu, glace) avec une dur�e et des d�g�ts associ�s.
    public void SetState(State state, float stateTime, float stateDamage)
    {
        if (state == State.None) // Prevent from affecting an effect that doesn't exist
        {
            return;
        }

        // Active l'�tat avec sa dur�e.
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

    // Coroutine g�rant les effets de l'�tat (d�g�ts sur la dur�e).
    private IEnumerator stateSuffering(State state, float stateTime, float stateDamage)
    {
        while (states[state].Item1 && stateTime > 0)
        {
            health -= stateDamage; // Inflige des d�g�ts � l'ennemi.
            stateTime -= 0.2f;     // R�duit le temps restant.
            isDead();              // V�rifie si l'ennemi est mort.
            print(health);         // Affiche les PV restants dans la console (utile pour le debug).
            yield return new WaitForSeconds(0.2f); // Attend avant la prochaine it�ration.
        }

        states[state] = new Tuple<bool, float>(false, 0f);
        yield return null;
    }

    // V�rifie si l'ennemi est mort et d�truit l'objet si c'est le cas.
    private void isDead()
    {
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // D�placement vers la cible : si aucune cible n'est d�finie, l'ennemi poursuit le joueur.
        if (target  == null)
        {
            // D�termine si l'ennemi doit se retourner en fonction de la position du joueur.
            bool flip = transform.position.x > FindObjectOfType<PlayerBehaviour>().transform.position.x;
            transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);
            // D�place l'ennemi vers le joueur.
            transform.position = Vector2.MoveTowards(transform.position, FindObjectOfType<PlayerBehaviour>().transform.position, MoveSpeed * Time.deltaTime);
        }
        else
        {
            // D�placement vers la cible d�finie (ex: une tour).
            bool flip = transform.position.x > target.transform.position.x;
            transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);
            // D�place l'ennemi vers la cible.
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, MoveSpeed * Time.deltaTime);
        }
        
    }

    // Appel� lorsque l'objet est d�truit.
    private void OnDestroy()
    {
        // Si le jeu est en pause, ne rien faire.
        if (FindObjectOfType<gameManager>().GetGameState() == gameManager.gameState.Paused) { return; }
        // Informe le EnemySpawner de la mort de l'ennemi pour donner des r�compenses au joueur.
        EnemySpawner Es = FindObjectOfType<EnemySpawner>();
        Es.OnEnemyKilled(money, experience);
    }

    
}
