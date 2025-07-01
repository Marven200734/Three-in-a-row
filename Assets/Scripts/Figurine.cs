using UnityEngine;

public class Figurine : MonoBehaviour
{
    private ActionBar _actionBar;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _actionBar = FindObjectOfType<ActionBar>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void OnMouseDown()
    {

        if (_actionBar.IsGameOver())
        {
            return;
        }

        if (_spriteRenderer != null && _spriteRenderer.sprite != null)
        {
            _actionBar.AddFigurineToBar(_spriteRenderer.sprite);
            GameManager.Instance.RegisterCollectedFigurine();
            Destroy(gameObject);
        }
    }
}