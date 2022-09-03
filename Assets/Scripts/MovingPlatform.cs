using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] positionObjects;
    private Vector3[] positions;
    private Quaternion[] rotations;
    [SerializeField]
    private float movementSpeed=1f;
    [SerializeField]
    private float rotationSpeed=5f;
    private int currentPosition = 0;
    private int currentRotation = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetPositionsAndRotations();
    }

    // Update is called once per frame
    void Update()
    {
    if(transform.position!=positions[currentPosition])
    {
        ChangePosition(positions[currentPosition],transform.position);
    }
        
    else
        if(currentPosition+1<positions.Length)
            currentPosition++;
        else
            currentPosition=0;


    if(transform.rotation!=rotations[currentRotation])
    {
        ChangeRotation(rotations[currentRotation],transform.rotation);
    }      
    else
        if(currentRotation+1<rotations.Length)
            currentRotation++;
        else
            currentRotation=0;
    
    }
    
    

    private void ChangePosition(Vector3 _position, Vector3 prePosition)
    {
        transform.position = Vector3.MoveTowards(prePosition, _position, movementSpeed*Time.deltaTime);
    }
    private void ChangeRotation(Quaternion _rotation, Quaternion preRotation)
    {
        transform.rotation = Quaternion.RotateTowards(preRotation, _rotation, rotationSpeed*Time.deltaTime);
    }

    private void SetPositionsAndRotations()
    {
        positions = new Vector3[positionObjects.Length];
        int a = 0;
        foreach(Transform positionObject in positionObjects)
        {
            positions[a] = positionObject.position;
            a++;
        }

        rotations = new Quaternion[positionObjects.Length];
        a = 0;
        foreach(Transform positionObject in positionObjects)
        {
            rotations[a] = positionObject.rotation;
            a++;
        }
    }
}
