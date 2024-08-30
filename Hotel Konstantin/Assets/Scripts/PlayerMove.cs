using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerRotation Rotation;
    [SerializeField] private Transform Camera;
    [SerializeField] private float Stamina;
    [SerializeField] private float MaxStamina;
    [SerializeField] private float Speed;
    private float CameraShake = 0;
    private Coroutine StabilizeCoroutine = null; //шн€га от 0 до 360 градусов, касера прыгает от синуса

    private void Update()
    {
        if (Pause._Paused)
        {
            return;
        }

        Vector3 direction = transform.forward * InputManager.GetAxis(InputManager.AxisEnum.Vertical) + transform.right * InputManager.GetAxis(InputManager.AxisEnum.Horizontal);

        if(direction != Vector3.zero)
        {
            if(StabilizeCoroutine != null)
            {
                StopCoroutine(StabilizeCoroutine);
            }

            direction.y = 0;
            direction = direction.normalized;

            float speed = Speed * Time.deltaTime;
            if (InputManager.GetButton(InputManager.ButtonEnum.Run) && direction != Vector3.zero && Stamina > 0)
            {
                speed *= 2;
                Stamina -= Time.deltaTime;
            }
            else if (Stamina < MaxStamina)
            {
                Stamina += Time.deltaTime;

                if (Stamina > MaxStamina)
                {
                    Stamina = MaxStamina;
                }
            }

            CameraShake = (CameraShake + speed * 150) % 360;

            Camera.localPosition = new Vector3(0, 1.7f + Mathf.Sin(CameraShake * Mathf.Deg2Rad) * 0.05f, 0);

            Vector3 rotation = Rotation._Rotation;
            rotation.z = Mathf.Sin(CameraShake * Mathf.Deg2Rad) * 1f;
            Rotation._Rotation = rotation;
            
            transform.position += direction * speed;
        }
        else
        {
            if (StabilizeCoroutine == null && CameraShake % 180 != 0)
            {
                StabilizeCoroutine = StartCoroutine(Stabilize());
            }
        }
    }

    private IEnumerator Stabilize()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        float target = 0;

        if (Mathf.Abs(CameraShake - 0) > Mathf.Abs(CameraShake - 360))
        {
            if(Mathf.Abs(CameraShake - 360) > Mathf.Abs(CameraShake - 180))
            {
                target = 180;
            }
            else
            {
                target = 360;
            }
        }
        else if (Mathf.Abs(CameraShake - 0) > Mathf.Abs(CameraShake - 180))
        {
            target = 0;
        }

        byte sign = (byte)Mathf.Sign(target - CameraShake);
        while (true)
        {
            CameraShake += Mathf.Sign(target - CameraShake) * Speed * 90 * Time.deltaTime;

            if (sign != (byte)Mathf.Sign(target - CameraShake))
            {
                CameraShake = target;
                break;
            }

            Camera.localPosition = new Vector3(0, 1.7f + Mathf.Sin(CameraShake * Mathf.Deg2Rad) * 0.05f, 0);

            Vector3 rotation = Rotation._Rotation;
            rotation.z = Mathf.Sin(CameraShake * Mathf.Deg2Rad) * 1f;
            Rotation._Rotation = rotation;

            yield return waitForEndOfFrame;
        }

        StabilizeCoroutine = null;
    }
}
