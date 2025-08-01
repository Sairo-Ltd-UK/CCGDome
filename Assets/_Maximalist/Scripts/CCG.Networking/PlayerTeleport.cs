using Mirror;
using UnityEngine;

namespace CCG.Networking
{
	public class PlayerTeleport : NetworkBehaviour
	{
		[Command]
		public void CmdTeleportToPosition(Vector3 targetPosition)
		{
			RpcTeleport(targetPosition);
		}

		[ClientRpc]
		private void RpcTeleport(Vector3 targetPosition)
		{
			transform.position = targetPosition;
		}
	}
}
