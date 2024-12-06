using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // R�f�rence au script EnemyBehaviour attach� au parent de cet objet.
    EnemyBehaviour parent;
    private void Start()
    {
        // Acc�de au composant EnemyBehaviour situ� sur l'objet parent de cet objet.
        parent = gameObject.transform.parent.GetComponent<EnemyBehaviour>();
    }
    // D�clench� lorsqu'un autre collider entre en collision avec ce collider (marqu� comme Trigger).
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet entrant a le tag "Player" ou "Tower".
        if (collision.CompareTag("Player") || collision.CompareTag("Tower"))
        {
            // D�finit la cible actuelle de l'ennemi sur l'objet qui vient d'entrer en collision.
            parent.target = collision.gameObject;
        }

        // Si une cible valide est d�finie, qu'une attaque n'est pas d�j� en cours,
        // et que l'�tat de la machine d'�tat de la tour n'est pas 2, commence les attaques r�p�t�es.
        if (parent.target != null && !IsInvoking("attacking") && parent.target.GetComponent<Tower>().stateMachine != 2)
        {
            // Appelle la m�thode attacking � des intervalles r�guliers (d�finis par parent.actionTime).
            InvokeRepeating("attacking", 0f, parent.actionTime);
        }
    }

    // D�clench� lorsqu'un autre collider quitte la collision avec ce collider.
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Arr�te les attaques si la cible qui quitte est la cible actuelle.
        if (IsInvoking("attacking") && parent.target == collision.gameObject)
        {
            // Annule l'appel r�p�t� de la m�thode attacking.
            CancelInvoke("attacking");
            // Supprime la r�f�rence � la cible actuelle.
            parent.target = null;
        }
    }

    // M�thode appel�e de mani�re r�p�t�e pour attaquer la cible.
    private void attacking()
    {
        // V�rifie si la cible est � port�e d'attaque (bas�e sur parent.attackRange).
        if (Vector3.Distance(parent.target.transform.position, transform.position) < parent.attackRange)
        {
            // Si la cible est un joueur, applique des d�g�ts au joueur.
            if (parent.target.CompareTag("Player"))
            {
                parent.target.GetComponent<PlayerBehaviour>().TakeDamage(parent.damage);
            }
            // Si la cible est une tour, applique des d�g�ts � la tour.
            if (parent.target.CompareTag("Tower"))
            {
                parent.target.GetComponent<Tower>().TakeDamage(parent.damage);
            }
        }
    }
}
