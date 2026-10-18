using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Views
{
    public sealed class EndGameView : MonoBehaviour
    {
        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject loseText;
        [SerializeField] private Button reloadButton;

        private void Awake() => reloadButton.onClick.AddListener(ReloadScene);

        private void OnDestroy() => reloadButton.onClick.RemoveListener(ReloadScene);

        public void ShowVictory() => Show(true);

        public void ShowDefeat() => Show(false);

        private void Show(bool victory)
        {
            winText.SetActive(victory);
            loseText.SetActive(!victory);
            gameObject.SetActive(true);
        }

        private static void ReloadScene() => 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
