using UnityEngine;

// Enumération représentant différents types d'effets que peut avoir un projectile
public enum State
{
    None,          // Aucun effet
    Poison,        // Effet poison
    Fire,          // Effet feu
    Electricity,   // Effet électricité
    Radioactive,   // Effet radioactif
    Ice,           // Effet glace
    Plasma,        // Effet plasma
    Light,         // Effet lumière
    Sun            // Effet soleil
}


public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidb; // Référence au Rigidbody2D du projectile pour appliquer la physique

    public float speed = 1; // Vitesse du projectile
    private Vector2 direction; // Direction du projectile, définie par la cible
    public float damage = 5; // Dégâts infligés au contact avec un ennemi
    private GameObject target; // Référence à la cible du projectile (généralement un ennemi)

    public bool hasEffect = false; // Booléen qui détermine si le projectile a un effet supplémentaire
    public State state = State.None; // L'effet spécifique du projectile 

    public float lifetime = 5f; // Durée de vie du projectile avant qu'il ne soit détruit
    private float timeAlive = 0f; // Temps écoulé depuis le lancement du projectile



    private void Start()
    {
        rigidb = GetComponent<Rigidbody2D>(); // On récupère le Rigidbody2D attaché au projectile
    }
    private void FixedUpdate()
    {
        timeAlive += Time.deltaTime; // On incrémente le temps de vie du projectile

        // Si le projectile a dépassé sa durée de vie, on le détruit
        if (timeAlive >= lifetime)
        {
            Destroy(gameObject); // Détruire le projectile après la durée de vie définie
        }

        // Si la cible existe, on met à jour la vitesse du Rigidbody2D du projectile
        if (target != null)
        {
            rigidb.velocity = direction * speed;  // Applique la vitesse dans la direction calculée
        }
    }

    // Méthode pour définir la cible du projectile
    public void setTarget(GameObject target)
    {
        this.target = target; // Assigner la cible du projectile
        direction = target.transform.position - transform.position; // Calculer la direction vers la cible
        transform.rotation = Quaternion.identity; // Réinitialiser la rotation du projectile
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBehaviour enemy = collision.GetComponent<EnemyBehaviour>(); // Vérifie si l'objet touché est un ennemi

        // Si un ennemi est touché
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Infliger des dégâts à l'ennemi

            // Si le projectile a un effet, appliquer cet effet à l'ennemi
            if (hasEffect)
            {
                enemy.SetState(state, 1.6f, 1f); // Appliquer l'état (effet) à l'ennemi (durée, dégâts)
            }
            Destroy(gameObject); // Détruire le projectile après qu'il ait touché l'ennemi
        }
    }
}
