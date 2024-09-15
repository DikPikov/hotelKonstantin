using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform Camera;
    [SerializeField] private float Sensibility;
    [SerializeField] private Vector3 Rotation;

    public float _Sensitivity
    {
        get
        {
            return Sensibility;
        }
        set
        {
            Sensibility = value;
        }
    }
    public Vector3 _Rotation
    {
        get
        {
            return Rotation;
        }
        set
        {
            Rotation = value;

            NormalizeRotation();

            if (Rotation.x < 270 && Rotation.x > 90)
            {
                if (Rotation.x > 135)
                {
                    Rotation.x = 270;
                }
                else
                {
                    Rotation.x = 90;
                }
            }

            Camera.localEulerAngles = new Vector3(Rotation.x, 0, Rotation.z);
            transform.eulerAngles = new Vector3(0, Rotation.y, 0);
        }
    }

    private void Update()
    {
        if (Pause._Paused)
        {
            return;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        _Rotation += Sensibility * new Vector3(-Input.GetAxis("mouseY"), Input.GetAxis("mouseX"), 0);
#else
        _Rotation += Sensibility * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
#endif
    }

    private void NormalizeRotation()
    {
        Rotation.x %= 360;
        if (Rotation.x < 0)
        {
            Rotation.x += 360;
        }
        Rotation.x %= 360;
    }
}