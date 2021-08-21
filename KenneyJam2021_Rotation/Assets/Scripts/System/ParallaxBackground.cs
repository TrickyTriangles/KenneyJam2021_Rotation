using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallax_coefficients;
    [SerializeField] private bool horizontal_loop;
    [SerializeField] private bool vertical_loop;

    private Transform camera_transform;
    private Vector3 last_camera_position;
    private Vector2 texture_unit_sizes;

    // Start is called before the first frame update
    void Start()
    {
        camera_transform = Camera.main.transform;
        last_camera_position = camera_transform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        texture_unit_sizes = new Vector2(texture.width / sprite.pixelsPerUnit, texture.height / sprite.pixelsPerUnit);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 delta = camera_transform.position - last_camera_position;
        transform.position += new Vector3(delta.x * parallax_coefficients.x, delta.y * parallax_coefficients.y, 0f);
        last_camera_position = camera_transform.position;

        if (horizontal_loop)
        {
            if (Mathf.Abs(camera_transform.position.x - transform.position.x) >= texture_unit_sizes.x)
            {
                float offset_x = (camera_transform.position.x - transform.position.x) % texture_unit_sizes.x;
                transform.position = new Vector3(camera_transform.position.x + offset_x, transform.position.y);
            }
        }

        if (vertical_loop)
        {
            if (Mathf.Abs(camera_transform.position.y - transform.position.y) >= texture_unit_sizes.y)
            {
                float offset_y = (camera_transform.position.y - transform.position.y) % texture_unit_sizes.y;
                transform.position = new Vector3(camera_transform.position.y + offset_y, transform.position.y);
            }
        }
    }
}
