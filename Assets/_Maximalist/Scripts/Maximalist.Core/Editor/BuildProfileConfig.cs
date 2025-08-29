// ------------------------------------------------------------------------------
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     1/04/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

namespace Maximalist.Builds
{
	[System.Serializable]
	public class BuildProfileConfig
	{
		public string profileName;
		public BuildType buildType = BuildType.Development;
		public bool shouldBumpVersion = true;
	}
}
