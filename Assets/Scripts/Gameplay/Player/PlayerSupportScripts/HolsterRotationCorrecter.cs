using UnityEngine;

public class HolsterRotationCorrecter : MonoBehaviour
{
    [SerializeField] InputDriver input;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(input.aimAngle * 2.0f, 0.0f, 0.0f));
        //DON'T ASK ME HOW THIS WORKS ANYMORE. THE SYSTEM HAS BECOME SELF AWARE AND I DO NOT UNDERSTAND HOW
        //THE REVERSE ROTATION SYSTEM IS STILL WORKING. IT SHOULDN'T BE BUT IT IS. I JUST THANK MY LUCKY
        // STARS THAT IT STILL WANTS TO WORK. ~RANDY
    }
}
