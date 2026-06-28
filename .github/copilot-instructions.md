# GitHub Copilot Instructions for Large Faction Bases (Continued) Mod

## Mod Overview and Purpose
The "Large Faction Bases (Continued)" mod for RimWorld aims to enhance the faction base experience by generating larger, more dynamic faction bases. These bases grow in size and manpower over time, providing a challenging gameplay experience that scales with difficulty level and incorporates randomness. This mod adds a high-risk, high-reward challenge for players seeking to test their strategies against formidable opponents. It is designed to be safe to add or remove from games at any stage.

## Key Features and Systems
- **Dynamic Base Growth**: Faction bases expand in size and workforce over time, creating engaging and evolving challenges.
- **Difficulty Scaling**: Base complexity and strength scale with the game's difficulty settings, making the challenge appropriate for a variety of player skill levels.
- **Compatibility Focus**: The mod works alongside others that modify faction bases, with recommended adjustments available through "Choose your enemy base" by BrokenBed.

## Coding Patterns and Conventions
- **Class Naming**: Classes are prefixed to denote their purpose or the system they belong to, such as `GenStep_`, `SymbolResolver_`, and `LordJob_`.
- **Method Convention**: Methods typically begin with verbs to describe their functionality clearly, such as `updateDesiredPowerOutputForAllGenerators`, `trySpawnBatteryNear`, etc.
- **Static Definitions**: Utilizes static classes like `Large_DefOf` for centralizing definitions that need to be accessible globally within the mod.

## XML Integration
- XML files are used to define new content data such as pawns, buildings, and faction details.
- XML structure benefits from using clear and concise element names to ensure easy interpretability by both the game engine and modders.
- Definitions generally align with vanilla content styles to maintain compatibility and stability.

## Harmony Patching
- Harmony is utilized to patch vanilla RimWorld methods to integrate functionalities such as expanding faction bases without overriding core game files.
- Identify core methods that need adjustment, then write Harmony patches as needed to inject or override behaviors.
- Use descriptive annotations and organize patches neatly to ensure clear understanding of what vanilla method interactions are being modified.

## Suggestions for Copilot
1. Utilize Copilot to suggest method names that conform to the existing naming conventions, ensuring consistency across files.
2. Leverage Copilot for boilerplate code for new Harmony patches, reducing the time spent on repetitive code structures.
3. Use Copilot to generate XML definitions by suggesting possible values or completing large repetitive sections.
4. Assist with creating test cases or debugging snippets by predicting potential errors or implementing common checks.
5. Utilize Copilot for documentation generation by auto-completing or suggesting documentation comments based on method functionality.

### Additional Resources
- Join the Discord channel provided in the mod description for community support and error reporting.
- Use tools like RimSort to organize mods for better compatibility and performance.
- Post logs using the Log Uploader when reporting issues to provide sufficient information for troubleshooting.

This file should act as a guide for getting the most out of GitHub Copilot while working with the "Large Faction Bases (Continued)" mod for RimWorld. Happy modding!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
