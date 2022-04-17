using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Memory;
using System.Threading;
using System.Runtime.InteropServices;

namespace Legit
{
    public partial class SkinChanger : Form
    {

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        #region OffsetsNoFijos
        string Playerbase = "client.dll+" + (signatures.dwLocalPlayer).ToString("X");
        string Client = "client.dll+";
        string forceupdate = "engine.dll+" + signatures.dwClientState.ToString("X");
        #endregion

        Mem m = new Mem();

        float wear = 0.00001f;

        public SkinChanger()
        {
            InitializeComponent();
        }
        bool alive = false;


        private void SkinChanger_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            this.TopMost = true;
            int PID = m.GetProcIdFromName("csgo.exe");
            if (PID > 0)
            {
                m.OpenProcess(PID);
                Thread sk = new Thread(Skin) { IsBackground = true };
                sk.Start();
                Thread up = new Thread(upd) { IsBackground = true };
                up.Start();
            }
        }

        void Skin()
        {
            while (true)
            {
                    skinstuff();
            }
        }
        void upd()
        {
            while (true)
            {
                update();
                Vivida();
                Thread.Sleep(50);
            }
        }


        void skinstuff()
        {
            if (alive == true)
            {
                updateskin(4, 38);
                updateskin(1, 711);
                updateskin(7, 639);
                updateskin(9, 344);//475
                updateskin(61, 653);
                updateskin(60, 440);
                updateskin(40, 361);

            }

        }

        void updateskin(int gun ,int skin)
        {

            for (int i = 0; i < 8; i++)
            {
                int Currentweapon = m.ReadInt(Playerbase + "," + (netvars.m_hMyWeapons + i * 0x4).ToString("X"));
                Currentweapon = Currentweapon & 0xFFF;
                int weaponpointer = m.ReadInt(Client + (signatures.dwEntityList + (Currentweapon - 1) * 0x10).ToString("X"));

                if (weaponpointer != 0)
                {
                    int weaponid = m.Read2Byte((weaponpointer + netvars.m_iItemDefinitionIndex).ToString("X"));
                    if (weaponid == gun)
                    {
                        m.WriteMemory((weaponpointer + netvars.m_iItemIDHigh).ToString("X"), "int", "-1");
                        m.WriteMemory((weaponpointer + netvars.m_nFallbackPaintKit).ToString("X"), "int", skin.ToString());
                        m.WriteMemory((weaponpointer + netvars.m_flFallbackWear).ToString("X"), "float", wear.ToString());
                    }
                }

            }


        }

        void Vivida()
        {
            int vida = m.ReadInt(Playerbase + "," + (netvars.m_iHealth).ToString("X"));
            if (vida > 0 && vida < 102)
            {
                alive = true;
            }
            else
            {
                alive = false;
            }
        }

        void update()
        {
            if (GetAsyncKeyState(Keys.F6)<0)
            {
                m.WriteMemory(forceupdate + "," + (signatures.clientstate_delta_ticks).ToString("X"), "int", "-1");
            }
        }

        
    }
}
