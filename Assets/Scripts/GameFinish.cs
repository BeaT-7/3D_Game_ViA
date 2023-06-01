using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinish : MonoBehaviour
{
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

    void Start()
    {
        startTransformPlayer = new Vector3(startTransform.position.x - 0.5f, startTransform.position.y + 0.1f, startTransform.position.z + 0.3f);
        endTransformPlayer = new Vector3(endTransform.position.x - 0.5f, endTransform.position.y + 0.1f, endTransform.position.z + 0.3f);
    }


    void Update()
    {
        if (gameOver)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            Vector3 newPositionVehicle = Vector3.Lerp(startTransform.position, endTransform.position, t);
            Vector3 newPositionPlayer = Vector3.Lerp(startTransformPlayer, endTransformPlayer, t);

            Debug.Log(newPositionPlayer);
            Debug.Log(newPositionVehicle);

            player.transform.position = newPositionPlayer;
            vehicle.transform.position = newPositionVehicle;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRotation.transform.rotation = vehicle.transform.rotation;
            playerRB.isKinematic = true;
            cameraScript.enabled = false;
            player.transform.position = new Vector3(vehicle.transform.position.x - 0.5f, vehicle.transform.position.y + 0.1f, vehicle.transform.position.z + 0.3f);
            gameOver = true;
        }
    }
}
