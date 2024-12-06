using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    // R�f�rence au script Tower attach� au parent de cet objet (la tour elle-m�me)
    Tower parent;

    private void Start()
    {
        // R�cup�re le composant Tower du parent de cet objet
        parent = gameObject.transform.parent.GetComponent<Tower>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet entrant a le tag "Enemy" et si cet ennemi n'est pas d�j� dans la liste des ennemis de la tour
        if (collision.CompareTag("Enemy") && !parent.enemies.Contains(collision.gameObject))
        {
            // Ajoute l'ennemi � la liste des ennemis de la tour
            parent.enemies.Add(collision.gameObject);
        }

        // V�rifie si la liste des ennemis n'est pas vide, si la tour n'est pas d�j� en train de lancer des projectiles
        // et si l'�tat de la machine d'�tat de la tour n'est pas 2 (broken)
        if (parent.enemies != null && !parent.IsInvoking("spawnProjectile") && parent.stateMachine != 2)
        {
            // Lance la m�thode spawnProjectile � intervalles r�guliers (actionTime) tant qu'il y a des ennemis
            parent.InvokeRepeating("spawnProjectile", 0f, parent.actionTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // V�rifie si l'objet sortant a le tag "Enemy" et si cet ennemi est bien dans la liste des ennemis
        if (collision.CompareTag("Enemy") && parent.enemies.Contains(collision.gameObject))
        {
            // Retire l'ennemi de la liste des ennemis de la tour
            parent.enemies.Remove(collision.gameObject);
        }

        // Si la liste des ennemis est vide et que la tour est en train d'invoquer "spawnProjectile"
        if (parent.enemies == null && parent.IsInvoking("spawnProjectile"))
        {
            // Annule l'invocation r�p�t�e de la m�thode spawnProjectile
            parent.CancelInvoke("spawnProjectile");
        }
    }
}
