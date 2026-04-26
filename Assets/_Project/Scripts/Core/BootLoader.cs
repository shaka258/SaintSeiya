using UnityEngine;
using System.Collections;

namespace SaintSeiya.Core
{
    /// <summary>
    /// Boot 씬 전용 초기화 스크립트
    /// GameManager, SaveManager, AudioManager를 생성하고
    /// 저장 데이터 로드 후 MainMenu로 전환
    /// </summary>
    public class BootLoader : MonoBehaviour
    {
        [Header("Manager Prefabs")]
        [SerializeField] private GameObject _gameManagerPrefab;
        [SerializeField] private GameObject _audioManagerPrefab;

        [Header("Settings")]
        [SerializeField] private float _minimumLoadTime = 1.5f;

        IEnumerator Start()
        {
            if (GameManager.Instance == null && _gameManagerPrefab != null)
                Instantiate(_gameManagerPrefab);

            if (AudioManager.Instance == null && _audioManagerPrefab != null)
                Instantiate(_audioManagerPrefab);

            GameManager.Instance?.SaveManager?.Load();

            yield return new WaitForSeconds(_minimumLoadTime);

            GameManager.Instance?.LoadScene("MainMenu");
        }
    }
}
