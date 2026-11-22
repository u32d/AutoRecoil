
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using autorecoil.Sources.Interop;
using autorecoil.Sources.Libraries;

namespace autorecoil.Sources.Forms
{
    public partial class AutoRecoil : Form
    {
        private CrossForm _crossForm;
        private IntPtr _targetHwnd = IntPtr.Zero;

        // 输入模式：由 grp_inputMode 三个单选框决定调用哪个 API
        private enum InputMode { PostMessage, SendMessage, SendInput }

        // 以下为后台工作者线程使用的共享配置（UI 线程写、工作线程读）
        private volatile InputMode _inputMode = InputMode.SendInput;
        private volatile int _triggerVk = NativeConstants.VK_LBUTTON; // 触发压枪的鼠标键
        private volatile int _speed = 1;   // 线程循环间隔（毫秒，Speed 滑块）
        private volatile int _shoot = 1;   // 单次移动拆分的细分次数（越大越平滑）
        private volatile int _moveX = 10;  // 左右偏位，以 10 为中心：<10 偏左 >10 偏右
        private volatile int _moveY = 4;   // 每 tick 向下移动的像素（MoveY 滑块）
        private volatile int _relay = 10;  // 首次按下额外下移像素（与 MoveY 叠加）
        private volatile bool _autoShoot = false; // Auto Shoot 勾选时自动按住触发键（鼠标按下开关）
        private volatile bool _moveSpeed = false; // Move Speed 勾选时启用左右偏位
        private bool _wasCapsDown = false;        // 热键边沿检测用的上一帧 CapsLock 状态
        private Thread _workerThread;
        private volatile bool _running = false;
        private volatile bool _sendKill = false; // Send Kill 勾选时，按 ~ 键向窗口打出 kill 指令
        private bool _diagLastTrigger = false;   // TEMP-DIAG
        private bool _diagLastMoveSpeed = false; // TEMP-DIAG

        public AutoRecoil()
        {
            InitializeComponent();
            this.Load += AutoRecoil_Load; // 主窗口加载时初始化
            this.FormClosing += AutoRecoil_FormClosing; // 关闭窗口时停止工作线程（事件绑定，替代 OnFormClosing 重写）

            this.btn_c.Click += btn_c_Click; // C 按钮绑定到切换准星窗口显示/隐藏的处理函数
            this.btn_f.Click += btn_f_Click; // F 按钮绑定到其他功能的处理函数

            // 每个滑块条各自绑定到自己的滚动处理函数，只更新自己对应的标签
            this.tb_moveX.Scroll += tb_moveX_Scroll;
            this.tb_moveY.Scroll += tb_moveY_Scroll;
            this.tb_relay.Scroll += tb_relay_Scroll;
            this.tb_speed.Scroll += tb_speed_Scroll;
            this.tb_shoot.Scroll += tb_shoot_Scroll;

            // 输入类型（左/右键）单选框 → 决定触发键
            this.rdo_leftMouse.CheckedChanged += rdo_leftMouse_CheckedChanged;
            this.rdo_rightMouse.CheckedChanged += rdo_rightMouse_CheckedChanged;
            // 输入模式（Post/Send/SInput）单选框 → 决定调用哪个 API
            this.rdo_postMessage.CheckedChanged += rdo_postMessage_CheckedChanged;
            this.rdo_sendMessage.CheckedChanged += rdo_sendMessage_CheckedChanged;
            this.rdo_event.CheckedChanged += rdo_event_CheckedChanged;

            // 两个开关的 CheckedChanged：更新文字颜色并同步给工作线程
            this.chk_autoShoot.CheckedChanged += chk_autoShoot_CheckedChanged;
            this.chk_moveSpeed.CheckedChanged += chk_moveSpeed_CheckedChanged;
            this.chk_sendKill.CheckedChanged += chk_sendKill_CheckedChanged;

        }

        #region 事件处理方法（组件绑定）

