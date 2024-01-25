using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DuckAnimation : MonoBehaviour
{
    public List<Sprite> nSprites;
    public List<Sprite> eSprites;
    public List<Sprite> sSprites;

    public List<Sprite> sIdleSprites;
    public List<Sprite> eIdleSprites;
    public List<Sprite> nIdleSprites;
    private List<Sprite> selectedSprites;
    private SpriteRenderer spriteRenderer;
    public float frameRate;
    private float changeX;
    public float changeCutoff;
    private float changeY;

    private int facing = 1;// 1 for east, 2 for north, 3 for south

    private Vector3 lastPosition;
    private Vector3 direction;

    //Underwater variables
    private bool playingAnimation;
    public float animationBuffer = 2.0f;
    private float appearStartTime;
    public bool isTimingAppear;
    private Animator anim;
    private Hiding hiding;
    private FollowParent followParent;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectedSprites = eSprites;
        hiding = GetComponent<Hiding>();
        anim = GetComponent<Animator>();
        followParent = GetComponent<FollowParent>();

        //Underwater variables
        anim.enabled = false;
        playingAnimation = false;
        isTimingAppear = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (hiding.GetHiding() && !playingAnimation && hiding.GetUnderWater() && followParent.isFollowing)
        {
            playingAnimation = true;

            anim.enabled = true;

            Debug.Log(anim.enabled);
            anim.SetTrigger("Underwater");
        }

        if (!hiding.GetHiding() && playingAnimation && !isTimingAppear)
        {
            anim.SetTrigger("Appear");
            appearStartTime = Time.time;
            isTimingAppear = true;
        }

        if ((Time.time - appearStartTime) > animationBuffer && playingAnimation && !hiding.GetHiding())
        {
            anim.enabled = false;
            playingAnimation = false;
            isTimingAppear = false;
        }

        if (!spriteRenderer.flipX && changeX < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && changeX > 0f)
        {
            spriteRenderer.flipX = false;
        }

        if (Mathf.Abs(changeX) > changeCutoff && Mathf.Abs(changeY) > changeCutoff)
        {
            SetSprite();
        }
        else
        {
            if (facing == 1)
            {
                selectedSprites = eIdleSprites;
            }
            else if (facing == 2)
            {
                selectedSprites = nIdleSprites;
            }
            else if (facing == 3)
            {
                selectedSprites = sIdleSprites;
            }
        }

        if (transform.position != lastPosition)
        {
            changeX = transform.position.x - lastPosition.x;
            changeY = transform.position.y - lastPosition.y;
            direction = (transform.position - lastPosition).normalized;
        }

        lastPosition = transform.position;

        int frame = (int)((Time.time * frameRate) % 3);

        spriteRenderer.sprite = selectedSprites[frame];
    }

    void FixedUpdate()
    {

    }
    void SetSprite()
    {
        if (Mathf.Abs(changeY) > Mathf.Abs(changeX) && changeY > 0)
        {
            selectedSprites = nSprites;
            facing = 2;

        }
        else if (Mathf.Abs(changeY) > Mathf.Abs(changeX) && changeY < 0)
        {
            selectedSprites = sSprites;
            facing = 3;
        }
        else if (Mathf.Abs(changeY) < Mathf.Abs(changeX))
        {
            selectedSprites = eSprites;
            facing = 1;
        }
    }
}
