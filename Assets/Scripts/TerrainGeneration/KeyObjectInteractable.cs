using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class KeyObjectInteractable : MonoBehaviour
{

    [SerializeField] private float rotateSpeed;
    [SerializeField] private float distance;
    [SerializeField] private float movementSpeed;

    [SerializeField] private GameObject player;

    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private MenuManager menuManager;
    private Vector3 _middlePosition;
    private bool _movingUp;
    private Vector3 _upEnd;

    // Start is called before the first frame update
    void Start()
    {
        _middlePosition = transform.position;
        _upEnd = _middlePosition + new Vector3(0, distance, 0);
        _movingUp = true;

    }

    // Update is called once per frame
    void Update()
    {

        var position2 = transform.position;
        Vector2 position = new Vector2(position2.x, position2.z);
        
        var position3 = player.transform.position;
        Vector2 playerPosition = new Vector2(position3.x, position3.z);
        
        Vector3 position1 = transform.position;
        
        var hits = Physics.RaycastAll(position1, cameraFollow.offset,
            Vector3.Distance(position1, position1 + cameraFollow.gameObject.transform.position));
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("Environment"))
            {
                hit.collider.gameObject.transform.GetChild(0).GetComponent<Renderer>().shadowCastingMode =
                    ShadowCastingMode.ShadowsOnly;
                hit.collider.gameObject.GetComponent<ObjectFader>().stayFaded = true;
            }
        }

        if (Vector2.Distance(position, playerPosition) <= 3)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                //AudioManager.instance.PickUp();
                Debug.Log(gameObject.name);
                if (gameObject.name == "Potion(Clone)")
                {
                    Debug.Log("Picked up Potion");
                    GameManager.Instance.GotPotion = true;
                    menuManager.PotionCrossOut();
                }
                else if (gameObject.name == "Candles(Clone)")
                {
                    Debug.Log("Picked up Candles");
                    GameManager.Instance.GotCandles = true;
                    menuManager.CandlesCrossOut();
                }
                else if (gameObject.name == "Keys(Clone)")
                {
                    Debug.Log("Picked up Keys");
                    GameManager.Instance.GotKeys = true;
                    menuManager.KeysCrossOut();
                }
                
                gameObject.SetActive(false);
            }
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
        
        if (_movingUp)
        {
            transform.position += new Vector3(0, movementSpeed, 0) * Time.deltaTime;
            if (transform.position.y >= _upEnd.y)
            {
                _movingUp = false;
            }
        }
        else
        {
            transform.position += new Vector3(0, -movementSpeed, 0) * Time.deltaTime;
            if (transform.position.y <= _middlePosition.y)
            {
                _movingUp = true;
            }
        }
        
        transform.Rotate(0, rotateSpeed, 0);
    }
}
