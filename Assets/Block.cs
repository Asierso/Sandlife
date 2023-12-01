using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Block : MonoBehaviour
{
    //Local and global vars of current block

    //Block definition
    public int blockID = 0;
    public int blockStatus = 0;
    public string internalInfo = "";

    //Sound and canvas vars
    public Canvas canvas;
    public GameObject musicControler;

    //Instanciable objects vars
    public Sprite[] AnimationIds;
    public Sprite[] PipeTransportSprite;
    public GameObject Text;
    public ParticleSystem[] Particles;
    public GameObject Light;

    //Local state and handle vars (only temporal not assign in editor)
    public int on = 0;
    public int blocked = 0;
    public GameObject parametersOfThings = null;
    private float timer = 0f;
    private int particleOn = 0;
    private int triggeredBlock = 0;
    private GameObject LightTemporal;
    private float gamepadwait = 0f;
    public int temporalInstanceVariantId = 0;
    GameObject tick = null;
    GameObject energyHandled;
    GameObject pistonHead;
    ParticleSystem particleBlockParticlesLocal;
    ParticleSystem particlesShit;
    GameObject temporalObj;

    //Block type definition
    public enum BlockTypes { Normal,Gravity};
    public enum TransportTypes {Other, Water,Lava,Acid,Sand,Gravel,EnergyDust,SpaceDust}
    public BlockTypes BlockType;
    public TransportTypes parametersTransportType = TransportTypes.Other;

    //Variatons of tank and plots
    public Sprite[] miniwood;
    public Sprite[] tanks;
    public Sprite[] dynamite;

    //IsExecuting
    private bool isExecuting = false;
    public bool isChainBlocked = false;
    public Sprite energizedVariantOn = null;
    public Sprite energizedVariantOff = null;

    //Summon info and definition for all blocks
    #region BlocksDefinition
    void Start()
    {
        Debug.Log("Block putted, IntanceID: " + gameObject.GetInstanceID());
        BlockType = BlockTypes.Normal;
        if(blockID >= 0)
        {
            try
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = canvas.GetComponent<Controler>().BlockSprites[blockID];
            }
            catch
            {
                //Id modified
            }
        }
        switch (blockID)
        {
            //Special triggers
            default: break;
            case -7: break;
            case -6: musicControler.GetComponent<Sounds>().PlaySound(6); if (DateTime.Now.Month == 12) { gameObject.GetComponent<SpriteRenderer>().sprite = miniwood[1]; } else if (DateTime.Now.Month == 7 && DateTime.Now.Day >= 1 && DateTime.Now.Day <= 5) { gameObject.GetComponent<SpriteRenderer>().sprite = miniwood[2]; } else if (DateTime.Now.Month == 8 && DateTime.Now.Day >= 14 && DateTime.Now.Day <= 19) { gameObject.GetComponent<SpriteRenderer>().sprite = miniwood[2]; } else { gameObject.GetComponent<SpriteRenderer>().sprite = miniwood[0]; } on = 1; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x - 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; canvas.GetComponent<Controler>().temporalObjects.Add(gameObject); if (blockStatus == 1) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[45]; } break; //12 = December Winter Skin; 7 and 8 = Mombeltran party
            case -5: gameObject.transform.tag = "piper"; if (temporalInstanceVariantId == 1) { gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x, gameObject.GetComponent<BoxCollider2D>().size.y - 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; } else if (temporalInstanceVariantId == 2) { gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x - 0.3f, gameObject.GetComponent<BoxCollider2D>().size.y - 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.GetComponents<BoxCollider2D>()[1].offset = new Vector2(gameObject.GetComponents<BoxCollider2D>()[1].offset.x - 0.15f, gameObject.GetComponents<BoxCollider2D>()[1].offset.y); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x - 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y - 0.3f); gameObject.GetComponents<BoxCollider2D>()[2].isTrigger = true; gameObject.GetComponents<BoxCollider2D>()[2].offset = new Vector2(gameObject.GetComponents<BoxCollider2D>()[2].offset.x, gameObject.GetComponents<BoxCollider2D>()[2].offset.y - 0.15f);} else { gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x, gameObject.GetComponent<BoxCollider2D>().size.y - 0.35f); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x - 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.GetComponents<BoxCollider2D>()[2].isTrigger = true; } gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case -4:break;
            case -3: gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<Block>().AnimationIds[20];break;
            case -2:break;
            case -1: gameObject.transform.tag = "Energy"; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; blockID = 7; break;
            
            //Normal blocks
            case 0: musicControler.GetComponent<Sounds>().PlaySound(1); break;
            case 1: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 2: musicControler.GetComponent<Sounds>().PlaySound(1);break;
            case 3: musicControler.GetComponent<Sounds>().PlaySound(5); if (blockStatus == 1) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[41]; } break;
            case 4: musicControler.GetComponent<Sounds>().PlaySound(5); if (blockStatus == 1) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[42]; } break;
            case 5: if (blockStatus == 0) { gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().mass = 100; gameObject.GetComponent<Rigidbody2D>().gravityScale = 43f; musicControler.GetComponent<Sounds>().PlaySound(0); gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().angularDrag = 0;  } if (blockStatus == 1) { gameObject.GetComponent<SpriteRenderer>().sprite = canvas.GetComponent<Controler>().BlockSprites[37]; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; } BlockType = BlockTypes.Gravity; break;
            case 6: gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().mass = 100; gameObject.GetComponent<Rigidbody2D>().gravityScale = 43f; musicControler.GetComponent<Sounds>().PlaySound(0); gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().angularDrag = 0; BlockType = BlockTypes.Gravity; break;
            case 7: musicControler.GetComponent<Sounds>().PlaySound(9); gameObject.transform.tag = "Energy"; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 8: musicControler.GetComponent<Sounds>().PlaySound(4); particleBlockParticlesLocal = Instantiate(Particles[0], new Vector3(gameObject.transform.position.x,gameObject.transform.position.y, -100), Particles[0].transform.rotation); canvas.GetComponent<Controler>().particlesObjects.Add(particleBlockParticlesLocal.gameObject); particleBlockParticlesLocal.Stop(); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; break;
            case 9: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; break;
            case 10: musicControler.GetComponent<Sounds>().PlaySound(6); if (blockStatus == 1) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[44]; } break;
            case 11: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 12: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 13: musicControler.GetComponent<Sounds>().PlaySound(7); particlesShit = Instantiate(Particles[1], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.3f, -100), Particles[1].transform.rotation); canvas.GetComponent<Controler>().particlesObjects.Add(particlesShit.gameObject); particlesShit.Play(); break;
            case 14: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 15: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 16: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 17: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 18: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 19: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 20: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 21: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 22: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 23: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 24: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 25: musicControler.GetComponent<Sounds>().PlaySound(8); break;
            case 26: musicControler.GetComponent<Sounds>().PlaySound(1); break;
            case 27: musicControler.GetComponent<Sounds>().PlaySound(5); if (internalInfo == "") { blockStatus = UnityEngine.Random.Range(0, 2); internalInfo = "nonew"; } break;
            case 28: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; timer = blockStatus;  break;
            case 29: musicControler.GetComponent<Sounds>().PlaySound(4); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; if (blockStatus == 0) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[10]; Destroy(temporalObj); } else { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[9]; temporalObj = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation); temporalObj.GetComponent<Block>().blockID = -1; canvas.GetComponent<Controler>().temporalObjects.Add(temporalObj); } break;
            case 30: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 31: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 32: musicControler.GetComponent<Sounds>().PlaySound(4); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 33: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 34: musicControler.GetComponent<Sounds>().PlaySound(5); if (blockStatus == 1) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[43]; } break;
            case 35: musicControler.GetComponent<Sounds>().PlaySound(12); if (blockStatus == 1) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[40]; } break;
            case 36: musicControler.GetComponent<Sounds>().PlaySound(9); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 37: musicControler.GetComponent<Sounds>().PlaySound(8); break;
            case 38: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 39: musicControler.GetComponent<Sounds>().PlaySound(21); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; timer = blockStatus; break;
            case 40: musicControler.GetComponent<Sounds>().PlaySound(21); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; pistonHead = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,100), gameObject.transform.rotation); pistonHead.GetComponent<Block>().blockID = -3; canvas.GetComponent<Controler>().temporalObjects.Add(pistonHead); break;
            case 41: musicControler.GetComponent<Sounds>().PlaySound(9); gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().mass = 100; gameObject.GetComponent<Rigidbody2D>().gravityScale = 43f; gameObject.GetComponent<Rigidbody2D>().angularDrag = 0; BlockType = BlockTypes.Gravity; gameObject.transform.tag = "Energy"; gameObject.AddComponent<BoxCollider2D>(); gameObject.GetComponents<BoxCollider2D>()[1].size = new Vector2(gameObject.GetComponents<BoxCollider2D>()[0].size.x + 0.35f, gameObject.GetComponents<BoxCollider2D>()[0].size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; break;
            case 42: musicControler.GetComponent<Sounds>().PlaySound(5); if (canvas.GetComponent<Controler>().lastVortexValue == 0) { blockStatus = 1; canvas.GetComponent<Controler>().lastVortexValue = 1; internalInfo = canvas.GetComponent<Controler>().vortexA.Count.ToString(); canvas.GetComponent<Controler>().vortexA.Add(gameObject); } else { blockStatus = 0; canvas.GetComponent<Controler>().lastVortexValue = 0; internalInfo = canvas.GetComponent<Controler>().vortexB.Count.ToString(); canvas.GetComponent<Controler>().vortexB.Add(gameObject); } gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 43: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().mass = 100; gameObject.GetComponent<Rigidbody2D>().gravityScale = 43f; gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().angularDrag = 0; BlockType = BlockTypes.Gravity; break;
            case 44: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.GetComponent<SpriteRenderer>().color = new Color32(149,255,163,255); LightTemporal = Instantiate(Light, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -100), gameObject.transform.rotation); LightTemporal.GetComponent<Lights>().SetGradLight(new Color32(149, 255, 163, 100), 80, 100); LightTemporal.transform.localScale = new Vector3(200, 200, 200); canvas.GetComponent<Controler>().temporalObjects.Add(LightTemporal); if (blockStatus == 0) { timer = 10f; blockStatus = UnityEngine.Random.Range(1, 5); } break;
            case 45: musicControler.GetComponent<Sounds>().PlaySound(21); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 46: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 47: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 48: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 49: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 50: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 51: musicControler.GetComponent<Sounds>().PlaySound(6); if (blockStatus == 1) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[44]; } break;
            case 52: musicControler.GetComponent<Sounds>().PlaySound(5); break;
            case 53: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 54: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; break;
            case 55: musicControler.GetComponent<Sounds>().PlaySound(5); internalInfo = "0"; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x, gameObject.GetComponent<BoxCollider2D>().size.y - 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 56: musicControler.GetComponent<Sounds>().PlaySound(5); internalInfo = "0"; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x - 0.3f, gameObject.GetComponent<BoxCollider2D>().size.y - 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.GetComponents<BoxCollider2D>()[1].offset = new Vector2(gameObject.GetComponents<BoxCollider2D>()[1].offset.x - 0.15f, gameObject.GetComponents<BoxCollider2D>()[1].offset.y); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x - 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y - 0.3f); gameObject.GetComponents<BoxCollider2D>()[2].isTrigger = true; gameObject.GetComponents<BoxCollider2D>()[2].offset = new Vector2(gameObject.GetComponents<BoxCollider2D>()[2].offset.x, gameObject.GetComponents<BoxCollider2D>()[2].offset.y - 0.15f); gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 57: musicControler.GetComponent<Sounds>().PlaySound(19); musicControler.GetComponent<Sounds>().PlaySound(0); break;
            case 58: musicControler.GetComponent<Sounds>().PlaySound(4); blockStatus = 0; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x, gameObject.GetComponent<BoxCollider2D>().size.y - 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x - 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y); gameObject.GetComponents<BoxCollider2D>()[2].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 59: musicControler.GetComponent<Sounds>().PlaySound(4); blockStatus = 0; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 60: musicControler.GetComponent<Sounds>().PlaySound(5); internalInfo = "0"; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y - 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 61: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 62: musicControler.GetComponent<Sounds>().PlaySound(5); internalInfo = "0"; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y - 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 63: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 64: musicControler.GetComponent<Sounds>().PlaySound(5); internalInfo = "0"; gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y - 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 65: musicControler.GetComponent<Sounds>().PlaySound(5); if (blockStatus == 0) { timer = 0; } else { timer = blockStatus; } break;
            case 66: musicControler.GetComponent<Sounds>().PlaySound(4); particleBlockParticlesLocal = Instantiate(Particles[2], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 30, -100), Particles[0].transform.rotation); canvas.GetComponent<Controler>().particlesObjects.Add(particleBlockParticlesLocal.gameObject); particleBlockParticlesLocal.Play(); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; break;
            case 67: musicControler.GetComponent<Sounds>().PlaySound(4); particleBlockParticlesLocal = Instantiate(Particles[3], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 30, -100), Particles[0].transform.rotation); canvas.GetComponent<Controler>().particlesObjects.Add(particleBlockParticlesLocal.gameObject); particleBlockParticlesLocal.Play(); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; break;
            case 68: musicControler.GetComponent<Sounds>().PlaySound(4); particleBlockParticlesLocal = Instantiate(Particles[4], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 30, -100), Particles[0].transform.rotation); canvas.GetComponent<Controler>().particlesObjects.Add(particleBlockParticlesLocal.gameObject); particleBlockParticlesLocal.Play(); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; break;
            case 69: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; gameObject.GetComponent<SpriteRenderer>().sprite = tanks[blockStatus]; break;
            case 70: musicControler.GetComponent<Sounds>().PlaySound(19); break;
            case 71: musicControler.GetComponent<Sounds>().PlaySound(20); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x - 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.15f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; break;
            case 72: musicControler.GetComponent<Sounds>().PlaySound(9); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; blockStatus = 1; break;
            case 73: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().mass = 100; gameObject.GetComponent<Rigidbody2D>().gravityScale = 43f; gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().angularDrag = 0; BlockType = BlockTypes.Gravity; break;
            case 74: musicControler.GetComponent<Sounds>().PlaySound(5); gameObject.AddComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x + 0.35f, gameObject.GetComponent<BoxCollider2D>().size.y + 0.35f); gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true; break;
            case 75: musicControler.GetComponent<Sounds>().PlaySound(5); if (blockStatus == 1) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[41]; } break;
            case 76: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 77: gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().mass = 100; gameObject.GetComponent<Rigidbody2D>().gravityScale = 43f; musicControler.GetComponent<Sounds>().PlaySound(0); gameObject.AddComponent<Rigidbody2D>(); gameObject.GetComponent<Rigidbody2D>().angularDrag = 0; BlockType = BlockTypes.Gravity; break;
            case 78: musicControler.GetComponent<Sounds>().PlaySound(5); if (blockStatus == 1) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[51]; } if (blockStatus == 2) { gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[52]; } break;
            case 79: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 80: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 81: musicControler.GetComponent<Sounds>().PlaySound(4); break;
            case 82: musicControler.GetComponent<Sounds>().PlaySound(4); break;
        }
        if (internalInfo == "destroyed" && BlockType == BlockTypes.Gravity)
        {
            gameObject.transform.localScale = new Vector2(25f, 25f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.80f, 0.80f);
        }
    }
    #endregion BlocksDefinition
    //Update method of current block
    #region Update
    void Update()
    {
        if(gamepadwait > 0)
        {
            gamepadwait -= Time.deltaTime;
        }
        if(canvas.GetComponent<UI>().eraseUsing == true) //Unused
        {
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).position == new Vector2(gameObject.transform.position.x, gameObject.transform.position.y))
                {
                    if(canvas.GetComponent<UI>().blockOpened == false && canvas.GetComponent<SaveLoad>().loadOpened == false && canvas.GetComponent<Command>().loadOpened == false && canvas.GetComponent<Settings>().blockSettings == false && canvas.GetComponent<Online>().blockOnline == false && canvas.GetComponent<Computers>().blockComputer == false && canvas.GetComponent<Trophies>().blockTrophies == false && canvas.GetComponent<Message>().blockAlert == false && canvas.GetComponent<Profile>().blockProfile == false && canvas.GetComponent<Piano>().blockOpened == false)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        //Lights
        if (on == 0 && blockID == 9)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[1];
        }

        //Leds
        if (on == 0 && blockID == 28)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[8];
        }
        else if (blockID == 28)
        {
            if(timer >= 5f)
            {
                timer = 0f;
            }
            timer += Time.deltaTime;
            blockStatus = System.Convert.ToInt32(timer);
            if(timer >= 0 && timer < 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[4];
                LightTemporal.GetComponent<Lights>().SetLight(new Color32(224, 63, 63, 100));
            }
            if (timer >= 1 && timer < 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[0];
                LightTemporal.GetComponent<Lights>().SetLight(new Color32(250, 250, 120, 100));
            }
            if (timer >= 2 && timer < 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[5];
                LightTemporal.GetComponent<Lights>().SetLight(new Color32(189, 240, 93, 100));
            }
            if (timer >= 3 && timer < 4)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[6];
                LightTemporal.GetComponent<Lights>().SetLight(new Color32(89, 134, 240, 100));
            }
            if (timer >= 4 && timer < 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[7];
                LightTemporal.GetComponent<Lights>().SetLight(new Color32(149, 70, 163, 100));
            }
        }
        //Particler
        if (particleOn == 0 && blockID == 8)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[3];
            particleBlockParticlesLocal.Stop();
        }
        //Present
        if(blockID == 27)
        {
            if(blockStatus == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = canvas.GetComponent<Controler>().BlockSprites[27];
            }
            if (blockStatus == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[49];
            }
            if (blockStatus == 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[50];
            }
        }

        //Command
        if(blockID == 30 && blockStatus == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[11];
        }
        if (blockID == 30 && blockStatus == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[12];
            if(on >= 1)
            {
                StartCoroutine(ExecuteCommands());
            }
        }
        if (blockID == 30 && blockStatus == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[13];
        }

        //Computer
        if (blockID == 36)
        {
            if(on >= 1)
            {
                
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[15];
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[14];
            }
        }

        //Covager
        if (on == 0 && blockID == 39)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[16];
        }
        else if (blockID == 39)
        {
            if (timer >= 1f)
            {
                timer = 0f;
            }
            timer += Time.deltaTime;
            blockStatus = System.Convert.ToInt32(timer);
            if (timer >= 0 && timer < 0.5f)
            {
                if(internalInfo =="")
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[16];
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[17];
                }
            }
            if (timer >= 0.5f && timer < 1)
            {
                if (internalInfo == "")
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[17];
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[16];
                }
            }
        }
        if(blockID == 39)
        {
            if (internalInfo == "")
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        //Piston
        if(blockID == 40)
        {
            if(on >= 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[19];
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[18];
            }
            switch(blockStatus)
            {
                case 0: gameObject.GetComponent<SpriteRenderer>().flipX = false; gameObject.transform.rotation = canvas.GetComponent<Controler>().Cube.gameObject.transform.rotation;
                    pistonHead.GetComponent<SpriteRenderer>().flipX = false; pistonHead.transform.rotation = canvas.GetComponent<Controler>().Cube.gameObject.transform.rotation; break;
                case 1: gameObject.GetComponent<SpriteRenderer>().flipX = false; gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                    pistonHead.GetComponent<SpriteRenderer>().flipX = false; pistonHead.transform.rotation = Quaternion.Euler(0, 0, -90); break;
                case 2: gameObject.GetComponent<SpriteRenderer>().flipX = true; gameObject.transform.rotation = canvas.GetComponent<Controler>().Cube.gameObject.transform.rotation;
                    pistonHead.GetComponent<SpriteRenderer>().flipX = true; pistonHead.transform.rotation = canvas.GetComponent<Controler>().Cube.gameObject.transform.rotation; break;
                case 3: gameObject.GetComponent<SpriteRenderer>().flipX = true; gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                    pistonHead.GetComponent<SpriteRenderer>().flipX = true; pistonHead.transform.rotation = Quaternion.Euler(0, 0, -90); break;
            }
        }

        //Pulse
        if(blockID == 32)
        {
            if (blockStatus == 1)
            {
                if (timer >= 0.5f)
                {
                    timer = 0f;
                    tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                    tick.GetComponent<Block>().blockID = -1;
                    canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                    blockStatus = 2;
                }
                timer += Time.deltaTime;
            }
            else if(blockStatus == 2)
            {
                if (timer >= 1.5f)
                {
                    blockStatus = 0;
                }
                timer += Time.deltaTime;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[22];
            }
            else
            {
                timer = 0;
                Destroy(tick);
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[21];
            }
            //Debug.Log("gmo in: " + tick.transform.position.x + ":" + tick.transform.position.y);
        }

        //Detector
        if (blockID == 33)
        {
            if (on >= 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[24];
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[23];
            }
        }

        //Vortex
        if(blockID == 42)
        {
            if (blockStatus == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[26];
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[25];
            }
        }

        //Uranium
        if (blockID == 44 && blockStatus == 1)
        {
            timer -= Time.deltaTime;
            if (timer < 6)
            {
                //Change light gradient color to red (Explosion warning)
                if (LightTemporal.GetComponent<Lights>().definedColor.r < 255)
                {
                    LightTemporal.GetComponent<Lights>().ChangeGradColor(new Color32(((byte)(LightTemporal.GetComponent<Lights>().definedColor.r + 1)), ((byte)(LightTemporal.GetComponent<Lights>().definedColor.g-1)), LightTemporal.GetComponent<Lights>().definedColor.b, LightTemporal.GetComponent<Lights>().definedColor.a));
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(LightTemporal.GetComponent<Lights>().definedColor.r, LightTemporal.GetComponent<Lights>().definedColor.g, LightTemporal.GetComponent<Lights>().definedColor.b, 255);
                }
            }
            if (timer <= 0f)
            {
                //Make gigant hitbox and destroy
                gameObject.AddComponent<CircleCollider2D>();
                gameObject.GetComponent<CircleCollider2D>().radius = 5f;
                gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
                gameObject.tag = "Energy";
                GameObject handle = Instantiate(Particles[5].gameObject, gameObject.transform.position, gameObject.transform.rotation);
                particleBlockParticlesLocal = handle.GetComponent<ParticleSystem>();
                canvas.GetComponent<Controler>().particlesObjects.Add(particleBlockParticlesLocal.gameObject);
                particleBlockParticlesLocal.Play();
                gameObject.AddComponent<Rigidbody2D>();
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            }
            if (timer <= -0.1f)
            {
                canvas.GetComponent<Controler>().cubeObjects.Remove(gameObject);
                particleBlockParticlesLocal.Stop();
                canvas.GetComponent<Controler>().particlesObjects.Remove(particleBlockParticlesLocal.gameObject);
                musicControler.GetComponent<Sounds>().PlaySound(16);
                Destroy(gameObject);
                Destroy(LightTemporal);
            }
        }

        //Lantern
        if (on == 0 && blockID == 54)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[27];
        }

        //Pipe
        if (blockID == 55)
        {
            switch (blockStatus)
            {
                case 0:
                    gameObject.GetComponent<SpriteRenderer>().flipX = false; gameObject.transform.rotation = canvas.GetComponent<Controler>().Cube.gameObject.transform.rotation;
                    break;
                case 1:
                    gameObject.GetComponent<SpriteRenderer>().flipX = false; gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
            }
            if (internalInfo == "1")
            {
                if (timer >= 0.5f)
                {
                    timer = 0f;
                    tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                    tick.GetComponent<Block>().temporalInstanceVariantId = 1;
                    tick.GetComponent<Block>().blockID = -5;
                    tick.GetComponent<Block>().parametersOfThings = parametersOfThings;
                    tick.GetComponent<Block>().parametersTransportType = parametersTransportType;
                    canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                    internalInfo = "2";
                }
                timer += Time.deltaTime;
            }
            else if (internalInfo == "2")
            {
                if (timer >= 1.5f)
                {
                    internalInfo = "0";
                }
                timer += Time.deltaTime;
                switch(parametersTransportType)
                {
                    case TransportTypes.Water: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[0];break;
                    case TransportTypes.Lava: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[1];break;
                    case TransportTypes.Acid: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[2];break;
                    case TransportTypes.Sand: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[15];break;
                    case TransportTypes.Gravel: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[16];break;
                    case TransportTypes.EnergyDust: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[17];break;
                    case TransportTypes.SpaceDust: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[18];break;
                    default: gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[30];break;
                }
                
            }
            else
            {
                timer = 0;
                Destroy(tick);
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[29];
            }
            //Debug.Log("gmo in: " + tick.transform.position.x + ":" + tick.transform.position.y);
        }

        //Side pipe
        if (blockID == 56)
        {
            switch (blockStatus)
            {
                case 0:
                    gameObject.GetComponent<SpriteRenderer>().flipX = false; gameObject.transform.rotation = gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 1:
                    gameObject.GetComponent<SpriteRenderer>().flipX = false; gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case 2:
                    gameObject.GetComponent<SpriteRenderer>().flipX = true; gameObject.transform.rotation = Quaternion.Euler(0, 0, -270);
                    break;
                case 3:
                    gameObject.GetComponent<SpriteRenderer>().flipX = true; gameObject.transform.rotation = Quaternion.Euler(0, 0, -360);
                    break;
            }
            if (internalInfo == "1")
            {
                if (timer >= 0.5f)
                {
                    timer = 0f;
                    tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                    tick.GetComponent<Block>().temporalInstanceVariantId = 2;
                    tick.GetComponent<Block>().blockID = -5;
                    tick.GetComponent<Block>().parametersOfThings = parametersOfThings;
                    tick.GetComponent<Block>().parametersTransportType = parametersTransportType;
                    canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                    internalInfo = "2";
                }
                timer += Time.deltaTime;
            }
            else if (internalInfo == "2")
            {
                if (timer >= 1.5f)
                {
                    internalInfo = "0";
                }
                timer += Time.deltaTime;
                switch (parametersTransportType)
                {
                    case TransportTypes.Water: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[3]; break;
                    case TransportTypes.Lava: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[4]; break;
                    case TransportTypes.Acid: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[5]; break;
                    case TransportTypes.Sand: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[19]; break;
                    case TransportTypes.Gravel: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[20]; break;
                    case TransportTypes.EnergyDust: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[21]; break;
                    case TransportTypes.SpaceDust: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[22]; break;
                    default: gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[32]; break;
                }
            }
            else
            {
                timer = 0;
                Destroy(tick);
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[31];
            }
        }

        //Hopper
        if(blockID == 59)
        {
            if(blockStatus == 1)
            {
                timer += Time.deltaTime;
                if(timer >= 2f)
                {
                    timer = 0f;
                    blockStatus = 0;
                    Destroy(tick);
                    on = 0;
                }
            }
        }

        //Gate pipe
        if (blockID == 60)
        {
            switch (blockStatus)
            {
                case 0:
                    gameObject.GetComponent<SpriteRenderer>().flipX = false; gameObject.transform.rotation = canvas.GetComponent<Controler>().Cube.gameObject.transform.rotation;
                    break;
                case 1:
                    gameObject.GetComponent<SpriteRenderer>().flipX = false; gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
            }
            if (internalInfo == "1")
            {
                if (timer >= 0.5f)
                {
                    timer = 0f;
                    tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                    tick.GetComponent<Block>().temporalInstanceVariantId = 1;
                    tick.GetComponent<Block>().blockID = -5;
                    tick.GetComponent<Block>().parametersOfThings = parametersOfThings;
                    tick.GetComponent<Block>().parametersTransportType = parametersTransportType;
                    canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                    internalInfo = "2";
                }
                timer += Time.deltaTime;
            }
            else if (internalInfo == "2")
            {
                if (timer >= 1.5f)
                {
                    internalInfo = "0";
                }
                timer += Time.deltaTime;
                switch (parametersTransportType)
                {
                    case TransportTypes.Water: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[6]; break;
                    case TransportTypes.Lava: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[7]; break;
                    case TransportTypes.Acid: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[8]; break;
                    case TransportTypes.Sand: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[23]; break;
                    case TransportTypes.Gravel: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[24]; break;
                    case TransportTypes.EnergyDust: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[25]; break;
                    case TransportTypes.SpaceDust: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[26]; break;
                    default: gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[34]; break;
                }
            }
            else
            {
                timer = 0;
                Destroy(tick);
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[33];
            }
            if(blocked >= 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[35];
            }
            //Debug.Log("gmo in: " + tick.transform.position.x + ":" + tick.transform.position.y);
        }

        //Detector pipe
        if (blockID == 62)
        {
            switch (blockStatus)
            {
                case 0:
                    gameObject.GetComponent<SpriteRenderer>().flipX = false; gameObject.transform.rotation = canvas.GetComponent<Controler>().Cube.gameObject.transform.rotation;
                    break;
                case 1:
                    gameObject.GetComponent<SpriteRenderer>().flipX = false; gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
            }
            if (internalInfo == "1")
            {
                if (timer >= 0.5f)
                {
                    timer = 0f;
                    tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                    tick.GetComponent<Block>().temporalInstanceVariantId = 1;
                    tick.GetComponent<Block>().blockID = -5;
                    tick.GetComponent<Block>().parametersOfThings = parametersOfThings;
                    tick.GetComponent<Block>().parametersTransportType = parametersTransportType;
                    canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                    energyHandled = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                    energyHandled.GetComponent<Block>().blockID = -1;
                    canvas.GetComponent<Controler>().temporalObjects.Add(energyHandled);

                    internalInfo = "2";
                }
                timer += Time.deltaTime;
            }
            else if (internalInfo == "2")
            {
                if (timer >= 1.5f)
                {
                    internalInfo = "0";
                }
                timer += Time.deltaTime;
                switch (parametersTransportType)
                {
                    case TransportTypes.Water: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[9]; break;
                    case TransportTypes.Lava: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[10]; break;
                    case TransportTypes.Acid: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[11]; break;
                    case TransportTypes.Sand: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[27]; break;
                    case TransportTypes.Gravel: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[28]; break;
                    case TransportTypes.EnergyDust: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[29]; break;
                    case TransportTypes.SpaceDust: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[30]; break;
                    default: gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[37]; break;
                }
            }
            else
            {
                timer = 0;
                Destroy(tick);
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[36];
            }
            //Debug.Log("gmo in: " + tick.transform.position.x + ":" + tick.transform.position.y);
        }

        //Corner pipe
        if (blockID == 64)
        {
            if (internalInfo == "1")
            {
                if (timer >= 0.5f)
                {
                    timer = 0f;
                    tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                    tick.GetComponent<Block>().temporalInstanceVariantId = 0;
                    tick.GetComponent<Block>().blockID = -5;
                    tick.GetComponent<Block>().parametersOfThings = parametersOfThings;
                    tick.GetComponent<Block>().parametersTransportType = parametersTransportType;
                    canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                    internalInfo = "2";
                }
                timer += Time.deltaTime;
            }
            else if (internalInfo == "2")
            {
                if (timer >= 1.5f)
                {
                    internalInfo = "0";
                }
                timer += Time.deltaTime;
                switch (parametersTransportType)
                {
                    case TransportTypes.Water: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[12]; break;
                    case TransportTypes.Lava: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[13]; break;
                    case TransportTypes.Acid: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[14]; break;
                    case TransportTypes.Sand: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[31]; break;
                    case TransportTypes.Gravel: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[32]; break;
                    case TransportTypes.EnergyDust: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[33]; break;
                    case TransportTypes.SpaceDust: gameObject.GetComponent<SpriteRenderer>().sprite = PipeTransportSprite[34]; break;
                    default: gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[39]; break;
                }
            }
            else
            {
                timer = 0;
                Destroy(tick);
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[38];
            }
            //Debug.Log("gmo in: " + tick.transform.position.x + ":" + tick.transform.position.y);
        }

        //Flowerpot
        if(blockID == 65)
        {
            if (blockStatus < 360)
            {
                timer += Time.deltaTime;
                blockStatus = (int)timer;
                if ((int)timer == 360)
                {
                    if (blocked == 0)
                    {
                        blocked = 1;
                        if (internalInfo == "")
                        {
                            internalInfo = (gameObject.transform.position.y + 25).ToString();
                        }
                        GameObject handle = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, float.Parse(internalInfo), -120), gameObject.transform.rotation);
                        handle.GetComponent<Block>().blockID = -6;                     
                        canvas.GetComponent<Controler>().cubeObjects.Add(handle);
                        internalInfo = (float.Parse(internalInfo) + 40).ToString();
                        PlayerPrefs.SetInt("flowerpotCount", PlayerPrefs.GetInt("flowerpotCount") + 1);
                    }
                }
                else
                {
                    if(timer >= 361)
                    {
                        timer = 361;
                    }
                    blocked = 0;
                }
            }
        }

        //Pipe id -5
        if(blockID == -5)
        {
            timer += Time.deltaTime;
            if(timer >= 5) //Fix bug
            {
                Destroy(gameObject);
            }
        }

        if(blockID == -6 && on == 0)
        {
            canvas.GetComponent<Controler>().cubeObjects.Remove(gameObject);
            Destroy(gameObject);
        }

        //Inversor
        if(blockID == 72 && blockStatus == 1)
        {
            if(on == 0)
            {
                tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                tick.GetComponent<Block>().blockID = -1;
                canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[47];
            }
            if(on >= 4)
            {
                Destroy(tick);
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[46];
            }
            blockStatus = 0;
        }

        //TNT
        if (blockID == 73)
        {
            if (blockStatus == 0)
            {
                timer = 4f;
            }
            if (blockStatus == 1)
            {
                timer -= Time.deltaTime;
                gameObject.GetComponent<SpriteRenderer>().sprite = dynamite[5-Convert.ToInt32(timer)];
                if (timer <= 0f)
                {
                    //Make gigant hitbox and destroy
                    PlayerPrefs.SetInt("tntCount", PlayerPrefs.GetInt("tntCount") + 1);
                    gameObject.AddComponent<CircleCollider2D>();
                    gameObject.GetComponent<CircleCollider2D>().radius = 3f;
                    gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
                    gameObject.tag = "Energy";
                    GameObject handle = Instantiate(Particles[5].gameObject, gameObject.transform.position, gameObject.transform.rotation);
                    particleBlockParticlesLocal = handle.GetComponent<ParticleSystem>();
                    canvas.GetComponent<Controler>().particlesObjects.Add(particleBlockParticlesLocal.gameObject);
                    particleBlockParticlesLocal.Play();
                    gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                }
                if (timer <= -0.1f && blockStatus == 1)
                {
                    canvas.GetComponent<Controler>().cubeObjects.Remove(gameObject);
                    particleBlockParticlesLocal.Stop();
                    canvas.GetComponent<Controler>().particlesObjects.Remove(particleBlockParticlesLocal.gameObject);
                    musicControler.GetComponent<Sounds>().PlaySound(16);
                    Destroy(gameObject);
                }
            }
        }
    }
    #endregion Update
    //Mouse detection and events
    #region MouseEvents
    private void OnMouseDown()
    {
        if (canvas.GetComponent<UI>().eraseUsing == true)
        {
            if (canvas.GetComponent<UI>().blockOpened == false && canvas.GetComponent<SaveLoad>().loadOpened == false && canvas.GetComponent<Command>().loadOpened == false && canvas.GetComponent<Settings>().blockSettings == false && canvas.GetComponent<Online>().blockOnline == false && canvas.GetComponent<Computers>().blockComputer == false && canvas.GetComponent<Trophies>().blockTrophies == false && canvas.GetComponent<Message>().blockAlert == false && canvas.GetComponent<Profile>().blockProfile == false && canvas.GetComponent<Piano>().blockOpened == false)
            {
                canvas.GetComponent<Controler>().cubeObjects.Remove(gameObject);
                Destroy(gameObject);
                musicControler.GetComponent<Sounds>().PlaySound(2);
                SpecialDestroy();
                PlayerPrefs.SetInt("eraseCount", PlayerPrefs.GetInt("eraseCount") + 1);
                GameObject handle = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                handle.GetComponent<DestroyParticles>().InvokeParticles(gameObject.transform.position, blockID);
            }
        }
        else
        {
            TouchTrigger();
        }
    }

    private void OnMouseEnter()
    {
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows) //Windows
        {
            if (Input.GetMouseButton(0))
            {
                if (canvas.GetComponent<UI>().eraseUsing == true)
                {
                    if (canvas.GetComponent<UI>().blockOpened == false && canvas.GetComponent<SaveLoad>().loadOpened == false && canvas.GetComponent<Command>().loadOpened == false && canvas.GetComponent<Settings>().blockSettings == false && canvas.GetComponent<Online>().blockOnline == false && canvas.GetComponent<Computers>().blockComputer == false && canvas.GetComponent<Trophies>().blockTrophies == false && canvas.GetComponent<Message>().blockAlert == false && canvas.GetComponent<Profile>().blockProfile == false && canvas.GetComponent<Piano>().blockOpened == false)
                    {
                        canvas.GetComponent<Controler>().cubeObjects.Remove(gameObject);
                        Destroy(gameObject);
                        musicControler.GetComponent<Sounds>().PlaySound(2);
                        SpecialDestroy();
                        PlayerPrefs.SetInt("eraseCount", PlayerPrefs.GetInt("eraseCount") + 1);
                        GameObject handle = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                        handle.GetComponent<DestroyParticles>().InvokeParticles(gameObject.transform.position, blockID);
                    }
                }
                else //Touch triggered
                {
                    TouchTrigger();
                }
            }
            if (Input.GetMouseButton(1))
            {
                canvas.GetComponent<Controler>().cubeObjects.Remove(gameObject);
                Destroy(gameObject);
                musicControler.GetComponent<Sounds>().PlaySound(2);
                SpecialDestroy();
                PlayerPrefs.SetInt("eraseCount", PlayerPrefs.GetInt("eraseCount") + 1);
                GameObject handle = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                handle.GetComponent<DestroyParticles>().InvokeParticles(gameObject.transform.position, blockID);
            }
            if (Input.GetMouseButton(2))
            {
                canvas.GetComponent<Controler>().SelectionSetID(blockID);
                canvas.GetComponent<Controler>().ChangeSelectionIcon(blockID);
            }
        }
        else
        {
            if (Input.touchCount == 1) //Detects mouse and touch entry
            {
                Input.GetTouch(0);
                if (canvas.GetComponent<UI>().eraseUsing == true)
                {
                    if (canvas.GetComponent<UI>().blockOpened == false && canvas.GetComponent<SaveLoad>().loadOpened == false && canvas.GetComponent<Command>().loadOpened == false && canvas.GetComponent<Settings>().blockSettings == false && canvas.GetComponent<Online>().blockOnline == false && canvas.GetComponent<Computers>().blockComputer == false && canvas.GetComponent<Trophies>().blockTrophies == false && canvas.GetComponent<Message>().blockAlert == false && canvas.GetComponent<Profile>().blockProfile == false && canvas.GetComponent<Piano>().blockOpened == false)
                    {
                        canvas.GetComponent<Controler>().cubeObjects.Remove(gameObject);
                        Destroy(gameObject);
                        musicControler.GetComponent<Sounds>().PlaySound(2);
                        SpecialDestroy();
                        PlayerPrefs.SetInt("eraseCount", PlayerPrefs.GetInt("eraseCount") + 1);
                        GameObject handle = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                        handle.GetComponent<DestroyParticles>().InvokeParticles(gameObject.transform.position, blockID);
                    }
                }
                else //Touch triggered
                {
                    TouchTrigger();
                }
            }
        }

    }
    #endregion MouseEvents
    #region TouchTrigger

    private void TouchTrigger()
    {
        if (canvas.GetComponent<UI>().eraseUsing == false && canvas.GetComponent<SaveLoad>().loadOpened == false && canvas.GetComponent<UI>().blockView == false && canvas.GetComponent<UI>().blockOpened == false && canvas.GetComponent<Settings>().blockSettings == false && canvas.GetComponent<Online>().blockOnline == false && canvas.GetComponent<Command>().loadOpened == false && canvas.GetComponent<Computers>().blockComputer == false && canvas.GetComponent<Trophies>().blockTrophies == false && canvas.GetComponent<Message>().blockAlert == false && canvas.GetComponent<Profile>().blockProfile == false && canvas.GetComponent<Piano>().blockOpened == false)
        {
            //Present
            if(blockID == 27 && blockStatus < 2)
            {
                blockStatus++;
            }
            else if(blockID == 27)
            {
                blockStatus = 0;
            }

            //Switch
            if (blockID == 29 && blockStatus == 1)
            {
                PlayerPrefs.SetInt("switchCount", PlayerPrefs.GetInt("switchCount") + 1);
                blockStatus = 0;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[10];
                Destroy(temporalObj);
                musicControler.GetComponent<Sounds>().PlaySound(11);
            }
            else if (blockID == 29 && blockStatus == 0)
            {
                blockStatus = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[9];
                temporalObj = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                temporalObj.GetComponent<Block>().blockID = -1;
                canvas.GetComponent<Controler>().temporalObjects.Add(temporalObj);
                musicControler.GetComponent<Sounds>().PlaySound(11);
                PlayerPrefs.SetInt("switchCount", PlayerPrefs.GetInt("switchCount") + 1);
            }

            //Command
            if (blockID == 30)
            {
                canvas.GetComponent<Command>().loadOpened = false;
                canvas.GetComponent<Command>().LoadUIClick(gameObject);
            }

            //Computer
            if (blockID == 36)
            {
                canvas.GetComponent<Computers>().blockComputer = false;
                canvas.GetComponent<Computers>().LoadUIClick(gameObject);
            }

            //Conveger
            if(blockID == 39)
            {
                musicControler.GetComponent<Sounds>().PlaySound(3);
                if (internalInfo == "")
                {
                    internalInfo = "reverse";
                }
                else
                {
                    internalInfo = "";
                }
            }

            //Piston
            if (blockID == 40)
            {
                if(on == 0)
                {
                    musicControler.GetComponent<Sounds>().PlaySound(3);
                    blockStatus++;
                    if (blockStatus == 4)
                    {
                        blockStatus = 0;
                    }
                }
            }

            //Pipe
            if (blockID == 55)
            {
                musicControler.GetComponent<Sounds>().PlaySound(3);
                blockStatus++;
                if (blockStatus == 2)
                {
                    blockStatus = 0;
                }
            }

            //Size pipe
            if (blockID == 56)
            {
                musicControler.GetComponent<Sounds>().PlaySound(3);
                blockStatus++;
                if (blockStatus == 4)
                {
                    blockStatus = 0;
                }
            }

            //Gate pipe
            if (blockID == 60)
            {
                musicControler.GetComponent<Sounds>().PlaySound(3);
                blockStatus++;
                if (blockStatus == 2)
                {
                    blockStatus = 0;
                }
            }

            //Detector pipe
            if (blockID == 62)
            {
                musicControler.GetComponent<Sounds>().PlaySound(3);
                blockStatus++;
                if (blockStatus == 2)
                {
                    blockStatus = 0;
                }
            }

            //Piano
            if (blockID == 74)
            {
                canvas.GetComponent<Piano>().blockOpened = false;
                canvas.GetComponent<Piano>().BlockUIClick(gameObject);
            }
        }
    }
    #endregion TouchTrigger
    //Destroy block events
    #region Destroy
    private void SpecialDestroy()
    {
        if(blockID == 13)
        {
            particlesShit.Stop();
            canvas.GetComponent<Controler>().particlesObjects.Remove(particlesShit.gameObject);
        }
        if (blockID == 8)
        {
            particleBlockParticlesLocal.Stop();
            canvas.GetComponent<Controler>().particlesObjects.Remove(particleBlockParticlesLocal.gameObject);
        }
        if (blockID == 25)
        {
            Destroy(tick);
        }
        if (blockID == 29)
        {
            Destroy(temporalObj);
        }
        if (blockID == 30)
        {
            Destroy(tick);
        }
        if (blockID == 32)
        {
            Destroy(tick);
        }
        if (blockID == 33)
        {
            Destroy(tick);
        }
        if (blockID == 40)
        {
            Destroy(pistonHead);
            canvas.GetComponent<Controler>().temporalObjects.Remove(pistonHead);
        }
        if(blockID == 42)
        {
            canvas.GetComponent<Controler>().lastVortexValue = 0;
            if (blockStatus == 1)
            {
                try
                {
                    GameObject handle = canvas.GetComponent<Controler>().vortexB[internalInfo.ToInt()];
                    Destroy(canvas.GetComponent<Controler>().vortexB[internalInfo.ToInt()]);
                    canvas.GetComponent<Controler>().vortexA[canvas.GetComponent<Controler>().vortexA.IndexOf(gameObject)] = null;
                    canvas.GetComponent<Controler>().vortexB[canvas.GetComponent<Controler>().vortexB.IndexOf(handle)] = null;
                }
                catch
                {
                    canvas.GetComponent<Controler>().vortexA[canvas.GetComponent<Controler>().vortexA.IndexOf(gameObject)] = null;
                }
            }
            else
            {
                try
                {
                    GameObject handle = canvas.GetComponent<Controler>().vortexA[internalInfo.ToInt()];
                    Destroy(canvas.GetComponent<Controler>().vortexA[internalInfo.ToInt()]);
                    canvas.GetComponent<Controler>().vortexB[canvas.GetComponent<Controler>().vortexB.IndexOf(gameObject)] = null;
                    canvas.GetComponent<Controler>().vortexA[canvas.GetComponent<Controler>().vortexA.IndexOf(handle)] = null;
                }
                catch
                {
                    canvas.GetComponent<Controler>().vortexB[canvas.GetComponent<Controler>().vortexB.IndexOf(gameObject)] = null;
                }

            }
        }

        if(blockID == 44)
        {
            canvas.GetComponent<Controler>().temporalObjects.Remove(LightTemporal);
            Destroy(LightTemporal);
        }
        if (blockID == 55)
        {
            Destroy(tick);
        }
        if (blockID == 56)
        {
            Destroy(tick);
        }
        if (blockID == 58)
        {
            Destroy(tick);
        }
        if (blockID == 59)
        {
            Destroy(tick);
        }
        if (blockID == 60)
        {
            Destroy(tick);
        }
        if (blockID == 62)
        {
            Destroy(tick);
            Destroy(energyHandled);
        }
        if (blockID == 66)
        {
            particleBlockParticlesLocal.Stop();
            canvas.GetComponent<Controler>().particlesObjects.Remove(particleBlockParticlesLocal.gameObject);
        }
        if (blockID == 67)
        {
            particleBlockParticlesLocal.Stop();
            canvas.GetComponent<Controler>().particlesObjects.Remove(particleBlockParticlesLocal.gameObject);
        }
        if (blockID == 68)
        {
            particleBlockParticlesLocal.Stop();
            canvas.GetComponent<Controler>().particlesObjects.Remove(particleBlockParticlesLocal.gameObject);
        }
        if (blockID == 72)
        {
            Destroy(tick);
        }
        try { Destroy(LightTemporal); } catch { }
    }
    #endregion Destroy
    //Trigger (Enter,Stay,Exit and ParticleStay) events
    #region TriggerEvents

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        try
        {
            //Pointer
            if (collision.transform.tag == "Pointer" && gamepadwait <= 0)
            {
                var gamepad = Gamepad.current;
                if (gamepad.buttonSouth.isPressed)
                {
                    OnMouseDown();
                    gamepadwait = 0.1f;
                }
                if (gamepad.leftTrigger.isPressed)
                {
                    if (canvas.GetComponent<UI>().blockOpened == false && canvas.GetComponent<SaveLoad>().loadOpened == false && canvas.GetComponent<Command>().loadOpened == false && canvas.GetComponent<Settings>().blockSettings == false && canvas.GetComponent<Online>().blockOnline == false && canvas.GetComponent<Computers>().blockComputer == false && canvas.GetComponent<Trophies>().blockTrophies == false && canvas.GetComponent<Message>().blockAlert == false && canvas.GetComponent<Profile>().blockProfile == false && canvas.GetComponent<Piano>().blockOpened == false)
                    {
                        canvas.GetComponent<Controler>().cubeObjects.Remove(gameObject);
                        Destroy(gameObject);
                        musicControler.GetComponent<Sounds>().PlaySound(2);
                        SpecialDestroy();
                        PlayerPrefs.SetInt("eraseCount", PlayerPrefs.GetInt("eraseCount") + 1);
                        GameObject handle = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                        handle.GetComponent<DestroyParticles>().InvokeParticles(gameObject.transform.position, blockID);
                        gamepadwait = 0.1f;
                    }
                }
            }
        }
        catch
        {

        }*/

        //Modified block
        if(collision.transform.tag == "Energy" && energizedVariantOn != null)
        {
            on++;
            if(on ==1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = energizedVariantOn;
            }
        }

        //Light
        if (collision.transform.tag == "Energy" && blockID == 9)
        {
            on++;
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[0];
            musicControler.GetComponent<Sounds>().PlaySound(10);
            if (on == 1)
            {
                LightTemporal = Instantiate(Light, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -100), gameObject.transform.rotation);
                LightTemporal.GetComponent<Lights>().SetGradLight(new Color32(250, 250, 120, 100), 80, 100);
                canvas.GetComponent<Controler>().temporalObjects.Add(LightTemporal);
            }
        }

        //Leds
        if (collision.transform.tag == "Energy" && blockID == 28)
        {
            on++;
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[0];
            musicControler.GetComponent<Sounds>().PlaySound(10);
            if (on == 1)
            {
                LightTemporal = Instantiate(Light, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -100), gameObject.transform.rotation);
                canvas.GetComponent<Controler>().temporalObjects.Add(LightTemporal);
            }
        }

        //Particler
        if (collision.transform.tag == "Energy" && blockID == 8)
        {
            particleOn++;
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[2];
            particleBlockParticlesLocal.Play();
        }

        //Switch
        try
        {
            if (collision.GetComponent<Block>().blockID == 29 && blockID == 29 && collision.transform.position.x == gameObject.transform.position.x && collision.transform.position.y == gameObject.transform.position.y)
            {
                if (collision.transform.position.z > gameObject.transform.position.z)
                {
                    Destroy(gameObject);
                }
                else if (collision.transform.position.z < gameObject.transform.position.z)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
        catch
        {

        }

        //Command
        if (collision.transform.tag == "Energy" && blockID == 30)
        {

            if (collision.gameObject.GetComponent<Block>().internalInfo != "spawned" && collision.gameObject.GetComponent<Block>().internalInfo != "spawned2")
            {
                on++;
                if (blockStatus != 1) StartCoroutine(ExecuteCommands());
            }
            else if(collision.gameObject.GetComponent<Block>().internalInfo == "spawned2" && blockStatus == 2)
            {
                if(isChainBlocked == false)
                {
                    Destroy(collision.gameObject);
                    StartCoroutine(ExecuteCommands());
                }
            }

        }
        else if (collision.transform.tag == "Energy" && blockID == 30 && collision.gameObject.GetComponent<Block>().blockStatus == 2 && collision.gameObject.GetComponent<Block>().blockID == 30)
        {
            if (collision.gameObject.GetComponent<Block>().internalInfo != "spawned")
            {
                on++;
                StartCoroutine(ExecuteCommands());
            }
        }

        //Computer
        if (collision.transform.tag == "Energy" && blockID == 36)
        {
            if (on <= 0)
            {
                musicControler.GetComponent<Sounds>().PlaySound(13);
            }
            on++;
        }

        //Conveyer
        if (collision.transform.tag == "Energy" && blockID == 39)
        {
            on++;
        }
        try
        {
            if (collision.gameObject.GetComponent<Block>().BlockType == BlockTypes.Gravity)
            {
                if (blockID == 39 && on >= 1)
                {
                    if (internalInfo == "")
                    {
                        StartCoroutine(speed(collision.gameObject, -700000));
                    }
                    else
                    {
                        StartCoroutine(speed(collision.gameObject, 700000));
                    }
                }
            }
        }
        catch
        {

        }

        //Piston
        if (collision.transform.tag == "Energy" && blockID == 40)
        {
            on++;
            float variety = 25f;
            if (on == 1)
            {
                musicControler.GetComponent<Sounds>().PlaySound(17);
                PlayerPrefs.SetInt("pistonCount", PlayerPrefs.GetInt("pistonCount") + 1);
                IEnumerator animate()
                {
                    variety = 12.5f;
                    switch (blockStatus)
                    {
                        case 0:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x + variety, gameObject.transform.position.y, 100); break;
                        case 1:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - variety, 100); break;
                        case 2:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x - variety, gameObject.transform.position.y, 100); break;
                        case 3:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + variety, 100); break;
                    }
                    yield return new WaitForSeconds(0.10f);
                    variety = 25.25f;
                    switch (blockStatus)
                    {
                        case 0:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x + variety, gameObject.transform.position.y, 100); break;
                        case 1:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - variety, 100); break;
                        case 2:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x - variety, gameObject.transform.position.y, 100); break;
                        case 3:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + variety, 100); break;
                    }
                }
                StartCoroutine(animate());
            }
        }

        //Pulse
        if (collision.transform.tag == "Energy" && blockID == 32)
        {
            on++;
            if (on == 1)
            {
                blockStatus = 1;
            }
        }

        //Detector
        if (blockID == 33)
        {
            try
            {
                if (collision.gameObject.GetComponent<Block>().BlockType == BlockTypes.Gravity)
                {
                    on++;
                    if (on == 1)
                    {
                        tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                        tick.GetComponent<Block>().blockID = -1;
                        canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                        PlayerPrefs.SetInt("detectorCount", PlayerPrefs.GetInt("detectorCount") + 1);
                    }
                }
            }
            catch
            {

            }

        }

        //Vortex
        if (blockID == 42)
        {
            if (blockStatus == 1)
            {
                try
                {
                    if (collision.gameObject.GetComponent<Block>().BlockType == BlockTypes.Gravity || collision.GetComponent<Rigidbody2D>().gravityScale > 0)
                    {
                        PlayerPrefs.SetInt("portalCount", PlayerPrefs.GetInt("portalCount") + 1);
                        collision.gameObject.transform.position = canvas.GetComponent<Controler>().vortexB[internalInfo.ToInt()].GetComponent<Transform>().position;
                    }
                }
                catch
                {

                }
            }
        }
        //Uranium
        if (blockID == 44)
        {
            try
            {
                if (collision.gameObject.GetComponent<Block>())
                {
                    if (blockStatus == 1 && timer <= 0)
                    {
                        if (collision.gameObject.GetComponent<Block>().blockID != 73 && collision.gameObject.GetComponent<Block>().blockID != 12)
                        {
                            GameObject handle = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                            handle.GetComponent<DestroyParticles>().InvokeParticles(collision.gameObject.transform.position, collision.gameObject.GetComponent<Block>().blockID);
                            canvas.GetComponent<Controler>().cubeObjects.Remove(collision.gameObject);
                            collision.gameObject.GetComponent<Block>().SpecialDestroy();
                            Destroy(collision.gameObject);
                        }
                    }
                    if (blockStatus == 0 && collision.transform.tag == "Energy")
                    {
                        blockStatus = 1;
                    }
                }
            }
            catch
            {

            }

        }


        //Destroyer
        if (blockID == 45)
        {
            try
            {
                if (collision.GetComponent<Block>().BlockType == BlockTypes.Gravity && collision.GetComponent<Block>().internalInfo != "destroyed")
                {
                    GameObject clone = collision.gameObject;
                    GameObject[] handle = { Instantiate(clone, gameObject.transform.position, gameObject.transform.rotation), Instantiate(clone, gameObject.transform.position, gameObject.transform.rotation), Instantiate(clone, gameObject.transform.position, gameObject.transform.rotation), Instantiate(clone, gameObject.transform.position, gameObject.transform.rotation), Instantiate(clone, gameObject.transform.position, gameObject.transform.rotation) };
                    handle.ToList().ForEach((Obj) =>
                    {
                        Obj.GetComponent<Block>().internalInfo = "destroyed";
                        Obj.transform.localScale = new Vector2(25f, 25f);
                        Obj.GetComponent<BoxCollider2D>().size = new Vector2(0.80f, 0.80f);
                        canvas.GetComponent<Controler>().cubeObjects.Add(Obj);
                    });
                    canvas.GetComponent<Controler>().cubeObjects.Remove(collision.gameObject);

                    Destroy(collision.gameObject);
                    PlayerPrefs.SetInt("divideCount", PlayerPrefs.GetInt("divideCount") + 1);
                }
            }
            catch
            {

            }
        }

        //Void
        if (blockID == 49)
        {
            try
            {
                if (collision.gameObject.GetComponent<Block>().BlockType == BlockTypes.Gravity)
                {
                    GameObject handle = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                    handle.GetComponent<DestroyParticles>().InvokeParticles(collision.gameObject.transform.position, collision.gameObject.GetComponent<Block>().blockID);
                    Destroy(collision.gameObject);
                    PlayerPrefs.SetInt("voidCount", PlayerPrefs.GetInt("voidCount") + 1);
                }
            }
            catch
            {

            }
        }

        //Lantern
        if (collision.transform.tag == "Energy" && blockID == 54)
        {
            on++;
            gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[28];
            musicControler.GetComponent<Sounds>().PlaySound(10);
            if (on == 1)
            {
                LightTemporal = Instantiate(Light, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -100), gameObject.transform.rotation);
                LightTemporal.GetComponent<Lights>().SetGradLight(new Color32(250, 250, 250, 100), 80, 100);
                canvas.GetComponent<Controler>().temporalObjects.Add(LightTemporal);
            }
        }

        //Pipe
        if (collision.transform.tag == "piper" && blockID == 55)
        {
            on++;
            Debug.Log("Transmit");
            parametersOfThings = collision.gameObject.GetComponent<Block>().parametersOfThings;
            parametersTransportType = collision.gameObject.GetComponent<Block>().parametersTransportType;
            if (on == 1)
            {
                internalInfo = "1";
                PlayerPrefs.SetInt("pipesCount", PlayerPrefs.GetInt("pipesCount") + 1);
            }
        }

        //Side pipe
        if (collision.transform.tag == "piper" && blockID == 56)
        {
            on++;
            Debug.Log("Transmit");
            parametersOfThings = collision.gameObject.GetComponent<Block>().parametersOfThings;
            parametersTransportType = collision.gameObject.GetComponent<Block>().parametersTransportType;
            if (on == 1)
            {
                internalInfo = "1";
                PlayerPrefs.SetInt("pipesCount", PlayerPrefs.GetInt("pipesCount") + 1);
            }
        }

        //Hopper
        if (blockID == 59)
        {
            try
            {
                if (collision.gameObject.GetComponent<Block>().BlockType == BlockTypes.Gravity)
                {
                    on++;
                }
                if (on == 1)
                {

                    if (collision.gameObject.GetComponent<Block>().BlockType == BlockTypes.Gravity)
                    {
                        GameObject handle = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                        handle.GetComponent<DestroyParticles>().InvokeParticles(collision.gameObject.transform.position, collision.gameObject.GetComponent<Block>().blockID);
                        collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                        parametersOfThings = collision.gameObject;

                        switch(collision.gameObject.GetComponent<Block>().blockID)
                        {
                            case 5:parametersTransportType = TransportTypes.Sand;break;
                            case 6:parametersTransportType = TransportTypes.Gravel;break;
                            case 41:parametersTransportType = TransportTypes.EnergyDust;break;
                            case 77:parametersTransportType = TransportTypes.SpaceDust;break;
                            default: parametersTransportType = TransportTypes.Other;break;
                        }


                        collision.gameObject.transform.position = new Vector2(-1000, -1000);
                        blockStatus = 1;
                        tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                        tick.GetComponent<Block>().blockID = -5;
                        tick.GetComponent<Block>().parametersOfThings = parametersOfThings;
                        tick.GetComponent<Block>().parametersTransportType = parametersTransportType;
                        musicControler.GetComponent<Sounds>().PlaySound(12);
                        Debug.Log("Block detected");
                    }
                }
            }
            catch
            {

            }

        }

        //PiperInfo -5
        if (blockID == -5)
        {
            try
            {
                if (collision.gameObject.GetComponent<Block>().blockID == -5 || collision.gameObject.GetComponent<Block>().blockID == 59)
                {
                    parametersOfThings = collision.gameObject.GetComponent<Block>().parametersOfThings;
                    parametersTransportType = collision.gameObject.GetComponent<Block>().parametersTransportType;
                    Debug.Log("Pass " + parametersOfThings.name);
                    internalInfo = "1";
                }
            }
            catch
            {

            }
        }
        //Sewer
        if (blockID == 58)
        {
            try
            {
                if (collision.gameObject.GetComponent<Block>().blockID == -5)
                {
                    on++;
                    if (on == 1)
                    {
                        musicControler.GetComponent<Sounds>().PlaySound(12);
                        parametersOfThings = collision.gameObject.GetComponent<Block>().parametersOfThings;
                        GameObject handle = Instantiate(parametersOfThings);

                        if (parametersOfThings.transform.tag == "Water" || parametersOfThings.transform.tag == "Lava" || parametersOfThings.transform.tag == "Acid" || parametersOfThings.transform.tag == "Petrol")
                        {
                            handle.gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 25f);
                            canvas.GetComponent<Controler>().particlesObjects.Add(handle);
                            IEnumerator removeTime(GameObject handle)
                            {
                                yield return new WaitForSeconds(2f);
                                handle.GetComponent<ParticleSystem>().Stop();
                            }
                            StartCoroutine(removeTime(handle));
                        }
                        else
                        {
                            handle.gameObject.transform.position = gameObject.transform.position;
                            handle.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            canvas.GetComponent<Controler>().cubeObjects.Add(handle);
                        }

                    }
                }
            }
            catch
            {
            }
        }

        //Gate pipe
        if (collision.transform.tag == "piper" && blockID == 60)
        {
            on++;
            Debug.Log("Transmit");
            parametersOfThings = collision.gameObject.GetComponent<Block>().parametersOfThings;
            parametersTransportType = collision.gameObject.GetComponent<Block>().parametersTransportType;
            if (on == 1)
            {
                if (blocked == 0)
                {
                    internalInfo = "1";
                    PlayerPrefs.SetInt("pipesCount", PlayerPrefs.GetInt("pipesCount") + 1);
                    PlayerPrefs.SetInt("gateCount", PlayerPrefs.GetInt("gateCount") + 1);
                }
            }
        }
        if (collision.transform.tag == "Energy" && blockID == 60)
        {
            blocked++;
            if (blocked == 1)
            {
                musicControler.GetComponent<Sounds>().PlaySound(21);
            }
        }

        //Detector pipe
        if (collision.transform.tag == "piper" && blockID == 62)
        {
            on++;
            Debug.Log("Transmit");
            parametersOfThings = collision.gameObject.GetComponent<Block>().parametersOfThings;
            parametersTransportType = collision.gameObject.GetComponent<Block>().parametersTransportType;
            if (on == 1)
            {
                internalInfo = "1";
                PlayerPrefs.SetInt("pipesCount", PlayerPrefs.GetInt("pipesCount") + 1);
            }
        }

        //Corner pipe
        if (collision.transform.tag == "piper" && blockID == 64)
        {
            on++;
            Debug.Log("Transmit");
            parametersOfThings = collision.gameObject.GetComponent<Block>().parametersOfThings;
            parametersTransportType = collision.gameObject.GetComponent<Block>().parametersTransportType;
            if (on == 1)
            {
                internalInfo = "1";
                PlayerPrefs.SetInt("pipesCount", PlayerPrefs.GetInt("pipesCount") + 1);
            }
        }

        //Miniwood -6
        if (blockID == -6)
        {
            try
            {
                if (collision.GetComponent<Block>().blockID == -6 || collision.GetComponent<Block>().blockID == 65)
                {
                    on++;
                }
            }
            catch
            {

            }
        }

        //Tank
        if (blockID == 69)
        {
            try
            {
                if (collision.gameObject.GetComponent<Block>().blockID == 55 || collision.gameObject.GetComponent<Block>().blockID == 56 || collision.gameObject.GetComponent<Block>().blockID == 60 || collision.gameObject.GetComponent<Block>().blockID == 62 || collision.gameObject.GetComponent<Block>().blockID == 64)
                {
                    if (blockStatus == 5)
                    {
                        var handleParams = Instantiate(Particles[2], new Vector3(-1000, -1000, 100), gameObject.transform.rotation);
                        switch (internalInfo)
                        {
                            case "0": handleParams.gameObject.transform.tag = "Water"; break;
                            case "1": handleParams.gameObject.transform.tag = "Lava"; break;
                            case "2": handleParams.gameObject.transform.tag = "Acid"; break;
                            case "3": handleParams.gameObject.transform.tag = "Petrol"; break;
                        }
                        tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                        tick.GetComponent<Block>().parametersOfThings = handleParams.gameObject;
                        tick.GetComponent<Block>().blockID = -5;
                        tick.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
                        canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                        IEnumerator wait(GameObject handle)
                        {
                            yield return new WaitForSeconds(2f);
                            if (handle.GetComponent<Block>().internalInfo == "1")
                            {
                                blockStatus = 0;
                                gameObject.GetComponent<SpriteRenderer>().sprite = tanks[0];
                            }
                        }
                        StartCoroutine(wait(tick));
                    }
                }
            }
            catch
            {

            }
        }

        //Slime
        if (blockID == 71)
        {
            try
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2000000f));
                musicControler.GetComponent<Sounds>().PlaySound(20);
                PlayerPrefs.SetInt("slimeCount", PlayerPrefs.GetInt("slimeCount") + 1);
            }
            catch
            {

            }
        }

        //Inversor
        if (blockID == 72 && collision.transform.tag == "Energy")
        {
            on++;
            blockStatus = 1;
        }

        //TNT
        if (blockID == 73)
        {
            try
            {
                if (collision.gameObject.GetComponent<Block>())
                {
                    if (blockStatus == 1 && timer <= 0)
                    {
                        if(collision.gameObject.GetComponent<Block>().blockID != 73 && collision.gameObject.GetComponent<Block>().blockID != 12)
                        {
                            GameObject handle = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                            handle.GetComponent<DestroyParticles>().InvokeParticles(collision.gameObject.transform.position, collision.gameObject.GetComponent<Block>().blockID);
                            canvas.GetComponent<Controler>().cubeObjects.Remove(collision.gameObject);
                            collision.gameObject.GetComponent<Block>().SpecialDestroy();
                            Destroy(collision.gameObject);
                        }
                    }
                    if (blockStatus == 0 && collision.transform.tag == "Energy")
                    {
                        blockStatus = 1;
                    }
                }
            }
            catch
            {

            }
            
        }

        //Piano
        if (collision.transform.tag == "Energy" && blockID == 74)
        {
            on++;
            if (on == 1)
            {
                canvas.GetComponent<Piano>().PlayNote(blockStatus,internalInfo);
                tick = Instantiate(Particles[7].gameObject, new Vector2(gameObject.transform.position.x,gameObject.transform.position.y + 30f), gameObject.transform.rotation);
                //Change particleMaterial
                Color32[] noteColors = new Color32[]
                {
                    new Color32(255,100,91,255),
                    new Color32(255,161,91,255),
                    new Color32(255,237,91,255),
                    new Color32(134,185,55,255),
                    new Color32(76,219,179,255),
                    new Color32(53,137,217,255),
                    new Color32(156,67,204,255)
                };
                ParticleSystemRenderer renderer = tick.GetComponent<ParticleSystemRenderer>();
                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetColor("_EmisColor", noteColors[blockStatus - 1]);
                renderer.SetPropertyBlock(materialPropertyBlock);
                tick.GetComponent<ParticleSystemRenderer>().SetPropertyBlock(materialPropertyBlock);
                tick.GetComponent<ParticleSystem>().Play();
                //Destroy(tick);
            }
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Water")
        {
            //Planks to pocha
            if (blockID==4 && gameObject.GetComponent<SpriteRenderer>().sprite != canvas.GetComponent<Controler>().BlockSprites[52] && blockStatus != 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = canvas.GetComponent<Controler>().BlockSprites[52];
                gameObject.GetComponent<Block>().blockID = 52;
                musicControler.GetComponent<Sounds>().PlaySound(12);
            }
            //Water dark log
            if (blockID == 78 && blockStatus != 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[52];
                blockStatus = 2;
                musicControler.GetComponent<Sounds>().PlaySound(12);
            }
            if (blockID==65)
            {
                timer += Time.deltaTime * 10;
            }
            if(blockID==35 && blockStatus == 0)
            {
                blockStatus = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[40];
                musicControler.GetComponent<Sounds>().PlaySound(12);
            }
            if (blockID == 69)
            {
                if (blockStatus < 5)
                {
                    if (blockStatus == 0)
                    {
                        internalInfo = "0";
                    }
                    timer -= Time.deltaTime;
                    if (timer <= 0 && internalInfo == "0")
                    {
                        blockStatus++;
                        gameObject.GetComponent<SpriteRenderer>().sprite = tanks[blockStatus];
                        timer = 1f;
                        if (blockStatus == 5)
                        {
                            var handleParams = Instantiate(Particles[2], new Vector3(-1000, -1000, 100), gameObject.transform.rotation);
                            handleParams.gameObject.transform.tag = "Water";
                            tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                            tick.GetComponent<Block>().parametersOfThings = handleParams.gameObject;
                            tick.GetComponent<Block>().parametersTransportType = TransportTypes.Water;
                            tick.GetComponent<Block>().blockID = -5;
                            tick.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
                            canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                            PlayerPrefs.SetInt("fillCount", PlayerPrefs.GetInt("fillCount") + 1);
                            IEnumerator wait(GameObject handle)
                            {
                                yield return new WaitForSeconds(2f);
                                if (handle.GetComponent<Block>().internalInfo == "1")
                                {
                                    blockStatus = 0;
                                    gameObject.GetComponent<SpriteRenderer>().sprite = tanks[0];
                                }
                            }
                            StartCoroutine(wait(tick));
                        }
                    }
                }
            }

            //TNT OFF
            if (blockStatus == 1 && blockID == 73)
            {
                blockStatus = 0;
                timer = 4f;
                gameObject.GetComponent<SpriteRenderer>().sprite = dynamite[0];
            }
        }
        if (other.gameObject.tag == "Lava")
        {
            if (blockID == 35 && blockStatus == 1)
            {
                blockStatus = 0;
                gameObject.GetComponent<SpriteRenderer>().sprite = canvas.GetComponent<Controler>().BlockSprites[35];
            }
            if(blockID == 4 && blockStatus == 0)
            {
                blockStatus = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[42];
                PlayerPrefs.SetInt("burnedCount", PlayerPrefs.GetInt("burnedCount") + 1);
            }
            if (blockID == 52 && blockStatus == 0)
            {
                blockStatus = 1;
                gameObject.GetComponent<Block>().blockID = 4;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[42];
                PlayerPrefs.SetInt("burnedCount", PlayerPrefs.GetInt("burnedCount") + 1);
            }
            if (blockID == 3 && blockStatus == 0)
            {
                blockStatus = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[41];
                PlayerPrefs.SetInt("burnedCount", PlayerPrefs.GetInt("burnedCount") + 1);
            }
            if (blockID == 75 && blockStatus == 0)
            {
                blockStatus = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[48];
                PlayerPrefs.SetInt("burnedCount", PlayerPrefs.GetInt("burnedCount") + 1);
            }
            if (blockID == 34 && blockStatus == 0)
            {
                blockStatus = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[43];
                PlayerPrefs.SetInt("burnedCount", PlayerPrefs.GetInt("burnedCount") + 1);
            }
            if (blockID == 10 && blockStatus == 0)
            {
                blockStatus = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[44];
                PlayerPrefs.SetInt("burnedCount", PlayerPrefs.GetInt("burnedCount") + 1);
            }
            if (blockID == 51 && blockStatus == 0)
            {
                blockStatus = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[44];
                PlayerPrefs.SetInt("burnedCount", PlayerPrefs.GetInt("burnedCount") + 1);
            }
            if (blockID == 78 && blockStatus == 0)
            {
                blockStatus = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[51];
                PlayerPrefs.SetInt("burnedCount", PlayerPrefs.GetInt("burnedCount") + 1);
            }
            if (blockID == -6 && blockStatus == 0)
            {
                /*
                canvas.GetComponent<Controler>().cubeObjects.Remove(gameObject);
                Destroy(gameObject);
                SpecialDestroy();*/
                blockStatus = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = AnimationIds[45];
                PlayerPrefs.SetInt("burnedCount", PlayerPrefs.GetInt("burnedCount") + 1);
            }
            if(blockID == 5 && blockStatus == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = canvas.GetComponent<Controler>().BlockSprites[37];
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                if(internalInfo == "destroyed")
                {
                    blockStatus = 1;
                }
                else
                {
                    blockID = 37;
                }
            }
            if(blockID == 25 && blockStatus == 0)
            {
                GameObject handle = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,100),gameObject.transform.rotation);
                handle.GetComponent<Block>().blockID = 66;
                handle.GetComponent<SpriteRenderer>().sprite = canvas.GetComponent<Controler>().BlockSprites[25];
                tick = handle;
                blockStatus = 1;
                canvas.GetComponent<Controler>().temporalObjects.Add(handle);
                IEnumerator destroyIceAndChildrenDelegate(GameObject handle)
                {
                    yield return new WaitForSeconds(0.5f);
                    handle.GetComponent<Block>().SpecialDestroy();
                    Destroy(handle);
                    GameObject handleParticles = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                    handleParticles.GetComponent<DestroyParticles>().InvokeParticles(gameObject.transform.position, blockID);
                    Destroy(gameObject);
                }
                StartCoroutine(destroyIceAndChildrenDelegate(tick));
                
            }
            if (blockID == 69)
            {
                if (blockStatus < 5)
                {
                    if (blockStatus == 0)
                    {
                        internalInfo = "1";
                    }
                    timer -= Time.deltaTime;
                    if (timer <= 0 && internalInfo == "1")
                    {
                        blockStatus++;
                        gameObject.GetComponent<SpriteRenderer>().sprite = tanks[6 + blockStatus];
                        timer = 1f;
                        if (blockStatus == 5)
                        {
                            var handleParams = Instantiate(Particles[3], new Vector3(-1000, -1000, 100), gameObject.transform.rotation);
                            handleParams.gameObject.transform.tag = "Lava";
                            tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                            tick.GetComponent<Block>().parametersOfThings = handleParams.gameObject;
                            tick.GetComponent<Block>().parametersTransportType = TransportTypes.Lava;
                            tick.GetComponent<Block>().blockID = -5;
                            tick.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
                            canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                            PlayerPrefs.SetInt("fillCount", PlayerPrefs.GetInt("fillCount") + 1);
                            IEnumerator wait(GameObject handle)
                            {
                                yield return new WaitForSeconds(2f);
                                if (handle.GetComponent<Block>().internalInfo == "1")
                                {
                                    blockStatus = 0;
                                    gameObject.GetComponent<SpriteRenderer>().sprite = tanks[6];
                                }
                            }
                            StartCoroutine(wait(tick));
                        }
                    }
                }
            }

            //TNT ON
            if (blockStatus == 0 && blockID == 73)
            {
                blockStatus = 1;
            }
        }
        if(other.tag == "Acid")
        {
            if (blockID != 37 && blockID != 12 && blockID != 7 && blockID != 42 && blockID != 49 && blockID != 55 && blockID != 56 && blockID != 58 && blockID != 59 && blockID != 60 && blockID != 62 && blockID != 64 && blockID != 30 && blockID != 44 && blockID != 40 && blockID != 39 && blockID != 45 && blockID != 66 && blockID != 67 && blockID != 68 && blockID != -3 && blockID != 36 && blockID != 69)
            {
                if (blockID == 5 && internalInfo == "destroyed" && blockStatus == 1)
                {
                    //Mini glass
                }
                else
                {
                    canvas.GetComponent<Controler>().cubeObjects.Remove(gameObject);
                    Destroy(gameObject);
                    SpecialDestroy();
                    PlayerPrefs.SetInt("acidCount", PlayerPrefs.GetInt("acidCount") + 1);
                    PlayerPrefs.SetInt("eraseCount", PlayerPrefs.GetInt("eraseCount") + 1);
                    GameObject handle = Instantiate(canvas.GetComponent<Controler>().destroyParticles);
                    handle.GetComponent<DestroyParticles>().InvokeParticles(gameObject.transform.position, blockID);
                }
            }
            if (blockID == 69)
            {
                if (blockStatus < 5)
                {
                    if (blockStatus == 0)
                    {
                        internalInfo = "2";
                    }
                    timer -= Time.deltaTime;
                    if (timer <= 0 && internalInfo == "2")
                    {
                        blockStatus++;
                        gameObject.GetComponent<SpriteRenderer>().sprite = tanks[12 + blockStatus];
                        timer = 1f;
                        if (blockStatus == 5)
                        {
                            var handleParams = Instantiate(Particles[4], new Vector3(-1000, -1000, 100), gameObject.transform.rotation);
                            handleParams.gameObject.transform.tag = "Acid";
                            tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                            tick.GetComponent<Block>().parametersOfThings = handleParams.gameObject;
                            tick.GetComponent<Block>().parametersTransportType = TransportTypes.Acid;
                            tick.GetComponent<Block>().blockID = -5;
                            tick.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
                            canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                            PlayerPrefs.SetInt("fillCount", PlayerPrefs.GetInt("fillCount") + 1);
                            IEnumerator wait(GameObject handle)
                            {
                                yield return new WaitForSeconds(2f);
                                if (handle.GetComponent<Block>().internalInfo == "1")
                                {
                                    blockStatus = 0;
                                    gameObject.GetComponent<SpriteRenderer>().sprite = tanks[12];
                                }
                            }
                            StartCoroutine(wait(tick));
                        }
                    }
                }
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {  
        //Command
        if (collision.transform.tag == "Energy" && blockID == 30 && collision.gameObject.GetComponent<Block>().blockID == 30 && on == 0 && triggeredBlock == 0)
        {
            if (collision.gameObject.GetComponent<Block>().internalInfo != "spawned")
            {
                on++;
                if (blockStatus != 1) StartCoroutine(ExecuteCommands());
            }
            triggeredBlock++;
            StartCoroutine(ExecuteCommands());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Modified block
        if (collision.transform.tag == "Energy" && energizedVariantOn != null)
        {
            on--;
            if (on == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = energizedVariantOff;
            }
        }

        //Light
        if (collision.transform.tag == "Energy" && blockID == 9)
        {
            on--;
            if(on == 0)
            {
                canvas.GetComponent<Controler>().temporalObjects.Remove(LightTemporal);
                Destroy(LightTemporal);
            }
        }

        //Leds
        if (collision.transform.tag == "Energy" && blockID == 28)
        {
            on--;
            if (on == 0)
            {
                canvas.GetComponent<Controler>().temporalObjects.Remove(LightTemporal);
                Destroy(LightTemporal);
            }
        }

        //Particler
        if (blockID == 8 && collision.transform.tag == "Energy")
        {
            particleOn--;
        }

        //Command
        if (collision.transform.tag == "Energy" && blockID == 30)
        {
            on--;
        }

        //Pulse
        if (collision.transform.tag == "Energy" && blockID == 32)
        {
            on--;
            if(on == 0)
            {
                Destroy(tick);
                blockStatus = 0;
            }
        }

        //Computer
        if (collision.transform.tag == "Energy" && blockID == 36)
        {
            on--;
        }

        //Conveyer
        if (collision.transform.tag == "Energy" && blockID == 39)
        {
            on--;
        }

        //Piston
        if (collision.transform.tag == "Energy" && blockID == 40)
        {
            on--;
            if(on == 0)
            {
                PlayerPrefs.SetInt("pistonCount", PlayerPrefs.GetInt("pistonCount") + 1);
                float variety = 25f;
                musicControler.GetComponent<Sounds>().PlaySound(17);
                IEnumerator animate()
                {
                    variety = 12.5f;
                    switch (blockStatus)
                    {
                        case 0:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x + variety, gameObject.transform.position.y, 100); break;
                        case 1:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - variety, 100); break;
                        case 2:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x - variety, gameObject.transform.position.y, 100); break;
                        case 3:
                            pistonHead.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + variety, 100); break;
                    }
                    yield return new WaitForSeconds(0.10f);
                    pistonHead.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100);
                }
                StartCoroutine(animate());

            }
        }

        //Detector
        if (blockID == 33)
        {
            try
            {
                if (collision.gameObject.GetComponent<Block>().BlockType == BlockTypes.Gravity)
                {
                    Debug.Log("Pulse off" + on);
                    on--;
                    if (on <= 0)
                    {
                        Debug.Log("DestroyB");
                        Destroy(tick);
                    }
                }
            }
            catch
            {

            }

        }

        //Lantern
        if (collision.transform.tag == "Energy" && blockID == 54)
        {
            on--;
            if (on == 0)
            {
                canvas.GetComponent<Controler>().temporalObjects.Remove(LightTemporal);
                Destroy(LightTemporal);
            }
        }

        //Pipe
        if (collision.transform.tag == "piper" && blockID == 55)
        {
            on--;
            if (on == 0)
            {
                Destroy(tick);
                internalInfo = "0";
            }
        }

        //Side pipe
        if (collision.transform.tag == "piper" && blockID == 56)
        {
            on--;
            if (on == 0)
            {
                Destroy(tick);
                internalInfo = "0";
            }
        }

        //Sewer
        if (blockID == 58)
        {
            try
            {
                if (collision.gameObject.GetComponent<Block>().blockID == -5)
                {
                    on--;
                }
            }
            catch
            {

            }
        }

        //Gate pipe
        if (collision.transform.tag == "piper" && blockID == 60)
        {
            on--;
            if (on == 0)
            {
                Destroy(tick);
                internalInfo = "0";
            }
        }
        if (collision.transform.tag == "Energy" && blockID == 60)
        {
            blocked--;
        }

        //Detector pipe
        if (collision.transform.tag == "piper" && blockID == 62)
        {
            on--;
            if (on == 0)
            {
                Destroy(tick);
                Destroy(energyHandled);
                internalInfo = "0";
            }
        }

        //Corner pipe
        if (collision.transform.tag == "piper" && blockID == 64)
        {
            on--;
            if (on == 0)
            {
                Destroy(tick);
                internalInfo = "0";
            }
        }

        //Miniwood -6
        if (blockID == -6)
        {
            try
            {
                if (collision.GetComponent<Block>().blockID == -6 || collision.GetComponent<Block>().blockID == 65)
                {
                    on--;
                    if(on <= 1)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            catch
            {

            }
        }

        //Inversor
        if (blockID == 72 && collision.transform.tag == "Energy")
        {
            on--;
            blockStatus = 1;
        }

        //Piano
        if (collision.transform.tag == "Energy" && blockID == 74)
        {
            on--;
        }
    }
    GameObject used = null;
    IEnumerator speed(GameObject handle,float value)
    {
        if(true) //used != handle
        {
            used = handle;
            yield return new WaitForSeconds(0.1f);
            handle.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(value, 0));
        }
    }
    #endregion TriggerEvents
    //Command block commands functions
    #region Commands
    private IEnumerator ExecuteCommands()
    {
        if (isExecuting == false)
        {
            isExecuting = true;
            isChainBlocked = true;
            yield return new WaitForEndOfFrame();
            string[] args = internalInfo.Split(' ');
            string[] nbt = internalInfo.Split('{');
            Debug.Log(nbt[0]);
            GameObject handle;
            try
            {
                switch (args[0])
                {
                    case "/place":
                        if (args[1].Contains("~"))
                        {
                            if (args[1] == "~")
                            {
                                args[1] = gameObject.transform.position.x.ToString();
                            }
                            else
                            {
                                //args[1] = Convert.ToString(gameObject.transform.position.x + args[1].Trim(new char[] { '~' }).ToFloat());
                                args[1] = Convert.ToString(gameObject.transform.position.x + (40 * args[1].Trim(new char[] { '~' }).ToFloat()));
                            }
                        }
                        else { args[1] = (args[1].ToFloat() * 40).ToString(); } 
                        if (args[2].Contains("~"))
                        {
                            if (args[2] == "~")
                            {
                                args[2] = gameObject.transform.position.y.ToString();
                            }
                            else
                            {
                                //args[2] = Convert.ToString(gameObject.transform.position.y + args[2].Trim(new char[] { '~' }).ToFloat());
                                args[2] = Convert.ToString(gameObject.transform.position.y + (40 * args[2].Trim(new char[] { '~' }).ToFloat()));
                            }
                        }
                        else { args[2] = (args[2].ToFloat() * 40).ToString(); }
                        handle = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(args[1].ToFloat(),args[2].ToFloat(), gameObject.transform.position.z), gameObject.transform.rotation);
                        handle.GetComponent<Block>().blockID = Convert.ToInt32(args[3]);
                        if (nbt.Length > 1)
                        {
                            string[] tags = nbt[1].Split(',');
                            foreach (string tagsSingle in tags)
                            {
                                string[] dataVal = tagsSingle.Split(':');

                                //Vars to load a texture
                                var bytes = new byte[] { };
                                var tex = new Texture2D(2, 2);

                                switch (dataVal[0])
                                {
                                    case "gravity":
                                        if (dataVal[1] == "1" || dataVal[1] == "true")
                                        {
                                            if (!handle.GetComponent<Rigidbody2D>()) handle.AddComponent<Rigidbody2D>();
                                            handle.GetComponent<Rigidbody2D>().mass = 100;
                                            handle.GetComponent<Rigidbody2D>().gravityScale = 43f;
                                            handle.GetComponent<Block>().BlockType = Block.BlockTypes.Gravity;
                                        }
                                        else
                                        {
                                            if (handle.GetComponent<Rigidbody2D>())
                                            {
                                                handle.GetComponent<Rigidbody2D>().mass = 0;
                                                handle.GetComponent<Rigidbody2D>().gravityScale = 0f;
                                                handle.GetComponent<Block>().BlockType = Block.BlockTypes.Normal;
                                            }
                                        }
                                        break;
                                    case "id": handle.GetComponent<Block>().blockID = Convert.ToInt32(dataVal[1]); break;
                                    case "status": handle.GetComponent<Block>().blockStatus = Convert.ToInt32(dataVal[1]); break;
                                    case "info": handle.GetComponent<Block>().internalInfo = dataVal[1]; break;
                                    case "texture":
                                        if (dataVal[1].Contains('~'))
                                        {
                                            try
                                            {
                                                handle.GetComponent<SpriteRenderer>().sprite = canvas.GetComponent<Controler>().BlockSprites[dataVal[1].Trim(new char[] { '~' }).ToInt()];
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        else
                                        {
                                            bytes = System.IO.File.ReadAllBytes(Application.persistentDataPath + "/" + dataVal[1]);
                                            tex = new Texture2D(2, 2);
                                            tex.LoadImage(bytes);
                                            handle.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0, 80, 80), new Vector2(0.5f, 0.5f));
                                        }
                                        break;
                                    case "image":
                                        bytes = System.IO.File.ReadAllBytes(Application.persistentDataPath + "/" + dataVal[1]);
                                        tex = new Texture2D(2, 2);
                                        tex.LoadImage(bytes);
                                        handle.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                                        break;
                                    case "rotation":
                                        var quaternionBlockRotation = handle.transform.rotation;
                                        handle.transform.rotation = Quaternion.Euler(quaternionBlockRotation.eulerAngles.x, quaternionBlockRotation.eulerAngles.y, dataVal[1].ToFloat());
                                        break;
                                    case "collider-size":
                                        handle.GetComponent<BoxCollider2D>().size = new Vector2(dataVal[1].ToFloat(), dataVal[1].ToFloat());
                                        break;
                                    case "collider-size-xy":
                                        handle.GetComponent<BoxCollider2D>().size = new Vector2(dataVal[1].ToFloat(), dataVal[2].ToFloat());
                                        break;
                                    case "energy":
                                        if (dataVal[1] == "1" || dataVal[1] == "true")
                                        {
                                            handle.tag = "Energy";
                                            handle.AddComponent<BoxCollider2D>().size = new Vector2(handle.GetComponent<BoxCollider2D>().size.x + 0.35f, handle.GetComponent<BoxCollider2D>().size.y + 0.35f);
                                            handle.GetComponents<BoxCollider2D>()[1].isTrigger = true;
                                        }
                                        else
                                        {
                                            handle.tag = "Untagged";
                                            try { handle.GetComponents<BoxCollider2D>()[1].enabled = false; } catch { }
                                        }
                                        break;
                                    case "trigger-size":
                                        //if (!handle.GetComponents<BoxCollider2D>()[1]) handle.AddComponent<BoxCollider2D>();
                                        //handle.GetComponents<BoxCollider2D>()[1].size = new Vector2(dataVal[1].ToFloat(), dataVal[1].ToFloat());
                                        break;
                                    case "trigger-size-xy":
                                        //if (!handle.GetComponents<BoxCollider2D>()[1]) handle.AddComponent<BoxCollider2D>();
                                        //handle.GetComponents<BoxCollider2D>()[1].size = new Vector2(dataVal[1].ToFloat(), dataVal[2].ToFloat());
                                        break;
                                    case "kinematic":
                                        if (dataVal[1] == "1" || dataVal[1] == "true")
                                        {
                                            if (handle.GetComponent<Rigidbody2D>())
                                            {
                                                handle.GetComponent<Rigidbody2D>().isKinematic = true;
                                            }
                                            else
                                            {
                                                handle.AddComponent<Rigidbody2D>().isKinematic = true;
                                            }
                                        }
                                        else
                                        {
                                            if (handle.GetComponent<Rigidbody2D>())
                                            {
                                                handle.GetComponent<Rigidbody2D>().isKinematic = false;
                                            }
                                            else
                                            {

                                            }
                                        }
                                        break;
                                    case "size":
                                        handle.transform.localScale = new Vector2(dataVal[1].ToFloat(), dataVal[1].ToFloat());
                                        break;
                                    case "size-xy":
                                        handle.transform.localScale = new Vector2(dataVal[1].ToFloat(), dataVal[2].ToFloat());
                                        break;
                                    case "energized-texture":
                                        if (dataVal[1].Contains('~'))
                                        {
                                            try
                                            {
                                                handle.GetComponent<Block>().energizedVariantOn = canvas.GetComponent<Controler>().BlockSprites[dataVal[1].Trim(new char[] { '~' }).ToInt()];
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        else
                                        {
                                            bytes = System.IO.File.ReadAllBytes(Application.persistentDataPath + "/" + dataVal[1]);
                                            tex = new Texture2D(2, 2);
                                            tex.LoadImage(bytes);
                                            handle.GetComponent<Block>().energizedVariantOn = Sprite.Create(tex, new Rect(0, 0, 80, 80), new Vector2(0.5f, 0.5f));
                                        }
                                        break;
                                    case "energized-image":
                                        bytes = System.IO.File.ReadAllBytes(Application.persistentDataPath + "/" + dataVal[1]);
                                        tex = new Texture2D(2, 2);
                                        tex.LoadImage(bytes);
                                        handle.GetComponent<Block>().energizedVariantOn = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                                        break;
                                    case "light":
                                        if (dataVal[1] == "1" || dataVal[1] == "true")
                                        {
                                            handle.GetComponent<Block>().LightTemporal = Instantiate(Light, new Vector3(handle.transform.position.x, handle.transform.position.y, -100), handle.transform.rotation);
                                            handle.GetComponent<Block>().LightTemporal.GetComponent<Lights>().SetGradLight(new Color32(250, 250, 120, 100), 80, 100);
                                            canvas.GetComponent<Controler>().temporalObjects.Add(handle.GetComponent<Block>().LightTemporal);
                                        }
                                        break;
                                    case "light-color":
                                        Color32 col = dataVal[1].ToColor();
                                        col.a = 100;
                                        handle.GetComponent<Block>().LightTemporal.GetComponent<Lights>().SetGradLight(col, 80, 100);
                                        break;
                                    case "light-color-rgb":
                                        handle.GetComponent<Block>().LightTemporal.GetComponent<Lights>().SetGradLight(new Color32(byte.Parse(dataVal[1]), byte.Parse(dataVal[2]), byte.Parse(dataVal[3]), 100), 80, 100);
                                        break;
                                    case "light-variants":
                                        handle.GetComponent<Block>().LightTemporal.GetComponent<Lights>().SetGradLight(new Color32(handle.GetComponent<Block>().LightTemporal.GetComponent<Lights>().definedColor.r, handle.GetComponent<Block>().LightTemporal.GetComponent<Lights>().definedColor.g, handle.GetComponent<Block>().LightTemporal.GetComponent<Lights>().definedColor.b, 100), byte.Parse(dataVal[1]), byte.Parse(dataVal[2]));
                                        break;
                                    case "light-size":
                                        handle.GetComponent<Block>().LightTemporal.transform.localScale = new Vector2(dataVal[1].ToFloat(), dataVal[1].ToFloat());
                                        break;
                                    case "light-size-xy":
                                        handle.GetComponent<Block>().LightTemporal.transform.localScale = new Vector2(dataVal[1].ToFloat(), dataVal[2].ToFloat());
                                        break;
                                    case "light-rotation":
                                        var quaternionLightRotation = handle.GetComponent<Block>().LightTemporal.transform.rotation;
                                        handle.GetComponent<Block>().LightTemporal.transform.rotation = Quaternion.Euler(quaternionLightRotation.eulerAngles.x, quaternionLightRotation.eulerAngles.y, dataVal[1].ToFloat());
                                        break;
                                    case "color": 
                                        handle.GetComponent<SpriteRenderer>().color = dataVal[1].ToColor(); 
                                        break;
                                    case "color-rgb":
                                        handle.GetComponent<SpriteRenderer>().color = new Color32(byte.Parse(dataVal[1]), byte.Parse(dataVal[2]), byte.Parse(dataVal[3]), 255);
                                        break;
                                    case "alpha":
                                        Color32 colParser = handle.GetComponent<SpriteRenderer>().color;
                                        handle.GetComponent<SpriteRenderer>().color = new Color32(colParser.r, colParser.g, colParser.b, byte.Parse(dataVal[1]));
                                        break;
                                }
                            }
                        }
                        handle.GetComponent<Block>().energizedVariantOff = handle.GetComponent<SpriteRenderer>().sprite;
                        //canvas.GetComponent<Controler>().cubeObjects.Add(handle);
                        break;
                    case "/save":
                        canvas.GetComponent<SaveLoad>().SaveChanges();
                        break;
                    case "/load":
                        canvas.GetComponent<SaveLoad>().LoadWorld(args[1]);
                        break;
                    case "/crash":
                        Environment.Exit(0);
                        break;
                    case "/exit":
                        Application.Quit();
                        break;
                    case "/particle":
                        if (args[1].Contains("~"))
                        {
                            if (args[1] == "~")
                            {
                                args[1] = gameObject.transform.position.x.ToString();
                            }
                            else
                            {
                                //args[1] = Convert.ToString(gameObject.transform.position.x + args[1].Trim(new char[] { '~' }).ToFloat());
                                args[1] = Convert.ToString(gameObject.transform.position.x + (40 * args[1].Trim(new char[] { '~' }).ToFloat()));
                            }
                        }
                        else { args[1] = (args[1].ToFloat() * 40).ToString(); }
                        if (args[2].Contains("~"))
                        {
                            if (args[2] == "~")
                            {
                                args[2] = gameObject.transform.position.y.ToString();
                            }
                            else
                            {
                                //args[2] = Convert.ToString(gameObject.transform.position.y + args[2].Trim(new char[] { '~' }).ToFloat());
                                args[2] = Convert.ToString(gameObject.transform.position.y + (40 * args[2].Trim(new char[] { '~' }).ToFloat()));
                            }
                        }
                        else { args[2] = (args[2].ToFloat() * 40).ToString(); }
                        handle = Instantiate(Particles[8].gameObject, new Vector3(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]), 100), Particles[0].gameObject.transform.rotation);
                        canvas.GetComponent<Controler>().particlesObjects.Add(handle);
                        handle.GetComponent<ParticleSystem>().Play();

                        ParticleSystem.MainModule ps = handle.GetComponent<ParticleSystem>().main;
                        if (nbt.Length > 1)
                        {
                            string[] tags = nbt[1].Split(',');
                            foreach (string tagsSingle in tags)
                            {
                                string[] dataVal = tagsSingle.Split(':');
                                switch (dataVal[0])
                                {
                                    case "speed": ps.startSpeed = dataVal[1].ToFloat(); break;
                                    case "lifetime": ps.startLifetime = dataVal[1].ToFloat(); break;
                                    case "color":
                                        //ps.startColor = dataVal[1].ToColor();
                                        ParticleSystemRenderer renderer = handle.GetComponent<ParticleSystemRenderer>();
                                        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                                        renderer.GetPropertyBlock(materialPropertyBlock);
                                        materialPropertyBlock.SetColor("_EmisColor", dataVal[1].ToColor());
                                        renderer.SetPropertyBlock(materialPropertyBlock);
                                        handle.GetComponent<ParticleSystemRenderer>().SetPropertyBlock(materialPropertyBlock);
                                        break;
                                    case "color-rgb":
                                        //ps.startColor = dataVal[1].ToColor();
                                        ParticleSystemRenderer renderer2 = handle.GetComponent<ParticleSystemRenderer>();
                                        MaterialPropertyBlock materialPropertyBlock2 = new MaterialPropertyBlock();
                                        renderer2.GetPropertyBlock(materialPropertyBlock2);
                                        materialPropertyBlock2.SetColor("_EmisColor", new Color32(byte.Parse(dataVal[1]), byte.Parse(dataVal[2]), byte.Parse(dataVal[3]), 255));
                                        renderer2.SetPropertyBlock(materialPropertyBlock2);
                                        handle.GetComponent<ParticleSystemRenderer>().SetPropertyBlock(materialPropertyBlock2);
                                        break;
                                    case "duration": ps.duration = dataVal[1].ToFloat(); break;
                                    case "gravity": ps.gravityModifier = dataVal[1].ToFloat(); break;
                                    case "size": ps.startSize = dataVal[1].ToFloat(); break;
                                    case "texture":
                                        var bytes = System.IO.File.ReadAllBytes(Application.persistentDataPath + "/" + dataVal[1]);
                                        Texture2D tex = new Texture2D(2, 2);
                                        tex.LoadImage(bytes);
                                        renderer = handle.GetComponent<ParticleSystemRenderer>();
                                        materialPropertyBlock = new MaterialPropertyBlock();
                                        renderer.GetPropertyBlock(materialPropertyBlock);
                                        materialPropertyBlock.SetTexture("_MainTex", tex);
                                        renderer.SetPropertyBlock(materialPropertyBlock);
                                        handle.GetComponent<ParticleSystemRenderer>().SetPropertyBlock(materialPropertyBlock);
                                        break;
                                    case "rotation": ps.startRotation = dataVal[1].ToFloat(); break;
                                    case "limit": ps.maxParticles = Convert.ToInt32(dataVal[1]); break;
                                    case "loop": if (dataVal[1] == "1" || dataVal[1] == "true") { ps.loop = true; } else if (dataVal[1] == "0" || dataVal[1] == "false") { ps.loop = false; } break;
                                }
                            }
                        }
                        canvas.GetComponent<Controler>().particlesObjects.Add(handle);
                        break;
                    case "/destroy":
                        canvas.GetComponent<UI>().EraseAll();
                        break;
                    case "/kill":
                        switch (args[1])
                        {
                            case "particle":
                                foreach (GameObject gmo in canvas.GetComponent<Controler>().particlesObjects)
                                {
                                    Destroy(gmo);
                                }
                                break;
                        }
                        break;
                    case "/bgcolor":
                        switch (args[1])
                        {
                            case "custom":
                                if (nbt.Length > 1)
                                {
                                    string[] tags = nbt[1].Split(',');
                                    Color32 color = new Color32(0, 0, 0, 255);
                                    foreach (string tagsSingle in tags)
                                    {
                                        string[] dataVal = tagsSingle.Split(':');
                                        switch (dataVal[0])
                                        {
                                            case "color": color = dataVal[1].ToColor(); break;
                                            case "color-rgb": color = new Color32(byte.Parse(dataVal[1]), byte.Parse(dataVal[2]), byte.Parse(dataVal[3]), 255); break;
                                        }
                                    }
                                    canvas.GetComponent<SaveLoad>().ChangeBgColor(color);
                                }
                                break;
                            default: canvas.GetComponent<SaveLoad>().ChangeBgColor(args[1].ToColor()); break;
                        }
                        break;
                    case "/var":
                        switch (args[1])
                        {
                            case "create": canvas.GetComponent<Controler>().var.Add(args[2], args[3]); Debug.Log("Created"); break;
                            case "remove": canvas.GetComponent<Controler>().var.Remove(args[2]); break;
                            case "set": canvas.GetComponent<Controler>().var[args[2]] = args[3]; break;
                            case "add": canvas.GetComponent<Controler>().var[args[2]] = Convert.ToString(canvas.GetComponent<Controler>().var[args[2]].ToInt() + args[3].ToInt()); break;
                            case "insert": canvas.GetComponent<Controler>().var[args[2]] = canvas.GetComponent<Controler>().var[args[2]] + args[3]; break;
                        }
                        break;
                    case "/testfor":
                        Debug.Log("a");
                        bool condition = false;
                        switch (args[2])
                        {
                            case "!=":
                                if (canvas.GetComponent<Controler>().var[args[1]] != args[3])
                                {
                                    condition = true;
                                }
                                break;
                            case "=":
                                if (canvas.GetComponent<Controler>().var[args[1]] == args[3])
                                {
                                    condition = true;
                                }
                                break;
                            case ">":
                                if (canvas.GetComponent<Controler>().var[args[1]].ToInt() > args[3].ToInt())
                                {
                                    condition = true;
                                }
                                break;
                            case "<":
                                if (canvas.GetComponent<Controler>().var[args[1]].ToInt() < args[3].ToInt())
                                {
                                    condition = true;
                                }
                                break;
                            case ">=":
                                if (canvas.GetComponent<Controler>().var[args[1]].ToInt() >= args[3].ToInt())
                                {
                                    condition = true;
                                }
                                break;
                            case "<=":
                                if (canvas.GetComponent<Controler>().var[args[1]].ToInt() <= args[3].ToInt())
                                {
                                    condition = true;
                                }
                                break;
                        }
                        if (condition == true)
                        {
                            Debug.Log("si");
                            try { Destroy(tick); } catch { }
                            tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                            tick.GetComponent<Block>().blockID = -1;
                            tick.GetComponent<Block>().internalInfo = "spawned";
                            StartCoroutine(selfActive());
                        }
                        break;
                    case "/text":
                        if (args[1].Contains("~"))
                        {
                            if (args[1] == "~")
                            {
                                args[1] = gameObject.transform.position.x.ToString();
                            }
                            else
                            {
                                //args[1] = Convert.ToString(gameObject.transform.position.x + args[1].Trim(new char[] { '~' }).ToFloat());
                                args[1] = Convert.ToString(gameObject.transform.position.x + (40 * args[1].Trim(new char[] { '~' }).ToFloat()));
                            }
                        }
                        else { args[1] = (args[1].ToFloat() * 40).ToString(); }
                        if (args[2].Contains("~"))
                        {
                            if (args[2] == "~")
                            {
                                args[2] = gameObject.transform.position.y.ToString();
                            }
                            else
                            {
                                //args[2] = Convert.ToString(gameObject.transform.position.y + args[2].Trim(new char[] { '~' }).ToFloat());
                                args[2] = Convert.ToString(gameObject.transform.position.y + (40 * args[2].Trim(new char[] { '~' }).ToFloat()));
                            }
                        }
                        else { args[2] = (args[2].ToFloat() * 40).ToString(); }
                        CreateNewText("", (textModifier) =>
                         {
                             textModifier.transform.position = new Vector2(args[1].ToFloat(), args[2].ToFloat());
                             if (nbt.Length > 1)
                             {
                                 string[] tags = nbt[1].Split(',');
                                 foreach (string tagsSingle in tags)
                                 {
                                     string[] dataVal = tagsSingle.Split(':');
                                     switch (dataVal[0])
                                     {
                                         case "text": textModifier.GetComponent<TMPro.TextMeshProUGUI>().text = dataVal[1].ToString(); break;
                                         case "color": textModifier.GetComponent<TMPro.TextMeshProUGUI>().color = dataVal[1].ToColor(); break;
                                         case "color-rgb": textModifier.GetComponent<TMPro.TextMeshProUGUI>().color = new Color32(byte.Parse(dataVal[1]), byte.Parse(dataVal[2]), byte.Parse(dataVal[3]), 255); break;
                                         case "size": textModifier.GetComponent<TMPro.TextMeshProUGUI>().fontSize = dataVal[1].ToInt(); break;
                                     }
                                 }
                             }
                         });
                        break;
                    case "/msgbox":
                        if (nbt.Length > 1)
                        {
                            string[] tags = nbt[1].Split(',');
                            string title = "";
                            string content = "";
                            string btnA = "";
                            string btnB = "";

                            foreach (string tagsSingle in tags)
                            {
                                string[] dataVal = tagsSingle.Split(':');
                                switch (dataVal[0])
                                {
                                    case "title": title = dataVal[1]; break;
                                    case "content": content = dataVal[1]; break;
                                    case "button-a": btnA = dataVal[1]; break;
                                    case "button-b": btnB = dataVal[1]; break;
                                }
                            }
                            canvas.GetComponent<Message>().ShowAlert(title, content, btnA, btnB, new System.Action(() => { musicControler.GetComponent<Sounds>().PlaySound(3); }), new System.Action(() => { musicControler.GetComponent<Sounds>().PlaySound(3); }));
                        }
                        break;
                    case "/sound":
                        int idSong = Int32.Parse(args[1]);
                        musicControler.GetComponent<Sounds>().PlaySound(idSong);
                        break;

                }
                if(blockStatus == 2)
                {
                    Debug.Log("Chainer");
                    try { Destroy(tick); } catch { }
                    tick = Instantiate(canvas.GetComponent<Controler>().Cube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 100), gameObject.transform.rotation);
                    tick.GetComponent<Block>().blockID = -1;
                    tick.GetComponent<Block>().internalInfo = "spawned2";
                    canvas.GetComponent<Controler>().temporalObjects.Add(tick);
                    StartCoroutine(selfActive());
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            isExecuting = false;
        }
    }
    private void CreateNewText(string textString,Action<GameObject> textModifier = null)  //Cretes new text
    {
        GameObject handle = Instantiate(Text,Text.transform);
        handle.GetComponent<TMPro.TextMeshProUGUI>().text = textString;
        textModifier?.Invoke(handle); //Execute action to customize text
        canvas.GetComponent<Controler>().temporalObjects.Add(handle);
    }
    IEnumerator selfActive() //Energy pulse
    {
        yield return new WaitForSeconds(5f);
        Destroy(tick);
        triggeredBlock = 0;
        isChainBlocked = false;
    }
}

