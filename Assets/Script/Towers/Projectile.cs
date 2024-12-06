using UnityEngine;

// Enum�ration repr�sentant diff�rents types d'effets que peut avoir un projectile
public enum State
{
    None,          // Aucun effet
    Poison,        // Effet poison
    Fire,          // Effet feu
    Electricity,   // Effet �lectricit�
    Radioactive,   // Effet radioactif
    Ice,           // Effet glace
    Plasma,        // Effet plasma
    Light,         // Effet lumi�re
    Sun            // Effet soleil
}


public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidb; // R�f�rence au Rigidbody2D du projectile pour appliquer la physique

    public float speed = 1; // Vitesse du projectile
    private Vector2 direction; // Direction du projectile, d�finie par la cible
    public float damage = 5; // D�g�ts inflig�s au contact avec un ennemi
    private GameObject target; // R�f�rence � la cible du projectile (g�n�ralement un ennemi)

    public bool hasEffect = false; // Bool�en qui d�termine si le projectile a un effet suppl�mentaire
    public State state = State.None; // L'effet sp�cifique du projectile 

    public float lifetime = 5f; // Dur�e de vie du projectile avant qu'il ne soit d�truit
    private float timeAlive = 0f; // Temps �coul� depuis le lancement du projectile



    private void Start()
    {
        rigidb = GetComponent<Rigidbody2D>(); // On r�cup�re le Rigidbody2D attach� au projectile
    }
    private void FixedUpdate()
    {
        timeAlive += Time.deltaTime; // On incr�mente le temps de vie du projectile

        // Si le projectile a d�pass� sa dur�e de vie, on le d�truit
        if (timeAlive >= lifetime)
        {
            Destroy(gameObject); // D�truire le projectile apr�s la dur�e de vie d�finie
        }

        // Si la cible existe, on met � jour la vitesse du Rigidbody2D du projectile
        if (target != null)
        {
            rigidb.velocity = direction * speed;  // Applique la vitesse dans la direction calcul�e
        }
    }

    // M�thode pour d�finir la cible du projectile
    public void setTarget(GameObject target)
    {
        this.target = target; // Assigner la cible du projectile
        direction = target.transform.position - transform.position; // Calculer la direction vers la cible
        transform.rotation = Quaternion.identity; // R�initialiser la rotation du projectile
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBehaviour enemy = collision.GetComponent<EnemyBehaviour>(); // V�rifie si l'objet touch� est un ennemi

        // Si un ennemi est touch�
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Infliger des d�g�ts � l'ennemi

            // Si le projectile a un effet, appliquer cet effet � l'ennemi
            if (hasEffect)
            {
                enemy.SetState(state, 1.6f, 1f); // Appliquer l'�tat (effet) � l'ennemi (dur�e, d�g�ts)
            }
            Destroy(gameObject); // D�truire le projectile apr�s qu'il ait touch� l'ennemi
        }
    }
}
