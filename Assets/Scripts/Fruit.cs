using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole, sliced;
    private ParticleSystem juiceParticleEffect;
    private Collider fruitCollider;
    private Rigidbody fruitRigidbody;
    private AudioSource fruitAudioSource;

    private void Awake()
    {
        fruitCollider = GetComponent<Collider>();
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitAudioSource = GetComponent<AudioSource>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.gameObject.transform.position, blade.direction, blade.sliceForce);
            FindObjectOfType<GameManager>().IncreaseScore();
        }

    }

    public void Slice(Vector3 position, Vector3 direction, float force) 
    {
        fruitCollider.enabled = false;
        whole.SetActive(false);
        sliced.SetActive(true);
        juiceParticleEffect.Play();
        fruitAudioSource.Play();


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody[] slicedRigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody slicedRigidbody in slicedRigidbodies)
        {
            slicedRigidbody.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }


}
