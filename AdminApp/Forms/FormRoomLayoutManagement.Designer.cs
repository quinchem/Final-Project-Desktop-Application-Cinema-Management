namespace AdminApp
{
    partial class FormRoomLayoutManagement
    {
            private System.ComponentModel.IContainer components = null;
            private System.Windows.Forms.Label lblTitle;
            private System.Windows.Forms.Label lblScreen;
            private System.Windows.Forms.Label lblLegendNormal;
            private System.Windows.Forms.Label lblLegendVIP;
            private System.Windows.Forms.Label lblLegendMaintenance;
            private System.Windows.Forms.Label lblSeatTypeTitle;
            private System.Windows.Forms.RadioButton rbNormalSeat;
            private System.Windows.Forms.RadioButton rbVIPSeat;
            private System.Windows.Forms.RadioButton rbNormalStatus;
            private System.Windows.Forms.RadioButton rbMaintenanceStatus;

            // Dictionary để lưu các button ghế
            private System.Collections.Generic.Dictionary<string, System.Windows.Forms.Button> seatButtons =
                new System.Collections.Generic.Dictionary<string, System.Windows.Forms.Button>();

            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges23 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges24 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges25 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges26 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges27 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges28 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges29 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges30 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges31 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges32 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges33 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges34 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges35 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges36 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges37 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges38 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges39 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges40 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges41 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges42 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges43 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges44 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges45 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges46 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges47 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges48 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges49 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges50 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges51 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges52 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges53 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges54 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges55 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges56 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges57 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges58 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges59 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges60 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges61 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges62 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges63 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges64 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges65 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges66 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges67 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges68 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges69 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges70 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges71 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges72 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges73 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges74 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges75 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges76 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges77 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges78 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges79 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges80 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges81 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges82 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges83 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges84 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges85 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges86 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges87 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges88 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges89 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges90 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges91 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges92 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges93 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges94 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges95 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges96 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges97 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges98 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges99 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges100 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges101 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges102 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges103 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges104 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges105 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges106 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges107 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges108 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges109 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges110 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges111 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges112 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges113 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges114 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges115 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges116 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges117 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges118 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges119 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges120 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges121 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges122 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges123 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges124 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges125 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges126 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges127 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges128 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges129 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges130 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges131 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges132 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges133 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges134 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges135 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges136 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges137 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges138 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges139 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges140 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges141 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges142 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges143 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges144 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges145 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges146 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges147 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges148 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges149 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges150 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges151 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges152 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges153 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges154 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges155 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges156 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges157 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges158 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges159 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges160 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges161 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges162 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges163 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges164 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges165 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges166 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges167 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges168 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges169 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges170 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges171 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges172 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges173 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges174 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges175 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges176 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges177 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges178 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges179 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges180 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges181 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges182 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges183 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges184 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges185 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges186 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges187 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges188 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges189 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges190 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges191 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges192 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges193 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges194 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges195 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges196 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges197 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges198 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges199 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges200 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges201 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges202 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges203 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges204 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges205 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges206 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblTitle = new Label();
            lblScreen = new Label();
            guna2Button94 = new Guna.UI2.WinForms.Guna2Button();
            label1 = new Label();
            guna2Button93 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button92 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button91 = new Guna.UI2.WinForms.Guna2Button();
            lblLegendNormal = new Label();
            lblLegendVIP = new Label();
            lblLegendMaintenance = new Label();
            lblSeatTypeTitle = new Label();
            rbNormalSeat = new RadioButton();
            rbVIPSeat = new RadioButton();
            rbNormalStatus = new RadioButton();
            rbMaintenanceStatus = new RadioButton();
            guna2Button15 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button3 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button4 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button5 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button6 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button7 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button14 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button16 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button8 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button9 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button10 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button11 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button12 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button13 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button17 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button18 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button19 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button20 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button21 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button22 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button23 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button24 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button25 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button26 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button27 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button28 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button29 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button30 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button31 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button32 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button33 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button34 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button35 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button36 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button37 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button38 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button39 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button40 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button41 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button42 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button43 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button44 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button45 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button53 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button54 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button55 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button56 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button57 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button59 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button60 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button46 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button47 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button48 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button49 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button50 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button51 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button52 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button58 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button61 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button62 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button63 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button64 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button65 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button66 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button67 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button68 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button69 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button70 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button71 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button72 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button73 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button74 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button75 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button76 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button77 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button78 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button79 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button80 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button81 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button82 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button83 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button84 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button85 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button86 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button87 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button88 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button89 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button90 = new Guna.UI2.WinForms.Guna2Button();
            guna2CustomGradientPanel1 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            guna2CustomGradientPanel2 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            label2 = new Label();
            guna2CustomGradientPanel3 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            guna2Button95 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button96 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button97 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button98 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button99 = new Guna.UI2.WinForms.Guna2Button();
            guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            btnThemPhim = new Guna.UI2.WinForms.Guna2Button();
            guna2CustomGradientPanel1.SuspendLayout();
            guna2CustomGradientPanel2.SuspendLayout();
            guna2CustomGradientPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(123, 168);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(318, 38);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "SƠ ĐỒ GHẾ";
            // 
            // lblScreen
            // 
            lblScreen.BackColor = Color.FromArgb(230, 230, 230);
            lblScreen.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblScreen.ForeColor = Color.Black;
            lblScreen.Location = new Point(123, 226);
            lblScreen.Name = "lblScreen";
            lblScreen.Size = new Size(968, 75);
            lblScreen.TabIndex = 2;
            lblScreen.Text = "MÀN HÌNH";
            lblScreen.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // guna2Button94
            // 
            guna2Button94.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button94.CustomizableEdges = customizableEdges1;
            guna2Button94.DisabledState.BorderColor = Color.DarkGray;
            guna2Button94.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button94.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button94.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button94.FillColor = Color.PaleGreen;
            guna2Button94.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button94.ForeColor = Color.Gray;
            guna2Button94.Location = new Point(652, 17);
            guna2Button94.Name = "guna2Button94";
            guna2Button94.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Button94.Size = new Size(59, 47);
            guna2Button94.TabIndex = 148;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 10F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(717, 28);
            label1.Name = "label1";
            label1.Size = new Size(137, 31);
            label1.TabIndex = 149;
            label1.Text = "Ghế đang chọn";
            // 
            // guna2Button93
            // 
            guna2Button93.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button93.CustomizableEdges = customizableEdges3;
            guna2Button93.DisabledState.BorderColor = Color.DarkGray;
            guna2Button93.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button93.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button93.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button93.FillColor = Color.DimGray;
            guna2Button93.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button93.ForeColor = Color.Gray;
            guna2Button93.Location = new Point(458, 18);
            guna2Button93.Name = "guna2Button93";
            guna2Button93.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Button93.Size = new Size(59, 47);
            guna2Button93.TabIndex = 148;
            guna2Button93.Click += guna2Button93_Click;
            // 
            // guna2Button92
            // 
            guna2Button92.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button92.BorderThickness = 5;
            guna2Button92.CustomizableEdges = customizableEdges5;
            guna2Button92.DisabledState.BorderColor = Color.DarkGray;
            guna2Button92.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button92.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button92.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button92.FillColor = Color.WhiteSmoke;
            guna2Button92.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button92.ForeColor = Color.Gray;
            guna2Button92.Location = new Point(275, 18);
            guna2Button92.Name = "guna2Button92";
            guna2Button92.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Button92.Size = new Size(59, 47);
            guna2Button92.TabIndex = 148;
            guna2Button92.Click += guna2Button92_Click;
            // 
            // guna2Button91
            // 
            guna2Button91.BorderColor = Color.DimGray;
            guna2Button91.BorderThickness = 5;
            guna2Button91.CustomizableEdges = customizableEdges7;
            guna2Button91.DisabledState.BorderColor = Color.DarkGray;
            guna2Button91.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button91.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button91.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button91.FillColor = Color.WhiteSmoke;
            guna2Button91.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button91.ForeColor = Color.Gray;
            guna2Button91.Location = new Point(68, 17);
            guna2Button91.Name = "guna2Button91";
            guna2Button91.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2Button91.Size = new Size(59, 47);
            guna2Button91.TabIndex = 148;
            // 
            // lblLegendNormal
            // 
            lblLegendNormal.Font = new Font("Segoe UI", 10F);
            lblLegendNormal.ForeColor = Color.White;
            lblLegendNormal.Location = new Point(133, 28);
            lblLegendNormal.Name = "lblLegendNormal";
            lblLegendNormal.Size = new Size(114, 31);
            lblLegendNormal.TabIndex = 1;
            lblLegendNormal.Text = "Ghế thường";
            // 
            // lblLegendVIP
            // 
            lblLegendVIP.Font = new Font("Segoe UI", 10F);
            lblLegendVIP.ForeColor = Color.White;
            lblLegendVIP.Location = new Point(340, 28);
            lblLegendVIP.Name = "lblLegendVIP";
            lblLegendVIP.Size = new Size(80, 31);
            lblLegendVIP.TabIndex = 3;
            lblLegendVIP.Text = "Ghế VIP";
            // 
            // lblLegendMaintenance
            // 
            lblLegendMaintenance.Font = new Font("Segoe UI", 10F);
            lblLegendMaintenance.ForeColor = Color.White;
            lblLegendMaintenance.Location = new Point(523, 28);
            lblLegendMaintenance.Name = "lblLegendMaintenance";
            lblLegendMaintenance.Size = new Size(100, 31);
            lblLegendMaintenance.TabIndex = 5;
            lblLegendMaintenance.Text = "Ghế bảo trì";
            // 
            // lblSeatTypeTitle
            // 
            lblSeatTypeTitle.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSeatTypeTitle.ForeColor = Color.White;
            lblSeatTypeTitle.Location = new Point(45, 25);
            lblSeatTypeTitle.Name = "lblSeatTypeTitle";
            lblSeatTypeTitle.Size = new Size(136, 38);
            lblSeatTypeTitle.TabIndex = 0;
            lblSeatTypeTitle.Text = "LOẠI GHẾ";
            // 
            // rbNormalSeat
            // 
            rbNormalSeat.Checked = true;
            rbNormalSeat.Font = new Font("Segoe UI", 10F);
            rbNormalSeat.ForeColor = Color.White;
            rbNormalSeat.Location = new Point(33, 67);
            rbNormalSeat.Margin = new Padding(3, 4, 3, 4);
            rbNormalSeat.Name = "rbNormalSeat";
            rbNormalSeat.Size = new Size(148, 38);
            rbNormalSeat.TabIndex = 1;
            rbNormalSeat.TabStop = true;
            rbNormalSeat.Text = "   Ghế thường";
            rbNormalSeat.CheckedChanged += SeatType_CheckedChanged;
            // 
            // rbVIPSeat
            // 
            rbVIPSeat.Font = new Font("Segoe UI", 10F);
            rbVIPSeat.ForeColor = Color.White;
            rbVIPSeat.Location = new Point(33, 113);
            rbVIPSeat.Margin = new Padding(3, 4, 3, 4);
            rbVIPSeat.Name = "rbVIPSeat";
            rbVIPSeat.Size = new Size(162, 38);
            rbVIPSeat.TabIndex = 2;
            rbVIPSeat.Text = "   Ghế VIP";
            rbVIPSeat.CheckedChanged += SeatType_CheckedChanged;
            // 
            // rbNormalStatus
            // 
            rbNormalStatus.Checked = true;
            rbNormalStatus.Font = new Font("Segoe UI", 10F);
            rbNormalStatus.ForeColor = Color.White;
            rbNormalStatus.Location = new Point(33, 67);
            rbNormalStatus.Margin = new Padding(3, 4, 3, 4);
            rbNormalStatus.Name = "rbNormalStatus";
            rbNormalStatus.Size = new Size(162, 38);
            rbNormalStatus.TabIndex = 1;
            rbNormalStatus.TabStop = true;
            rbNormalStatus.Text = "   Bình thường";
            rbNormalStatus.CheckedChanged += SeatStatus_CheckedChanged;
            // 
            // rbMaintenanceStatus
            // 
            rbMaintenanceStatus.Font = new Font("Segoe UI", 10F);
            rbMaintenanceStatus.ForeColor = Color.White;
            rbMaintenanceStatus.Location = new Point(33, 110);
            rbMaintenanceStatus.Margin = new Padding(3, 4, 3, 4);
            rbMaintenanceStatus.Name = "rbMaintenanceStatus";
            rbMaintenanceStatus.Size = new Size(148, 38);
            rbMaintenanceStatus.TabIndex = 2;
            rbMaintenanceStatus.Text = "   Bảo trì";
            rbMaintenanceStatus.CheckedChanged += SeatStatus_CheckedChanged;
            // 
            // guna2Button15
            // 
            guna2Button15.BorderColor = Color.DimGray;
            guna2Button15.BorderThickness = 5;
            guna2Button15.CustomizableEdges = customizableEdges9;
            guna2Button15.DisabledState.BorderColor = Color.DarkGray;
            guna2Button15.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button15.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button15.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button15.FillColor = Color.WhiteSmoke;
            guna2Button15.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button15.ForeColor = Color.Gray;
            guna2Button15.Location = new Point(123, 326);
            guna2Button15.Name = "guna2Button15";
            guna2Button15.ShadowDecoration.CustomizableEdges = customizableEdges10;
            guna2Button15.Size = new Size(59, 47);
            guna2Button15.TabIndex = 20;
            guna2Button15.Text = "A1";
            // 
            // guna2Button1
            // 
            guna2Button1.BorderColor = Color.DimGray;
            guna2Button1.BorderThickness = 5;
            guna2Button1.CustomizableEdges = customizableEdges11;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.WhiteSmoke;
            guna2Button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.Gray;
            guna2Button1.Location = new Point(188, 326);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges12;
            guna2Button1.Size = new Size(59, 47);
            guna2Button1.TabIndex = 21;
            guna2Button1.Text = "A2";
            // 
            // guna2Button2
            // 
            guna2Button2.BorderColor = Color.DimGray;
            guna2Button2.BorderThickness = 5;
            guna2Button2.CustomizableEdges = customizableEdges13;
            guna2Button2.DisabledState.BorderColor = Color.DarkGray;
            guna2Button2.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button2.FillColor = Color.WhiteSmoke;
            guna2Button2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button2.ForeColor = Color.Gray;
            guna2Button2.Location = new Point(577, 326);
            guna2Button2.Name = "guna2Button2";
            guna2Button2.ShadowDecoration.CustomizableEdges = customizableEdges14;
            guna2Button2.Size = new Size(59, 47);
            guna2Button2.TabIndex = 22;
            guna2Button2.Text = "A8";
            // 
            // guna2Button3
            // 
            guna2Button3.BorderColor = Color.DimGray;
            guna2Button3.BorderThickness = 5;
            guna2Button3.CustomizableEdges = customizableEdges15;
            guna2Button3.DisabledState.BorderColor = Color.DarkGray;
            guna2Button3.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button3.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button3.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button3.FillColor = Color.WhiteSmoke;
            guna2Button3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button3.ForeColor = Color.Gray;
            guna2Button3.Location = new Point(512, 326);
            guna2Button3.Name = "guna2Button3";
            guna2Button3.ShadowDecoration.CustomizableEdges = customizableEdges16;
            guna2Button3.Size = new Size(59, 47);
            guna2Button3.TabIndex = 23;
            guna2Button3.Text = "A7";
            // 
            // guna2Button4
            // 
            guna2Button4.BorderColor = Color.DimGray;
            guna2Button4.BorderThickness = 5;
            guna2Button4.CustomizableEdges = customizableEdges17;
            guna2Button4.DisabledState.BorderColor = Color.DarkGray;
            guna2Button4.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button4.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button4.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button4.FillColor = Color.WhiteSmoke;
            guna2Button4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button4.ForeColor = Color.Gray;
            guna2Button4.Location = new Point(447, 326);
            guna2Button4.Name = "guna2Button4";
            guna2Button4.ShadowDecoration.CustomizableEdges = customizableEdges18;
            guna2Button4.Size = new Size(59, 47);
            guna2Button4.TabIndex = 24;
            guna2Button4.Text = "A6";
            guna2Button4.Click += guna2Button4_Click;
            // 
            // guna2Button5
            // 
            guna2Button5.BorderColor = Color.DimGray;
            guna2Button5.BorderThickness = 5;
            guna2Button5.CustomizableEdges = customizableEdges19;
            guna2Button5.DisabledState.BorderColor = Color.DarkGray;
            guna2Button5.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button5.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button5.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button5.FillColor = Color.WhiteSmoke;
            guna2Button5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button5.ForeColor = Color.Gray;
            guna2Button5.Location = new Point(382, 326);
            guna2Button5.Name = "guna2Button5";
            guna2Button5.ShadowDecoration.CustomizableEdges = customizableEdges20;
            guna2Button5.Size = new Size(59, 47);
            guna2Button5.TabIndex = 25;
            guna2Button5.Text = "A5";
            // 
            // guna2Button6
            // 
            guna2Button6.BorderColor = Color.DimGray;
            guna2Button6.BorderThickness = 5;
            guna2Button6.CustomizableEdges = customizableEdges21;
            guna2Button6.DisabledState.BorderColor = Color.DarkGray;
            guna2Button6.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button6.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button6.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button6.FillColor = Color.WhiteSmoke;
            guna2Button6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button6.ForeColor = Color.Gray;
            guna2Button6.Location = new Point(317, 326);
            guna2Button6.Name = "guna2Button6";
            guna2Button6.ShadowDecoration.CustomizableEdges = customizableEdges22;
            guna2Button6.Size = new Size(59, 47);
            guna2Button6.TabIndex = 26;
            guna2Button6.Text = "A4";
            // 
            // guna2Button7
            // 
            guna2Button7.BorderColor = Color.DimGray;
            guna2Button7.BorderThickness = 5;
            guna2Button7.CustomizableEdges = customizableEdges23;
            guna2Button7.DisabledState.BorderColor = Color.DarkGray;
            guna2Button7.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button7.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button7.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button7.FillColor = Color.WhiteSmoke;
            guna2Button7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button7.ForeColor = Color.Gray;
            guna2Button7.Location = new Point(253, 326);
            guna2Button7.Name = "guna2Button7";
            guna2Button7.ShadowDecoration.CustomizableEdges = customizableEdges24;
            guna2Button7.Size = new Size(59, 47);
            guna2Button7.TabIndex = 27;
            guna2Button7.Text = "A3";
            // 
            // guna2Button14
            // 
            guna2Button14.BorderColor = Color.DimGray;
            guna2Button14.BorderThickness = 5;
            guna2Button14.CustomizableEdges = customizableEdges25;
            guna2Button14.DisabledState.BorderColor = Color.DarkGray;
            guna2Button14.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button14.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button14.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button14.FillColor = Color.WhiteSmoke;
            guna2Button14.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button14.ForeColor = Color.Gray;
            guna2Button14.Location = new Point(707, 326);
            guna2Button14.Name = "guna2Button14";
            guna2Button14.ShadowDecoration.CustomizableEdges = customizableEdges26;
            guna2Button14.Size = new Size(59, 47);
            guna2Button14.TabIndex = 29;
            guna2Button14.Text = "A10";
            // 
            // guna2Button16
            // 
            guna2Button16.BorderColor = Color.DimGray;
            guna2Button16.BorderThickness = 5;
            guna2Button16.CustomizableEdges = customizableEdges27;
            guna2Button16.DisabledState.BorderColor = Color.DarkGray;
            guna2Button16.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button16.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button16.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button16.FillColor = Color.WhiteSmoke;
            guna2Button16.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button16.ForeColor = Color.Gray;
            guna2Button16.Location = new Point(642, 326);
            guna2Button16.Name = "guna2Button16";
            guna2Button16.ShadowDecoration.CustomizableEdges = customizableEdges28;
            guna2Button16.Size = new Size(59, 47);
            guna2Button16.TabIndex = 28;
            guna2Button16.Text = "A9";
            // 
            // guna2Button8
            // 
            guna2Button8.BorderColor = Color.DimGray;
            guna2Button8.BorderThickness = 5;
            guna2Button8.CustomizableEdges = customizableEdges29;
            guna2Button8.DisabledState.BorderColor = Color.DarkGray;
            guna2Button8.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button8.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button8.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button8.FillColor = Color.WhiteSmoke;
            guna2Button8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button8.ForeColor = Color.Gray;
            guna2Button8.Location = new Point(1032, 326);
            guna2Button8.Name = "guna2Button8";
            guna2Button8.ShadowDecoration.CustomizableEdges = customizableEdges30;
            guna2Button8.Size = new Size(59, 47);
            guna2Button8.TabIndex = 34;
            guna2Button8.Text = "A15";
            // 
            // guna2Button9
            // 
            guna2Button9.BorderColor = Color.DimGray;
            guna2Button9.BorderThickness = 5;
            guna2Button9.CustomizableEdges = customizableEdges31;
            guna2Button9.DisabledState.BorderColor = Color.DarkGray;
            guna2Button9.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button9.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button9.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button9.FillColor = Color.WhiteSmoke;
            guna2Button9.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button9.ForeColor = Color.Gray;
            guna2Button9.Location = new Point(967, 326);
            guna2Button9.Name = "guna2Button9";
            guna2Button9.ShadowDecoration.CustomizableEdges = customizableEdges32;
            guna2Button9.Size = new Size(59, 47);
            guna2Button9.TabIndex = 33;
            guna2Button9.Text = "A14";
            // 
            // guna2Button10
            // 
            guna2Button10.BorderColor = Color.DimGray;
            guna2Button10.BorderThickness = 5;
            guna2Button10.CustomizableEdges = customizableEdges33;
            guna2Button10.DisabledState.BorderColor = Color.DarkGray;
            guna2Button10.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button10.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button10.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button10.FillColor = Color.WhiteSmoke;
            guna2Button10.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button10.ForeColor = Color.Gray;
            guna2Button10.Location = new Point(772, 326);
            guna2Button10.Name = "guna2Button10";
            guna2Button10.ShadowDecoration.CustomizableEdges = customizableEdges34;
            guna2Button10.Size = new Size(59, 47);
            guna2Button10.TabIndex = 32;
            guna2Button10.Text = "A11";
            // 
            // guna2Button11
            // 
            guna2Button11.BorderColor = Color.DimGray;
            guna2Button11.BorderThickness = 5;
            guna2Button11.CustomizableEdges = customizableEdges35;
            guna2Button11.DisabledState.BorderColor = Color.DarkGray;
            guna2Button11.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button11.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button11.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button11.FillColor = Color.WhiteSmoke;
            guna2Button11.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button11.ForeColor = Color.Gray;
            guna2Button11.Location = new Point(837, 326);
            guna2Button11.Name = "guna2Button11";
            guna2Button11.ShadowDecoration.CustomizableEdges = customizableEdges36;
            guna2Button11.Size = new Size(59, 47);
            guna2Button11.TabIndex = 31;
            guna2Button11.Text = "A12";
            // 
            // guna2Button12
            // 
            guna2Button12.BorderColor = Color.DimGray;
            guna2Button12.BorderThickness = 5;
            guna2Button12.CustomizableEdges = customizableEdges37;
            guna2Button12.DisabledState.BorderColor = Color.DarkGray;
            guna2Button12.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button12.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button12.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button12.FillColor = Color.WhiteSmoke;
            guna2Button12.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button12.ForeColor = Color.Gray;
            guna2Button12.Location = new Point(902, 326);
            guna2Button12.Name = "guna2Button12";
            guna2Button12.ShadowDecoration.CustomizableEdges = customizableEdges38;
            guna2Button12.Size = new Size(59, 47);
            guna2Button12.TabIndex = 30;
            guna2Button12.Text = "A13";
            // 
            // guna2Button13
            // 
            guna2Button13.BorderColor = Color.DimGray;
            guna2Button13.BorderThickness = 5;
            guna2Button13.CustomizableEdges = customizableEdges39;
            guna2Button13.DisabledState.BorderColor = Color.DarkGray;
            guna2Button13.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button13.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button13.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button13.FillColor = Color.WhiteSmoke;
            guna2Button13.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button13.ForeColor = Color.Gray;
            guna2Button13.Location = new Point(1032, 393);
            guna2Button13.Name = "guna2Button13";
            guna2Button13.ShadowDecoration.CustomizableEdges = customizableEdges40;
            guna2Button13.Size = new Size(59, 47);
            guna2Button13.TabIndex = 49;
            guna2Button13.Text = "B15";
            // 
            // guna2Button17
            // 
            guna2Button17.BorderColor = Color.DimGray;
            guna2Button17.BorderThickness = 5;
            guna2Button17.CustomizableEdges = customizableEdges41;
            guna2Button17.DisabledState.BorderColor = Color.DarkGray;
            guna2Button17.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button17.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button17.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button17.FillColor = Color.WhiteSmoke;
            guna2Button17.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button17.ForeColor = Color.Gray;
            guna2Button17.Location = new Point(967, 393);
            guna2Button17.Name = "guna2Button17";
            guna2Button17.ShadowDecoration.CustomizableEdges = customizableEdges42;
            guna2Button17.Size = new Size(59, 47);
            guna2Button17.TabIndex = 48;
            guna2Button17.Text = "B14";
            // 
            // guna2Button18
            // 
            guna2Button18.BorderColor = Color.DimGray;
            guna2Button18.BorderThickness = 5;
            guna2Button18.CustomizableEdges = customizableEdges43;
            guna2Button18.DisabledState.BorderColor = Color.DarkGray;
            guna2Button18.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button18.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button18.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button18.FillColor = Color.WhiteSmoke;
            guna2Button18.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button18.ForeColor = Color.Gray;
            guna2Button18.Location = new Point(772, 393);
            guna2Button18.Name = "guna2Button18";
            guna2Button18.ShadowDecoration.CustomizableEdges = customizableEdges44;
            guna2Button18.Size = new Size(59, 47);
            guna2Button18.TabIndex = 47;
            guna2Button18.Text = "B11";
            // 
            // guna2Button19
            // 
            guna2Button19.BorderColor = Color.DimGray;
            guna2Button19.BorderThickness = 5;
            guna2Button19.CustomizableEdges = customizableEdges45;
            guna2Button19.DisabledState.BorderColor = Color.DarkGray;
            guna2Button19.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button19.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button19.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button19.FillColor = Color.WhiteSmoke;
            guna2Button19.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button19.ForeColor = Color.Gray;
            guna2Button19.Location = new Point(837, 393);
            guna2Button19.Name = "guna2Button19";
            guna2Button19.ShadowDecoration.CustomizableEdges = customizableEdges46;
            guna2Button19.Size = new Size(59, 47);
            guna2Button19.TabIndex = 46;
            guna2Button19.Text = "B12";
            // 
            // guna2Button20
            // 
            guna2Button20.BorderColor = Color.DimGray;
            guna2Button20.BorderThickness = 5;
            guna2Button20.CustomizableEdges = customizableEdges47;
            guna2Button20.DisabledState.BorderColor = Color.DarkGray;
            guna2Button20.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button20.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button20.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button20.FillColor = Color.WhiteSmoke;
            guna2Button20.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button20.ForeColor = Color.Gray;
            guna2Button20.Location = new Point(902, 393);
            guna2Button20.Name = "guna2Button20";
            guna2Button20.ShadowDecoration.CustomizableEdges = customizableEdges48;
            guna2Button20.Size = new Size(59, 47);
            guna2Button20.TabIndex = 45;
            guna2Button20.Text = "B13";
            // 
            // guna2Button21
            // 
            guna2Button21.BorderColor = Color.DimGray;
            guna2Button21.BorderThickness = 5;
            guna2Button21.CustomizableEdges = customizableEdges49;
            guna2Button21.DisabledState.BorderColor = Color.DarkGray;
            guna2Button21.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button21.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button21.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button21.FillColor = Color.WhiteSmoke;
            guna2Button21.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button21.ForeColor = Color.Gray;
            guna2Button21.Location = new Point(707, 393);
            guna2Button21.Name = "guna2Button21";
            guna2Button21.ShadowDecoration.CustomizableEdges = customizableEdges50;
            guna2Button21.Size = new Size(59, 47);
            guna2Button21.TabIndex = 44;
            guna2Button21.Text = "B10";
            // 
            // guna2Button22
            // 
            guna2Button22.BorderColor = Color.DimGray;
            guna2Button22.BorderThickness = 5;
            guna2Button22.CustomizableEdges = customizableEdges51;
            guna2Button22.DisabledState.BorderColor = Color.DarkGray;
            guna2Button22.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button22.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button22.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button22.FillColor = Color.WhiteSmoke;
            guna2Button22.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button22.ForeColor = Color.Gray;
            guna2Button22.Location = new Point(642, 393);
            guna2Button22.Name = "guna2Button22";
            guna2Button22.ShadowDecoration.CustomizableEdges = customizableEdges52;
            guna2Button22.Size = new Size(59, 47);
            guna2Button22.TabIndex = 43;
            guna2Button22.Text = "B9";
            // 
            // guna2Button23
            // 
            guna2Button23.BorderColor = Color.DimGray;
            guna2Button23.BorderThickness = 5;
            guna2Button23.CustomizableEdges = customizableEdges53;
            guna2Button23.DisabledState.BorderColor = Color.DarkGray;
            guna2Button23.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button23.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button23.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button23.FillColor = Color.WhiteSmoke;
            guna2Button23.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button23.ForeColor = Color.Gray;
            guna2Button23.Location = new Point(253, 393);
            guna2Button23.Name = "guna2Button23";
            guna2Button23.ShadowDecoration.CustomizableEdges = customizableEdges54;
            guna2Button23.Size = new Size(59, 47);
            guna2Button23.TabIndex = 42;
            guna2Button23.Text = "B3";
            // 
            // guna2Button24
            // 
            guna2Button24.BorderColor = Color.DimGray;
            guna2Button24.BorderThickness = 5;
            guna2Button24.CustomizableEdges = customizableEdges55;
            guna2Button24.DisabledState.BorderColor = Color.DarkGray;
            guna2Button24.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button24.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button24.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button24.FillColor = Color.WhiteSmoke;
            guna2Button24.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button24.ForeColor = Color.Gray;
            guna2Button24.Location = new Point(317, 393);
            guna2Button24.Name = "guna2Button24";
            guna2Button24.ShadowDecoration.CustomizableEdges = customizableEdges56;
            guna2Button24.Size = new Size(59, 47);
            guna2Button24.TabIndex = 41;
            guna2Button24.Text = "B4";
            // 
            // guna2Button25
            // 
            guna2Button25.BorderColor = Color.DimGray;
            guna2Button25.BorderThickness = 5;
            guna2Button25.CustomizableEdges = customizableEdges57;
            guna2Button25.DisabledState.BorderColor = Color.DarkGray;
            guna2Button25.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button25.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button25.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button25.FillColor = Color.WhiteSmoke;
            guna2Button25.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button25.ForeColor = Color.Gray;
            guna2Button25.Location = new Point(382, 393);
            guna2Button25.Name = "guna2Button25";
            guna2Button25.ShadowDecoration.CustomizableEdges = customizableEdges58;
            guna2Button25.Size = new Size(59, 47);
            guna2Button25.TabIndex = 40;
            guna2Button25.Text = "B5";
            // 
            // guna2Button26
            // 
            guna2Button26.BorderColor = Color.DimGray;
            guna2Button26.BorderThickness = 5;
            guna2Button26.CustomizableEdges = customizableEdges59;
            guna2Button26.DisabledState.BorderColor = Color.DarkGray;
            guna2Button26.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button26.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button26.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button26.FillColor = Color.WhiteSmoke;
            guna2Button26.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button26.ForeColor = Color.Gray;
            guna2Button26.Location = new Point(447, 393);
            guna2Button26.Name = "guna2Button26";
            guna2Button26.ShadowDecoration.CustomizableEdges = customizableEdges60;
            guna2Button26.Size = new Size(59, 47);
            guna2Button26.TabIndex = 39;
            guna2Button26.Text = "B6";
            // 
            // guna2Button27
            // 
            guna2Button27.BorderColor = Color.DimGray;
            guna2Button27.BorderThickness = 5;
            guna2Button27.CustomizableEdges = customizableEdges61;
            guna2Button27.DisabledState.BorderColor = Color.DarkGray;
            guna2Button27.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button27.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button27.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button27.FillColor = Color.WhiteSmoke;
            guna2Button27.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button27.ForeColor = Color.Gray;
            guna2Button27.Location = new Point(512, 393);
            guna2Button27.Name = "guna2Button27";
            guna2Button27.ShadowDecoration.CustomizableEdges = customizableEdges62;
            guna2Button27.Size = new Size(59, 47);
            guna2Button27.TabIndex = 38;
            guna2Button27.Text = "B7";
            // 
            // guna2Button28
            // 
            guna2Button28.BorderColor = Color.DimGray;
            guna2Button28.BorderThickness = 5;
            guna2Button28.CustomizableEdges = customizableEdges63;
            guna2Button28.DisabledState.BorderColor = Color.DarkGray;
            guna2Button28.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button28.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button28.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button28.FillColor = Color.WhiteSmoke;
            guna2Button28.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button28.ForeColor = Color.Gray;
            guna2Button28.Location = new Point(577, 393);
            guna2Button28.Name = "guna2Button28";
            guna2Button28.ShadowDecoration.CustomizableEdges = customizableEdges64;
            guna2Button28.Size = new Size(59, 47);
            guna2Button28.TabIndex = 37;
            guna2Button28.Text = "B8";
            // 
            // guna2Button29
            // 
            guna2Button29.BorderColor = Color.DimGray;
            guna2Button29.BorderThickness = 5;
            guna2Button29.CustomizableEdges = customizableEdges65;
            guna2Button29.DisabledState.BorderColor = Color.DarkGray;
            guna2Button29.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button29.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button29.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button29.FillColor = Color.WhiteSmoke;
            guna2Button29.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button29.ForeColor = Color.Gray;
            guna2Button29.Location = new Point(188, 393);
            guna2Button29.Name = "guna2Button29";
            guna2Button29.ShadowDecoration.CustomizableEdges = customizableEdges66;
            guna2Button29.Size = new Size(59, 47);
            guna2Button29.TabIndex = 36;
            guna2Button29.Text = "B2";
            // 
            // guna2Button30
            // 
            guna2Button30.BorderColor = Color.DimGray;
            guna2Button30.BorderThickness = 5;
            guna2Button30.CustomizableEdges = customizableEdges67;
            guna2Button30.DisabledState.BorderColor = Color.DarkGray;
            guna2Button30.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button30.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button30.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button30.FillColor = Color.WhiteSmoke;
            guna2Button30.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button30.ForeColor = Color.Gray;
            guna2Button30.Location = new Point(123, 393);
            guna2Button30.Name = "guna2Button30";
            guna2Button30.ShadowDecoration.CustomizableEdges = customizableEdges68;
            guna2Button30.Size = new Size(59, 47);
            guna2Button30.TabIndex = 35;
            guna2Button30.Text = "B1";
            // 
            // guna2Button31
            // 
            guna2Button31.BorderColor = Color.DimGray;
            guna2Button31.BorderThickness = 5;
            guna2Button31.CustomizableEdges = customizableEdges69;
            guna2Button31.DisabledState.BorderColor = Color.DarkGray;
            guna2Button31.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button31.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button31.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button31.FillColor = Color.WhiteSmoke;
            guna2Button31.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button31.ForeColor = Color.Gray;
            guna2Button31.Location = new Point(1032, 466);
            guna2Button31.Name = "guna2Button31";
            guna2Button31.ShadowDecoration.CustomizableEdges = customizableEdges70;
            guna2Button31.Size = new Size(59, 47);
            guna2Button31.TabIndex = 64;
            guna2Button31.Text = "C15";
            // 
            // guna2Button32
            // 
            guna2Button32.BorderColor = Color.DimGray;
            guna2Button32.BorderThickness = 5;
            guna2Button32.CustomizableEdges = customizableEdges71;
            guna2Button32.DisabledState.BorderColor = Color.DarkGray;
            guna2Button32.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button32.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button32.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button32.FillColor = Color.WhiteSmoke;
            guna2Button32.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button32.ForeColor = Color.Gray;
            guna2Button32.Location = new Point(967, 466);
            guna2Button32.Name = "guna2Button32";
            guna2Button32.ShadowDecoration.CustomizableEdges = customizableEdges72;
            guna2Button32.Size = new Size(59, 47);
            guna2Button32.TabIndex = 63;
            guna2Button32.Text = "C14";
            // 
            // guna2Button33
            // 
            guna2Button33.BorderColor = Color.DimGray;
            guna2Button33.BorderThickness = 5;
            guna2Button33.CustomizableEdges = customizableEdges73;
            guna2Button33.DisabledState.BorderColor = Color.DarkGray;
            guna2Button33.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button33.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button33.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button33.FillColor = Color.WhiteSmoke;
            guna2Button33.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button33.ForeColor = Color.Gray;
            guna2Button33.Location = new Point(772, 466);
            guna2Button33.Name = "guna2Button33";
            guna2Button33.ShadowDecoration.CustomizableEdges = customizableEdges74;
            guna2Button33.Size = new Size(59, 47);
            guna2Button33.TabIndex = 62;
            guna2Button33.Text = "C11";
            // 
            // guna2Button34
            // 
            guna2Button34.BorderColor = Color.DimGray;
            guna2Button34.BorderThickness = 5;
            guna2Button34.CustomizableEdges = customizableEdges75;
            guna2Button34.DisabledState.BorderColor = Color.DarkGray;
            guna2Button34.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button34.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button34.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button34.FillColor = Color.WhiteSmoke;
            guna2Button34.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button34.ForeColor = Color.Gray;
            guna2Button34.Location = new Point(837, 466);
            guna2Button34.Name = "guna2Button34";
            guna2Button34.ShadowDecoration.CustomizableEdges = customizableEdges76;
            guna2Button34.Size = new Size(59, 47);
            guna2Button34.TabIndex = 61;
            guna2Button34.Text = "C12";
            // 
            // guna2Button35
            // 
            guna2Button35.BorderColor = Color.DimGray;
            guna2Button35.BorderThickness = 5;
            guna2Button35.CustomizableEdges = customizableEdges77;
            guna2Button35.DisabledState.BorderColor = Color.DarkGray;
            guna2Button35.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button35.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button35.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button35.FillColor = Color.WhiteSmoke;
            guna2Button35.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button35.ForeColor = Color.Gray;
            guna2Button35.Location = new Point(902, 466);
            guna2Button35.Name = "guna2Button35";
            guna2Button35.ShadowDecoration.CustomizableEdges = customizableEdges78;
            guna2Button35.Size = new Size(59, 47);
            guna2Button35.TabIndex = 60;
            guna2Button35.Text = "C13";
            // 
            // guna2Button36
            // 
            guna2Button36.BorderColor = Color.DimGray;
            guna2Button36.BorderThickness = 5;
            guna2Button36.CustomizableEdges = customizableEdges79;
            guna2Button36.DisabledState.BorderColor = Color.DarkGray;
            guna2Button36.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button36.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button36.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button36.FillColor = Color.WhiteSmoke;
            guna2Button36.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button36.ForeColor = Color.Gray;
            guna2Button36.Location = new Point(707, 466);
            guna2Button36.Name = "guna2Button36";
            guna2Button36.ShadowDecoration.CustomizableEdges = customizableEdges80;
            guna2Button36.Size = new Size(59, 47);
            guna2Button36.TabIndex = 59;
            guna2Button36.Text = "C10";
            // 
            // guna2Button37
            // 
            guna2Button37.BorderColor = Color.DimGray;
            guna2Button37.BorderThickness = 5;
            guna2Button37.CustomizableEdges = customizableEdges81;
            guna2Button37.DisabledState.BorderColor = Color.DarkGray;
            guna2Button37.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button37.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button37.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button37.FillColor = Color.WhiteSmoke;
            guna2Button37.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button37.ForeColor = Color.Gray;
            guna2Button37.Location = new Point(642, 466);
            guna2Button37.Name = "guna2Button37";
            guna2Button37.ShadowDecoration.CustomizableEdges = customizableEdges82;
            guna2Button37.Size = new Size(59, 47);
            guna2Button37.TabIndex = 58;
            guna2Button37.Text = "C9";
            // 
            // guna2Button38
            // 
            guna2Button38.BorderColor = Color.DimGray;
            guna2Button38.BorderThickness = 5;
            guna2Button38.CustomizableEdges = customizableEdges83;
            guna2Button38.DisabledState.BorderColor = Color.DarkGray;
            guna2Button38.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button38.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button38.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button38.FillColor = Color.WhiteSmoke;
            guna2Button38.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button38.ForeColor = Color.Gray;
            guna2Button38.Location = new Point(253, 466);
            guna2Button38.Name = "guna2Button38";
            guna2Button38.ShadowDecoration.CustomizableEdges = customizableEdges84;
            guna2Button38.Size = new Size(59, 47);
            guna2Button38.TabIndex = 57;
            guna2Button38.Text = "C3";
            // 
            // guna2Button39
            // 
            guna2Button39.BorderColor = Color.DimGray;
            guna2Button39.BorderThickness = 5;
            guna2Button39.CustomizableEdges = customizableEdges85;
            guna2Button39.DisabledState.BorderColor = Color.DarkGray;
            guna2Button39.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button39.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button39.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button39.FillColor = Color.WhiteSmoke;
            guna2Button39.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button39.ForeColor = Color.Gray;
            guna2Button39.Location = new Point(317, 466);
            guna2Button39.Name = "guna2Button39";
            guna2Button39.ShadowDecoration.CustomizableEdges = customizableEdges86;
            guna2Button39.Size = new Size(59, 47);
            guna2Button39.TabIndex = 56;
            guna2Button39.Text = "C4";
            // 
            // guna2Button40
            // 
            guna2Button40.BorderColor = Color.DimGray;
            guna2Button40.BorderThickness = 5;
            guna2Button40.CustomizableEdges = customizableEdges87;
            guna2Button40.DisabledState.BorderColor = Color.DarkGray;
            guna2Button40.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button40.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button40.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button40.FillColor = Color.WhiteSmoke;
            guna2Button40.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button40.ForeColor = Color.Gray;
            guna2Button40.Location = new Point(382, 466);
            guna2Button40.Name = "guna2Button40";
            guna2Button40.ShadowDecoration.CustomizableEdges = customizableEdges88;
            guna2Button40.Size = new Size(59, 47);
            guna2Button40.TabIndex = 55;
            guna2Button40.Text = "C5";
            // 
            // guna2Button41
            // 
            guna2Button41.BorderColor = Color.DimGray;
            guna2Button41.BorderThickness = 5;
            guna2Button41.CustomizableEdges = customizableEdges89;
            guna2Button41.DisabledState.BorderColor = Color.DarkGray;
            guna2Button41.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button41.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button41.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button41.FillColor = Color.WhiteSmoke;
            guna2Button41.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button41.ForeColor = Color.Gray;
            guna2Button41.Location = new Point(447, 466);
            guna2Button41.Name = "guna2Button41";
            guna2Button41.ShadowDecoration.CustomizableEdges = customizableEdges90;
            guna2Button41.Size = new Size(59, 47);
            guna2Button41.TabIndex = 54;
            guna2Button41.Text = "C6";
            // 
            // guna2Button42
            // 
            guna2Button42.BorderColor = Color.DimGray;
            guna2Button42.BorderThickness = 5;
            guna2Button42.CustomizableEdges = customizableEdges91;
            guna2Button42.DisabledState.BorderColor = Color.DarkGray;
            guna2Button42.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button42.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button42.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button42.FillColor = Color.WhiteSmoke;
            guna2Button42.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button42.ForeColor = Color.Gray;
            guna2Button42.Location = new Point(512, 466);
            guna2Button42.Name = "guna2Button42";
            guna2Button42.ShadowDecoration.CustomizableEdges = customizableEdges92;
            guna2Button42.Size = new Size(59, 47);
            guna2Button42.TabIndex = 53;
            guna2Button42.Text = "C7";
            // 
            // guna2Button43
            // 
            guna2Button43.BorderColor = Color.DimGray;
            guna2Button43.BorderThickness = 5;
            guna2Button43.CustomizableEdges = customizableEdges93;
            guna2Button43.DisabledState.BorderColor = Color.DarkGray;
            guna2Button43.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button43.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button43.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button43.FillColor = Color.WhiteSmoke;
            guna2Button43.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button43.ForeColor = Color.Gray;
            guna2Button43.Location = new Point(577, 466);
            guna2Button43.Name = "guna2Button43";
            guna2Button43.ShadowDecoration.CustomizableEdges = customizableEdges94;
            guna2Button43.Size = new Size(59, 47);
            guna2Button43.TabIndex = 52;
            guna2Button43.Text = "C8";
            // 
            // guna2Button44
            // 
            guna2Button44.BorderColor = Color.DimGray;
            guna2Button44.BorderThickness = 5;
            guna2Button44.CustomizableEdges = customizableEdges95;
            guna2Button44.DisabledState.BorderColor = Color.DarkGray;
            guna2Button44.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button44.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button44.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button44.FillColor = Color.WhiteSmoke;
            guna2Button44.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button44.ForeColor = Color.Gray;
            guna2Button44.Location = new Point(188, 466);
            guna2Button44.Name = "guna2Button44";
            guna2Button44.ShadowDecoration.CustomizableEdges = customizableEdges96;
            guna2Button44.Size = new Size(59, 47);
            guna2Button44.TabIndex = 51;
            guna2Button44.Text = "C2";
            // 
            // guna2Button45
            // 
            guna2Button45.BorderColor = Color.DimGray;
            guna2Button45.BorderThickness = 5;
            guna2Button45.CustomizableEdges = customizableEdges97;
            guna2Button45.DisabledState.BorderColor = Color.DarkGray;
            guna2Button45.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button45.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button45.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button45.FillColor = Color.WhiteSmoke;
            guna2Button45.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button45.ForeColor = Color.Gray;
            guna2Button45.Location = new Point(123, 466);
            guna2Button45.Name = "guna2Button45";
            guna2Button45.ShadowDecoration.CustomizableEdges = customizableEdges98;
            guna2Button45.Size = new Size(59, 47);
            guna2Button45.TabIndex = 50;
            guna2Button45.Text = "C1";
            // 
            // guna2Button53
            // 
            guna2Button53.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button53.BorderThickness = 5;
            guna2Button53.CustomizableEdges = customizableEdges99;
            guna2Button53.DisabledState.BorderColor = Color.DarkGray;
            guna2Button53.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button53.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button53.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button53.FillColor = Color.WhiteSmoke;
            guna2Button53.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button53.ForeColor = Color.Gray;
            guna2Button53.Location = new Point(253, 538);
            guna2Button53.Name = "guna2Button53";
            guna2Button53.ShadowDecoration.CustomizableEdges = customizableEdges100;
            guna2Button53.Size = new Size(59, 47);
            guna2Button53.TabIndex = 72;
            guna2Button53.Text = "D3";
            // 
            // guna2Button54
            // 
            guna2Button54.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button54.BorderThickness = 5;
            guna2Button54.CustomizableEdges = customizableEdges101;
            guna2Button54.DisabledState.BorderColor = Color.DarkGray;
            guna2Button54.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button54.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button54.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button54.FillColor = Color.WhiteSmoke;
            guna2Button54.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button54.ForeColor = Color.Gray;
            guna2Button54.Location = new Point(317, 538);
            guna2Button54.Name = "guna2Button54";
            guna2Button54.ShadowDecoration.CustomizableEdges = customizableEdges102;
            guna2Button54.Size = new Size(59, 47);
            guna2Button54.TabIndex = 71;
            guna2Button54.Text = "D4";
            // 
            // guna2Button55
            // 
            guna2Button55.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button55.BorderThickness = 5;
            guna2Button55.CustomizableEdges = customizableEdges103;
            guna2Button55.DisabledState.BorderColor = Color.DarkGray;
            guna2Button55.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button55.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button55.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button55.FillColor = Color.WhiteSmoke;
            guna2Button55.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button55.ForeColor = Color.Gray;
            guna2Button55.Location = new Point(382, 538);
            guna2Button55.Name = "guna2Button55";
            guna2Button55.ShadowDecoration.CustomizableEdges = customizableEdges104;
            guna2Button55.Size = new Size(59, 47);
            guna2Button55.TabIndex = 70;
            guna2Button55.Text = "D5";
            // 
            // guna2Button56
            // 
            guna2Button56.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button56.BorderThickness = 5;
            guna2Button56.CustomizableEdges = customizableEdges105;
            guna2Button56.DisabledState.BorderColor = Color.DarkGray;
            guna2Button56.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button56.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button56.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button56.FillColor = Color.WhiteSmoke;
            guna2Button56.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button56.ForeColor = Color.Gray;
            guna2Button56.Location = new Point(447, 538);
            guna2Button56.Name = "guna2Button56";
            guna2Button56.ShadowDecoration.CustomizableEdges = customizableEdges106;
            guna2Button56.Size = new Size(59, 47);
            guna2Button56.TabIndex = 69;
            guna2Button56.Text = "D6";
            // 
            // guna2Button57
            // 
            guna2Button57.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button57.BorderThickness = 5;
            guna2Button57.CustomizableEdges = customizableEdges107;
            guna2Button57.DisabledState.BorderColor = Color.DarkGray;
            guna2Button57.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button57.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button57.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button57.FillColor = Color.WhiteSmoke;
            guna2Button57.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button57.ForeColor = Color.Gray;
            guna2Button57.Location = new Point(512, 538);
            guna2Button57.Name = "guna2Button57";
            guna2Button57.ShadowDecoration.CustomizableEdges = customizableEdges108;
            guna2Button57.Size = new Size(59, 47);
            guna2Button57.TabIndex = 68;
            guna2Button57.Text = "D7";
            guna2Button57.Click += guna2Button57_Click;
            // 
            // guna2Button59
            // 
            guna2Button59.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button59.BorderThickness = 5;
            guna2Button59.CustomizableEdges = customizableEdges109;
            guna2Button59.DisabledState.BorderColor = Color.DarkGray;
            guna2Button59.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button59.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button59.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button59.FillColor = Color.WhiteSmoke;
            guna2Button59.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button59.ForeColor = Color.Gray;
            guna2Button59.Location = new Point(188, 538);
            guna2Button59.Name = "guna2Button59";
            guna2Button59.ShadowDecoration.CustomizableEdges = customizableEdges110;
            guna2Button59.Size = new Size(59, 47);
            guna2Button59.TabIndex = 66;
            guna2Button59.Text = "D2";
            // 
            // guna2Button60
            // 
            guna2Button60.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button60.BorderThickness = 5;
            guna2Button60.CustomizableEdges = customizableEdges111;
            guna2Button60.DisabledState.BorderColor = Color.DarkGray;
            guna2Button60.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button60.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button60.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button60.FillColor = Color.WhiteSmoke;
            guna2Button60.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button60.ForeColor = Color.Gray;
            guna2Button60.Location = new Point(123, 538);
            guna2Button60.Name = "guna2Button60";
            guna2Button60.ShadowDecoration.CustomizableEdges = customizableEdges112;
            guna2Button60.Size = new Size(59, 47);
            guna2Button60.TabIndex = 65;
            guna2Button60.Text = "D1";
            // 
            // guna2Button46
            // 
            guna2Button46.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button46.BorderThickness = 5;
            guna2Button46.CustomizableEdges = customizableEdges113;
            guna2Button46.DisabledState.BorderColor = Color.DarkGray;
            guna2Button46.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button46.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button46.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button46.FillColor = Color.WhiteSmoke;
            guna2Button46.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button46.ForeColor = Color.Gray;
            guna2Button46.Location = new Point(707, 538);
            guna2Button46.Name = "guna2Button46";
            guna2Button46.ShadowDecoration.CustomizableEdges = customizableEdges114;
            guna2Button46.Size = new Size(59, 47);
            guna2Button46.TabIndex = 116;
            guna2Button46.Text = "D10";
            // 
            // guna2Button47
            // 
            guna2Button47.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button47.BorderThickness = 5;
            guna2Button47.CustomizableEdges = customizableEdges115;
            guna2Button47.DisabledState.BorderColor = Color.DarkGray;
            guna2Button47.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button47.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button47.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button47.FillColor = Color.WhiteSmoke;
            guna2Button47.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button47.ForeColor = Color.Gray;
            guna2Button47.Location = new Point(771, 538);
            guna2Button47.Name = "guna2Button47";
            guna2Button47.ShadowDecoration.CustomizableEdges = customizableEdges116;
            guna2Button47.Size = new Size(59, 47);
            guna2Button47.TabIndex = 115;
            guna2Button47.Text = "D11";
            // 
            // guna2Button48
            // 
            guna2Button48.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button48.BorderThickness = 5;
            guna2Button48.CustomizableEdges = customizableEdges117;
            guna2Button48.DisabledState.BorderColor = Color.DarkGray;
            guna2Button48.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button48.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button48.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button48.FillColor = Color.WhiteSmoke;
            guna2Button48.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button48.ForeColor = Color.Gray;
            guna2Button48.Location = new Point(836, 538);
            guna2Button48.Name = "guna2Button48";
            guna2Button48.ShadowDecoration.CustomizableEdges = customizableEdges118;
            guna2Button48.Size = new Size(59, 47);
            guna2Button48.TabIndex = 114;
            guna2Button48.Text = "D12";
            // 
            // guna2Button49
            // 
            guna2Button49.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button49.BorderThickness = 5;
            guna2Button49.CustomizableEdges = customizableEdges119;
            guna2Button49.DisabledState.BorderColor = Color.DarkGray;
            guna2Button49.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button49.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button49.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button49.FillColor = Color.WhiteSmoke;
            guna2Button49.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button49.ForeColor = Color.Gray;
            guna2Button49.Location = new Point(901, 538);
            guna2Button49.Name = "guna2Button49";
            guna2Button49.ShadowDecoration.CustomizableEdges = customizableEdges120;
            guna2Button49.Size = new Size(59, 47);
            guna2Button49.TabIndex = 113;
            guna2Button49.Text = "D13";
            // 
            // guna2Button50
            // 
            guna2Button50.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button50.BorderThickness = 5;
            guna2Button50.CustomizableEdges = customizableEdges121;
            guna2Button50.DisabledState.BorderColor = Color.DarkGray;
            guna2Button50.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button50.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button50.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button50.FillColor = Color.WhiteSmoke;
            guna2Button50.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button50.ForeColor = Color.Gray;
            guna2Button50.Location = new Point(966, 538);
            guna2Button50.Name = "guna2Button50";
            guna2Button50.ShadowDecoration.CustomizableEdges = customizableEdges122;
            guna2Button50.Size = new Size(59, 47);
            guna2Button50.TabIndex = 112;
            guna2Button50.Text = "D14";
            // 
            // guna2Button51
            // 
            guna2Button51.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button51.BorderThickness = 5;
            guna2Button51.CustomizableEdges = customizableEdges123;
            guna2Button51.DisabledState.BorderColor = Color.DarkGray;
            guna2Button51.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button51.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button51.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button51.FillColor = Color.WhiteSmoke;
            guna2Button51.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button51.ForeColor = Color.Gray;
            guna2Button51.Location = new Point(642, 538);
            guna2Button51.Name = "guna2Button51";
            guna2Button51.ShadowDecoration.CustomizableEdges = customizableEdges124;
            guna2Button51.Size = new Size(59, 47);
            guna2Button51.TabIndex = 111;
            guna2Button51.Text = "D9";
            // 
            // guna2Button52
            // 
            guna2Button52.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button52.BorderThickness = 5;
            guna2Button52.CustomizableEdges = customizableEdges125;
            guna2Button52.DisabledState.BorderColor = Color.DarkGray;
            guna2Button52.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button52.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button52.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button52.FillColor = Color.WhiteSmoke;
            guna2Button52.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button52.ForeColor = Color.Gray;
            guna2Button52.Location = new Point(577, 538);
            guna2Button52.Name = "guna2Button52";
            guna2Button52.ShadowDecoration.CustomizableEdges = customizableEdges126;
            guna2Button52.Size = new Size(59, 47);
            guna2Button52.TabIndex = 110;
            guna2Button52.Text = "D8";
            // 
            // guna2Button58
            // 
            guna2Button58.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button58.BorderThickness = 5;
            guna2Button58.CustomizableEdges = customizableEdges127;
            guna2Button58.DisabledState.BorderColor = Color.DarkGray;
            guna2Button58.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button58.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button58.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button58.FillColor = Color.WhiteSmoke;
            guna2Button58.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button58.ForeColor = Color.Gray;
            guna2Button58.Location = new Point(1032, 538);
            guna2Button58.Name = "guna2Button58";
            guna2Button58.ShadowDecoration.CustomizableEdges = customizableEdges128;
            guna2Button58.Size = new Size(59, 47);
            guna2Button58.TabIndex = 117;
            guna2Button58.Text = "D15";
            // 
            // guna2Button61
            // 
            guna2Button61.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button61.BorderThickness = 5;
            guna2Button61.CustomizableEdges = customizableEdges129;
            guna2Button61.DisabledState.BorderColor = Color.DarkGray;
            guna2Button61.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button61.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button61.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button61.FillColor = Color.WhiteSmoke;
            guna2Button61.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button61.ForeColor = Color.Gray;
            guna2Button61.Location = new Point(1032, 614);
            guna2Button61.Name = "guna2Button61";
            guna2Button61.ShadowDecoration.CustomizableEdges = customizableEdges130;
            guna2Button61.Size = new Size(59, 47);
            guna2Button61.TabIndex = 132;
            guna2Button61.Text = "E15";
            // 
            // guna2Button62
            // 
            guna2Button62.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button62.BorderThickness = 5;
            guna2Button62.CustomizableEdges = customizableEdges131;
            guna2Button62.DisabledState.BorderColor = Color.DarkGray;
            guna2Button62.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button62.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button62.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button62.FillColor = Color.WhiteSmoke;
            guna2Button62.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button62.ForeColor = Color.Gray;
            guna2Button62.Location = new Point(707, 614);
            guna2Button62.Name = "guna2Button62";
            guna2Button62.ShadowDecoration.CustomizableEdges = customizableEdges132;
            guna2Button62.Size = new Size(59, 47);
            guna2Button62.TabIndex = 131;
            guna2Button62.Text = "E10";
            // 
            // guna2Button63
            // 
            guna2Button63.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button63.BorderThickness = 5;
            guna2Button63.CustomizableEdges = customizableEdges133;
            guna2Button63.DisabledState.BorderColor = Color.DarkGray;
            guna2Button63.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button63.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button63.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button63.FillColor = Color.WhiteSmoke;
            guna2Button63.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button63.ForeColor = Color.Gray;
            guna2Button63.Location = new Point(771, 614);
            guna2Button63.Name = "guna2Button63";
            guna2Button63.ShadowDecoration.CustomizableEdges = customizableEdges134;
            guna2Button63.Size = new Size(59, 47);
            guna2Button63.TabIndex = 130;
            guna2Button63.Text = "E11";
            // 
            // guna2Button64
            // 
            guna2Button64.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button64.BorderThickness = 5;
            guna2Button64.CustomizableEdges = customizableEdges135;
            guna2Button64.DisabledState.BorderColor = Color.DarkGray;
            guna2Button64.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button64.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button64.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button64.FillColor = Color.WhiteSmoke;
            guna2Button64.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button64.ForeColor = Color.Gray;
            guna2Button64.Location = new Point(836, 614);
            guna2Button64.Name = "guna2Button64";
            guna2Button64.ShadowDecoration.CustomizableEdges = customizableEdges136;
            guna2Button64.Size = new Size(59, 47);
            guna2Button64.TabIndex = 129;
            guna2Button64.Text = "E12";
            // 
            // guna2Button65
            // 
            guna2Button65.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button65.BorderThickness = 5;
            guna2Button65.CustomizableEdges = customizableEdges137;
            guna2Button65.DisabledState.BorderColor = Color.DarkGray;
            guna2Button65.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button65.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button65.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button65.FillColor = Color.WhiteSmoke;
            guna2Button65.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button65.ForeColor = Color.Gray;
            guna2Button65.Location = new Point(901, 614);
            guna2Button65.Name = "guna2Button65";
            guna2Button65.ShadowDecoration.CustomizableEdges = customizableEdges138;
            guna2Button65.Size = new Size(59, 47);
            guna2Button65.TabIndex = 128;
            guna2Button65.Text = "E13";
            // 
            // guna2Button66
            // 
            guna2Button66.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button66.BorderThickness = 5;
            guna2Button66.CustomizableEdges = customizableEdges139;
            guna2Button66.DisabledState.BorderColor = Color.DarkGray;
            guna2Button66.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button66.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button66.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button66.FillColor = Color.WhiteSmoke;
            guna2Button66.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button66.ForeColor = Color.Gray;
            guna2Button66.Location = new Point(966, 614);
            guna2Button66.Name = "guna2Button66";
            guna2Button66.ShadowDecoration.CustomizableEdges = customizableEdges140;
            guna2Button66.Size = new Size(59, 47);
            guna2Button66.TabIndex = 127;
            guna2Button66.Text = "E14";
            // 
            // guna2Button67
            // 
            guna2Button67.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button67.BorderThickness = 5;
            guna2Button67.CustomizableEdges = customizableEdges141;
            guna2Button67.DisabledState.BorderColor = Color.DarkGray;
            guna2Button67.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button67.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button67.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button67.FillColor = Color.WhiteSmoke;
            guna2Button67.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button67.ForeColor = Color.Gray;
            guna2Button67.Location = new Point(642, 614);
            guna2Button67.Name = "guna2Button67";
            guna2Button67.ShadowDecoration.CustomizableEdges = customizableEdges142;
            guna2Button67.Size = new Size(59, 47);
            guna2Button67.TabIndex = 126;
            guna2Button67.Text = "E9";
            // 
            // guna2Button68
            // 
            guna2Button68.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button68.BorderThickness = 5;
            guna2Button68.CustomizableEdges = customizableEdges143;
            guna2Button68.DisabledState.BorderColor = Color.DarkGray;
            guna2Button68.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button68.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button68.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button68.FillColor = Color.PaleGreen;
            guna2Button68.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button68.ForeColor = Color.Gray;
            guna2Button68.Location = new Point(577, 614);
            guna2Button68.Name = "guna2Button68";
            guna2Button68.ShadowDecoration.CustomizableEdges = customizableEdges144;
            guna2Button68.Size = new Size(59, 47);
            guna2Button68.TabIndex = 125;
            guna2Button68.Text = "E8";
            // 
            // guna2Button69
            // 
            guna2Button69.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button69.BorderThickness = 5;
            guna2Button69.CustomizableEdges = customizableEdges145;
            guna2Button69.DisabledState.BorderColor = Color.DarkGray;
            guna2Button69.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button69.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button69.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button69.FillColor = Color.WhiteSmoke;
            guna2Button69.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button69.ForeColor = Color.Gray;
            guna2Button69.Location = new Point(253, 614);
            guna2Button69.Name = "guna2Button69";
            guna2Button69.ShadowDecoration.CustomizableEdges = customizableEdges146;
            guna2Button69.Size = new Size(59, 47);
            guna2Button69.TabIndex = 124;
            guna2Button69.Text = "E3";
            // 
            // guna2Button70
            // 
            guna2Button70.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button70.BorderThickness = 5;
            guna2Button70.CustomizableEdges = customizableEdges147;
            guna2Button70.DisabledState.BorderColor = Color.DarkGray;
            guna2Button70.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button70.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button70.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button70.FillColor = Color.WhiteSmoke;
            guna2Button70.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button70.ForeColor = Color.Gray;
            guna2Button70.Location = new Point(317, 614);
            guna2Button70.Name = "guna2Button70";
            guna2Button70.ShadowDecoration.CustomizableEdges = customizableEdges148;
            guna2Button70.Size = new Size(59, 47);
            guna2Button70.TabIndex = 123;
            guna2Button70.Text = "E4";
            // 
            // guna2Button71
            // 
            guna2Button71.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button71.BorderThickness = 5;
            guna2Button71.CustomizableEdges = customizableEdges149;
            guna2Button71.DisabledState.BorderColor = Color.DarkGray;
            guna2Button71.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button71.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button71.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button71.FillColor = Color.WhiteSmoke;
            guna2Button71.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button71.ForeColor = Color.Gray;
            guna2Button71.Location = new Point(382, 614);
            guna2Button71.Name = "guna2Button71";
            guna2Button71.ShadowDecoration.CustomizableEdges = customizableEdges150;
            guna2Button71.Size = new Size(59, 47);
            guna2Button71.TabIndex = 122;
            guna2Button71.Text = "E5";
            // 
            // guna2Button72
            // 
            guna2Button72.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button72.BorderThickness = 5;
            guna2Button72.CustomizableEdges = customizableEdges151;
            guna2Button72.DisabledState.BorderColor = Color.DarkGray;
            guna2Button72.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button72.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button72.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button72.FillColor = Color.WhiteSmoke;
            guna2Button72.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button72.ForeColor = Color.Gray;
            guna2Button72.Location = new Point(447, 614);
            guna2Button72.Name = "guna2Button72";
            guna2Button72.ShadowDecoration.CustomizableEdges = customizableEdges152;
            guna2Button72.Size = new Size(59, 47);
            guna2Button72.TabIndex = 121;
            guna2Button72.Text = "E6";
            // 
            // guna2Button73
            // 
            guna2Button73.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button73.BorderThickness = 5;
            guna2Button73.CustomizableEdges = customizableEdges153;
            guna2Button73.DisabledState.BorderColor = Color.DarkGray;
            guna2Button73.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button73.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button73.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button73.FillColor = Color.WhiteSmoke;
            guna2Button73.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button73.ForeColor = Color.Gray;
            guna2Button73.Location = new Point(512, 614);
            guna2Button73.Name = "guna2Button73";
            guna2Button73.ShadowDecoration.CustomizableEdges = customizableEdges154;
            guna2Button73.Size = new Size(59, 47);
            guna2Button73.TabIndex = 120;
            guna2Button73.Text = "E7";
            // 
            // guna2Button74
            // 
            guna2Button74.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button74.BorderThickness = 5;
            guna2Button74.CustomizableEdges = customizableEdges155;
            guna2Button74.DisabledState.BorderColor = Color.DarkGray;
            guna2Button74.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button74.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button74.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button74.FillColor = Color.WhiteSmoke;
            guna2Button74.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button74.ForeColor = Color.Gray;
            guna2Button74.Location = new Point(188, 614);
            guna2Button74.Name = "guna2Button74";
            guna2Button74.ShadowDecoration.CustomizableEdges = customizableEdges156;
            guna2Button74.Size = new Size(59, 47);
            guna2Button74.TabIndex = 119;
            guna2Button74.Text = "E2";
            // 
            // guna2Button75
            // 
            guna2Button75.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button75.BorderThickness = 5;
            guna2Button75.CustomizableEdges = customizableEdges157;
            guna2Button75.DisabledState.BorderColor = Color.DarkGray;
            guna2Button75.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button75.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button75.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button75.FillColor = Color.WhiteSmoke;
            guna2Button75.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button75.ForeColor = Color.Gray;
            guna2Button75.Location = new Point(123, 614);
            guna2Button75.Name = "guna2Button75";
            guna2Button75.ShadowDecoration.CustomizableEdges = customizableEdges158;
            guna2Button75.Size = new Size(59, 47);
            guna2Button75.TabIndex = 118;
            guna2Button75.Text = "E1";
            // 
            // guna2Button76
            // 
            guna2Button76.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button76.BorderThickness = 5;
            guna2Button76.CustomizableEdges = customizableEdges159;
            guna2Button76.DisabledState.BorderColor = Color.DarkGray;
            guna2Button76.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button76.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button76.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button76.FillColor = Color.DimGray;
            guna2Button76.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button76.ForeColor = Color.Gray;
            guna2Button76.Location = new Point(1032, 692);
            guna2Button76.Name = "guna2Button76";
            guna2Button76.ShadowDecoration.CustomizableEdges = customizableEdges160;
            guna2Button76.Size = new Size(59, 47);
            guna2Button76.TabIndex = 147;
            guna2Button76.Text = "F15";
            // 
            // guna2Button77
            // 
            guna2Button77.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button77.BorderThickness = 5;
            guna2Button77.CustomizableEdges = customizableEdges161;
            guna2Button77.DisabledState.BorderColor = Color.DarkGray;
            guna2Button77.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button77.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button77.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button77.FillColor = Color.WhiteSmoke;
            guna2Button77.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button77.ForeColor = Color.Gray;
            guna2Button77.Location = new Point(707, 692);
            guna2Button77.Name = "guna2Button77";
            guna2Button77.ShadowDecoration.CustomizableEdges = customizableEdges162;
            guna2Button77.Size = new Size(59, 47);
            guna2Button77.TabIndex = 146;
            guna2Button77.Text = "F10";
            // 
            // guna2Button78
            // 
            guna2Button78.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button78.BorderThickness = 5;
            guna2Button78.CustomizableEdges = customizableEdges163;
            guna2Button78.DisabledState.BorderColor = Color.DarkGray;
            guna2Button78.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button78.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button78.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button78.FillColor = Color.WhiteSmoke;
            guna2Button78.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button78.ForeColor = Color.Gray;
            guna2Button78.Location = new Point(771, 692);
            guna2Button78.Name = "guna2Button78";
            guna2Button78.ShadowDecoration.CustomizableEdges = customizableEdges164;
            guna2Button78.Size = new Size(59, 47);
            guna2Button78.TabIndex = 145;
            guna2Button78.Text = "F11";
            // 
            // guna2Button79
            // 
            guna2Button79.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button79.BorderThickness = 5;
            guna2Button79.CustomizableEdges = customizableEdges165;
            guna2Button79.DisabledState.BorderColor = Color.DarkGray;
            guna2Button79.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button79.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button79.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button79.FillColor = Color.WhiteSmoke;
            guna2Button79.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button79.ForeColor = Color.Gray;
            guna2Button79.Location = new Point(836, 692);
            guna2Button79.Name = "guna2Button79";
            guna2Button79.ShadowDecoration.CustomizableEdges = customizableEdges166;
            guna2Button79.Size = new Size(59, 47);
            guna2Button79.TabIndex = 144;
            guna2Button79.Text = "F12";
            // 
            // guna2Button80
            // 
            guna2Button80.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button80.BorderThickness = 5;
            guna2Button80.CustomizableEdges = customizableEdges167;
            guna2Button80.DisabledState.BorderColor = Color.DarkGray;
            guna2Button80.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button80.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button80.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button80.FillColor = Color.WhiteSmoke;
            guna2Button80.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button80.ForeColor = Color.Gray;
            guna2Button80.Location = new Point(901, 692);
            guna2Button80.Name = "guna2Button80";
            guna2Button80.ShadowDecoration.CustomizableEdges = customizableEdges168;
            guna2Button80.Size = new Size(59, 47);
            guna2Button80.TabIndex = 143;
            guna2Button80.Text = "F13";
            // 
            // guna2Button81
            // 
            guna2Button81.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button81.BorderThickness = 5;
            guna2Button81.CustomizableEdges = customizableEdges169;
            guna2Button81.DisabledState.BorderColor = Color.DarkGray;
            guna2Button81.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button81.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button81.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button81.FillColor = Color.WhiteSmoke;
            guna2Button81.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button81.ForeColor = Color.Gray;
            guna2Button81.Location = new Point(966, 692);
            guna2Button81.Name = "guna2Button81";
            guna2Button81.ShadowDecoration.CustomizableEdges = customizableEdges170;
            guna2Button81.Size = new Size(59, 47);
            guna2Button81.TabIndex = 142;
            guna2Button81.Text = "F14";
            // 
            // guna2Button82
            // 
            guna2Button82.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button82.BorderThickness = 5;
            guna2Button82.CustomizableEdges = customizableEdges171;
            guna2Button82.DisabledState.BorderColor = Color.DarkGray;
            guna2Button82.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button82.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button82.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button82.FillColor = Color.WhiteSmoke;
            guna2Button82.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button82.ForeColor = Color.Gray;
            guna2Button82.Location = new Point(642, 692);
            guna2Button82.Name = "guna2Button82";
            guna2Button82.ShadowDecoration.CustomizableEdges = customizableEdges172;
            guna2Button82.Size = new Size(59, 47);
            guna2Button82.TabIndex = 141;
            guna2Button82.Text = "F9";
            // 
            // guna2Button83
            // 
            guna2Button83.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button83.BorderThickness = 5;
            guna2Button83.CustomizableEdges = customizableEdges173;
            guna2Button83.DisabledState.BorderColor = Color.DarkGray;
            guna2Button83.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button83.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button83.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button83.FillColor = Color.WhiteSmoke;
            guna2Button83.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button83.ForeColor = Color.Gray;
            guna2Button83.Location = new Point(577, 692);
            guna2Button83.Name = "guna2Button83";
            guna2Button83.ShadowDecoration.CustomizableEdges = customizableEdges174;
            guna2Button83.Size = new Size(59, 47);
            guna2Button83.TabIndex = 140;
            guna2Button83.Text = "F8";
            // 
            // guna2Button84
            // 
            guna2Button84.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button84.BorderThickness = 5;
            guna2Button84.CustomizableEdges = customizableEdges175;
            guna2Button84.DisabledState.BorderColor = Color.DarkGray;
            guna2Button84.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button84.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button84.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button84.FillColor = Color.WhiteSmoke;
            guna2Button84.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button84.ForeColor = Color.Gray;
            guna2Button84.Location = new Point(253, 692);
            guna2Button84.Name = "guna2Button84";
            guna2Button84.ShadowDecoration.CustomizableEdges = customizableEdges176;
            guna2Button84.Size = new Size(59, 47);
            guna2Button84.TabIndex = 139;
            guna2Button84.Text = "F3";
            // 
            // guna2Button85
            // 
            guna2Button85.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button85.BorderThickness = 5;
            guna2Button85.CustomizableEdges = customizableEdges177;
            guna2Button85.DisabledState.BorderColor = Color.DarkGray;
            guna2Button85.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button85.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button85.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button85.FillColor = Color.WhiteSmoke;
            guna2Button85.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button85.ForeColor = Color.Gray;
            guna2Button85.Location = new Point(317, 692);
            guna2Button85.Name = "guna2Button85";
            guna2Button85.ShadowDecoration.CustomizableEdges = customizableEdges178;
            guna2Button85.Size = new Size(59, 47);
            guna2Button85.TabIndex = 138;
            guna2Button85.Text = "F4";
            // 
            // guna2Button86
            // 
            guna2Button86.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button86.BorderThickness = 5;
            guna2Button86.CustomizableEdges = customizableEdges179;
            guna2Button86.DisabledState.BorderColor = Color.DarkGray;
            guna2Button86.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button86.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button86.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button86.FillColor = Color.WhiteSmoke;
            guna2Button86.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button86.ForeColor = Color.Gray;
            guna2Button86.Location = new Point(382, 692);
            guna2Button86.Name = "guna2Button86";
            guna2Button86.ShadowDecoration.CustomizableEdges = customizableEdges180;
            guna2Button86.Size = new Size(59, 47);
            guna2Button86.TabIndex = 137;
            guna2Button86.Text = "F5";
            // 
            // guna2Button87
            // 
            guna2Button87.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button87.BorderThickness = 5;
            guna2Button87.CustomizableEdges = customizableEdges181;
            guna2Button87.DisabledState.BorderColor = Color.DarkGray;
            guna2Button87.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button87.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button87.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button87.FillColor = Color.WhiteSmoke;
            guna2Button87.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button87.ForeColor = Color.Gray;
            guna2Button87.Location = new Point(447, 692);
            guna2Button87.Name = "guna2Button87";
            guna2Button87.ShadowDecoration.CustomizableEdges = customizableEdges182;
            guna2Button87.Size = new Size(59, 47);
            guna2Button87.TabIndex = 136;
            guna2Button87.Text = "F6";
            // 
            // guna2Button88
            // 
            guna2Button88.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button88.BorderThickness = 5;
            guna2Button88.CustomizableEdges = customizableEdges183;
            guna2Button88.DisabledState.BorderColor = Color.DarkGray;
            guna2Button88.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button88.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button88.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button88.FillColor = Color.WhiteSmoke;
            guna2Button88.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button88.ForeColor = Color.Gray;
            guna2Button88.Location = new Point(512, 692);
            guna2Button88.Name = "guna2Button88";
            guna2Button88.ShadowDecoration.CustomizableEdges = customizableEdges184;
            guna2Button88.Size = new Size(59, 47);
            guna2Button88.TabIndex = 135;
            guna2Button88.Text = "F7";
            // 
            // guna2Button89
            // 
            guna2Button89.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button89.BorderThickness = 5;
            guna2Button89.CustomizableEdges = customizableEdges185;
            guna2Button89.DisabledState.BorderColor = Color.DarkGray;
            guna2Button89.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button89.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button89.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button89.FillColor = Color.WhiteSmoke;
            guna2Button89.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button89.ForeColor = Color.Gray;
            guna2Button89.Location = new Point(188, 692);
            guna2Button89.Name = "guna2Button89";
            guna2Button89.ShadowDecoration.CustomizableEdges = customizableEdges186;
            guna2Button89.Size = new Size(59, 47);
            guna2Button89.TabIndex = 134;
            guna2Button89.Text = "F2";
            // 
            // guna2Button90
            // 
            guna2Button90.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button90.BorderThickness = 5;
            guna2Button90.CustomizableEdges = customizableEdges187;
            guna2Button90.DisabledState.BorderColor = Color.DarkGray;
            guna2Button90.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button90.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button90.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button90.FillColor = Color.WhiteSmoke;
            guna2Button90.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button90.ForeColor = Color.Gray;
            guna2Button90.Location = new Point(123, 692);
            guna2Button90.Name = "guna2Button90";
            guna2Button90.ShadowDecoration.CustomizableEdges = customizableEdges188;
            guna2Button90.Size = new Size(59, 47);
            guna2Button90.TabIndex = 133;
            guna2Button90.Text = "F1";
            // 
            // guna2CustomGradientPanel1
            // 
            guna2CustomGradientPanel1.BorderColor = Color.FromArgb(255, 192, 128);
            guna2CustomGradientPanel1.BorderRadius = 5;
            guna2CustomGradientPanel1.BorderThickness = 4;
            guna2CustomGradientPanel1.Controls.Add(rbVIPSeat);
            guna2CustomGradientPanel1.Controls.Add(lblSeatTypeTitle);
            guna2CustomGradientPanel1.Controls.Add(rbNormalSeat);
            guna2CustomGradientPanel1.CustomizableEdges = customizableEdges189;
            guna2CustomGradientPanel1.FillColor = Color.Transparent;
            guna2CustomGradientPanel1.FillColor2 = Color.Transparent;
            guna2CustomGradientPanel1.FillColor3 = Color.Transparent;
            guna2CustomGradientPanel1.FillColor4 = Color.Transparent;
            guna2CustomGradientPanel1.Location = new Point(1143, 271);
            guna2CustomGradientPanel1.Name = "guna2CustomGradientPanel1";
            guna2CustomGradientPanel1.ShadowDecoration.CustomizableEdges = customizableEdges190;
            guna2CustomGradientPanel1.Size = new Size(221, 169);
            guna2CustomGradientPanel1.TabIndex = 148;
            // 
            // guna2CustomGradientPanel2
            // 
            guna2CustomGradientPanel2.BorderColor = Color.FromArgb(255, 192, 128);
            guna2CustomGradientPanel2.BorderRadius = 5;
            guna2CustomGradientPanel2.BorderThickness = 4;
            guna2CustomGradientPanel2.Controls.Add(label2);
            guna2CustomGradientPanel2.Controls.Add(rbNormalStatus);
            guna2CustomGradientPanel2.Controls.Add(rbMaintenanceStatus);
            guna2CustomGradientPanel2.CustomizableEdges = customizableEdges191;
            guna2CustomGradientPanel2.FillColor = Color.Transparent;
            guna2CustomGradientPanel2.FillColor2 = Color.Transparent;
            guna2CustomGradientPanel2.FillColor3 = Color.Transparent;
            guna2CustomGradientPanel2.FillColor4 = Color.Transparent;
            guna2CustomGradientPanel2.Location = new Point(1143, 483);
            guna2CustomGradientPanel2.Name = "guna2CustomGradientPanel2";
            guna2CustomGradientPanel2.ShadowDecoration.CustomizableEdges = customizableEdges192;
            guna2CustomGradientPanel2.Size = new Size(221, 169);
            guna2CustomGradientPanel2.TabIndex = 149;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(39, 25);
            label2.Name = "label2";
            label2.Size = new Size(156, 38);
            label2.TabIndex = 0;
            label2.Text = "TÌNH TRẠNG";
            // 
            // guna2CustomGradientPanel3
            // 
            guna2CustomGradientPanel3.BorderColor = Color.Gainsboro;
            guna2CustomGradientPanel3.BorderRadius = 2;
            guna2CustomGradientPanel3.BorderThickness = 4;
            guna2CustomGradientPanel3.Controls.Add(lblLegendNormal);
            guna2CustomGradientPanel3.Controls.Add(guna2Button94);
            guna2CustomGradientPanel3.Controls.Add(guna2Button91);
            guna2CustomGradientPanel3.Controls.Add(lblLegendVIP);
            guna2CustomGradientPanel3.Controls.Add(label1);
            guna2CustomGradientPanel3.Controls.Add(guna2Button92);
            guna2CustomGradientPanel3.Controls.Add(lblLegendMaintenance);
            guna2CustomGradientPanel3.Controls.Add(guna2Button93);
            guna2CustomGradientPanel3.CustomizableEdges = customizableEdges193;
            guna2CustomGradientPanel3.FillColor = Color.Transparent;
            guna2CustomGradientPanel3.FillColor2 = Color.Transparent;
            guna2CustomGradientPanel3.FillColor3 = Color.Transparent;
            guna2CustomGradientPanel3.FillColor4 = Color.Transparent;
            guna2CustomGradientPanel3.Location = new Point(159, 778);
            guna2CustomGradientPanel3.Name = "guna2CustomGradientPanel3";
            guna2CustomGradientPanel3.ShadowDecoration.CustomizableEdges = customizableEdges194;
            guna2CustomGradientPanel3.Size = new Size(903, 84);
            guna2CustomGradientPanel3.TabIndex = 149;
            // 
            // guna2Button95
            // 
            guna2Button95.BorderRadius = 5;
            guna2Button95.CustomizableEdges = customizableEdges195;
            guna2Button95.DisabledState.BorderColor = Color.DarkGray;
            guna2Button95.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button95.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button95.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button95.FillColor = Color.Silver;
            guna2Button95.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button95.ForeColor = Color.White;
            guna2Button95.Location = new Point(597, 78);
            guna2Button95.Name = "guna2Button95";
            guna2Button95.ShadowDecoration.CustomizableEdges = customizableEdges196;
            guna2Button95.Size = new Size(104, 37);
            guna2Button95.TabIndex = 155;
            guna2Button95.Text = "Phòng 5";
            // 
            // guna2Button96
            // 
            guna2Button96.BorderRadius = 5;
            guna2Button96.CustomizableEdges = customizableEdges197;
            guna2Button96.DisabledState.BorderColor = Color.DarkGray;
            guna2Button96.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button96.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button96.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button96.FillColor = Color.Silver;
            guna2Button96.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button96.ForeColor = Color.White;
            guna2Button96.Location = new Point(482, 78);
            guna2Button96.Name = "guna2Button96";
            guna2Button96.ShadowDecoration.CustomizableEdges = customizableEdges198;
            guna2Button96.Size = new Size(108, 37);
            guna2Button96.TabIndex = 154;
            guna2Button96.Text = "Phòng 4";
            // 
            // guna2Button97
            // 
            guna2Button97.BorderRadius = 5;
            guna2Button97.CustomizableEdges = customizableEdges199;
            guna2Button97.DisabledState.BorderColor = Color.DarkGray;
            guna2Button97.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button97.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button97.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button97.FillColor = Color.Silver;
            guna2Button97.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button97.ForeColor = Color.White;
            guna2Button97.Location = new Point(366, 78);
            guna2Button97.Name = "guna2Button97";
            guna2Button97.ShadowDecoration.CustomizableEdges = customizableEdges200;
            guna2Button97.Size = new Size(108, 37);
            guna2Button97.TabIndex = 153;
            guna2Button97.Text = "Phòng 3";
            // 
            // guna2Button98
            // 
            guna2Button98.BorderRadius = 5;
            guna2Button98.CustomizableEdges = customizableEdges201;
            guna2Button98.DisabledState.BorderColor = Color.DarkGray;
            guna2Button98.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button98.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button98.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button98.FillColor = Color.Silver;
            guna2Button98.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button98.ForeColor = Color.White;
            guna2Button98.Location = new Point(248, 78);
            guna2Button98.Name = "guna2Button98";
            guna2Button98.ShadowDecoration.CustomizableEdges = customizableEdges202;
            guna2Button98.Size = new Size(109, 37);
            guna2Button98.TabIndex = 152;
            guna2Button98.Text = "Phòng 2";
            // 
            // guna2Button99
            // 
            guna2Button99.BorderRadius = 5;
            guna2Button99.CustomizableEdges = customizableEdges203;
            guna2Button99.DisabledState.BorderColor = Color.DarkGray;
            guna2Button99.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button99.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button99.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button99.FillColor = Color.Silver;
            guna2Button99.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button99.ForeColor = Color.White;
            guna2Button99.Location = new Point(132, 78);
            guna2Button99.Name = "guna2Button99";
            guna2Button99.ShadowDecoration.CustomizableEdges = customizableEdges204;
            guna2Button99.Size = new Size(107, 37);
            guna2Button99.TabIndex = 151;
            guna2Button99.Text = "Phòng 1";
            // 
            // guna2Separator1
            // 
            guna2Separator1.Location = new Point(123, 141);
            guna2Separator1.Name = "guna2Separator1";
            guna2Separator1.Size = new Size(1241, 12);
            guna2Separator1.TabIndex = 156;
            // 
            // btnThemPhim
            // 
            btnThemPhim.BorderRadius = 5;
            btnThemPhim.CustomizableEdges = customizableEdges205;
            btnThemPhim.DisabledState.BorderColor = Color.DarkGray;
            btnThemPhim.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThemPhim.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThemPhim.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThemPhim.FillColor = Color.FromArgb(255, 128, 0);
            btnThemPhim.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThemPhim.ForeColor = Color.White;
            btnThemPhim.Location = new Point(1188, 692);
            btnThemPhim.Name = "btnThemPhim";
            btnThemPhim.ShadowDecoration.CustomizableEdges = customizableEdges206;
            btnThemPhim.Size = new Size(138, 47);
            btnThemPhim.TabIndex = 157;
            btnThemPhim.Text = "Lưu";
            // 
            // FormRoomLayoutManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(88, 115, 140);
            ClientSize = new Size(1420, 938);
            Controls.Add(btnThemPhim);
            Controls.Add(guna2Separator1);
            Controls.Add(guna2Button95);
            Controls.Add(guna2Button96);
            Controls.Add(guna2Button97);
            Controls.Add(guna2Button98);
            Controls.Add(guna2Button99);
            Controls.Add(guna2CustomGradientPanel3);
            Controls.Add(guna2CustomGradientPanel2);
            Controls.Add(guna2CustomGradientPanel1);
            Controls.Add(guna2Button76);
            Controls.Add(guna2Button77);
            Controls.Add(guna2Button78);
            Controls.Add(guna2Button79);
            Controls.Add(guna2Button80);
            Controls.Add(guna2Button81);
            Controls.Add(guna2Button82);
            Controls.Add(guna2Button83);
            Controls.Add(guna2Button84);
            Controls.Add(guna2Button85);
            Controls.Add(guna2Button86);
            Controls.Add(guna2Button87);
            Controls.Add(guna2Button88);
            Controls.Add(guna2Button89);
            Controls.Add(guna2Button90);
            Controls.Add(guna2Button61);
            Controls.Add(guna2Button62);
            Controls.Add(guna2Button63);
            Controls.Add(guna2Button64);
            Controls.Add(guna2Button65);
            Controls.Add(guna2Button66);
            Controls.Add(guna2Button67);
            Controls.Add(guna2Button68);
            Controls.Add(guna2Button69);
            Controls.Add(guna2Button70);
            Controls.Add(guna2Button71);
            Controls.Add(guna2Button72);
            Controls.Add(guna2Button73);
            Controls.Add(guna2Button74);
            Controls.Add(guna2Button75);
            Controls.Add(guna2Button58);
            Controls.Add(guna2Button46);
            Controls.Add(guna2Button47);
            Controls.Add(guna2Button48);
            Controls.Add(guna2Button49);
            Controls.Add(guna2Button50);
            Controls.Add(guna2Button51);
            Controls.Add(guna2Button52);
            Controls.Add(guna2Button53);
            Controls.Add(guna2Button54);
            Controls.Add(guna2Button55);
            Controls.Add(guna2Button56);
            Controls.Add(guna2Button57);
            Controls.Add(guna2Button59);
            Controls.Add(guna2Button60);
            Controls.Add(guna2Button31);
            Controls.Add(guna2Button32);
            Controls.Add(guna2Button33);
            Controls.Add(guna2Button34);
            Controls.Add(guna2Button35);
            Controls.Add(guna2Button36);
            Controls.Add(guna2Button37);
            Controls.Add(guna2Button38);
            Controls.Add(guna2Button39);
            Controls.Add(guna2Button40);
            Controls.Add(guna2Button41);
            Controls.Add(guna2Button42);
            Controls.Add(guna2Button43);
            Controls.Add(guna2Button44);
            Controls.Add(guna2Button45);
            Controls.Add(guna2Button13);
            Controls.Add(guna2Button17);
            Controls.Add(guna2Button18);
            Controls.Add(guna2Button19);
            Controls.Add(guna2Button20);
            Controls.Add(guna2Button21);
            Controls.Add(guna2Button22);
            Controls.Add(guna2Button23);
            Controls.Add(guna2Button24);
            Controls.Add(guna2Button25);
            Controls.Add(guna2Button26);
            Controls.Add(guna2Button27);
            Controls.Add(guna2Button28);
            Controls.Add(guna2Button29);
            Controls.Add(guna2Button30);
            Controls.Add(guna2Button8);
            Controls.Add(guna2Button9);
            Controls.Add(guna2Button10);
            Controls.Add(guna2Button11);
            Controls.Add(guna2Button12);
            Controls.Add(guna2Button14);
            Controls.Add(guna2Button16);
            Controls.Add(guna2Button7);
            Controls.Add(guna2Button6);
            Controls.Add(guna2Button5);
            Controls.Add(guna2Button4);
            Controls.Add(guna2Button3);
            Controls.Add(guna2Button2);
            Controls.Add(guna2Button1);
            Controls.Add(guna2Button15);
            Controls.Add(lblTitle);
            Controls.Add(lblScreen);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormRoomLayoutManagement";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản Lý Ghế Rạp Phim";
            Load += FormRoomLayoutManagement_Load;
            guna2CustomGradientPanel1.ResumeLayout(false);
            guna2CustomGradientPanel2.ResumeLayout(false);
            guna2CustomGradientPanel3.ResumeLayout(false);
            ResumeLayout(false);
        }


        private Button CreateSingleSeat(string seatId, int x, int y, int size, System.Drawing.Color backColor, System.Drawing.Color foreColor)
            {
                Button btnSeat = new Button();
                btnSeat.Name = "btn" + seatId;
                btnSeat.Location = new System.Drawing.Point(x, y);
                btnSeat.Size = new System.Drawing.Size(size, size);
                btnSeat.Text = seatId;
                btnSeat.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
                btnSeat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btnSeat.Cursor = System.Windows.Forms.Cursors.Hand;
                btnSeat.Tag = seatId;
                btnSeat.BackColor = backColor;
                btnSeat.ForeColor = foreColor;
                btnSeat.FlatAppearance.BorderSize = 1;
                btnSeat.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(100, 100, 100);
                btnSeat.Click += new System.EventHandler(this.SeatButton_Click);
                return btnSeat;
            }

            private void SetMaintenanceSeats(string row, int fromCol, int toCol)
            {
                Button btn10 = seatButtons[row + "10"];
                btn10.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn10.ForeColor = System.Drawing.Color.White;

                Button btn11 = seatButtons[row + "11"];
                btn11.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn11.ForeColor = System.Drawing.Color.White;

                Button btn12 = seatButtons[row + "12"];
                btn12.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn12.ForeColor = System.Drawing.Color.White;

                Button btn13 = seatButtons[row + "13"];
                btn13.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn13.ForeColor = System.Drawing.Color.White;

                Button btn14 = seatButtons[row + "14"];
                btn14.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn14.ForeColor = System.Drawing.Color.White;

                Button btn15 = seatButtons[row + "15"];
                btn15.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn15.ForeColor = System.Drawing.Color.White;
            }

            private void SetSelectedSeat(string seatId)
            {
                Button btn = seatButtons[seatId];
                btn.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            }

            // Event handlers - Bạn cần implement trong file .cs chính
            private void SeatButton_Click(object sender, System.EventArgs e)
            {
                Button clickedSeat = sender as Button;
                string seatId = clickedSeat.Tag.ToString();
                System.Windows.Forms.MessageBox.Show("Đã click vào ghế: " + seatId);
            }

            private void SeatType_CheckedChanged(object sender, System.EventArgs e)
            {
                // Xử lý khi thay đổi loại ghế
            }

            private void SeatStatus_CheckedChanged(object sender, System.EventArgs e)
            {
            // Xử lý khi thay đổi tình trạng ghế
        }
        private Guna.UI2.WinForms.Guna2Button guna2Button15;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button guna2Button3;
        private Guna.UI2.WinForms.Guna2Button guna2Button4;
        private Guna.UI2.WinForms.Guna2Button guna2Button5;
        private Guna.UI2.WinForms.Guna2Button guna2Button6;
        private Guna.UI2.WinForms.Guna2Button guna2Button7;
        private Guna.UI2.WinForms.Guna2Button guna2Button14;
        private Guna.UI2.WinForms.Guna2Button guna2Button16;
        private Guna.UI2.WinForms.Guna2Button guna2Button8;
        private Guna.UI2.WinForms.Guna2Button guna2Button9;
        private Guna.UI2.WinForms.Guna2Button guna2Button10;
        private Guna.UI2.WinForms.Guna2Button guna2Button11;
        private Guna.UI2.WinForms.Guna2Button guna2Button12;
        private Guna.UI2.WinForms.Guna2Button guna2Button13;
        private Guna.UI2.WinForms.Guna2Button guna2Button17;
        private Guna.UI2.WinForms.Guna2Button guna2Button18;
        private Guna.UI2.WinForms.Guna2Button guna2Button19;
        private Guna.UI2.WinForms.Guna2Button guna2Button20;
        private Guna.UI2.WinForms.Guna2Button guna2Button21;
        private Guna.UI2.WinForms.Guna2Button guna2Button22;
        private Guna.UI2.WinForms.Guna2Button guna2Button23;
        private Guna.UI2.WinForms.Guna2Button guna2Button24;
        private Guna.UI2.WinForms.Guna2Button guna2Button25;
        private Guna.UI2.WinForms.Guna2Button guna2Button26;
        private Guna.UI2.WinForms.Guna2Button guna2Button27;
        private Guna.UI2.WinForms.Guna2Button guna2Button28;
        private Guna.UI2.WinForms.Guna2Button guna2Button29;
        private Guna.UI2.WinForms.Guna2Button guna2Button30;
        private Guna.UI2.WinForms.Guna2Button guna2Button31;
        private Guna.UI2.WinForms.Guna2Button guna2Button32;
        private Guna.UI2.WinForms.Guna2Button guna2Button33;
        private Guna.UI2.WinForms.Guna2Button guna2Button34;
        private Guna.UI2.WinForms.Guna2Button guna2Button35;
        private Guna.UI2.WinForms.Guna2Button guna2Button36;
        private Guna.UI2.WinForms.Guna2Button guna2Button37;
        private Guna.UI2.WinForms.Guna2Button guna2Button38;
        private Guna.UI2.WinForms.Guna2Button guna2Button39;
        private Guna.UI2.WinForms.Guna2Button guna2Button40;
        private Guna.UI2.WinForms.Guna2Button guna2Button41;
        private Guna.UI2.WinForms.Guna2Button guna2Button42;
        private Guna.UI2.WinForms.Guna2Button guna2Button43;
        private Guna.UI2.WinForms.Guna2Button guna2Button44;
        private Guna.UI2.WinForms.Guna2Button guna2Button45;
        private Guna.UI2.WinForms.Guna2Button guna2Button53;
        private Guna.UI2.WinForms.Guna2Button guna2Button54;
        private Guna.UI2.WinForms.Guna2Button guna2Button55;
        private Guna.UI2.WinForms.Guna2Button guna2Button56;
        private Guna.UI2.WinForms.Guna2Button guna2Button57;
        private Guna.UI2.WinForms.Guna2Button guna2Button59;
        private Guna.UI2.WinForms.Guna2Button guna2Button60;
        private Guna.UI2.WinForms.Guna2Button guna2Button46;
        private Guna.UI2.WinForms.Guna2Button guna2Button47;
        private Guna.UI2.WinForms.Guna2Button guna2Button48;
        private Guna.UI2.WinForms.Guna2Button guna2Button49;
        private Guna.UI2.WinForms.Guna2Button guna2Button50;
        private Guna.UI2.WinForms.Guna2Button guna2Button51;
        private Guna.UI2.WinForms.Guna2Button guna2Button52;
        private Guna.UI2.WinForms.Guna2Button guna2Button58;
        private Guna.UI2.WinForms.Guna2Button guna2Button61;
        private Guna.UI2.WinForms.Guna2Button guna2Button62;
        private Guna.UI2.WinForms.Guna2Button guna2Button63;
        private Guna.UI2.WinForms.Guna2Button guna2Button64;
        private Guna.UI2.WinForms.Guna2Button guna2Button65;
        private Guna.UI2.WinForms.Guna2Button guna2Button66;
        private Guna.UI2.WinForms.Guna2Button guna2Button67;
        private Guna.UI2.WinForms.Guna2Button guna2Button68;
        private Guna.UI2.WinForms.Guna2Button guna2Button69;
        private Guna.UI2.WinForms.Guna2Button guna2Button70;
        private Guna.UI2.WinForms.Guna2Button guna2Button71;
        private Guna.UI2.WinForms.Guna2Button guna2Button72;
        private Guna.UI2.WinForms.Guna2Button guna2Button73;
        private Guna.UI2.WinForms.Guna2Button guna2Button74;
        private Guna.UI2.WinForms.Guna2Button guna2Button75;
        private Guna.UI2.WinForms.Guna2Button guna2Button76;
        private Guna.UI2.WinForms.Guna2Button guna2Button77;
        private Guna.UI2.WinForms.Guna2Button guna2Button78;
        private Guna.UI2.WinForms.Guna2Button guna2Button79;
        private Guna.UI2.WinForms.Guna2Button guna2Button80;
        private Guna.UI2.WinForms.Guna2Button guna2Button81;
        private Guna.UI2.WinForms.Guna2Button guna2Button82;
        private Guna.UI2.WinForms.Guna2Button guna2Button83;
        private Guna.UI2.WinForms.Guna2Button guna2Button84;
        private Guna.UI2.WinForms.Guna2Button guna2Button85;
        private Guna.UI2.WinForms.Guna2Button guna2Button86;
        private Guna.UI2.WinForms.Guna2Button guna2Button87;
        private Guna.UI2.WinForms.Guna2Button guna2Button88;
        private Guna.UI2.WinForms.Guna2Button guna2Button89;
        private Guna.UI2.WinForms.Guna2Button guna2Button90;
        private Guna.UI2.WinForms.Guna2Button guna2Button92;
        private Guna.UI2.WinForms.Guna2Button guna2Button91;
        private Guna.UI2.WinForms.Guna2Button guna2Button93;
        private Guna.UI2.WinForms.Guna2Button guna2Button94;
        private Label label1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel2;
        private Label label2;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel3;
        private Guna.UI2.WinForms.Guna2Button guna2Button95;
        private Guna.UI2.WinForms.Guna2Button guna2Button96;
        private Guna.UI2.WinForms.Guna2Button guna2Button97;
        private Guna.UI2.WinForms.Guna2Button guna2Button98;
        private Guna.UI2.WinForms.Guna2Button guna2Button99;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2Button btnThemPhim;
    }
    }




