using UnityEngine;

public class ClearShape : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    private ActionBar _actionBarInstance; 

    void Start() 
    {
        _actionBarInstance = FindObjectOfType<ActionBar>();
        
    }
    private void OnMouseDown()
    {
        
        _actionBarInstance.AddFigurineToBar(_sprite);
        Destroy(gameObject);
        
    }
}
