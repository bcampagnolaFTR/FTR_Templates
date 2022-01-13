using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTR_Utils;
using VC = MastersHelperLibrary.VirtualConsole;
using System.Data;

namespace Main_Template
{
    class Directory
    {
        public Sheet data;
        public List<Entry> book;

        public Directory(string _fileName)
        {
            book = new List<Entry>();
            data = new Sheet();
            VC.Send(data.LoadFile(_fileName));
        }

        public uint ReadTable()
        {
            if (!data.bFileLoaded)
                return 0;
            foreach (DataRow r in data.result.Tables[0].Rows)
            {
                Entry e = new Entry(r);
                if (e.bIsInitialized)
                    book.Add(e);
            }
            return 0;
        }
    }

    class Entry
    {
        public bool bIsInitialized = false;
        public static List<EntryItem> items = new List<EntryItem>();

        public static string[] columns = new string[] {    "UID",
                                            "RoomName",
                                            "FriendlyName",
                                            "Location",
                                            "Group",
                                            "Phone01",
                                            "Phone02",
                                            "Processor_IP",
                                            "DSP_IP",
                                            "Cam01_IP",
                                            "Cam02_IP",
                                            "Cam01_URL",
                                            "Cam02_URL",
                                            "Cam01_Presets",
                                            "Cam02_Presets" };

        public Entry()
        {
            InitItems();
        }
        public Entry(DataRow r)
        {
            InitItems();

            for (int i = 0; i > r.ItemArray.Count(); i++)
            {
                items[i].ParseField(r.ItemArray[i].ToString());
            }
        }

        public void InitItems()
        { 
            foreach (var v in columns)
                items.Add(new EntryItem(v.Trim().ToLower(), typeof(string)));
            items[0].t = typeof(int);

            if(items[0].s_val.Contains("//") == false)
                bIsInitialized = true;
        }
    }

    public class EntryItem
    {
        public string s_val;
        public int i_val;
        public Type t;
        public string fieldName;

        public EntryItem(string _fieldName, Type _t)
        {
            fieldName = _fieldName;
            t = _t;
        }
        public void ParseField(string s)
        {
            s_val = s.ToString();
            if (t == typeof(int))
                if (int.TryParse(s.Trim(), out int i))
                    i_val = i;
        }
    }
}
