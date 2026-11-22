@echo off
setlocal EnableExtensions
cd /d "%~dp0"

rem ============================================================
rem  autorecoil 一键构建脚本
rem
rem  仅使用 Windows 系统自带的 .NET Framework MSBuild 编译，
rem  无需安装 Visual Studio / Build Tools / Developer Pack。
rem
rem  产物：x86 / x64 两个架构 x Debug / Release 两种配置 = 4 个版本
rem    autorecoil\bin\x86\Debug\autorecoil.exe
rem    autorecoil\bin\x86\Release\autorecoil.exe
rem    autorecoil\bin\x64\Debug\autorecoil.exe
rem    autorecoil\bin\x64\Release\autorecoil.exe
rem
rem  说明：
rem   1. 架构通过命令行属性覆盖传入（Platform / PlatformTarget /
rem      OutputPath / IntermediateOutputPath），不改动工程文件，
rem      IDE 里默认的 AnyCPU 配置保持原样。
rem   2. FrameworkPathOverride 指向系统运行时目录：多数机器未安装
rem      .NET Framework Developer Pack（无引用程序集），直接引用
rem      运行时程序集即可编译，系统运行时与目标框架同为 4.8。
rem   3. 本文件需保存为 ANSI/GBK 编码，cmd 才能正确解析中文。
rem ============================================================

set "PROJECT=autorecoil\autorecoil.csproj"
set "MSBUILD="
set "FRAMEWORKDIR="

rem --- 1) 系统自带 MSBuild：Windows 自带 .NET Framework 4.x ---
if exist "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" (
    set "MSBUILD=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
    set "FRAMEWORKDIR=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319"
)
if not defined MSBUILD if exist "%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" (
    set "MSBUILD=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
    set "FRAMEWORKDIR=%WINDIR%\Microsoft.NET\Framework\v4.0.30319"
)

rem --- 2) 兜底：系统 MSBuild 缺失时找 Visual Studio 自带的 ---
if not defined MSBUILD if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
    for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set "MSBUILD=%%i"
)

if not defined MSBUILD (
    echo [ERROR] 未找到 MSBuild。系统缺少自带 MSBuild 且未安装 Visual Studio。
    echo         请先安装 .NET Framework 4.x 运行时，Windows 10/11 系统自带。
    exit /b 1
)

if not defined FRAMEWORKDIR set "FRAMEWORKDIR=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319"

echo [INFO] MSBuild: %MSBUILD%
echo [INFO] FrameworkPathOverride: %FRAMEWORKDIR%
echo.

set "FAILED=0"

for %%C in (Debug Release) do for %%P in (x86 x64) do (
    echo ====== 构建 %%P / %%C ======
    "%MSBUILD%" "%PROJECT%" /nologo /verbosity:minimal /p:Configuration=%%C /p:Platform=%%P /p:PlatformTarget=%%P /p:OutputPath=bin\%%P\%%C\ /p:IntermediateOutputPath=obj\%%P\%%C\ /p:FrameworkPathOverride=%FRAMEWORKDIR%
    if errorlevel 1 (
        echo [FAIL] %%P / %%C 构建失败，请查看上方日志。
        set "FAILED=1"
    )
    echo.
)

if "%FAILED%"=="1" (
    echo [ERROR] 存在失败的构建目标，未完成全部版本编译。
    exit /b 1
)

echo [OK] 双架构全部版本构建完成：
for %%P in (x86 x64) do for %%C in (Debug Release) do (
    if exist "autorecoil\bin\%%P\%%C\autorecoil.exe" (
        echo   autorecoil\bin\%%P\%%C\autorecoil.exe
    ) else (
        echo   [警告] 缺少 autorecoil\bin\%%P\%%C\autorecoil.exe
    )
)

endlocal
exit /b 0
