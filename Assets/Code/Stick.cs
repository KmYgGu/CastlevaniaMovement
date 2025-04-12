using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public int Go;
    private void Start()
    {
        transform.localScale = new Vector3(PlayerMove.LookKey * -1, 1, 1);
        Go = PlayerMove.LookKey;
    }
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Translate((Vector2.left * Go) * 10f * Time.deltaTime);
        
    }
}
