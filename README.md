# Settlement Culture Changer (M&amp;B2:B)
#### Summary
Allows for configurable realignment of a settlement's culture in Mount &amp; Blade II: Bannerlord.

## Features
- Automatically aligns a settlement's culture to match its owner
  - Configure random amount of days it takes to convert, OR
  - Configure to convert immediately upon ownership change
- Configurable notification system to keep track of culture changes
- Configure whose settlements get converted
- Tools/Cheats to convert culture with the click of a button

#### Coming Soon & WIP
- Choose alignment by Governor instead of Owner
- Settlement menu option to determine current alignment progress

## Version Requirement Matrix
_Note that this mod automatically comes packaged with the required MCM, ButterLib, and Harmony DLL's._

| Component                   | Version Required | Included With Mod |
|-----------------------------|------------------|-------------------|
| Mount & Blade 2: Bannerlord | 1.7.x            | No                |
| Mod Configuration Menu      | 4.7.x            | Yes               |
| Harmony                     | 2.2.x            | Yes               |
| ButterLib                   | 2.1.x            | Yes               |

## Installation
Extract the "Settlement Culture Changer" folder from within the `.zip` into your game's "Modules" folder.

## Usage
1. Install the mod
2. Load your save and open the mod's options through MCM
3. Configure as needed to match your play style
4. Depending on your selected options, you'll receive messages as settlements align their culture to their owner

## Developer Notes
- When opening the solution, you may have to manually change/adjust the assembly references for all TaleWorld DLL's, Module DLL's, and MCM DLL's. This API doesn't assume that every dev has installed M&B2 into the same Steam directory.
