using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuImpl : MonoBehaviour, Menu
{
    public GameObject spawnPosition;
    public GameObject objectMenu;
    public GameObject[] objectMenuList;
    public GameObject[] objectList;
    public int[] numberOfObject;
    public string[] titles;
    public Text title;

    private int currentHorizontalIndex = 0;
    private int currentVerticalIndex = 0;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private float menuSpeed = 2f;
    private float initialHorizontalMenuPosition;
    private float initialVerticalMenuPosition;
    public float distanceBetweenMenuObjects = 0.2f;

    public void Start()
    {
        initialHorizontalMenuPosition = objectMenu.transform.localPosition.x;
        initialVerticalMenuPosition = objectMenu.transform.localPosition.y;
        title.text = createTitle(currentHorizontalIndex);
        title.enabled = false;
    }

    public void Update()
    {
        moveMenu();
    }

    public void enable()
    {
        //Debug.Log("Menu enabled");
        objectMenu.SetActive(true);
        title.enabled = true;
    }

    public void disable()
    {
        //Debug.Log("Menu disabled");
        objectMenu.SetActive(false);
        title.enabled = false;
    }

    public void navigateUp()
    {
        //Debug.Log("Menu up");
        if (!isMoving())
        {
            currentVerticalIndex += 1;
            if (currentVerticalIndex > objectList.Length - 1)
            {
                currentVerticalIndex = objectList.Length - 1;
            }
            else
            {
                isMovingUp = true;
                isMovingDown = false;
            }
        }
        //FIXME title.text = createTitle(currentHorizontalIndex);
    }

    public void navigateDown()
    {
        //Debug.Log("Menu down");
        if (!isMoving())
        {
            currentVerticalIndex -= 1;
            if (currentVerticalIndex < 0)
            {
                currentVerticalIndex = 0;
            }
            else
            {
                isMovingUp = false;
                isMovingDown = true;
            }
        }
        //FIXME title.text = createTitle(currentHorizontalIndex);
    }

    public void navigateLeft()
    {
        //Debug.Log("Menu left");
        if (!isMoving())
        {
            currentHorizontalIndex -= 1;
            if (currentHorizontalIndex < 0)
            {
                currentHorizontalIndex = 0;
            }
            else
            {
                isMovingLeft = false;
                isMovingRight = true;
            }
        }
        title.text = createTitle(currentHorizontalIndex);
    }

    public void navigateRight()
    {
        //Debug.Log("Menu right");
        if (!isMoving())
        {
            currentHorizontalIndex += 1;
            if (currentHorizontalIndex > objectList.Length - 1)
            {
                currentHorizontalIndex = objectList.Length - 1;
            }
            else
            {
                isMovingLeft = true;
                isMovingRight = false;
            }
        }
        title.text = createTitle(currentHorizontalIndex);

    }

    public void navigateSelect()
    {
        //Debug.Log("Menu selected " + currentPosition);

        //Spawn a new object instance
        int count = numberOfObject[currentHorizontalIndex];
        if (count > 0)
        {
            GameObject newObject = Instantiate(objectList[currentHorizontalIndex], spawnPosition.transform.position, objectMenuList[currentHorizontalIndex].transform.rotation);
            newObject.SetActive(true);

            print("before " + count + ":" + numberOfObject[currentHorizontalIndex]);

            numberOfObject[currentHorizontalIndex] = count - 1;

            print("after " + count + ":" + numberOfObject[currentHorizontalIndex]);
            title.text = createTitle(currentHorizontalIndex);
        }
    }

    private void moveMenu()
    {
        if (isMovingLeft)
        {
            objectMenu.transform.Translate(Vector2.left * menuSpeed * Time.deltaTime);
            if (menuReachedNextHorizontalPosition(currentHorizontalIndex, objectMenu.transform.localPosition.x, initialHorizontalMenuPosition))
            {
                isMovingLeft = false;
            }
        }
        else if (isMovingRight)
        {
            objectMenu.transform.Translate(Vector2.right * menuSpeed * Time.deltaTime);
            if (menuReachedNextHorizontalPosition(currentHorizontalIndex, objectMenu.transform.localPosition.x, initialHorizontalMenuPosition))
            {
                isMovingRight = false;
            }
        }
        else if (isMovingUp)
        {
            objectMenu.transform.Translate(Vector2.down * menuSpeed * Time.deltaTime);
            if (menuReachedNextVerticalPosition(currentVerticalIndex, objectMenu.transform.localPosition.y, initialVerticalMenuPosition))
            {
                isMovingUp = false;
            }
        }
        else if (isMovingDown)
        {
            objectMenu.transform.Translate(Vector2.up * menuSpeed * Time.deltaTime);
            if (menuReachedNextVerticalPosition(currentVerticalIndex, objectMenu.transform.localPosition.y, initialVerticalMenuPosition))
            {
                isMovingDown = false;
            }
        }
    }

    //Return true if the menuObject as reached the next position in the menu
    private bool menuReachedNextHorizontalPosition(int index, float currentPosition, float initialPosition)
    {
        if (isMovingLeft )
        {
            float currentMenuObjectPosition = currentPosition - initialPosition;
            float target = -(index * distanceBetweenMenuObjects);
            //Debug.Log("left " + currentMenuObjectPosition + " " + xPosition + " " + target);
            return currentMenuObjectPosition <= target;
        }
        else
        {
            float currentMenuObjectPosition = currentPosition - initialPosition;
            float target = -(index * distanceBetweenMenuObjects);
            //Debug.Log("Right " + currentMenuObjectPosition + " " + xPosition + " " + target);
            return currentMenuObjectPosition >= target;
        }
    }

    private bool menuReachedNextVerticalPosition(int index, float currentPosition, float initialPosition)
    {
        if (isMovingDown)
        {
            float currentMenuObjectPosition = currentPosition - initialPosition;
            float target = -(index * distanceBetweenMenuObjects);
            Debug.Log("down " + currentMenuObjectPosition + " " + currentPosition + " " + target);
            return currentMenuObjectPosition >= target;
        }
        else
        {
            float currentMenuObjectPosition = currentPosition - initialPosition;
            float target = -(index * distanceBetweenMenuObjects);
            Debug.Log("up " + currentMenuObjectPosition + " " + currentPosition + " " + target);
            return currentMenuObjectPosition <= target;
        }
    }

    private string createTitle(int currentIndex)
    {
        return titles[currentIndex] + "\nx" + numberOfObject[currentIndex];
    }

    private bool isMoving()
    {
        return isMovingDown || isMovingLeft || isMovingRight || isMovingUp;
    }
}
