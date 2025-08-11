version: 1.1
builds:
  Build B: # replace with the name for your build
    executableName: CCGDome_DedicatedServer.0.2.x86_64 # the name of your build executable
    buildPath: C:\_Maximalist\_Builds\CCGDome_DedicatedServer.0.2
    excludePaths: # paths to exclude from upload (supports only basic patterns)
      - C:\_Maximalist\_Builds\CCGDome_DedicatedServer.0.2\CCGDome_BurstDebugInformation_DoNotShip\*.*
buildConfigurations:
  Config B: # replace with the name for your build configuration
    build: Build B # replace with the name for your build
    queryType: sqp # sqp or a2s, delete if you do not have logs to query
    binaryPath:  CCGDome_DedicatedServer.0.2.x86_64 # the name of your build executable
    commandLine: -nographics -batchmode -port $$port$$ -queryport $$query_port$$ -logFile $$log_dir$$/Engine.log -multiplay -dedicatedServer # launch parameters for your server
    variables: {}
fleets:
  Fleet B: # replace with the name for your fleet
    buildConfigurations:
      - Config B # replace with the names of your build configuration
    regions:
      Europe: # North America, Europe, Asia, South America, Australia
        minAvailable: 1 # minimum number of servers running in the region
        maxServers: 1 # maximum number of servers running in the region
    usageSettings:
      - hardwareType: CLOUD #The hardware type of a machine. Can be CLOUD or METAL.
        machineType: GCP-N2 # Machine type to be associated with these setting.   * For CLOUD setting: In most cases, the only machine type available for your fleet is GCP-N2.   * For METAL setting: Please omit this field. All metal machines will be using the same setting, regardless of its type.
        maxServersPerMachine: 1 # Maximum number of servers to be allocated per machine.   * For CLOUD setting: This is a required field.   * For METAL setting: This is an optional field.