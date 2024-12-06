using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    // Référence au script Tower attaché au parent de cet objet (la tour elle-même)
    Tower parent;

    private void Start()
    {
        // Récupère le composant Tower du parent de cet objet
        parent = gameObject.transform.parent.GetComponent<Tower>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet entrant a le tag "Enemy" et si cet ennemi n'est pas déjà dans la liste des ennemis de la tour
        if (collision.CompareTag("Enemy") && !parent.enemies.Contains(collision.gameObject))
        {
            // Ajoute l'ennemi à la liste des ennemis de la tour
            parent.enemies.Add(collision.gameObject);
        }

        // Vérifie si la liste des ennemis n'est pas vide, si la tour n'est pas déjà en train de lancer des projectiles
        // et si l'état de la machine d'état de la tour n'est pas 2 (broken)
        if (parent.enemies != null && !parent.IsInvoking("spawnProjectile") && parent.stateMachine != 2)
        {
            // Lance la méthode spawnProjectile à intervalles réguliers (actionTime) tant qu'il y a des ennemis
            parent.InvokeRepeating("spawnProjectile", 0f, parent.actionTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Vérifie si l'objet sortant a le tag "Enemy" et si cet ennemi est bien dans la liste des ennemis
        if (collision.CompareTag("Enemy") && parent.enemies.Contains(collision.gameObject))
        {
            // Retire l'ennemi de la liste des ennemis de la tour
            parent.enemies.Remove(collision.gameObject);
        }

        // Si la liste des ennemis est vide et que la tour est en train d'invoquer "spawnProjectile"
        if (parent.enemies == null && parent.IsInvoking("spawnProjectile"))
        {
            // Annule l'invocation répétée de la méthode spawnProjectile
            parent.CancelInvoke("spawnProjectile");
        }
    }
}
