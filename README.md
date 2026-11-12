# DefocusMirror

[English guide](README.en.md) · [中文完整说明](README.zh.md)

利用人眼色差（LCA）原理，将桌面画面以分通道模糊的方式镜像到副屏，模拟视网膜离焦信号，用于近视/远视防控的辅助研究工具。

---

Mirror your desktop with per-channel chromatic blur to a secondary screen, simulating retinal defocus via longitudinal chromatic aberration (LCA). Based on the research paper *"Myopia: Why the Retina Stops Inhibiting Eye Growth"* (Sci. Reports 12:21704, 2022).

---

## 原理 / Principle

人眼不同波长的光聚焦位置不同（色差）：红光偏近视网膜前，蓝光偏后。本程序利用这一特性，对屏幕 RGB 三个通道施加不同程度的高斯模糊，在副屏上重建一个"离焦"画面——视网膜感知到的色差信号与真实光学离焦等效，从而可能抑制眼轴异常增长。

不同波长的等效离焦量（以 570nm 为基准）：

| 通道 | 波长 | 等效离焦 |
|------|------|----------|
| R (红) | 630nm | −0.23D |
| G (绿) | 538nm | +0.24D |
| B (蓝) | 450nm | +1.10D |

---

## 功能 / Features

- **近视离焦模式**：红通道清晰，绿/蓝通道模糊（模拟红光聚焦、蓝光离焦）
- **远视离焦模式**：蓝通道清晰，红/绿通道模糊（模拟蓝光聚焦、红光离焦）
- **自定义模式**：独立调节 R/G/B 三通道模糊半径
- **眼距参数**：设定眼睛到屏幕的距离（mm），影响模糊圈的实际呈现大小
- **帧率控制**：30/50/60/25 fps 可选，适配不同显卡性能
- **多屏支持**：自动识别所有显示器，自由选择源屏与目标屏
- **全屏/窗口**：支持全屏和窗口两种显示模式
- **托盘驻留**：最小化到系统托盘，不占用任务栏
- **单实例**：同一时间只允许运行一个实例
- **GPU 加速**：基于 SharpDX + Direct3D 11 的像素着色器实时渲染

---

## 环境要求 / Requirements

- Windows 10/11
- .NET 10 SDK（或更高版本）
- 支持 DirectX 11 的显卡
- 两台显示器（推荐）

---

## 构建 / Build

```bash
dotnet build DefocusMirror.sln
```

---

## 使用 / Usage

1. 运行 `DefocusMirror.exe`
2. 在工具栏选择**源屏幕**（捕捉）和**目标屏幕**（离焦输出）
3. 设置**眼距**（默认 700mm，即正常阅读距离）
4. 点击「近视离焦」或「远视离焦」按钮，或在托盘图标右键菜单启动
5. 目标屏将显示带模糊的镜像画面
6. 关闭控制面板会隐藏到托盘，托盘图标右键可退出

---

## 技术栈 / Tech Stack

- C# 13 / .NET 10 / WinForms
- SharpDX 4.2.0（Direct3D 11 / DXGI / D3DCompiler）
- HLSL 像素着色器（7×7 二维高斯模糊，49 次采样）
- Win32 P/Invoke（SetWindowPos 等）

---

## 参考文献 / References

1. Swiatczak, B. & Schaeffel, F. "Myopia: why the retina stops inhibiting eye growth." *Scientific Reports* 12, 21704 (2022). https://doi.org/10.1038/s41598-022-26323-7
2. Atchison, D.A. & Smith, G. *Optics of the Human Eye*. Butterworth-Heinemann, 2000.（LCA 参考值）

`DefocusMirror/paper.pdf` 是第 1 篇论文的副本，依 [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/) 许可分发，著作权归原作者所有。 / A copy of the first paper, distributed under CC BY 4.0, copyright remains with the original authors.

---

## 许可证 / License

本项目仅供学术研究和个人使用，请勿用于临床诊断或治疗。使用本工具产生的任何后果由使用者自行承担。
