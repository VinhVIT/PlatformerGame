using UnityEngine;

public class ConditionalHideAttribute : PropertyAttribute
{
    public string ConditionalSourceField;
    public bool HideInInspector;

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector = false)
    {
        ConditionalSourceField = conditionalSourceField;
        HideInInspector = hideInInspector;
    }
}
