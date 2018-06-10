using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Game game;
    public enum playerState { movement, battle };
    public playerState state;  

    private GameObject destinationTile;

    private float width;
    private float height;
    public float cameraSpeed;
    public GameObject mainCamera;

    void Awake()
    {
        width = Screen.width;
        height = Screen.height;
        state = playerState.movement;
    }

    void Update()
    {
        if (state == playerState.movement)
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("movement");
                RaycastHit hit;
                Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                int mask = 1 << 10;
                if (Physics.Raycast(cameraRay, out hit, 100.0f, mask))
                {
                    if (hit.transform.tag == "Floor")
                    {
                        destinationTile = hit.transform.gameObject;
                        game.gameController.SetCharacterPath(destinationTile);
                    }
                }
            }
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x / width > 0.9f && mousePos.x / width <= 1.0f)
            {
                mainCamera.transform.position = mainCamera.transform.position + mainCamera.transform.right * cameraSpeed;
            }
            else if (mousePos.x / width < 0.1f && mousePos.x / width >= 0.0f)
            {
                mainCamera.transform.position = mainCamera.transform.position - mainCamera.transform.right * cameraSpeed;
            }
            else if (mousePos.y / height > 0.9f && mousePos.y / height <= 1.0f)
            {
                mainCamera.transform.position = mainCamera.transform.position + mainCamera.transform.forward * cameraSpeed * 2;
            }
            else if (mousePos.y / height < 0.1f && mousePos.y / height >= 0.0f)
            {
                mainCamera.transform.position = mainCamera.transform.position - mainCamera.transform.forward * cameraSpeed * 2;
            }
        }
        else if (state == playerState.battle)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                int mask = 1 << 8;
                if (Physics.Raycast(cameraRay, out hit, 100.0f, mask))
                {
                    if (hit.transform.tag == "Die")
                    {
                        game.gameController.OnClickDieDown(hit.transform.gameObject);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                int mask = 1 << 11;
                if (Physics.Raycast(cameraRay, out hit, 100.0f, mask))
                {
                    if (hit.transform.tag == "BattleUI")
                    {
                        game.gameController.OnClickDieUp(hit.point);
                    }
                }
            }
        }
    }

}
