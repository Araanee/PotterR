using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject canvasTutoriel;

    void Start()
    {
        // S'assurer que le canvas est désactivé au démarrage
        if (canvasTutoriel != null)
        {
            canvasTutoriel.SetActive(false);
        }
    }

    // Cette méthode sera appelée par le bouton
    public void AfficherTutoriel()
    {
        if (canvasTutoriel != null)
        {
            canvasTutoriel.SetActive(true);
        }
    }

    // Optionnel : méthode pour fermer le tutoriel
    public void FermerTutoriel()
    {
        if (canvasTutoriel != null)
        {
            canvasTutoriel.SetActive(false);
        }
    }
}