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
<<<<<<< HEAD
        [SerializeField] private float _minimumLoadTime = 1.5f; // 최소 로딩 시간 (초)

        IEnumerator Start()
        {
            // GameManager가 없으면 생성
            if (GameManager.Instance == null && _gameManagerPrefab != null)
            {
                Instantiate(_gameManagerPrefab);
            }

            // AudioManager가 없으면 생성
            if (AudioManager.Instance == null && _audioManagerPrefab != null)
            {
                Instantiate(_audioManagerPrefab);
            }

            // 저장 데이터 로드
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SaveManager?.Load();
            }

            // 최소 로딩 시간 보장 (스플래시 화면용)
            yield return new WaitForSeconds(_minimumLoadTime);

            // MainMenu로 전환
=======
        [SerializeField] private float _minimumLoadTime = 1.5f;

        IEnumerator Start()
        {
            if (GameManager.Instance == null && _gameManagerPrefab != null)
                Instantiate(_gameManagerPrefab);

            if (AudioManager.Instance == null && _audioManagerPrefab != null)
                Instantiate(_audioManagerPrefab);

            GameManager.Instance?.SaveManager?.Load();

            yield return new WaitForSeconds(_minimumLoadTime);

>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
            GameManager.Instance?.LoadScene("MainMenu");
        }
    }
}
