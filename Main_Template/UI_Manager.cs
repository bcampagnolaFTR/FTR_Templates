using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crestron.SimplSharpPro.UI;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;
using FTR_Utils;

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

            AddPanel(new XpanelForSmartGraphics(0x20, cs));
            AddPanel(new XpanelForSmartGraphics(0x21, cs));
            AddPanel(new XpanelForSmartGraphics(0x22, cs));
        }

        public void AddPanel(BasicTriListWithSmartObject _panel)
        {
            AddPanel(panel.Count() + 1, _panel);
        }
        public void AddPanel(int id, BasicTriListWithSmartObject _panel)
        {
            uint i;

            panel.Add(id, _panel);
            i = panel[id].LoadSmartObjects(String.Format("{0}{1}", filePath, panelStation_sgd_file));
            
            panel[id].SigChange += panel_SigChange;
            if (i > 0)
                for (uint j = 1; j <= i; j++)
                    panel[id].SmartObjects[j].SigChange += panel_SigChange_SG;

        }

        private void panel_SigChange(BasicTriList b, SigEventArgs args)
        {
            panel_SigInfo(args.Sig.Type, args.Sig.Number, 0, b.ID);
        }

        private void panel_SigChange_SG(GenericBase b, SmartObjectEventArgs args)
        {
            panel_SigInfo(args.Sig.Type, args.Sig.Number, args.SmartObjectArgs.ID, b.ID);
        }

        private void panel_SigInfo(eSigType _type, uint _n, uint _smartID, uint _panelID)
        {
            PanelSigInfo psi = new PanelSigInfo(_type, _n, _panelID);
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
