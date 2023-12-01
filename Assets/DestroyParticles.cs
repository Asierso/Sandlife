using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    public GameObject ParticleDestroy;
    public Canvas canvas;
    public void InvokeParticles(Vector3 pos,int blockID)
    {
        if (PlayerPrefs.GetInt("destroyParticles") == 1)
        {
            //GameObject handle = Instantiate(ParticleDestroy, pos, ParticleDestroy.transform.rotation);
            gameObject.transform.position = pos;
            ParticleSystemRenderer renderer = gameObject.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                try
                {
                    renderer.material.mainTexture = TextureFromSprite(canvas.GetComponent<Controler>().BlockSprites[blockID]);
                }
                catch
                {
                    if (blockID == -6)
                    {
                        renderer.material.mainTexture = TextureFromSprite(canvas.GetComponent<Controler>().BlockSprites[10]);
                    }
                }

            }
            else
            {
                Debug.LogError("renderer is null");
            }
            gameObject.GetComponent<ParticleSystem>().Play();
        }
        StartCoroutine(ParticleKiller(gameObject));
    }
    public static Texture2D TextureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
    IEnumerator ParticleKiller(GameObject handle)
    {
        yield return new WaitForSeconds(1f);
        Destroy(handle);
    }
}
