using CCG.Networking;
using UnityEngine;

namespace CCG.XR
{
	public class XRPlayerRig : MonoBehaviour
	{
		public Transform rHandTransform;
		public Transform lHandTransform;
		public Transform headTransform;

		public Transform canvasUIPosition;

		public XRNetworkPlayerScript localVRNetworkPlayerScript;

		private void Update()
		{
			if (localVRNetworkPlayerScript == null)
				return;

			localVRNetworkPlayerScript.gameObject.transform.position = transform.position;

			// presuming you want a head object to sync, optional, same as hands.
			localVRNetworkPlayerScript.headTransform.position = headTransform.position;
			localVRNetworkPlayerScript.headTransform.rotation = headTransform.rotation;
			localVRNetworkPlayerScript.rHandTransform.position = rHandTransform.position;
			localVRNetworkPlayerScript.rHandTransform.rotation = rHandTransform.rotation;
			localVRNetworkPlayerScript.lHandTransform.position = lHandTransform.position;
			localVRNetworkPlayerScript.lHandTransform.rotation = lHandTransform.rotation;
		}

	}
}
