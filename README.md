# AutoRecoil

Windows 桌面端的压枪 / 连点辅助小工具，基于 C# WinForms 与 Win32 P/Invoke 实现。通过后台线程轮询鼠标按键状态，在你按住开火键时以 `SendInput` 注入相对鼠标移动来抵消枪口上跳；同时提供连点、准星悬浮窗、按键宏等功能，所有参数均可实时调节。

> 本项目主要用于学习 Win32 输入模拟（SendInput / PostMessage / SendMessage）、WinForms 多线程模型等桌面开发技术。**请勿在在线游戏中使用**，详见文末[免责声明](#免责声明)。

## 功能特性

- **压枪（Recoil Compensation）**：按住开火键时，每个循环周期向下移动鼠标抵消后座；首发可叠加额外下压量
- **连点（Auto Shoot）**：按住开火键时自动连发，支持三种注入方式（SendInput / SendMessage / PostMessage）
- **按键宏（Send Kill）**：按 `~` 键自动打出 `kill` + 回车（文本可在代码中修改）
- **准星悬浮窗（CrossForm）**：透明背景红点准星，可一键贴到目标窗口中心
- **窗口查找**：按窗口标题关键字枚举顶层窗口，找到目标句柄
- **全局热键**：CapsLock 系列组合键实时切换各功能开关，无需切回主界面
- **实时调参**：全部参数通过滑块调节，后台线程即时生效

## 环境要求

| 项目 | 要求 |
| --- | --- |
| 操作系统 | Windows 7 SP1 及以上（建议 Windows 10/11） |
| 运行时 | .NET Framework 4.8 |
| 构建 | 无需额外安装：仓库自带 `build.cmd`，使用 Windows 系统自带的 MSBuild；也可用 Visual Studio 2019/2022 或 SharpDevelop 5.1 |

## 构建

### 方式一：一键构建脚本（推荐，无需安装任何 IDE/SDK）

仓库根目录的 [build.cmd](build.cmd) 使用 Windows 系统自带的 .NET Framework MSBuild，一键编译出**双架构全部版本**：

```bash
build.cmd
```

产物：

| 架构 | Debug | Release |
| --- | --- | --- |
| x86（I386） | `autorecoil\bin\x86\Debug\autorecoil.exe` | `autorecoil\bin\x86\Release\autorecoil.exe` |
| x64（AMD64） | `autorecoil\bin\x64\Debug\autorecoil.exe` | `autorecoil\bin\x64\Release\autorecoil.exe` |

脚本要点（详见 [docs/architecture.md](docs/architecture.md) 构建系统一节）：

- 架构通过命令行属性覆盖传入，**不改动工程文件**，IDE 里默认的 AnyCPU 配置保持原样
- 通过 `FrameworkPathOverride` 直接引用系统运行时程序集，未安装 .NET Developer Pack 的机器也能编译
- 系统 MSBuild 缺失时自动回退到 Visual Studio 自带的 MSBuild（vswhere 查找）
- 脚本需保存为 ANSI/GBK 编码，cmd 才能正确解析中文注释

### 方式二：Visual Studio / SharpDevelop

直接打开 `autorecoil.sln` 选择配置生成，产物位于 `autorecoil/bin/Release/autorecoil.exe`（AnyCPU，x64 系统上以 64 位进程运行）；`SendInput` / `GetAsyncKeyState` 的行为与目标程序位数无关。

## 快速上手

1. 运行 `autorecoil.exe`，主窗口标题为 **Auto Recoil**
2. 在 `Input Info` 的 `TITLE` 框输入目标窗口标题关键字（默认 `Insurgency`），点击 **F** 搜索，命中后显示窗口句柄（蓝色）
3. 点击 **C** 将准星悬浮窗贴到目标窗口中心
4. 按 `CapsLock` 开启 **Move Speed**（压枪总开关），或直接在界面勾选
5. 按住开火键（默认鼠标左键），工具自动向下移动鼠标压枪
6. 需要连发时按 `Shift+CapsLock` 开启 **Auto Shoot**

## 参数与控件

### 滑块

| 控件 | 范围 | 默认 | 含义 |
| --- | --- | --- | --- |
| MoveX | 0–20 | 10 | 左右偏移，以 10 为中心：`<10` 偏左，`>10` 偏右 |
| MoveY | 0–10 | 4 | 每 tick 向下移动的像素 |
| RelaY | 0–10 | 10 | 首次按下时额外下压的像素（与 MoveY 叠加） |
| Speed | 1–20 | 1 | 工作线程循环间隔（毫秒） |
| Shoot | 1–100 | 1 | 压枪移动的细分次数（越大越平滑）；同时是 Auto Shoot 的连点间隔（毫秒） |

### 开关与选项

| 控件 | 含义 |
| --- | --- |
| Auto Shoot | 按住触发键时自动连发 |
| Move Speed | 压枪总开关 |
| Send Kill | 启用后按 `~` 发送 kill 宏 |
| Input Type | 触发键选择：Left Mouse / Right Mouse |
| Input Mode | 注入方式：SendMessage / PostMessage（默认）/ SInput（SendInput） |

### 快捷键

| 按键 | 功能 |
| --- | --- |
| `CapsLock` | 切换 Move Speed（压枪） |
| `Shift+CapsLock` | 切换 Auto Shoot（连点） |
| `Alt+CapsLock` | 切换 Send Kill |
| `~` | Send Kill 开启时发送 kill 宏 |

> 含 `Ctrl` 的 CapsLock 组合键不会触发任何切换；所有热键均为按下沿触发，不会连续翻转。

### 按钮

| 按钮 | 功能 |
| --- | --- |
| F | 后台线程按标题关键字查找目标窗口，句柄显示在 hWnd（失败显示红色 failed!） |
| C | 将准星悬浮窗对准目标窗口中心（未找到目标时对准主窗口中心），再按切换显示/隐藏 |

## 输入模式怎么选

| 模式 | 原理 | 适用场景 |
| --- | --- | --- |
| SInput (SendInput) | 向系统输入队列注入合成事件 | **首选**。全局生效，多数游戏（包括读 Raw Input 的）都能收到 |
| PostMessage | 向目标窗口投递鼠标消息（非阻塞） | 普通窗口程序；需先用 F 找到窗口句柄 |
| SendMessage | 向目标窗口投递鼠标消息（阻塞至处理完） | 同上 |

> 压枪移动恒用 SendInput（窗口消息对鼠标移动无效）；Send/Post 仅用于 Auto Shoot 的连点与后台窗口点击。

## 运行日志

启用诊断时会在 exe 同目录生成 `recoil-diag.log`，记录输入注入链路（当前为 TEMP-DIAG 调试代码，发布版本建议移除，见 [docs/architecture.md](docs/architecture.md)）。

## 项目结构

```
autorecoil/
├─ autorecoil.sln
├─ build.cmd                  # 一键构建脚本（x86/x64 × Debug/Release）
├─ autorecoil/
│  ├─ app.config
│  ├─ autorecoil.csproj
│  └─ Sources/
│     ├─ Program.cs            # 入口
│     ├─ Forms/
│     │  ├─ MainForm.cs        # 主窗体 + RecoilWorker 后台线程（核心逻辑）
│     │  └─ CrossForm.cs       # 准星悬浮窗（透明底 + 红点）
│     ├─ Interop/
│     │  ├─ NativeMethods.cs   # P/Invoke 原始声明
│     │  ├─ NativeConstants.cs # 虚拟键码 / 窗口消息 / 输入标志常量
│     │  └─ NativeStructs.cs   # INPUT / RECT / POINT 结构（含 x64 对齐说明）
│     └─ Libraries/
│        ├─ InputLibrary.cs    # 输入模拟高层封装
│        └─ WindowLibrary.cs   # 窗口查找封装
└─ docs/
   ├─ usage.md                 # 详细使用说明与常见问题
   └─ architecture.md          # 架构设计与实现原理
```

## 已知限制

- 深度集成 Raw Input / 内核级驱动的游戏可能忽略或检测合成输入，此类目标不受支持
- 目标程序以管理员权限运行时，本工具也必须以管理员运行（UIPI 限制），否则注入无效
- C 按钮居中准星时对窗口矩形使用硬编码偏移（`+26/+12`），不同 DPI / 窗口边框下可能不准
- kill 宏文本硬编码在 `MainForm.SendKillKey`，需要改文本请修改源码
- 无配置持久化，重启后参数回到默认值

## 免责声明

本项目仅供学习与研究 Win32 输入模拟、WinForms 多线程等桌面开发技术使用。在在线游戏中使用任何形式的辅助工具通常违反其用户协议，可能导致账号封禁等后果；请在单机 / 离线环境或自建测试程序中验证功能。使用本项目产生的一切后果由使用者自行承担，作者不对任何滥用行为负责。

## License

暂未指定，正式发布前请补充开源许可证（如 MIT / Apache-2.0）。
