using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _playerBehaviour;
    [SerializeField] private PlayerInputHandler _input;

    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private ScoreEventPort _pickUpBombEvent;
    [SerializeField] private int _bombsInInventory = 5;
    [SerializeField] private float _bombPlaceCooldown = 10f;
    private float _timeSincePlacedBomb = 10;
    
    private void OnValidate()
    {
        if(!_playerBehaviour)
            if (!TryGetComponent(out _playerBehaviour))
                Debug.LogWarning("Inventory missing PlayerBehaviour reference", this);
        
        if(!_input)
            if(!TryGetComponent(out _input))
                Debug.LogWarning("Inventory missing PlayerInputHandler reference", this);
        
        if(!_bombPrefab)
            Debug.LogWarning("Inventory missing Bomb prefab reference", this);
    }

    private void Start()
    {
        _pickUpBombEvent.UpdateScore(_bombsInInventory);
    }

    private void OnEnable()
    {
        _input.OnPlaceBomb += TryPlaceBomb;
        _pickUpBombEvent.OnGainScore += IncreaseBombs;
        _pickUpBombEvent.UpdateScore(_bombsInInventory);
    }

    private void OnDisable()
    {
        _input.OnPlaceBomb -= TryPlaceBomb;
        _pickUpBombEvent.OnGainScore -= IncreaseBombs;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeSincePlacedBomb < _bombPlaceCooldown)
            _timeSincePlacedBomb += Time.deltaTime;
    }

    private void TryPlaceBomb()
    {
        if(_timeSincePlacedBomb < _bombPlaceCooldown)
            return;

        if (_playerBehaviour.currentHeldItem != null)
        {
            _playerBehaviour.currentHeldItem.PlaceItemDown();
        }
        Item bomb = Instantiate(_bombPrefab, transform.position, Quaternion.identity).GetComponent<Item>();
        _playerBehaviour.currentHeldItem = bomb;
        bomb.PickUp(_playerBehaviour._itemPickupPoint);
        _timeSincePlacedBomb = 0;
        _bombsInInventory--;
        _pickUpBombEvent.UpdateScore(_bombsInInventory);
    }

    private void IncreaseBombs(int amount)
    {
        _bombsInInventory += amount;
        _pickUpBombEvent.UpdateScore(_bombsInInventory);
    }
}
