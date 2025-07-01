using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour
{
    [SerializeField] private List<Image> _actionSlots;
    private bool _isGameOver = false;
    public static event System.Action OnGameLose;

    private void Awake()
    {
        if (_actionSlots != null && _actionSlots.Count > 0) 
        {
            foreach (Image slot in _actionSlots)
            {
                if (slot != null)
                {
                    slot.sprite = null;
                    slot.enabled = false;
                }
            }
        }
    }

    public void AddFigurineToBar(Sprite spriteToAdd)
    {
        if (_isGameOver)
        {
            return;
        }

        if (spriteToAdd == null)
        {
            return;
        }

        bool addedToEmptySlot = false;
        for (int i = 0; i < _actionSlots.Count; i++)
        {
            if (_actionSlots[i] != null && _actionSlots[i].sprite == null)
            {
                _actionSlots[i].sprite = spriteToAdd;
                _actionSlots[i].enabled = true;
                addedToEmptySlot = true;
                break;
            }
        }

        if (addedToEmptySlot)
        {
            CheckAndClearMatches();

            int currentFilledSlots = 0;
            foreach (Image slot in _actionSlots)
            {
                if (slot.sprite != null)
                {
                    currentFilledSlots++;
                }
            }

            if (currentFilledSlots >= _actionSlots.Count)
            {
                _isGameOver = true;
                OnGameLose?.Invoke();
            }
        }
        else
        {
            _isGameOver = true;
            OnGameLose?.Invoke();
        }
    }

    private void CheckAndClearMatches()
    {
        if (_isGameOver) return;

        Dictionary<Sprite, int> spriteCounts = new Dictionary<Sprite, int>();
        List<Sprite> spritesToRemove = new List<Sprite>();

        foreach (Image slot in _actionSlots)
        {
            if (slot != null && slot.sprite != null)
            {
                if (spriteCounts.ContainsKey(slot.sprite))
                {
                    spriteCounts[slot.sprite]++;
                }
                else
                {
                    spriteCounts[slot.sprite] = 1;
                }
            }
        }

        bool matchFound = false;
        foreach (var pair in spriteCounts)
        {
            if (pair.Value >= 3)
            {
                spritesToRemove.Add(pair.Key);
                matchFound = true;
            }
        }

        if (!matchFound)
        {
            return;
        }

        bool itemsWereActuallyRemoved = false;
        for (int i = 0; i < _actionSlots.Count; i++)
        {
            if (_actionSlots[i] != null && _actionSlots[i].sprite != null)
            {
                if (spritesToRemove.Contains(_actionSlots[i].sprite))
                {
                    _actionSlots[i].sprite = null;
                    _actionSlots[i].enabled = false;
                    itemsWereActuallyRemoved = true;
                }
            }
        }

        if (itemsWereActuallyRemoved)
        {
            CompactBarEfficiently();
        }
    }

    private void CompactBarEfficiently()
    {
        if (_isGameOver) return;

        int writeIndex = 0;
        for (int readIndex = 0; readIndex < _actionSlots.Count; readIndex++)
        {
            if (_actionSlots[readIndex] != null && _actionSlots[readIndex].sprite != null)
            {
                if (writeIndex < readIndex)
                {
                    _actionSlots[writeIndex].sprite = _actionSlots[readIndex].sprite;
                    _actionSlots[writeIndex].enabled = true;

                    _actionSlots[readIndex].sprite = null;
                    _actionSlots[readIndex].enabled = false;
                }
                else if (writeIndex == readIndex)
                {
                    _actionSlots[writeIndex].enabled = true;
                }
                writeIndex++;
            }
        }
        for (int i = writeIndex; i < _actionSlots.Count; i++)
        {
            if (_actionSlots[i] != null)
            {
                _actionSlots[i].sprite = null;
                _actionSlots[i].enabled = false;
            }
        }
    }

    public void ResetBar()
    {
        _isGameOver = false;
        foreach (Image slot in _actionSlots)
        {
            if (slot != null)
            {
                slot.sprite = null;
                slot.enabled = false;
            }
        }
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }
}