// ------------------------------------------------------------------------------------------------
// <copyright file="Home.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Pamux.GameDev.Tools.Tabs
{
    using Pamux.GameDev.Tools.Models;
    using System.Windows.Forms;

    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        private void txtVoiceSaveDirectory_TextChanged(object sender, System.EventArgs e)
        {
            Settings.Instance.VoiceSaveDirectory = txtVoiceSaveDirectory.Text;
        }
    }
}