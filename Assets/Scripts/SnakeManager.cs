using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] float distanceBetween = 0.2f;
    [SerializeField] float speed = 280;
    [SerializeField] float turnSpeed = 180;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> snakeBody = new List<GameObject>();

    float countUp = 0;
    private void Start()
    {
        CreateBodyParts();
    }
    private void FixedUpdate()
    {
        ManageSnakeBody();
        SnakeMovement();
    }
    void ManageSnakeBody()
    {
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        for (int i = 0; i < snakeBody.Count; i++)
        {
            if (snakeBody[i] == null)
            {
                snakeBody.RemoveAt(i);
                i = i - 1;
            }
        }
        if (snakeBody.Count == 0)
        {
            Destroy(this);
        }
    }
    private void CreateBodyParts()
    {
        if (snakeBody.Count == 0)
        {
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp1.GetComponent<MarkerManager>())
            {
                temp1.AddComponent<MarkerManager>();
            }
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);
        }
        MarkerManager markM = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();
        if (countUp == 0)
        {
            markM.ClearMarkerList();
        }
        countUp += Time.deltaTime;
        if (countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], markM.markerList[0].position, markM.markerList[0].rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }
    }
    public void AddBodyParts(GameObject obj)
    {
        bodyParts.Add(obj);
    }

    void SnakeMovement()
    {
        snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeBody[0].transform.right * speed * Time.deltaTime;
        if (Input.GetAxis("Horizontal") != 0)
        {
            snakeBody[0].transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal")));
        }
        if (snakeBody.Count > 1)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                MarkerManager markM = snakeBody[i - 1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = markM.markerList[0].position;
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }
}
