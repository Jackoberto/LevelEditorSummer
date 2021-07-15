using UnityEngine;

public class EditorCamera : MonoBehaviour {

	public float scrollSens;
    public float speed;
    public bool moveCameraWithMouse;
    public float maxZoomOut = 100;
    private float originalSpeed;
    private Vector3 startPos;
    private SaveMenu menu;

    private void Start()
    {
	    menu = FindObjectOfType<SaveMenu>(true);
	    originalSpeed = speed;
	    startPos = transform.position;
    }

    void Update ()
	{
		if (AnyMenuIsOpen)
			return;
		var mousePosScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		speed = Input.GetKey(KeyCode.LeftShift) ? originalSpeed * 2 : originalSpeed;
		MoveCameraWithKeys();
		ScrollFunction(mousePosScreen);
		if (moveCameraWithMouse)
			MoveCameraWithMouse(mousePosScreen);
	}

    private bool AnyMenuIsOpen => menu.gameObject.activeSelf;

    void ScrollFunction(Vector3 mousePosScreen)
    {
	    if (mousePosScreen.x < 0.9)
		{
			if (Input.GetAxis("Mouse ScrollWheel") != 0f)
			{
				transform.position += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * (400 * scrollSens * speed) * Time.deltaTime);
				if (transform.position.z < -maxZoomOut)
					transform.position = new Vector3(transform.position.x, transform.position.y, -maxZoomOut);
				if (transform.position.z > -10)
					transform.position = new Vector3(transform.position.x, transform.position.y, -10);
			}
		}
    }

	void MoveCameraWithMouse(Vector3 mousePosScreen)
	{
		if (mousePosScreen.x > 1)
		{
			transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
		}

		else if (mousePosScreen.x < 0)
		{
			transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
		}

		if (mousePosScreen.y > 1)
		{
			transform.position += new Vector3(0, speed * Time.deltaTime, 0);
		}

		else if (mousePosScreen.y < 0)
		{
			transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
		}
	}

	void MoveCameraWithKeys()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			transform.position = startPos;
		}
		
		if (Input.GetAxis("Horizontal") != 0)
		{
			transform.position += new Vector3 (Input.GetAxis("Horizontal") * (4 * speed) * Time.deltaTime, 0, 0);
		}

		if (Input.GetAxis("Vertical") != 0)
		{
			transform.position += new Vector3(0, Input.GetAxis("Vertical") * (4 * speed) * Time.deltaTime, 0);
		}
	}
}
