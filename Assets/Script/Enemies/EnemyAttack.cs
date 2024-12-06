using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Référence au script EnemyBehaviour attaché au parent de cet objet.
    EnemyBehaviour parent;
    private void Start()
    {
        // Accède au composant EnemyBehaviour situé sur l'objet parent de cet objet.
        parent = gameObject.transform.parent.GetComponent<EnemyBehaviour>();
    }
    // Déclenché lorsqu'un autre collider entre en collision avec ce collider (marqué comme Trigger).
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet entrant a le tag "Player" ou "Tower".
        if (collision.CompareTag("Player") || collision.CompareTag("Tower"))
        {
            // Définit la cible actuelle de l'ennemi sur l'objet qui vient d'entrer en collision.
            parent.target = collision.gameObject;
        }

        // Si une cible valide est définie, qu'une attaque n'est pas déjà en cours,
        // et que l'état de la machine d'état de la tour n'est pas 2, commence les attaques répétées.
        if (parent.target != null && !IsInvoking("attacking") && parent.target.GetComponent<Tower>().stateMachine != 2)
        {
            // Appelle la méthode attacking à des intervalles réguliers (définis par parent.actionTime).
            InvokeRepeating("attacking", 0f, parent.actionTime);
        }
    }

    // Déclenché lorsqu'un autre collider quitte la collision avec ce collider.
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Arrête les attaques si la cible qui quitte est la cible actuelle.
        if (IsInvoking("attacking") && parent.target == collision.gameObject)
        {
            // Annule l'appel répété de la méthode attacking.
            CancelInvoke("attacking");
            // Supprime la référence à la cible actuelle.
            parent.target = null;
        }
    }

    // Méthode appelée de manière répétée pour attaquer la cible.
    private void attacking()
    {
        // Vérifie si la cible est à portée d'attaque (basée sur parent.attackRange).
        if (Vector3.Distance(parent.target.transform.position, transform.position) < parent.attackRange)
        {
            // Si la cible est un joueur, applique des dégâts au joueur.
            if (parent.target.CompareTag("Player"))
            {
                parent.target.GetComponent<PlayerBehaviour>().TakeDamage(parent.damage);
            }
            // Si la cible est une tour, applique des dégâts à la tour.
            if (parent.target.CompareTag("Tower"))
            {
                parent.target.GetComponent<Tower>().TakeDamage(parent.damage);
            }
        }
    }
}
