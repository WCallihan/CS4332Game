using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    [Header("Health Settings")]
    [SerializeField] int _maxHealth = 100;
    [SerializeField] Slider _healthBar;

    [Header("Hurt Settings")]
    [SerializeField] Image _flashPanel;
    [SerializeField] float _flashLength;
    [SerializeField] AudioClip _hurtSound;

    [Header("Death Settings")]
    [SerializeField] GameObject[] _playerVisuals;
    [SerializeField] AudioClip _deathSound;

    private AudioSource _audioSource;

    private int _currentHealth;

    void Awake() {
        _audioSource = GetComponent<AudioSource>();
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage) {
        _currentHealth -= damage; //damage the player
        if(_healthBar) _healthBar.value = _currentHealth; //update health bar
        StartCoroutine(RedFlash()); //flash the screen red
        if(_hurtSound) _audioSource.PlayOneShot(_hurtSound); //play hurt sound effect
        if(_currentHealth <= 0) {
            Die(); //kill player
        }
    }

    public bool Heal(int healing) {
        //if the player is not at full health, heal them and update the health bar, and return true
        if(_currentHealth < _maxHealth) {
            _currentHealth += healing; //heal the player
            if(_currentHealth > _maxHealth) {
                _currentHealth = _maxHealth; //make sure the health doesn't exceed the max
            }

            if(_healthBar) _healthBar.value = _currentHealth; //update health bar
            return true;
        //if the player is at full health, do nothing and return false
        } else {
            return false;
        }
    }

    private void Die() {
        Time.timeScale = 0f; //freeze the game
        //deactivate all player visuals
        foreach(var v in _playerVisuals) {
            v.SetActive(false);
        }
        if(_deathSound) _audioSource.PlayOneShot(_deathSound); //play death sound effect
        //FindObjectOfType<Level01Controller>().DeathScreen(); //call level controller to enter death state
    }

    private IEnumerator RedFlash() {
		if(_flashPanel == null) yield return null;

        float fadeLength = _flashLength / 2;
        //fade red in
        for(float i = 0; i < fadeLength; i += Time.deltaTime) {
            Color panelColor = _flashPanel.color;
            panelColor.a = Mathf.Lerp(0, 0.25f, i/fadeLength);
            _flashPanel.color = panelColor;
            yield return null;
        }
        
        //fade red out
        for(float i = 0; i < fadeLength; i += Time.deltaTime) {
            Color panelColor = _flashPanel.color;
            panelColor.a = Mathf.Lerp(0.25f, 0, i/fadeLength);
            _flashPanel.color = panelColor;
            yield return null;
        }

        //ensure alpha is back at 0
        _flashPanel.color = new Color(255, 0, 0, 0);
    }
}