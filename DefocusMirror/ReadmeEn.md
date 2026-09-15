# DefocusMirror — User Guide

## What is DefocusMirror?

DefocusMirror mirrors your desktop to a secondary screen with per-channel chromatic blur, simulating retinal defocus via longitudinal chromatic aberration (LCA). It is a research tool for myopia/hyperopia control.

Based on the paper: Barbara Swiatczak, Frank Schaeffel. *"Myopia: why the retina stops inhibiting eye growth."* Scientific Reports 12, 21704 (2022). https://doi.org/10.1038/s41598-022-26323-7

The paper found that when the red channel is sharp and the blue channel is blurred (simulating hyperopic defocus), emmetropic eyes shorten; when the blue channel is sharp and red is blurred (simulating myopic defocus), eyes elongate. However, already-myopic eyes no longer respond to this chromatic defocus signal. This software attempts to recreate that chromatic defocus signal on a computer screen.

## How it works

You need at least two monitors:

- **Source screen**: runs your applications (browser, video player, games, etc.)
- **Defocus mirror screen**: shows a blurred mirror of the source — you look at this screen to work

**Key point: once the defocus mirror is running, you work entirely from the defocus screen.** The source screen just runs apps in the background. The cursor on the defocus screen has small "m" characters so you can tell it apart from the real mouse cursor (the defocus cursor operates as a mirrored version of the source).

## Getting started

1. **Connect at least two monitors** and extend (not duplicate) your desktop to the second monitor
2. **Move all application windows to the source screen** — the defocus mirror will cover it, so arrange your workspace first
3. Open the DefocusMirror control box
4. Select the **source screen** (capture) and **defocus mirror screen** (output) in the toolbar
5. Set the **viewing distance** — 700mm is typical for desktop use
6. Click "Myopia defocus" or "Hyperopia defocus" to start
7. **After the defocus screen starts, just look at it and work normally**

## Modes

- **Myopia defocus**: Red channel sharp, green/blue blurred. For myopia control.
- **Hyperopia defocus**: Blue channel sharp, red/green blurred. For hyperopia control.
- **Custom**: Manually adjust R/G/B blur intensity and spread on the "Advanced" tab.

## Reference pixels

The bottom-right of the first tab shows "Reference pixels" — a 1-pixel dot for each R/G/B channel at current blur settings. **You must move the control box to the source screen to see the correct effect.**

## Viewing distance

The physical distance from your eyes to the defocus screen (in mm). This controls the blur circle size:

| Scenario | Suggested distance |
|----------|-------------------|
| Desktop work | 700mm |
| TV viewing | 1500–2000mm |

## Frame rate

Default 30fps is usually fine. Set higher (50/60fps) for video or gaming. Lower if your GPU struggles.

## Full-screen vs. windowed

- **Full-screen** (default): the defocus image fills the entire target monitor
- **Windowed**: freely resize and position the defocus window

## Tray icon

Closing the control box minimizes it to the system tray; the defocus screen keeps running. Tray icon context menu:

- **Start with last config**: quickly restart with previous mode and settings
- **Close defocus output**: stop the defocus screen
- **Show control box**: reopen the control panel
- **Exit**: quit the application

Double-click the tray icon to reopen the control box.

## Exiting

Right-click the tray icon and select "Exit". Closing the control box only hides it to the tray — it does not quit the app.

## Disclaimer

This software is for **academic research and personal experimentation only**. It is **not medical advice and not a treatment**. The underlying scientific principle is based on a published peer-reviewed paper, but it is still under active research and has not been clinically validated as a therapy. **Always consult an ophthalmologist before using any tool for vision intervention.** The author assumes no responsibility for any direct or indirect consequences arising from the use of this software. If you experience any changes in vision, seek professional medical attention immediately.

## FAQ

**Q: The defocus screen is black / no image?**
A: Make sure your desktop is extended (not duplicated) to the second monitor. Check Windows display settings.

**Q: The cursor on the defocus screen looks different?**
A: That's expected. The defocus cursor has "m" markers and mirrors the source cursor. Moving it on the defocus screen controls the source cursor.

**Q: Can I use just one monitor?**
A: Technically you can try it in windowed mode, but the defocus effect requires at least two monitors to work properly.

## Tech stack

- C# 13 / .NET 10 / WinForms
- SharpDX 4.2.0 (Direct3D 11)
- HLSL pixel shader (7×7 Gaussian blur)

## References

1. Swiatczak, B. & Schaeffel, F. "Myopia: why the retina stops inhibiting eye growth." *Scientific Reports* 12, 21704 (2022). https://doi.org/10.1038/s41598-022-26323-7
2. Atchison, D.A. & Smith, G. *Optics of the Human Eye*. Butterworth-Heinemann, 2000.

## License

For academic research and personal use only. Not for clinical diagnosis or treatment.
