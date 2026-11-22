
namespace autorecoil.Sources.Forms
{
    partial class AutoRecoil
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
        	this.lbl_moveX = new System.Windows.Forms.Label();
        	this.tb_moveX = new System.Windows.Forms.TrackBar();
        	this.lbl_moveXValue = new System.Windows.Forms.Label();
        	this.lbl_moveY = new System.Windows.Forms.Label();
        	this.tb_moveY = new System.Windows.Forms.TrackBar();
        	this.lbl_moveYValue = new System.Windows.Forms.Label();
        	this.lbl_relay = new System.Windows.Forms.Label();
        	this.tb_relay = new System.Windows.Forms.TrackBar();
        	this.lbl_relayValue = new System.Windows.Forms.Label();
        	this.lbl_speed = new System.Windows.Forms.Label();
        	this.tb_speed = new System.Windows.Forms.TrackBar();
        	this.lbl_speedValue = new System.Windows.Forms.Label();
        	this.lbl_shoot = new System.Windows.Forms.Label();
        	this.tb_shoot = new System.Windows.Forms.TrackBar();
        	this.lbl_shootValue = new System.Windows.Forms.Label();
        	this.chk_autoShoot = new System.Windows.Forms.CheckBox();
        	this.chk_moveSpeed = new System.Windows.Forms.CheckBox();
        	this.chk_sendKill = new System.Windows.Forms.CheckBox();
        	this.grp_inputType = new System.Windows.Forms.GroupBox();
        	this.rdo_leftMouse = new System.Windows.Forms.RadioButton();
        	this.rdo_rightMouse = new System.Windows.Forms.RadioButton();
        	this.grp_inputMode = new System.Windows.Forms.GroupBox();
        	this.rdo_sendMessage = new System.Windows.Forms.RadioButton();
        	this.rdo_postMessage = new System.Windows.Forms.RadioButton();
        	this.rdo_event = new System.Windows.Forms.RadioButton();
        	this.grp_inputInfo = new System.Windows.Forms.GroupBox();
        	this.lbl_hWnd = new System.Windows.Forms.Label();
        	this.lbl_hWndValue = new System.Windows.Forms.Label();
        	this.lbl_title = new System.Windows.Forms.Label();
        	this.txt_title = new System.Windows.Forms.TextBox();
        	this.btn_f = new System.Windows.Forms.Button();
        	this.btn_c = new System.Windows.Forms.Button();
        	((System.ComponentModel.ISupportInitialize)(this.tb_moveX)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.tb_moveY)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.tb_relay)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.tb_speed)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.tb_shoot)).BeginInit();
        	this.grp_inputType.SuspendLayout();
        	this.grp_inputMode.SuspendLayout();
        	this.grp_inputInfo.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// lbl_moveX
        	// 
        	this.lbl_moveX.AutoSize = true;
        	this.lbl_moveX.Location = new System.Drawing.Point(8, 14);
        	this.lbl_moveX.Name = "lbl_moveX";
        	this.lbl_moveX.Size = new System.Drawing.Size(49, 14);
        	this.lbl_moveX.TabIndex = 0;
        	this.lbl_moveX.Text = "MoveX:";
        	// 
        	// tb_moveX
        	// 
        	this.tb_moveX.LargeChange = 2;
        	this.tb_moveX.Location = new System.Drawing.Point(58, 6);
        	this.tb_moveX.Maximum = 20;
        	this.tb_moveX.Name = "tb_moveX";
        	this.tb_moveX.Size = new System.Drawing.Size(210, 45);
        	this.tb_moveX.TabIndex = 1;
        	this.tb_moveX.TickStyle = System.Windows.Forms.TickStyle.Both;
        	this.tb_moveX.Value = 10;
        	// 
        	// lbl_moveXValue
        	// 
        	this.lbl_moveXValue.Location = new System.Drawing.Point(272, 14);
        	this.lbl_moveXValue.Name = "lbl_moveXValue";
        	this.lbl_moveXValue.Size = new System.Drawing.Size(34, 14);
        	this.lbl_moveXValue.TabIndex = 2;
        	this.lbl_moveXValue.Text = "0";
        	this.lbl_moveXValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	// 
        	// lbl_moveY
        	// 
        	this.lbl_moveY.AutoSize = true;
        	this.lbl_moveY.Location = new System.Drawing.Point(8, 50);
        	this.lbl_moveY.Name = "lbl_moveY";
        	this.lbl_moveY.Size = new System.Drawing.Size(49, 14);
        	this.lbl_moveY.TabIndex = 3;
        	this.lbl_moveY.Text = "MoveY:";
        	// 
        	// tb_moveY
        	// 
        	this.tb_moveY.LargeChange = 2;
        	this.tb_moveY.Location = new System.Drawing.Point(58, 42);
        	this.tb_moveY.Name = "tb_moveY";
        	this.tb_moveY.Size = new System.Drawing.Size(210, 45);
        	this.tb_moveY.TabIndex = 4;
        	this.tb_moveY.TickStyle = System.Windows.Forms.TickStyle.Both;
        	this.tb_moveY.Value = 4;
        	// 
        	// lbl_moveYValue
        	// 
        	this.lbl_moveYValue.Location = new System.Drawing.Point(272, 50);
        	this.lbl_moveYValue.Name = "lbl_moveYValue";
        	this.lbl_moveYValue.Size = new System.Drawing.Size(34, 14);
        	this.lbl_moveYValue.TabIndex = 5;
        	this.lbl_moveYValue.Text = "0";
        	this.lbl_moveYValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	// 
        	// lbl_relay
        	// 
        	this.lbl_relay.AutoSize = true;
        	this.lbl_relay.Location = new System.Drawing.Point(8, 86);
        	this.lbl_relay.Name = "lbl_relay";
        	this.lbl_relay.Size = new System.Drawing.Size(49, 14);
        	this.lbl_relay.TabIndex = 6;
        	this.lbl_relay.Text = "RelaY:";
        	// 
        	// tb_relay
        	// 
        	this.tb_relay.LargeChange = 2;
        	this.tb_relay.Location = new System.Drawing.Point(58, 78);
        	this.tb_relay.Name = "tb_relay";
        	this.tb_relay.Size = new System.Drawing.Size(210, 45);
        	this.tb_relay.TabIndex = 7;
        	this.tb_relay.TickStyle = System.Windows.Forms.TickStyle.Both;
        	this.tb_relay.Value = 10;
        	// 
        	// lbl_relayValue
        	// 
        	this.lbl_relayValue.Location = new System.Drawing.Point(272, 86);
        	this.lbl_relayValue.Name = "lbl_relayValue";
        	this.lbl_relayValue.Size = new System.Drawing.Size(34, 14);
        	this.lbl_relayValue.TabIndex = 8;
        	this.lbl_relayValue.Text = "0";
        	this.lbl_relayValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	// 
        	// lbl_speed
        	// 
        	this.lbl_speed.AutoSize = true;
        	this.lbl_speed.Location = new System.Drawing.Point(8, 122);
        	this.lbl_speed.Name = "lbl_speed";
        	this.lbl_speed.Size = new System.Drawing.Size(49, 14);
        	this.lbl_speed.TabIndex = 9;
        	this.lbl_speed.Text = "Speed:";
        	// 
        	// tb_speed
        	// 
        	this.tb_speed.LargeChange = 1;
        	this.tb_speed.Location = new System.Drawing.Point(58, 114);
        	this.tb_speed.Maximum = 20;
        	this.tb_speed.Minimum = 1;
        	this.tb_speed.Name = "tb_speed";
        	this.tb_speed.Size = new System.Drawing.Size(210, 45);
        	this.tb_speed.TabIndex = 10;
        	this.tb_speed.TickStyle = System.Windows.Forms.TickStyle.Both;
        	this.tb_speed.Value = 1;
        	// 
        	// lbl_speedValue
        	// 
        	this.lbl_speedValue.Location = new System.Drawing.Point(272, 122);
        	this.lbl_speedValue.Name = "lbl_speedValue";
        	this.lbl_speedValue.Size = new System.Drawing.Size(34, 14);
        	this.lbl_speedValue.TabIndex = 11;
        	this.lbl_speedValue.Text = "0";
        	this.lbl_speedValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	// 
        	// lbl_shoot
        	// 
        	this.lbl_shoot.AutoSize = true;
        	this.lbl_shoot.Location = new System.Drawing.Point(8, 158);
        	this.lbl_shoot.Name = "lbl_shoot";
        	this.lbl_shoot.Size = new System.Drawing.Size(49, 14);
        	this.lbl_shoot.TabIndex = 12;
        	this.lbl_shoot.Text = "Shoot:";
        	// 
        	// tb_shoot
        	// 
        	this.tb_shoot.LargeChange = 1;
        	this.tb_shoot.Location = new System.Drawing.Point(58, 150);
        	this.tb_shoot.Maximum = 100;
        	this.tb_shoot.Minimum = 1;
        	this.tb_shoot.Name = "tb_shoot";
        	this.tb_shoot.Size = new System.Drawing.Size(210, 45);
        	this.tb_shoot.TabIndex = 13;
        	this.tb_shoot.TickFrequency = 10;
        	this.tb_shoot.TickStyle = System.Windows.Forms.TickStyle.Both;
        	this.tb_shoot.Value = 1;
        	// 
        	// lbl_shootValue
        	// 
        	this.lbl_shootValue.Location = new System.Drawing.Point(272, 158);
        	this.lbl_shootValue.Name = "lbl_shootValue";
        	this.lbl_shootValue.Size = new System.Drawing.Size(34, 14);
        	this.lbl_shootValue.TabIndex = 14;
        	this.lbl_shootValue.Text = "0";
        	this.lbl_shootValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	// 
        	// chk_autoShoot
        	// 
        	this.chk_autoShoot.AutoSize = true;
        	this.chk_autoShoot.Location = new System.Drawing.Point(8, 196);
        	this.chk_autoShoot.Name = "chk_autoShoot";
        	this.chk_autoShoot.Size = new System.Drawing.Size(96, 18);
        	this.chk_autoShoot.TabIndex = 15;
        	this.chk_autoShoot.Text = "Auto Shoot";
        	// 
        	// chk_moveSpeed
        	// 
        	this.chk_moveSpeed.AutoSize = true;
        	this.chk_moveSpeed.Location = new System.Drawing.Point(108, 196);
        	this.chk_moveSpeed.Name = "chk_moveSpeed";
        	this.chk_moveSpeed.Size = new System.Drawing.Size(96, 18);
        	this.chk_moveSpeed.TabIndex = 16;
        	this.chk_moveSpeed.Text = "Move Speed";
        	// 
        	// chk_sendKill
        	// 
        	this.chk_sendKill.AutoSize = true;
        	this.chk_sendKill.Location = new System.Drawing.Point(208, 196);
        	this.chk_sendKill.Name = "chk_sendKill";
        	this.chk_sendKill.Size = new System.Drawing.Size(89, 18);
        	this.chk_sendKill.TabIndex = 17;
        	this.chk_sendKill.Text = "Send Kill";
        	// 
        	// grp_inputType
        	// 
        	this.grp_inputType.Controls.Add(this.rdo_leftMouse);
        	this.grp_inputType.Controls.Add(this.rdo_rightMouse);
        	this.grp_inputType.Location = new System.Drawing.Point(8, 222);
        	this.grp_inputType.Name = "grp_inputType";
        	this.grp_inputType.Size = new System.Drawing.Size(298, 56);
        	this.grp_inputType.TabIndex = 18;
        	this.grp_inputType.TabStop = false;
        	this.grp_inputType.Text = "Input Type";
        	// 
        	// rdo_leftMouse
        	// 
        	this.rdo_leftMouse.AutoSize = true;
        	this.rdo_leftMouse.Checked = true;
        	this.rdo_leftMouse.Location = new System.Drawing.Point(25, 24);
        	this.rdo_leftMouse.Name = "rdo_leftMouse";
        	this.rdo_leftMouse.Size = new System.Drawing.Size(95, 18);
        	this.rdo_leftMouse.TabIndex = 0;
        	this.rdo_leftMouse.TabStop = true;
        	this.rdo_leftMouse.Text = "Left Mouse";
        	// 
        	// rdo_rightMouse
        	// 
        	this.rdo_rightMouse.AutoSize = true;
        	this.rdo_rightMouse.Location = new System.Drawing.Point(150, 24);
        	this.rdo_rightMouse.Name = "rdo_rightMouse";
        	this.rdo_rightMouse.Size = new System.Drawing.Size(102, 18);
        	this.rdo_rightMouse.TabIndex = 1;
        	this.rdo_rightMouse.Text = "Right Mouse";
        	// 
        	// grp_inputMode
        	// 
        	this.grp_inputMode.Controls.Add(this.rdo_sendMessage);
        	this.grp_inputMode.Controls.Add(this.rdo_postMessage);
        	this.grp_inputMode.Controls.Add(this.rdo_event);
        	this.grp_inputMode.Location = new System.Drawing.Point(8, 284);
        	this.grp_inputMode.Name = "grp_inputMode";
        	this.grp_inputMode.Size = new System.Drawing.Size(298, 56);
        	this.grp_inputMode.TabIndex = 0;
        	this.grp_inputMode.TabStop = false;
        	this.grp_inputMode.Text = "Input Mode";
        	// 
        	// rdo_sendMessage
        	// 
        	this.rdo_sendMessage.AutoSize = true;
        	this.rdo_sendMessage.Location = new System.Drawing.Point(10, 24);
        	this.rdo_sendMessage.Name = "rdo_sendMessage";
        	this.rdo_sendMessage.Size = new System.Drawing.Size(102, 18);
        	this.rdo_sendMessage.TabIndex = 0;
        	this.rdo_sendMessage.Text = "SendMessage";
        	// 
        	// rdo_postMessage
        	// 
        	this.rdo_postMessage.AutoSize = true;
        	this.rdo_postMessage.Checked = true;
        	this.rdo_postMessage.Location = new System.Drawing.Point(118, 24);
        	this.rdo_postMessage.Name = "rdo_postMessage";
        	this.rdo_postMessage.Size = new System.Drawing.Size(102, 18);
        	this.rdo_postMessage.TabIndex = 1;
        	this.rdo_postMessage.TabStop = true;
        	this.rdo_postMessage.Text = "PostMessage";
        	// 
        	// rdo_event
        	// 
        	this.rdo_event.AutoSize = true;
        	this.rdo_event.Location = new System.Drawing.Point(222, 24);
        	this.rdo_event.Name = "rdo_event";
        	this.rdo_event.Size = new System.Drawing.Size(67, 18);
        	this.rdo_event.TabIndex = 2;
        	this.rdo_event.Text = "SInput";
        	// 
        	// grp_inputInfo
        	// 
        	this.grp_inputInfo.Controls.Add(this.lbl_hWnd);
        	this.grp_inputInfo.Controls.Add(this.lbl_hWndValue);
        	this.grp_inputInfo.Controls.Add(this.lbl_title);
        	this.grp_inputInfo.Controls.Add(this.txt_title);
        	this.grp_inputInfo.Controls.Add(this.btn_f);
        	this.grp_inputInfo.Controls.Add(this.btn_c);
        	this.grp_inputInfo.Location = new System.Drawing.Point(8, 346);
        	this.grp_inputInfo.Name = "grp_inputInfo";
        	this.grp_inputInfo.Size = new System.Drawing.Size(298, 56);
        	this.grp_inputInfo.TabIndex = 20;
        	this.grp_inputInfo.TabStop = false;
        	this.grp_inputInfo.Text = "Input Info";
        	// 
        	// lbl_hWnd
        	// 
        	this.lbl_hWnd.AutoSize = true;
        	this.lbl_hWnd.Location = new System.Drawing.Point(12, 24);
        	this.lbl_hWnd.Name = "lbl_hWnd";
        	this.lbl_hWnd.Size = new System.Drawing.Size(42, 14);
        	this.lbl_hWnd.TabIndex = 0;
        	this.lbl_hWnd.Text = "hWnd:";
        	// 
        	// lbl_hWndValue
        	// 
        	this.lbl_hWndValue.AutoSize = true;
        	this.lbl_hWndValue.Location = new System.Drawing.Point(50, 24);
        	this.lbl_hWndValue.Name = "lbl_hWndValue";
        	this.lbl_hWndValue.Size = new System.Drawing.Size(63, 14);
        	this.lbl_hWndValue.TabIndex = 1;
        	this.lbl_hWndValue.Text = "0x114514";
        	// 
        	// lbl_title
        	// 
        	this.lbl_title.AutoSize = true;
        	this.lbl_title.Location = new System.Drawing.Point(112, 24);
        	this.lbl_title.Name = "lbl_title";
        	this.lbl_title.Size = new System.Drawing.Size(49, 14);
        	this.lbl_title.TabIndex = 2;
        	this.lbl_title.Text = "TITLE:";
        	// 
        	// txt_title
        	// 
        	this.txt_title.Location = new System.Drawing.Point(160, 22);
        	this.txt_title.Name = "txt_title";
        	this.txt_title.Size = new System.Drawing.Size(76, 20);
        	this.txt_title.TabIndex = 3;
        	this.txt_title.Text = "Insurgency";
        	// 
        	// btn_f
        	// 
        	this.btn_f.FlatStyle = System.Windows.Forms.FlatStyle.System;
        	this.btn_f.Location = new System.Drawing.Point(242, 20);
        	this.btn_f.Name = "btn_f";
        	this.btn_f.Size = new System.Drawing.Size(22, 24);
        	this.btn_f.TabIndex = 4;
        	this.btn_f.Text = "F";
        	this.btn_f.UseVisualStyleBackColor = true;
        	// 
        	// btn_c
        	// 
        	this.btn_c.FlatStyle = System.Windows.Forms.FlatStyle.System;
        	this.btn_c.Location = new System.Drawing.Point(270, 20);
        	this.btn_c.Name = "btn_c";
        	this.btn_c.Size = new System.Drawing.Size(22, 24);
        	this.btn_c.TabIndex = 5;
        	this.btn_c.Text = "C";
        	this.btn_c.UseVisualStyleBackColor = true;
        	// 
        	// AutoRecoil
        	// 
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
        	this.ClientSize = new System.Drawing.Size(314, 418);
        	this.Controls.Add(this.lbl_moveX);
        	this.Controls.Add(this.tb_moveX);
        	this.Controls.Add(this.lbl_moveXValue);
        	this.Controls.Add(this.lbl_moveY);
        	this.Controls.Add(this.tb_moveY);
        	this.Controls.Add(this.lbl_moveYValue);
        	this.Controls.Add(this.lbl_relay);
        	this.Controls.Add(this.tb_relay);
        	this.Controls.Add(this.lbl_relayValue);
        	this.Controls.Add(this.lbl_speed);
        	this.Controls.Add(this.tb_speed);
        	this.Controls.Add(this.lbl_speedValue);
        	this.Controls.Add(this.lbl_shoot);
        	this.Controls.Add(this.tb_shoot);
        	this.Controls.Add(this.lbl_shootValue);
        	this.Controls.Add(this.chk_autoShoot);
        	this.Controls.Add(this.chk_moveSpeed);
        	this.Controls.Add(this.chk_sendKill);
        	this.Controls.Add(this.grp_inputType);
        	this.Controls.Add(this.grp_inputMode);
        	this.Controls.Add(this.grp_inputInfo);
        	this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        	this.Name = "AutoRecoil";
        	this.Text = "Auto Recoil";
        	((System.ComponentModel.ISupportInitialize)(this.tb_moveX)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.tb_moveY)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.tb_relay)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.tb_speed)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.tb_shoot)).EndInit();
        	this.grp_inputType.ResumeLayout(false);
        	this.grp_inputType.PerformLayout();
        	this.grp_inputMode.ResumeLayout(false);
        	this.grp_inputMode.PerformLayout();
        	this.grp_inputInfo.ResumeLayout(false);
        	this.grp_inputInfo.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();

        }

        private System.Windows.Forms.Label lbl_moveX;
        private System.Windows.Forms.TrackBar tb_moveX;
        private System.Windows.Forms.Label lbl_moveXValue;
        private System.Windows.Forms.Label lbl_moveY;
        private System.Windows.Forms.TrackBar tb_moveY;
        private System.Windows.Forms.Label lbl_moveYValue;
        private System.Windows.Forms.Label lbl_relay;
        private System.Windows.Forms.TrackBar tb_relay;
        private System.Windows.Forms.Label lbl_relayValue;
        private System.Windows.Forms.Label lbl_speed;
        private System.Windows.Forms.TrackBar tb_speed;
        private System.Windows.Forms.Label lbl_speedValue;
        private System.Windows.Forms.Label lbl_shoot;
        private System.Windows.Forms.TrackBar tb_shoot;
        private System.Windows.Forms.Label lbl_shootValue;
        private System.Windows.Forms.CheckBox chk_autoShoot;
        private System.Windows.Forms.CheckBox chk_moveSpeed;
        private System.Windows.Forms.CheckBox chk_sendKill;
        private System.Windows.Forms.GroupBox grp_inputType;
        private System.Windows.Forms.RadioButton rdo_leftMouse;
        private System.Windows.Forms.RadioButton rdo_rightMouse;
        private System.Windows.Forms.GroupBox grp_inputMode;
        private System.Windows.Forms.RadioButton rdo_sendMessage;
        private System.Windows.Forms.RadioButton rdo_postMessage;
        private System.Windows.Forms.RadioButton rdo_event;
        private System.Windows.Forms.GroupBox grp_inputInfo;
        private System.Windows.Forms.Label lbl_hWnd;
        private System.Windows.Forms.Label lbl_hWndValue;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.TextBox txt_title;
        private System.Windows.Forms.Button btn_f;
        private System.Windows.Forms.Button btn_c;
    }
}
