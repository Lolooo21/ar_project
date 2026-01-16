using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PyramidConf", menuName = "Scriptable Objects/PyramidConf")]
public class PyramidConf : ScriptableObject
{
    [Header("Apparition")] // titre visuel dans l'inspecteur
    public Vector2 tailleRandom;
    public Vector2 masseRandom;
    public List<Material> materiauxRandom = new();
    
    [Header("Mouvements")]
    public Vector2 rayonMovement;

    public Vector2 tempsAttente;
    public float stopMouvement;
    
    [Header("Vitesses")]
    public float acceleration;
    public float vitesseMax;
    
    [Header("Saut")]
    public float tempsSaut;
    public float vitesseSaut;
    public Vector2 tempsSautRandom;
    public Vector2 forceSautRandom;

    [Header("Nourriture")]
    public float rayonNourriture;
    public float distanceNourriture;
    public float grossir;
}
