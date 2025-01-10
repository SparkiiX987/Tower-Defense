using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] Transform particleSpawnPoint;
    [SerializeField] GameObject explosionEffect;
    AudioSource audioSource;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAnimation()
    {
        audioSource.Play();
        GameObject particles = Instantiate(explosionEffect);
        particles.transform.position = particleSpawnPoint.position;
        StartCoroutine(CastleAnimationBreak());
        Destroy(particles, 1.5f);
    }

    private IEnumerator CastleAnimationBreak()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(0.3f);
        }
    }
}
