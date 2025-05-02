using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundedSystem))]
public class FootSteps : MonoBehaviour
{
    [SerializeField]
    MaterialSoundDictionaryAsset soundDictionaryAsset;
    [SerializeField]
    RepeatingEventTimeline footstepTimeline;
    [SerializeField]
    float footStepRate;

    public SoundCollection OverideSound;
    MaterialSoundDictionary soundDictionary;
    GroundedSystem groundedSystem;
    PlayerCharacterController characterController;
    Material groundMaterial;

    void Awake()
    {
        characterController = GetComponent<PlayerCharacterController>();
        groundedSystem = GetComponent<GroundedSystem>();
        soundDictionary = soundDictionaryAsset.GetDictionary();
        footstepTimeline.Event += PlaySound;
    }

    private void OnEnable()
    {
        groundedSystem.OnCheckGround.AddListener(SaveMaterialName);
    }

    private void OnDisable()
    {
        groundedSystem.OnCheckGround.RemoveListener(SaveMaterialName);
    }

    private void Update()
    {
        if (!characterController.IsGrounded) return;
        footstepTimeline.TraverseTimeline(characterController.Velocity.magnitude * footStepRate);
    }

    private void PlaySound()
    {
        if (OverideSound != null)
        {
            AudioSource.PlayClipAtPoint(OverideSound.GetClip(), transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(soundDictionary.GetClip(groundMaterial), transform.position);
        }
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
