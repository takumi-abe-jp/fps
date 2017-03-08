using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 center;
    // Use this for initialization
    void Start () {
        // 画面中央の座標を取得。
        //center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {

        //Screen.lockCursor = true;

        CharacterController controller = GetComponent<CharacterController>();
        //xとyの移動距離をゲットして移動
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        /*
        //マウス
        float sensitivity = 0.1f; // マウス感度
        float mouse_move_x = Input.GetAxis("Mouse X") * sensitivity;
        float mouse_move_y = Input.GetAxis("Mouse Y") * sensitivity;


        gameObject.transform.Rotate(0, 0, mouse_move_x, Space.World);
        gameObject.transform.Rotate(0, 0, mouse_move_y, Space.World);

        */
        //クリックしたところにブロック作成
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localPosition = new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y + 1, hit.collider.gameObject.transform.position.z);
            }

        }

        //クリックしたところのブロック削除
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Destroy(hit.collider.gameObject);
            }
        }

    }
}
