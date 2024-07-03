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
    [Header("得分")]

    public int stepPoint;       //单次得分

    private int pointResult;    //总得分

    [Header("跳跃")]
    public float jumpDistance;

    private float moveDistance;

    private Vector2 destination;

    private Vector2 touchPosition;

    private bool ButtonHeld;

    private bool isJump;

    private bool canJump;

    private bool isDead;

    [Header("方向指示")]

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

    #region INPUT 输入回调函数
    public void Jump(InputAction.CallbackContext context)
    {
        //执行跳跃，跳跃的距离，记录分数，播放跳跃的音效
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
                   
                    transform.parent=hit.collider.transform;    //将青蛙的父级物体设为木板，让青蛙跟随木板移动
                    inWater = false;
                }
              
            }
            if(inWater&&!isJump)
            {
                Debug.Log("In Water Game over");
                isDead= true;
            }
            //没有木板游戏结束
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
            //广播通知游戏结束
            EventHandler.CallGameOverEvent();
        }
    }

   

    #endregion

    private void TriggerJump()
    {
        //todo:获得移动方向，播放动画
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
        AudioManager.instance.PlayJumpFX(); //播放跳跃音效
        
        isJump = true; //改变状态

        transform.parent= null; 

      
    }

    public void FinishJumpAnimationEvent()
    {
        isJump=false;

        if(dir==Direction.Up&&!isDead)
        {
            //todo:得分,触发地图检测
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
