using UnityEngine;

public static class HelperTools
{
    public static void AssignedInEditorCheck(Object objectToCheck)
    {
        if(objectToCheck == null)
        {
            Debug.LogWarning(objectToCheck + " is null!");
            Debug.Break();
        }
    }

    public static void AssignedInEditorCheck(string stringToCheck)
    {
        if (stringToCheck == null || stringToCheck == "")
        {
            Debug.LogWarning(stringToCheck + " is null or empty!");
            Debug.Break();
        }
    }
}