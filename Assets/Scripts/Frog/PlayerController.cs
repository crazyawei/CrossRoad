using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   // public TerrianManager terrianManager;
    private enum Direction
    {
        Up,Right,Left
    }

    private PlayerInput _playerInput;

    private Direction dir;

    private Rigidbody2D rb;

    private Animator anim;
    [Header("�÷�")]

    public int stepPoint;       //���ε÷�

    private int pointResult;    //�ܵ÷�

    [Header("��Ծ")]
    public float jumpDistance;

    private float moveDistance;

    private Vector2 destination;

    private Vector2 touchPosition;

    private bool ButtonHeld;

    private bool isJump;

    private bool canJump;

    private bool isDead;

    [Header("����ָʾ")]

    public SpriteRenderer signRenderer;

    public Sprite upSign;

    public Sprite leftSign;

    public Sprite rightSign;

    private RaycastHit2D[] result=new RaycastHit2D[2];
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _playerInput= GetComponent<PlayerInput>();
    }
    private void Update()
    {
        if(isDead)
        {
            DisableInput();
            return;
        }
        if(canJump)
        {
            TriggerJump();
        }
    }
    private void FixedUpdate()
    {
        if(isJump)
        rb.position = Vector2.Lerp(transform.position, destination, 0.134f);
    }

    #region INPUT ����ص�����
    public void Jump(InputAction.CallbackContext context)
    {
        //ִ����Ծ����Ծ�ľ��룬��¼������������Ծ����Ч
        if(context.performed&&!isJump)
        {
            moveDistance = jumpDistance;
            destination=new Vector2 (transform.position.x,transform.position.y+moveDistance);
            canJump = true;

            AudioManager.instance.SetJumpClip(0);
        }
        if(dir==Direction.Up&&context.performed&&!isJump)
        {
            pointResult += stepPoint;
        }
    }

    public void LongJump(InputAction.CallbackContext context)
    {
        if(context.performed&&!isJump)
        {
            moveDistance = jumpDistance * 2;
            ButtonHeld = true;

            AudioManager.instance.SetJumpClip(1);

            signRenderer.gameObject.SetActive(true);

        }
        if(context.canceled&&ButtonHeld&&!isJump)
        {
            if (dir == Direction.Up)
                pointResult += stepPoint * 2;

            ButtonHeld = false;
            canJump = true;
            signRenderer.gameObject.SetActive(false);
        }
    }

    public void GetTouchPosition(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            // Debug.Log(touchPosition);
            var offset = ((Vector3)touchPosition - transform.position).normalized;

            if (Mathf.Abs(offset.x) <= 0.7f)
            {
                dir = Direction.Up;
                signRenderer.sprite = upSign;
            }
            else if (offset.x < 0)
            {
                dir = Direction.Left;
                if(transform.localScale.x==-1)
                    signRenderer.sprite = rightSign;
                else
                    signRenderer.sprite = leftSign;
            }
            else if (offset.x > 0)
            {
                dir = Direction.Right;
                if (transform.localScale.x == -1)
                    signRenderer.sprite = leftSign;
                else
                    signRenderer.sprite=rightSign;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Water")&&!isJump)
        {
            Physics2D.RaycastNonAlloc(transform.position + Vector3.up * 0.1f, Vector2.zero, result);

            bool inWater=true;

            foreach (var hit in result)
            {
                if(hit.collider==null) continue;
                if(hit.collider.tag=="Wood")
                {
                   
                    transform.parent=hit.collider.transform;    //�����ܵĸ���������Ϊľ�壬�����ܸ���ľ���ƶ�
                    inWater = false;
                }
              
            }
            if(inWater&&!isJump)
            {
                Debug.Log("In Water Game over");
                isDead= true;
            }
            //û��ľ����Ϸ����
        }
        if (other.CompareTag("Border") || other.CompareTag("Car"))
        {
            Debug.Log("Game over");
            isDead = true;
        }
        if(!isJump&&other.CompareTag("Obstacle"))
        {
            Debug.Log("Game over");
            isDead = true;
        }

        if(isDead)
        {
            //�㲥֪ͨ��Ϸ����
            EventHandler.CallGameOverEvent();
        }
    }

   

    #endregion

    private void TriggerJump()
    {
        //todo:����ƶ����򣬲��Ŷ���
        canJump = false;
        switch (dir)
        {
            case Direction.Up:
                anim.SetBool("isSide", false);
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
                transform.localScale=Vector3.one;
                break;
            case Direction.Right:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
                transform.localScale = new Vector3(-1, 1, 1);
                break;
            case Direction.Left:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
                transform.localScale = Vector3.one;
                break;
            default:
                break;
        }
        anim.SetTrigger("Jump");
    }

    #region Animation Event
    public void JumpAnimationEvent()
    {
        AudioManager.instance.PlayJumpFX(); //������Ծ��Ч
        
        isJump = true; //�ı�״̬

        transform.parent= null; 

      
    }

    public void FinishJumpAnimationEvent()
    {
        isJump=false;

        if(dir==Direction.Up&&!isDead)
        {
            //todo:�÷�,������ͼ���
            //terrianManager.CheckPosition();

            EventHandler.CallGetPointEvent(pointResult);

            
        }
    }
    #endregion

    private void DisableInput()
    {
        _playerInput.enabled = false;
    }
}