#endregion Commands

//Sandlife Block IDs
#region ID
/*
ID:0 Grass
ID:1 Stone
ID:2 Dirt
ID:3 Wood
ID:4 Planks
ID:5 Sand
ID:6 Gravel
ID:7 Energy
ID:8 Particler
ID:9 Light
ID:10 Leaves
ID:11 Bricks
ID:12 Obsidian
ID:13 Shit
ID:14 Pumpkin
ID:15 Red Wool
ID:16 Orange Wool
ID:17 Yellow Wool
ID:18 Lime Wool
ID:19 Green Wool
ID:20 Blue Wool
ID:21 Purple Wool
ID:22 Pink Wool
ID:23 White Wool
ID:24 Black Wool
ID:25 Ice
ID:26 Snow
ID:27 Present
ID:28 Leds
ID:29 Switch
ID:30 Command
ID:31 StoneBrick
ID:32 Pulse
ID:33 Detector
ID:34 Walnut Wood
ID:34 Walnut Planks
ID:35 Sponge
ID:36 Computer
ID:37 Glass
ID:38 Amethyst
ID:39 Conveyer
ID:40 Piston
ID:41 Energy dust
ID:42 Vortex
ID:43 Box
ID:44 Uranium
id:45 Destroyer
ID:46 Marble
ID:47 Broken Bricks
ID:48 Wheat
ID:49 Void
ID:50 Broken Stone
ID:51 Berry
ID:52 Rotten planks
ID:53 Diorite
ID:54 Latern
ID:55 Pipe
ID:56 Side Pipe
ID:57 Rocks
ID:58 Sewer
ID:59 Hopper
ID:60 Gate pipe
ID:61 Quartz
ID:62 Signal pipe
ID:63 Copper
ID:64 Corner pipe
ID:65 Flowerpot
ID:66 Water tap
ID:67 Lava tap
ID:68 Acid tap
ID:69 Tank
ID:70 Meteorite
ID:71 Slime
ID:72 Inversor
ID:73 Dynamite
ID:74 Piano
ID:75 Dark wood
*/
#endregion ID