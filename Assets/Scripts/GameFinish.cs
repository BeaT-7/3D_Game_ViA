using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameFinish : MonoBehaviour
{
    [SerializeField] TMP_Text endScreen;
    [SerializeField] TMP_Text timerText;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject restartButton;

    public GameObject player;
    public GameObject vehicle;
    public Transform playerRotation;

    public Rigidbody playerRB;
    public CameraMovement cameraScript;

    private bool gameOver = false;

    public Transform startTransform;
    public Transform endTransform;
    private Vector3 startTransformPlayer;
    private Vector3 endTransformPlayer;
    public float duration;

    private float elapsedTime = 0f;

    float gameTime = 0f;

    void Start()
    {

        startTransformPlayer = new Vector3(startTransform.position.x - 0.5f, startTransform.position.y + 0.1f, startTransform.position.z + 0.3f);
        endTransformPlayer = new Vector3(endTransform.position.x - 0.5f, endTransform.position.y + 0.1f, endTransform.position.z + 0.3f);
    }


    void Update()
    {
        gameTime += Time.deltaTime;
        if (gameOver)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            Vector3 newPositionVehicle = Vector3.Lerp(startTransform.position, endTransform.position, t);
            Vector3 newPositionPlayer = Vector3.Lerp(startTransformPlayer, endTransformPlayer, t);

            player.transform.position = newPositionPlayer;
            vehicle.transform.position = newPositionVehicle;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            quitButton.SetActive(true);
            restartButton.SetActive(true);

            string text = "You Won!";
            endScreen.text = text;
            endScreen.color = Color.green;

            int min = (int)gameTime / 60;
            int sec = (int)gameTime % 60;
            text = "It took you " + min.ToString("00") + ":" + sec.ToString("00") + "!";
            timerText.text = text;

            playerRotation.transform.rotation = vehicle.transform.rotation;
            playerRB.isKinematic = true;
            cameraScript.enabled = false;
            player.transform.position = new Vector3(vehicle.transform.position.x - 0.5f, vehicle.transform.position.y + 0.1f, vehicle.transform.position.z + 0.3f);
            gameOver = true;
        }
    }
}
