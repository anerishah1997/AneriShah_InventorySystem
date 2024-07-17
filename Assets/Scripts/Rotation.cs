using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0, 10f, 0) * 5 * Time.deltaTime);
    }
}