        // --- 窗体生命周期 ---
        // 主窗口加载时，创建并配置准星窗口
        private void AutoRecoil_Load(object sender, EventArgs e)
        {
            // 主窗口载入后再创建并配置准星窗口（不在按钮里创建）
            _crossForm = new CrossForm();
            _crossForm.StartPosition = FormStartPosition.Manual;
            _crossForm.TopMost = true; // 置顶，叠在目标窗口之上

            // 初始摆放在主窗口中心
            Point mainCenter = new Point(this.Left + this.Width / 2, this.Top + this.Height / 2);
            _crossForm.Location = new Point(mainCenter.X - _crossForm.Width / 2, mainCenter.Y - _crossForm.Height / 2);

            // 先 Show 应用置顶/位置等参数，再 Hide 使其初始不可见，等待 C 切换显示
            _crossForm.Show();
            _crossForm.Hide();

            // 载入时把各滑块条的初始值显示到右侧标签
            this.lbl_moveXValue.Text = this.tb_moveX.Value.ToString();
            this.lbl_moveYValue.Text = this.tb_moveY.Value.ToString();
            this.lbl_relayValue.Text = this.tb_relay.Value.ToString();
            this.lbl_speedValue.Text = this.tb_speed.Value.ToString();
            this.lbl_shootValue.Text = this.tb_shoot.Value.ToString();

            // 初始化后台线程使用的配置（与界面当前状态保持一致）
            _inputMode = this.rdo_sendMessage.Checked ? InputMode.SendMessage :
                         this.rdo_event.Checked ? InputMode.SendInput : InputMode.PostMessage;
            _triggerVk = this.rdo_leftMouse.Checked ? NativeConstants.VK_LBUTTON : NativeConstants.VK_RBUTTON;
            _speed = this.tb_speed.Value;
            _shoot = this.tb_shoot.Value;
            _moveX = this.tb_moveX.Value;
            _moveY = this.tb_moveY.Value;
            _relay = this.tb_relay.Value;
            _autoShoot = this.chk_autoShoot.Checked;
            _moveSpeed = this.chk_moveSpeed.Checked;
            _sendKill = this.chk_sendKill.Checked;

            // 启动单条后台工作者线程：轮询触发键，按住时按选定 API 做压枪
            _running = true;
            _workerThread = new Thread(RecoilWorker);
            _workerThread.IsBackground = true;
            _workerThread.Start();
            InputLibrary.Diag("AutoRecoil_Load done: worker started, moveSpeed={0} mode={1} triggerVk={2}", _moveSpeed, _inputMode, _triggerVk);
        }

        // 关闭窗口时停止工作线程（构造函数中绑定 FormClosing 事件）
        private void AutoRecoil_FormClosing(object sender, FormClosingEventArgs e)
        {
            _running = false;
            if (_workerThread != null)
            {
                _workerThread.Join(500);
            }
        }

        // --- 按钮 ---
        // C 按钮切换准星窗口显示/隐藏
        private void btn_c_Click(object sender, EventArgs e)
        {
            if (_crossForm == null || _crossForm.IsDisposed)
            {
                return;
            }

            // 默认对准主窗口中心
            Point center = new Point(this.Left + this.Width / 2, this.Top + this.Height / 2);

            // 若已成功获取目标窗口句柄，则取目标窗口矩形，用其中心来摆放准星
            if (_targetHwnd != IntPtr.Zero)
            {
                RECT rect;
                if (NativeMethods.GetWindowRect(_targetHwnd, out rect))
                {
                    center = new Point(rect.Left +26 + (rect.Right - rect.Left) / 2,
                                       rect.Top  +12 + (rect.Bottom - rect.Top) / 2);
                }
            }

            // CrossForm 是 128x128 无边框窗口，让其中心对准 center，红点即落在正中
            _crossForm.Location = new Point(center.X - _crossForm.Width / 2, center.Y - _crossForm.Height / 2);

            if (_crossForm.Visible)
            {
                _crossForm.Hide();
            }
            else
            {
                _crossForm.Show();
            }
        }

        // F 按钮查找目标窗口
        private void btn_f_Click(object sender, EventArgs e)
        {
            string keyword = (this.txt_title.Text ?? string.Empty).Trim();
            if (keyword.Length == 0)
            {
                this.lbl_hWndValue.Text = "failed!";
                return;
            }

            // 在线程外先取好关键字，再启动后台线程去查找窗口，避免卡 UI
            this.lbl_hWndValue.Text = "search...";
            string searchText = keyword;

            Thread searchThread = new Thread(() =>
            {
                // 纯查找逻辑在 TargetFinder 里，与 UI 无关，可在后台线程安全调用
                IntPtr found = WindowLibrary.FindMainWindowByTitle(searchText);
                string display = found != IntPtr.Zero ? found.ToString("X") : "failed!";

                // 跨线程更新控件，必须通过 Invoke 回到 UI 线程
                this.Invoke(new Action(() =>
                {
                    this.lbl_hWndValue.Text = display;
                    // 找到句柄显示蓝色，没找到显示红色
                    this.lbl_hWndValue.ForeColor = found != IntPtr.Zero ? Color.Blue : Color.Red;
                    // 记录最近一次搜索到的目标句柄，供 btn_c 定位准星使用
                    this._targetHwnd = found;
                }));
            });
            searchThread.IsBackground = true;
            searchThread.Start();
        }

