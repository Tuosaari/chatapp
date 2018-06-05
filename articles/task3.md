Task 2 - Service Fabric conversion
===

Goal was to convert app to be deployable to service fabric cluster

## Conversion

Final result can be seen in branch "SFConversion". Service Fabric application was done by adding service fabric application to the solution and adding web site as existing service to it.

Web site was converted to build an .exe instead of .dll (by introducing runtime identifier for the project). Main function was altered to initialize Kestrel to work in Service Fabric environment and basic settings and manifest files were added. Additionally appsettings.json loading was added as part of startup as secrects were no longer available. In production configuration would probably move to Key Vault.
