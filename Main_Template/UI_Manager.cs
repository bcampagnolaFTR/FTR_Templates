using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crestron.SimplSharpPro.UI;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;
using FTR_Utils;
using VC = MastersHelperLibrary.VirtualConsole;

namespace Main_Template
{
    public class UI_Manager
    {
        private string filePath = "/opt/crestron/virtualcontrol/RunningPrograms/MAIN/User/";
        private string panelStation_sgd_file = "AZMC_RI_DGE200_v0.02.00.sgd";

        private CrestronControlSystem cs;

        Dictionary<int, BasicTriListWithSmartObject> panel = new Dictionary<int, BasicTriListWithSmartObject>();
        
        public UI_Manager(CrestronControlSystem _cs)
        {
            cs = _cs;


            //AddPanel(new XpanelForSmartGraphics(0x20, cs));
            //AddPanel(new XpanelForSmartGraphics(0x21, cs));
            //AddPanel(new XpanelForSmartGraphics(0x22, cs));

            VC.AddNewConsoleCommand(VC_Register, "reg", "");
            VC.AddNewConsoleCommand(VC_LoadSG, "loadsg", "");
            VC.AddNewConsoleCommand(VC_RegisterSG, "regsg", "");
        }

        public string VC_Register(string s)
        {
            try
            {
                panel.Add(0x20, new XpanelForSmartGraphics(0x20, cs));
                panel[0x20].Register();
                panel[0x20].SigChange += panel_SigChange;

            }
            catch(Exception e)
            {
                VC.Send(String.Format(">>> {0}", e));
            }
            return "";
        }
        public string VC_LoadSG(string s)
        {
            uint i;
            try
            {
                i = panel[0x20].LoadSmartObjects(String.Format("{0}{1}", filePath, panelStation_sgd_file));
                if (i > 0)
                    for (uint j = 1; j <= i; j++)
                    {
                        VC.Send(String.Format(">>> {0}", panel[0x20].SmartObjects[j].ToString()));
                        panel[0x20].SmartObjects[j].SigChange += panel_SigChange_SG;

                    }
                else VC.Send(">>> NumOf SG Loaded = 0");
            }
            catch (Exception e)
            {
                VC.Send(String.Format(">>> {0}", e));
            }
            return "";
        }

        public string VC_RegisterSG(string s)
        {
            return "";
        }

        public void AddPanel(BasicTriListWithSmartObject _panel)
        {
            AddPanel(panel.Count() + 1, _panel);
        }
        public void AddPanel(int id, BasicTriListWithSmartObject _panel)
        {
            uint i;

            panel.Add(id, _panel);

            panel[id].Register();

            panel[id].SigChange += panel_SigChange;

            i = panel[id].LoadSmartObjects(String.Format("{0}{1}", filePath, panelStation_sgd_file));
            if (i > 0)
                for (uint j = 1; j <= i; j++)
                    panel[id].SmartObjects[j].SigChange += panel_SigChange_SG;

        }

        private void panel_SigChange(BasicTriList b, SigEventArgs args)
        {
            //VC.Send(">>> in panel_SigChange");
            panel_SigInfo(args.Sig.Type, args.Sig.Number, 0, b.ID);
        }

        private void panel_SigChange_SG(GenericBase b, SmartObjectEventArgs args)
        {
            //VC.Send(">>> in panel_SigChange_SG");
            panel_SigInfo(args.Sig.Type, args.Sig.Number, args.SmartObjectArgs.ID, b.ID);
        }

        private void panel_SigInfo(eSigType _type, uint _n, uint _smartID, uint _panelID)
        {
            PanelSigInfo psi = new PanelSigInfo(_type, _n, _panelID);
            VC.Send(String.Format(">>> PanelSig - type={0} n={1} sgID={2} panelID={3}", _type, _n, _smartID, _panelID));
        }

        public string SigValue(SigEventArgs args)
        {
            if (args.Sig.Type == eSigType.Bool) return args.Sig.BoolValue.ToString();
            else if (args.Sig.Type == eSigType.UShort) return args.Sig.UShortValue.ToString();
            else if (args.Sig.Type == eSigType.String) return args.Sig.StringValue;
            return "";
        }
    }
}
