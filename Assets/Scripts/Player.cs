using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int ScoreValue = 0;
    private Vector2 _movementInput;
    private Vector3 _movement;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private ScenarioData _scenario;
    [SerializeField] private GameObject _wallPrefab;

    private int maxFallDistance = -10;
    private float ballSize = 1f;
    private float ballSizeModifier = 0.001f;
    private float timer = 10f;
    private Vector3 _previousPosition = new Vector3();
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        transform.localScale = new Vector3(ballSize, ballSize, ballSize);

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            ScoreValue = PlayerPrefs.GetInt("ScoreValue");
        }
        _scoreText.text = "Score : " + ScoreValue;
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            _rigidbody.AddForce(Input.GetAxis("Horizontal") * _speed * Time.deltaTime, 0f, Input.GetAxis("Vertical") * _speed * Time.deltaTime);
        }

        if (transform.position.y <= maxFallDistance)
        {
            SceneManager.LoadScene("Tutorial");
        }

     //   ballSize = timer;
     //   Debug.Log(ballSize);

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
        }

        // Verifier la distance entre la position precedente (_previousPosition) et la position actuelle (transform local)
        //var distance = Math.Sqrt(Math.Pow(_previousPosition) - Math.Pow(transform.localScale));

        // si il y'a une distance entre les 2 positions, faut ajouter le modifier de taille de la boule

        transform.localScale = new Vector3(ballSize, ballSize, ballSize);
    }

    void OnMove(InputValue AxisValues)
    {
        _movementInput = AxisValues.Get<Vector2>();
    }

    void OnJump()
    {
        Debug.Log("test ok");
        transform.GetComponent<Renderer>().material.color = Color.blue;
    }

    private void FixedUpdate()
    {
        _movement = new Vector3(_movementInput.x, 0f, _movementInput.y);
        _rigidbody.AddForce(_movement * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target_Trigger"))
        {
            Destroy(other.gameObject);
            UpdateScore();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Destroy(collision.gameObject);
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        Instantiate(_wallPrefab, _scenario.FirstWalls[ScoreValue % _scenario.FirstWalls.Length].position, Quaternion.Euler(_scenario.FirstWalls[ScoreValue % _scenario.FirstWalls.Length].orientation));

        ScoreValue++;

        _scoreText.text = "Score : " + ScoreValue;

        if (PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.SetString("Score", "Score : " + ScoreValue.ToString());
        }
        if (ScoreValue >= 8 && SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.SetInt("ScoreValue", ScoreValue);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (transform.localScale.x > 5)
        {
            transform.localScale = new Vector3(5, 5, 5);
        }
    }
    void OnGUI()
    {

        GUI.Box(new Rect(10, 10, 50, 20), "" + timer.ToString("0"));
    }

}
