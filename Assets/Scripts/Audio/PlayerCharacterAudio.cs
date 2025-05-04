using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundedSystem))]
public class PlayerCharacterAudio : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField]
    MaterialSoundDictionaryAsset soundDictionaryAsset;
    [SerializeField]
    RepeatingEventTimeline footstepTimeline;
    [SerializeField]
    float footStepRate;
    public SoundCollection OverideSound;
    [Header("Jumping")]
    [SerializeField]
    private SoundCollection jumpSound;

    private SoundInstance overrideSoundInstance;
    private SoundInstance jumpSoundInstance;

    MaterialSoundDictionary soundDictionary;
    GroundedSystem groundedSystem;
    PlayerCharacterController characterController;
    Material groundMaterial;

    void Awake()
    {
        characterController = GetComponent<PlayerCharacterController>();
        groundedSystem = GetComponent<GroundedSystem>();
        soundDictionary = soundDictionaryAsset.GetDictionary();
        footstepTimeline.Event += PlayFootstepSound;

        overrideSoundInstance = OverideSound.GetInstance();
        jumpSoundInstance = jumpSound.GetInstance();
    }

    private void OnEnable()
    {
        groundedSystem.OnCheckGround.AddListener(SaveMaterialName);
        characterController.JumpStarted.AddListener(PlayJumpSound);
    }

    private void OnDisable()
    {
        groundedSystem.OnCheckGround.RemoveListener(SaveMaterialName);
        characterController.JumpStarted.RemoveListener(PlayJumpSound);
    }

    private void Update()
    {
        if (!characterController.IsGrounded) return;
        footstepTimeline.TraverseTimeline(characterController.Velocity.magnitude * footStepRate);
    }

    private void PlayFootstepSound()
    {
        if (OverideSound != null)
        {
            var clip = overrideSoundInstance.GetClip();
            if (clip == null) return;
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
        else
        {
            var clip = soundDictionary.GetClip(groundMaterial);
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(soundDictionary.GetClip(groundMaterial), transform.position);
            }
        }
    }

    private void PlayJumpSound()
    {
        if (jumpSound == null) return;
        var clip = jumpSoundInstance.GetClip();
        if (clip == null) return;

        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    private void SaveMaterialName(RaycastHit? groundCheckResult)
    {
        if (groundCheckResult == null) return;
        groundCheckResult = groundCheckResult.Value;
        RaycastHit ground = groundCheckResult.Value;

        if (ground.collider is not MeshCollider)
        {
            var meshRenderer = ground.collider.GetComponentInParent<MeshRenderer>();
            if (meshRenderer != null)
            {
                groundMaterial = ground.collider.GetComponentInParent<MeshRenderer>().sharedMaterial;
            }
            groundMaterial = null;
            return;
        }

        int triangleIndex = ground.triangleIndex;
        Mesh mesh = (ground.collider as MeshCollider).sharedMesh;
        
        if (ground.collider.TryGetComponent<MeshRenderer>(out var renderer))
        {
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                int[] tris = mesh.GetTriangles(i);
                for (int j = 0; j < tris.Length; j++)
                {
                    if (tris[j] == triangleIndex)
                    {
                        groundMaterial = renderer.sharedMaterials[i];
                        return;
                    }
                }
            }
        }
        else
        {
            groundMaterial = null;
            return;
        }
    }
}
