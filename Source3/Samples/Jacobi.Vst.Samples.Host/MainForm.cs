using Jacobi.Vst3.Host;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Jacobi.Vst.Samples.Host
{
    partial class MainForm : Form
    {
        private List<Module> _plugins = new List<Module>();
        //PlugProvider _plugProvider;
        //AudioClient _vst3Processor;

        public MainForm()
        {
            InitializeComponent();
            Text = "VST.NET 3 Dummy Host Sample";
        }

        private void FillPluginList()
        {
            PluginListVw.Items.Clear();

            foreach (Module ctx in _plugins)
            {
                ListViewItem lvItem = new ListViewItem("ctx.PluginCommandStub.Commands.GetEffectName()");
                lvItem.SubItems.Add("ctx.PluginCommandStub.Commands.GetProductString()");
                lvItem.SubItems.Add("ctx.PluginCommandStub.Commands.GetVendorString()");
                lvItem.SubItems.Add("ctx.PluginCommandStub.Commands.GetVendorVersion().ToString()");
                lvItem.SubItems.Add("ctx.Find<string>('PluginPath')");
                lvItem.Tag = ctx;

                PluginListVw.Items.Add(lvItem);
            }
        }

        private Module OpenPlugin(string pluginPath)
        {
            try
            {
                var ctx = Module.Create(pluginPath, out var error);
                if (ctx == null)
                    throw new Exception($"Could not create Module for file:{pluginPath}\nError: {error}");

                //// add custom data to the context
                //ctx.Set("PluginPath", pluginPath);
                //ctx.Set("HostCmdStub", hostCmdStub);

                //// actually open the plugin itself
                //ctx.PluginCommandStub.Commands.Open();

                return ctx;
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        private void ReleaseAllPlugins()
        {
            foreach (Module ctx in _plugins)
            {
                // dispose of all (unmanaged) resources
                ctx.Dispose();
            }

            _plugins.Clear();
        }

        private Module SelectedPluginContext
        {
            get
            {
                if (PluginListVw.SelectedItems.Count > 0)
                {
                    return (Module)PluginListVw.SelectedItems[0].Tag;
                }

                return null;
            }
        }

        //private void HostCmdStub_PluginCalled(object sender, PluginCalledEventArgs e)
        //{
        //    //HostCommandStub hostCmdStub = (HostCommandStub)sender;

        //    // can be null when called from inside the plugin main entry point.
        //    if (hostCmdStub.PluginContext.PluginInfo != null)
        //    {
        //        Debug.WriteLine("Plugin " + hostCmdStub.PluginContext.PluginInfo.PluginID + " called:" + e.Message);
        //    }
        //    else
        //    {
        //        Debug.WriteLine("The loading Plugin called:" + e.Message);
        //    }
        //}

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDlg.FileName = PluginPathTxt.Text;

            if (OpenFileDlg.ShowDialog(this) == DialogResult.OK)
            {
                PluginPathTxt.Text = OpenFileDlg.FileName;
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            var ctx = OpenPlugin(PluginPathTxt.Text);

            if (ctx != null)
            {
                _plugins.Add(ctx);

                FillPluginList();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ReleaseAllPlugins();
        }

        private void ViewPluginBtn_Click(object sender, EventArgs e)
        {
            //PluginForm dlg = new PluginForm
            //{
            //    PluginContext = SelectedPluginContext
            //};

            //dlg.ShowDialog(this);
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            var ctx = SelectedPluginContext;

            if (ctx != null)
            {
                ctx.Dispose();

                _plugins.Remove(ctx);

                FillPluginList();
            }
        }
    }
}
