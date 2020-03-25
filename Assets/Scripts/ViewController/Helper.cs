using UnityEngine;

public static class Helper
{
    /// <summary>
    /// Converts a number of pixels into a distance in world space
    /// </summary>
    /// <param name="canvasPixels"></param>
    /// <returns></returns>
    public static float CanvasToWorldScale(float canvasPixels)
    {
        float canvasWorldSize = Vector3.Distance(CanvasToWorldSpace(Vector2.zero), CanvasToWorldSpace(new Vector2(Screen.width, 0f)));
        return canvasPixels * canvasWorldSize / Screen.width;
    }

    public static Vector3 CanvasToWorldSpace(Vector2 canvasPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(canvasPosition);
        return new Vector3(worldPosition.x, worldPosition.y, 0f);
    }

    public static Vector2 NearestEdgePosition(Vector2 anchoredPosition)
    {
        //Position relative to the bottom left corner of the cavnas;
        float canvasPositionX = anchoredPosition.x + Screen.width / 2f;
        float canvasPositionY = anchoredPosition.y + Screen.height / 2f;

        float distanceLeft = canvasPositionX;
        float distanceDown = canvasPositionY;
        float distanceRight = Screen.width - canvasPositionX;
        float distanceUp = Screen.height - canvasPositionY;

        if (distanceLeft < distanceDown && distanceLeft < distanceRight && distanceLeft < distanceUp)
        {
            //Left edge is closest
            return new Vector2(-Screen.width / 2f, anchoredPosition.y);
        }
        else if (distanceDown < distanceRight && distanceDown < distanceUp)
        {
            //Down edge is closest
            return new Vector2(anchoredPosition.x, -Screen.height / 2f);
        }
        else if (distanceRight < distanceUp)
        {
            //Right edge is closest
            return new Vector2(Screen.width / 2f, anchoredPosition.y);
        }
        else
        {
            //Up edge is closest
            return new Vector2(anchoredPosition.x, Screen.height / 2f);
        }
    }
}
