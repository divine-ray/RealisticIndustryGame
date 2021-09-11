using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a list of all active attractors.
/// Useful for Newtonian physics, but also used in other ways.
/// </summary>
public class AttractorList : MonoBehaviour {

    public static List<Attractor> list = new List<Attractor>();
    
    /// <summary>
    /// Enables the attractor.
    /// </summary>
    public static void AddToList (Attractor attractor)
    {
        if (!list.Contains(attractor))
            list.Add(attractor);
    }

    /// <summary>
    /// Disables the attractor.
    /// </summary>
    public static void RemoveFromList(Attractor attractor)
    {
        if (list.Contains(attractor))
            list.Remove(attractor);
    }
}