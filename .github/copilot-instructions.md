# GitHub Copilot Instructions for Large Faction Bases (Continued) Mod

## Mod Overview and Purpose

**Mod Name:** Large Faction Bases (Continued)  
**Description:** This mod enhances the gameplay experience in RimWorld by generating larger and more complex faction bases. These bases evolve over time, increasing in size and manpower, while taking into account the difficulty level and introducing an element of randomness. The goal is to provide a high-risk, high-reward challenge for players who seek to engage with more formidable foes.

The mod is safe to add or remove in existing games. To ensure compatibility with other mods that alter faction bases, users should reference "Choose your enemy base" from the BrokenBed mod.

## Key Features and Systems

- **Evolving Faction Bases:** Bases dynamically change in size and manpower based on time progression, difficulty level, and random factors.
- **Complex Power Networks:** Advanced power network system allowing nuanced control and connection logic for generators, batteries, and power users.
- **Diverse Symbol Resolvers:** Multiple `SymbolResolver` implementations introduce diverse structures and item placements within faction bases.

## Coding Patterns and Conventions

- **Class Naming:** Classes generally follow a descriptive naming pattern, denoting their role in the generation process, e.g., `GenStep_LargeFactionBase`, `SymbolResolver_EdgeDefense2`.
- **Method Signature Consistency:** Methods are named using camelCase, clearly describing their action and often indicating the expected input (`tryFindClosestReachableNet`, `spawnTransmitters`).
- **Comments and Documentation:** Descriptive comments are encouraged to clarify the purpose of complex logic, especially where randomization or evolutionary mechanics are involved.

## XML Integration

- **XML Definitions:** Definitions such as `PawnKindDef`, `ThingSetMakerDef`, and others are statically declared in class files like `Large_DefOf.cs` and `Large_PawnKindDefOf.cs`.
- **XML Modding Structure:** The integration of custom XML configurations follows RimWorld's modding guidelines to define new pawn kinds, duties, and item sets which the C# code then references.

## Harmony Patching

- **Harmony Use:** Although specifics of Harmony patches are not detailed in the summary, it's suggested to leverage Harmony for altering or extending existing game mechanics and behaviors distinctly.
- **Patch Organization:** Keep patches organized by their target functionality and ensure they are grouped logically within the project's structure.

## Suggestions for Copilot

- **Class Expansions:** When expanding class functionality, suggest new methods that complement existing ones, e.g., additional power management utilities in `GenStep_LargePower`.
- **Randomization Logic:** Generate code snippets that introduce and manage randomness in base evolution, respecting existing patterns.
- **Error Handling:** Implement robust error handling in methods where base connectivity might fail, such as in `tryFindClosestReachableNet`.
- **Consistent Naming:** Ensure new names for variables, methods, or classes are consistent with established naming patterns.
- **New Feature Prototypes:** For new features or systems, provide prototype implementations with comments to explain the flow and logic.

By following these instructions, developers can maintain consistency and code quality while leveraging Copilot to expand and enhance the Large Faction Bases (Continued) mod.
