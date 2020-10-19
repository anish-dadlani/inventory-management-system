using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Medical_Store
{
    public partial class Back_up_Restore : Sample2
    {
        public Back_up_Restore()
        {
            InitializeComponent();
        }

        private void Back_up_Restore_Load(object sender, EventArgs e)
        {
            userLabel.Text = reterival.EMP_NAME;
        }

         public static void BackupDatabase(string backUpFile)
        {
            ServerConnection con = new ServerConnection(@"WINSOFTSYSTEMLR\SQLEXPRESS");
            Server server = new Server(con);
            Backup source = new Backup();
            source.Action = BackupActionType.Database;
            source.Database = "imsDB";
            BackupDeviceItem destination = new BackupDeviceItem(backUpFile, DeviceType.File);
            source.Devices.Add(destination);
            source.SqlBackup(server);
            con.Disconnect();
        }

        public static void RestoreDatabase(string backUpFile)
        {
            ServerConnection con = new ServerConnection(@"xxxxx\SQLEXPRESS");
            Server server = new Server(con);
            Restore destination = new Restore();
            destination.Action = RestoreActionType.Database;
            destination.Database = "imsDB";
            BackupDeviceItem source = new BackupDeviceItem(backUpFile, DeviceType.File);
            destination.Devices.Add(source);
            destination.ReplaceDatabase = true;
            destination.SqlRestore(server);
        }

        private void backUpBtn_Click(object sender, EventArgs e)
        {
            //BackupDatabase();
        }
    }
}
