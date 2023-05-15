using UnityEngine;
using Vuforia;
using Photon.Pun;

public class anchorTracking : MonoBehaviour
{
    private TrackableBehaviour trackableBehaviour;
    private bool isTracking;
    private Vector3 anchorPosition;
    private Quaternion anchorRotation;

    private PhotonView photonView;

    private void Awake()
    {
        trackableBehaviour = GetComponent<TrackableBehaviour>();
        photonView = GetComponent<PhotonView>();
  
    }

    private void OnDestroy()
    {
    
    }

    public void trackingImage() {
        anchorPosition = transform.position;
        anchorRotation = transform.rotation;

        // Update the dynamic anchor's position and rotation across the network
        photonView.RPC("UpdateDynamicAnchor", RpcTarget.Others, anchorPosition, anchorRotation);
    }


    [PunRPC]
    private void UpdateDynamicAnchor(Vector3 position, Quaternion rotation)
    {
        // Update the dynamic anchor's position and rotation
        anchorPosition = position;
        anchorRotation = rotation;
    }
}
