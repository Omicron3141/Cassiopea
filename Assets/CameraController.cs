using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public float panspeed = 0.9f;
    public float zoomspeed = 10f;

    float zoomlevel = 5;
    public float minzoom = 0;
    public float maxzoom = 7;

    public float xBound = 40;
    public float zBound = 40;

    public float edgeBoundary = 30;

	Camera cam;



	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {

		// Move camera to deal with arrow keys
		this.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * panspeed * Time.unscaledDeltaTime, Input.GetAxis("Vertical") * panspeed * Time.unscaledDeltaTime, 0f), Space.World);

		// If we aren't pressing the arrow keys, but our mouse is close to the edge, move in that direction
        if (Input.GetAxis("Horizontal")==0 && Input.GetAxis("Vertical") == 0)
        {
            if(Input.mousePosition.x>Screen.width - edgeBoundary)
            {
				this.transform.Translate(new Vector3(0.5f * panspeed * Time.unscaledDeltaTime, 0f, 0f), Space.World);
            }else if (Input.mousePosition.x < edgeBoundary)
            {
				this.transform.Translate(new Vector3(-0.5f * panspeed * Time.unscaledDeltaTime, 0f, 0f), Space.World);
            }


            if (Input.mousePosition.y > Screen.height - edgeBoundary)
            {
				this.transform.Translate(new Vector3(0f, 0.5f * panspeed * Time.unscaledDeltaTime, 0f), Space.World);
            }
            else if (Input.mousePosition.y < edgeBoundary)
            {
				this.transform.Translate(new Vector3(0f, -0.5f * panspeed * Time.unscaledDeltaTime, 0f), Space.World);
            }
        }


		// Don't go off the map
        if (transform.position.x > xBound)
        {
            this.transform.Translate(new Vector3((xBound - transform.position.x), 0f, 0f), Space.World);
        }
        else if (transform.position.x < -xBound)
        {
            this.transform.Translate(new Vector3((-xBound - transform.position.x), 0f, 0f), Space.World);
        }

        if (transform.position.z > zBound)
        {
			this.transform.Translate(new Vector3(0f, (zBound - transform.position.z), 0f), Space.World);
        }
        else if (transform.position.z < -zBound)
        {
			this.transform.Translate(new Vector3(0f, (-zBound - transform.position.z), 0f), Space.World);
        }

    }


	//Handles scrolling
    public void OnGUI()
    {
        if (Event.current.type == EventType.ScrollWheel)
        {
            // do stuff with  Event.current.delta
			zoomlevel += Event.current.delta.y * Time.unscaledDeltaTime * zoomspeed;
			// lock zoom to boundaries
            if (zoomlevel < minzoom)
            {
                zoomlevel = minzoom;
            }
            else if (zoomlevel > maxzoom)
            {
                zoomlevel = maxzoom;
            }
			cam.orthographicSize = zoomlevel;

        }
    }
}