        // --- 滑块 ---
        // 各滑块条滚动时，只刷新自己对应的右侧数值标签
        private void tb_moveX_Scroll(object sender, EventArgs e)
        {
            this.lbl_moveXValue.Text = this.tb_moveX.Value.ToString();
            _moveX = this.tb_moveX.Value; // 同步给工作线程（左右偏位，中心 10）
        }

        private void tb_moveY_Scroll(object sender, EventArgs e)
        {
            this.lbl_moveYValue.Text = this.tb_moveY.Value.ToString();
            _moveY = this.tb_moveY.Value; // 同步给工作线程（每 tick 下移像素）
        }

        private void tb_relay_Scroll(object sender, EventArgs e)
        {
            this.lbl_relayValue.Text = this.tb_relay.Value.ToString();
            _relay = this.tb_relay.Value; // 同步给工作线程（首发额外下移）
        }

        private void tb_speed_Scroll(object sender, EventArgs e)
        {
            this.lbl_speedValue.Text = this.tb_speed.Value.ToString();
            _speed = this.tb_speed.Value; // 同步给工作线程（循环延迟）
        }

        private void tb_shoot_Scroll(object sender, EventArgs e)
        {
            this.lbl_shootValue.Text = this.tb_shoot.Value.ToString();
            _shoot = this.tb_shoot.Value; // 同步给工作线程（细分次数）
        }

