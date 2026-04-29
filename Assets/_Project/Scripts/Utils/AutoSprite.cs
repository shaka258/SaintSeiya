using UnityEngine;

namespace SaintSeiya.Utils
{
    /// <summary>
    /// 스프라이트 없이도 테스트 가능하도록
    /// 런타임에 컬러 사각형/원형 스프라이트를 자동 생성
    /// </summary>
    public static class DebugSpriteFactory
    {
        public static Sprite CreateRect(Color color, int width = 32, int height = 48)
        {
            var tex = new Texture2D(width, height);
            var pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = color;
            tex.SetPixels(pixels);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, width, height),
                new Vector2(0.5f, 0.5f), 32f);
        }

        public static Sprite CreateCircle(Color color, int size = 32)
        {
            var tex = new Texture2D(size, size);
            var pixels = new Color[size * size];
            float r = size * 0.5f;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dx = x - r, dy = y - r;
                pixels[y * size + x] = (dx*dx + dy*dy <= r*r) ? color : Color.clear;
            }
            tex.SetPixels(pixels);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size),
                new Vector2(0.5f, 0.5f), 32f);
        }
    }

    /// <summary>
    /// 씬 시작 시 스프라이트가 없는 오브젝트에
    /// 자동으로 컬러 사각형을 부여하는 컴포넌트
    /// </summary>
    public class AutoSprite : MonoBehaviour
    {
        [Header("Debug Sprite")]
        public Color color = Color.white;
        public bool isCircle = false;

        void Awake()
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr == null || sr.sprite != null) return;

            sr.sprite = isCircle
                ? DebugSpriteFactory.CreateCircle(color)
                : DebugSpriteFactory.CreateRect(color);
        }
    }
}
