using UnityEngine;
public class CameraTouchMove : MonoBehaviour
{

    public float panSpeed = 0.5f; // Kamera hareket hızı
    public Vector2 panLimit; // Kamera hareket sınırları

    private Vector2 initialTouchPosition; // İlk dokunma pozisyonu
    private void Awake()
    {
        TDScenes.Game.Grid.Grid.MoveableCamera += SetValue;
    }
    bool _isMoveable = false;
    private void SetValue(bool value)
    {
        _isMoveable = value;
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // İlk dokunma pozisyonunu kaydet
                    initialTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    // Dokunma hareketini algıla ve kamerayı hareket ettir
                    Vector2 touchDeltaPosition = touch.position - initialTouchPosition;
                    transform.Translate(-touchDeltaPosition.x * panSpeed * Time.deltaTime, -touchDeltaPosition.y * panSpeed * Time.deltaTime, 0);

                    // Kamera sınırlamaları
                    Vector3 clampedPosition = transform.position;
                    clampedPosition.x = Mathf.Clamp(transform.position.x, -panLimit.x, panLimit.x);
                    clampedPosition.y = Mathf.Clamp(transform.position.y, -panLimit.y, panLimit.y);
                    transform.position = clampedPosition;

                    // Güncellenen pozisyonu başlangıç pozisyonu olarak ayarla
                    initialTouchPosition = touch.position;
                    break;
            }
        }
    }
}