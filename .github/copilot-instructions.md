# GitHub Copilot Instructions for RimWorld Mod: Large Faction Bases (Continued)

## Mod Overview and Purpose

**Mod Name:** Large Faction Bases (Continued)  
**Author:** newcolonist  
**PackageId:** Mlie.LargeFactionBases  
**Mod Description:** This mod is an update to the "newcolonists" mod and focuses on generating larger faction bases. The size and manpower of these bases evolve over time, factoring in difficulty levels and randomness. It presents a high-risk, high-reward challenge for players.

## Key Features and Systems

- **Dynamic Base Generation:** Generates larger faction bases whose size and manpower evolve with gameplay progression.
- **Difficulty Levels:** Adjusts the size and complexity of bases based on the current difficulty level.
- **Randomness:** Introduces randomness in base generation to provide varied and unpredictable challenges.
- **Compatibility:** Compatible with other mods that change faction bases, with the option to use the "Choose Your Enemy Base" mod from BrokenBed for enhanced functionality.

## Coding Patterns and Conventions

- **Namespace:** The mod follows a consistent namespace structure, typically starting with `LargeFactionBase`.
- **Class Naming:** Classes are named descriptively, such as `GenStep_LargeFactionBase2`, `Large_DutyDefOf`, and `SymbolResolver_EdgeDefense2`.
- **Member Naming:** Public members are named using PascalCase, while private members are prefixed with an underscore.
- **Comments and Documentation:** The code includes comments and documentation to help developers understand the purpose and functionality of each class and method.

## XML Integration

The mod uses XML files to define various game elements such as duty definitions, rules, and thing set makers. Here are some key XML files and their purposes:

- **Duties_Settlement2.xml:** Contains duty definitions for faction settlements.
- **Rules_Indoors.xml and Rules.xml:** Contains rule definitions for indoor and general gameplay rules.
- **IED.xml:** Defines the properties and behavior of Improvised Explosive Devices (IEDs).
- **ThingSetMakers_MapGen.xml:** Contains definitions for generating things in map gen.

## Harmony Patching

The mod uses Harmony for patching existing RimWorld code. Common targets for patching include:

- **GenStep:** Custom generation steps for faction bases.
- **DefOf:** Definitions of various game elements such as duties, thing set makers, and symbols.

Here’s an example of a Harmony patch:

csharp
using HarmonyLib;
using RimWorld;
using Verse;

namespace LargeFactionBase
{
    [HarmonyPatch(typeof(GenStep), "Generate")]
    public static class GenStep_LargeFactionBase2Patch
    {
        [HarmonyPostfix]
        public static void Postfix(GenStep __instance, Map map, GenStepParams parms)
        {
            // Custom generation logic here
        }
    }
}


## Suggestions for Copilot

To ensure that Copilot provides relevant suggestions, here are some guidelines:

1. **Namespace and Class Prefixes:** Use the consistent namespace and class prefixes (`LargeFactionBase`) when generating new classes or methods.
2. **XML Definitions:** When working with XML files, ensure that the definitions follow the existing schema and conventions.
3. **Harmony Patches:** When creating Harmony patches, make sure to target the correct methods and use appropriate patching attributes (`[HarmonyPrefix]`, `[HarmonyPostfix]`).
4. **Code Comments:** Include comments in the code to explain complex logic or new features, which can help Copilot generate better suggestions in the future.

By following these guidelines, you can ensure that Copilot provides accurate and helpful suggestions for your mod development.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Temporary files

- Do not create temporary files, build output, test artifacts, or Copilot session
  folders inside the mod root.
- Store temporary artifacts under `%USERPROFILE%\CopilotTemp\[modname]`.
- Direct build and test output there as well, without changing project settings.
- Clean up task-specific temporary artifacts when finished.

## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

