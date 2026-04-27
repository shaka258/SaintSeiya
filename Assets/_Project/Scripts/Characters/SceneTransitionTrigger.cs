using UnityEngine;

namespace SaintSeiya.Characters
{
    public class SceneTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private string _targetScene;
        [SerializeField] private Vector2 _spawnPosition;
        [SerializeField] private float _transitionDelay = 0.3f;
        private bool _triggered;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (_triggered || !other.CompareTag("Player")) return;
            _triggered = true;
            StartCoroutine(TransitionRoutine());
        }

        System.Collections.IEnumerator TransitionRoutine()
        {
            GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>()?.SetCanMove(false);
            Core.EventBus.Publish(new Core.SceneTransitionEvent { FromScene = gameObject.scene.name, ToScene = _targetScene });
            yield return new WaitForSeconds(_transitionDelay);
            PlayerPrefs.SetFloat("SpawnX", _spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", _spawnPosition.y);
            Core.GameManager.Instance?.LoadScene(_targetScene);
        }
    }
}