        // --- 单选框 ---
        // 输入类型单选框：决定触发压枪的鼠标键
        private void rdo_leftMouse_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdo_leftMouse.Checked) _triggerVk = NativeConstants.VK_LBUTTON;
        }

        private void rdo_rightMouse_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdo_rightMouse.Checked) _triggerVk = NativeConstants.VK_RBUTTON;
        }

        // 输入模式单选框：决定调用哪个 API
        private void rdo_postMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdo_postMessage.Checked) _inputMode = InputMode.PostMessage;
        }

        private void rdo_sendMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdo_sendMessage.Checked) _inputMode = InputMode.SendMessage;
        }

        private void rdo_event_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdo_event.Checked) _inputMode = InputMode.SendInput;
        }

        // --- 复选框 ---
        // Auto Shoot：勾选变红，并同步给工作线程作为"自动按住触发键"开关
        private void chk_autoShoot_CheckedChanged(object sender, EventArgs e)
        {
            this.chk_autoShoot.ForeColor = this.chk_autoShoot.Checked ? Color.Red : SystemColors.ControlText;
            _autoShoot = this.chk_autoShoot.Checked;
        }

        // Move Speed：勾选变绿，并同步给工作线程控制是否启用左右偏位
        private void chk_moveSpeed_CheckedChanged(object sender, EventArgs e)
        {
            this.chk_moveSpeed.ForeColor = this.chk_moveSpeed.Checked ? Color.Green : SystemColors.ControlText;
            _moveSpeed = this.chk_moveSpeed.Checked;
        }

        // Send Kill：勾选变橙，并同步给工作线程
        private void chk_sendKill_CheckedChanged(object sender, EventArgs e)
        {
            this.chk_sendKill.ForeColor = this.chk_sendKill.Checked ? Color.Orange : SystemColors.ControlText;
            _sendKill = this.chk_sendKill.Checked;
        }

        #endregion 事件处理方法（组件绑定）

        #region 核心逻辑（工作者线程与辅助）

        // 单条后台工作者线程：轮询触发键状态，按住时按选定 API 做向下压枪
        private void RecoilWorker()
        {
            bool wasDown = false; // 记录上一轮状态，用于区分首次按下/持续按住
            bool wasKillDown = false; // 记录 ~ 波浪键上一轮状态，用于边沿检测
            int clickAccum = 0;   // Auto Shoot 连射累加器（按 shoot 滑块定速）
            bool wasAutoFiring = false; // 上一轮是否正在连射
            while (_running)
            {
                int interval = _speed > 0 ? _speed : 1; // Speed 滑块 = 循环延迟
                Thread.Sleep(interval);

                // 热键检测：Shift+CapsLock 切换 Auto Shoot，CapsLock 切换 Move Speed
                CheckHotkeys();

                // 读取当前共享配置（volatile，UI 改了这边立刻看到）
                InputMode mode = _inputMode;
                int vk = _triggerVk;
                IntPtr hwnd = _targetHwnd;
                int moveX = _moveX;
                int moveY = _moveY;
                int relay = _relay;
                int shoot = _shoot;
                bool autoShoot = _autoShoot;
                bool moveSpeed = _moveSpeed;
                bool sendKill = _sendKill;

                // Move Speed 是压枪总开关；压枪（含首发额外量）仅在物理按住触发键时发生
                bool triggerHeld = InputLibrary.IsKeyDown(vk);
                bool isDown = moveSpeed && triggerHeld;
                if (triggerHeld != _diagLastTrigger || moveSpeed != _diagLastMoveSpeed) { // TEMP-DIAG
                    InputLibrary.Diag("state: triggerHeld={0} moveSpeed={1} mode={2} vk={3}", triggerHeld, moveSpeed, _inputMode, vk);
                    _diagLastTrigger = triggerHeld;
                    _diagLastMoveSpeed = moveSpeed;
                }

                if (isDown)
                {
                    // 左右偏位：相对中心 10 的偏移，正=右，负=左
                    int dx = moveX - 10;

                    if (!wasDown)
                    {
                        // 第一次按下：下移 MoveY + Relay（首发额外量）
                        PerformRecoil(moveY + relay, dx, shoot);
                    }
                    else
                    {
                        // 持续按住：每 tick 下移 MoveY
                        PerformRecoil(moveY, dx, shoot);
                    }
                }

                // Auto Shoot：勾选后，物理按住触发键（左/右键）即按 shoot 滑块速度持续连射。
                // 三种输入方法：SendInput（合成事件，游戏最易收）、SendMessage / PostMessage（向目标窗口投递，需有效 hwnd）。
                // firing = 勾选了 Auto Shoot 且当前物理按住触发键
                bool firing = autoShoot && triggerHeld;
                if (mode == InputMode.SendInput)
                {
                    // SendInput 模式：用"按住 + 周期探测"实现连射，避免合成事件污染 IsKeyDown 导致无法松开
                    if (firing && !wasAutoFiring)
                    {
                        InputLibrary.SetMouseButton(vk, true); // 按下，开始持续开火
                        clickAccum = 0;
                    }
                    else if (wasAutoFiring)
                    {
                        if (!autoShoot)
                        {
                            InputLibrary.SetMouseButton(vk, false); // 取消 Auto Shoot → 抬起
                        }
                        else
                        {
                            clickAccum += interval;
                            if (clickAccum >= shoot)
                            {
                                clickAccum = 0;
                                // 探测物理键是否仍按住：先抬起再读状态
                                InputLibrary.SetMouseButton(vk, false);
                                if (InputLibrary.IsKeyDown(vk))
                                {
                                    InputLibrary.SetMouseButton(vk, true); // 仍按住 → 继续（即一次点击）
                                }
                                else
                                {
                                    firing = false; // 已松开 → 停止连射
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Send/Post 模式：按 shoot 间隔向目标窗口发送一次鼠标左/右点击（down+up）；hwnd 无效则跳过
                    if (firing)
                    {
                        if (!wasAutoFiring)
                        {
                            SendMouseClick(mode, hwnd, vk);
                            clickAccum = 0;
                        }
                        else
                        {
                            clickAccum += interval;
                            if (clickAccum >= shoot)
                            {
                                clickAccum = 0;
                                SendMouseClick(mode, hwnd, vk);
                            }
                        }
                    }
                    else clickAccum = 0;
                }

                // Send Kill：开启后，按下 ~/` 波浪键向窗口发送一次 kill 按键（按选定模式）
                if (sendKill)
                {
                    bool killKeyHeld = InputLibrary.IsKeyDown(NativeConstants.VK_OEM_3);
                    if (killKeyHeld && !wasKillDown)
                    {
                        SendKillKey();
                    }
                    wasKillDown = killKeyHeld;
                }

                // isDown 的上升沿即"按下"、下降沿即"弹起"，wasDown 已记录前一状态
                wasDown = isDown;
                wasAutoFiring = firing;
            }
        }

        // 键盘热键：在工作者线程里轮询，不阻塞 UI；用上升沿避免系统按键重复多次触发
        private void CheckHotkeys()
        {
            bool capsDown = InputLibrary.IsKeyDown(NativeConstants.VK_CAPITAL);
            if (capsDown && !_wasCapsDown)
            {
                InputLibrary.Diag("caps edge: shift={0} alt={1} ctrl={2}", // TEMP-DIAG
                    InputLibrary.IsKeyDown(NativeConstants.VK_SHIFT),
                    InputLibrary.IsKeyDown(NativeConstants.VK_MENU),
                    InputLibrary.IsKeyDown(NativeConstants.VK_CONTROL));
                // 仅在 CapsLock 单键或 Shift+CapsLock 时响应，其它组合键（Alt/Ctrl）不影响本功能
                bool shift = InputLibrary.IsKeyDown(NativeConstants.VK_SHIFT);
                bool alt = InputLibrary.IsKeyDown(NativeConstants.VK_MENU);
                bool ctrl = InputLibrary.IsKeyDown(NativeConstants.VK_CONTROL);

                if (shift && !alt && !ctrl)
                {
                    // Shift + CapsLock → 切换 Auto Shoot
                    this.Invoke(new Action(() => this.chk_autoShoot.Checked = !this.chk_autoShoot.Checked));
                }
                else if (alt && !shift && !ctrl)
                {
                    // Alt + CapsLock → 切换 Send Kill
                    this.Invoke(new Action(() => this.chk_sendKill.Checked = !this.chk_sendKill.Checked));
                }
                else if (!shift && !alt && !ctrl)
                {
                    // 仅 CapsLock（无其它修饰键）→ 切换 Move Speed
                    this.Invoke(new Action(() => this.chk_moveSpeed.Checked = !this.chk_moveSpeed.Checked));
                }
                // 含 Ctrl 的组合键：不触发任何切换
            }
            _wasCapsDown = capsDown;
        }

        // 执行一次压枪移动：dy=下移像素，dx=左右像素；shoot 决定拆成几次发（越平滑），恒用 SendInput
        private void PerformRecoil(int dy, int dx, int shoot)
        {
            if (dy <= 0 && dx == 0) return;

            int subSteps = Math.Max(1, shoot / 10);
            int dyPer = dy / subSteps;
            int dyRem = dy - dyPer * subSteps;
            int dxPer = dx / subSteps;
            int dxRem = dx - dxPer * subSteps;

            for (int i = 0; i < subSteps; i++)
            {
                int sdy = dyPer + (i == 0 ? dyRem : 0);
                int sdx = dxPer + (i == 0 ? dxRem : 0);
                if (sdy == 0 && sdx == 0) break;
                SendMouseMove(sdx, sdy); // 压枪移动恒用 SendInput
            }
        }

        // 压枪移动恒用 SendInput（SendMessage/PostMessage 对移动无效）
        private void SendMouseMove(int dx, int dy)
        {
            InputLibrary.MoveMouseRelative(dx, dy);
        }

        // Send Kill：开启后，按 ~/` 波浪键向当前活动窗口打出 "kill" 并回车（SendInput，全局生效）。
        // 按游戏实际需要的处决/指令字符串修改下方文本即可。
        private void SendKillKey()
        {
            InputLibrary.TypeText("kill");                      // 打出 kill（k-i-l-l）
            InputLibrary.KeyClick(NativeConstants.VK_RETURN);   // 再打回车
        }

        // Auto Shoot（Send/Post 模式）：按选定模式向目标窗口发送一次鼠标左/右点击（down+up）。
        // 与 SendInput 模式不同，这里是"每次发一个完整点击"，适用于普通窗口（非 raw input 游戏）。
        private void SendMouseClick(InputMode mode, IntPtr hwnd, int vk)
        {
            if (hwnd == IntPtr.Zero) return; // 没有有效目标窗口时跳过
            bool post = mode == InputMode.PostMessage;
            try
            {
                InputLibrary.ClickMouseToWindow(hwnd, vk, post);
            }
            catch (Exception ex)
            {
                // 向窗口投递可能失败（窗口已销毁 / 跨会话 / 权限不足）。
                // 绝不能让后台线程抛未处理异常——否则会直接终止整个进程（闪退）。
                System.Diagnostics.Debug.WriteLine("SendMouseClick failed: " + ex.Message);
            }
        }

        #endregion 核心逻辑（工作者线程与辅助）

    }
}
