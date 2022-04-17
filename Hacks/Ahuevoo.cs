using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Memory;
using Wenardo;

namespace Legit
{
    public partial class Ahuevoo : Form
    {

        Graphics g;

        List<ESPPP> Jugadores = new List<ESPPP>();

        Pen TeamPen = new Pen(Color.Blue, 2);
        Font siuu = new Font(new FontFamily("Calibri"), 15);
        Brush ajaa = new SolidBrush(Color.Red);
        Brush ajaaa = new SolidBrush(Color.Blue);
        Pen CabezaPen = new Pen(Color.Purple, 5);
        Pen snapLine = new Pen(Color.Red, 1);
        Pen Healthbar = new Pen(Color.Red, 5);
        Pen ArmorBarr = new Pen(Color.Blue, 5);


        #region OffsetsNoFijos
        string VIEWMATRIX = "client.dll+" + (signatures.dwViewMatrix).ToString("X");
        string LOCALPLAYER = "client.dll+" + (signatures.dwLocalPlayer).ToString("X");
        int entityoff1 = signatures.dwEntityList;
        int entityobject = signatures.dwEntityList + 0x7A0;//7A0
        #endregion

        #region Offsets
        string Armour = ",117CC";
        string entitylist1 = "client.dll+";
        string HP = ",100";
        string X = ",138";
        string Z = ",13C";
        string Y = ",140";
        string team = ",F4";
        string dormant = ",ED";
        string flags = ",104";
        string BonecabezaX = ",18C";
        string BonecabezaZ = ",19C";
        string BonecabezaY = ",1AC";
        string BoneMatrix = ",26A8";
        #endregion

        int MyTeam;

        Mem m = new Mem();
        Overlay over = new Overlay();
        public Ahuevoo()
        {
            InitializeComponent();
        }

        private void Ahuevoo_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            int PID = m.GetProcIdFromName("csgo.exe");
            if (PID > 0)
            {
                m.OpenProcess(PID);


                over.cosas(this);

                over.Tamaño("Counter-Strike: Global Offensive - Direct3D 9", this);

                Thread LB = new Thread(ESP) { IsBackground = true };
                LB.Start();
            }
        }


        void ESP()
        {
            while (true)
            {
                Jugadores.Clear();
                MyTeam = m.ReadInt(LOCALPLAYER + team);
                for (int i = 0; i < 64; i++)
                {
                    string CurrentPlayer = entitylist1 + (entityoff1 + i * 0x10).ToString("X");

                    int Vida = m.ReadInt(CurrentPlayer + HP);

                    int Dormant = m.ReadInt(CurrentPlayer + dormant);

                    if (Dormant != 0 || Vida < 1 || Vida > 100)
                        continue;

                    float Xpos = m.ReadFloat(CurrentPlayer + X);
                    float Zpos = m.ReadFloat(CurrentPlayer + Z);
                    float Ypos = m.ReadFloat(CurrentPlayer + Y);

                    int Armor = m.ReadInt(CurrentPlayer + Armour);

                    int M_MFlags = m.ReadInt(CurrentPlayer + flags);

                    float CabezaX = m.ReadFloat(CurrentPlayer + BoneMatrix + BonecabezaX);
                    float CabezaZ = m.ReadFloat(CurrentPlayer + BoneMatrix + BonecabezaZ);
                    float CabezaY = m.ReadFloat(CurrentPlayer + BoneMatrix + BonecabezaY);

                    int Teamnumm = m.ReadInt(CurrentPlayer + team);

                    var Jugador = new ESPPP
                    {
                        X = Xpos,
                        Z = Zpos,
                        Y = Ypos,
                        VidaA = Vida,
                        dormantt = Dormant,
                        m_flag = M_MFlags,
                        cabezaX = CabezaX,
                        cabezaZ = CabezaZ,
                        cabezaY = CabezaY,
                        Teamnum = Teamnumm,
                        armour = Armor
                    };

                    Jugador.bottom = WorldToScreen(IntoTheMatrix(),Jugador.X, Jugador.Z, Jugador.Y, Jugador, Width, Height, false, false);
                    Jugador.top = WorldToScreen(IntoTheMatrix(), Jugador.X, Jugador.Z, Jugador.Y, Jugador, Width, Height, true, false);
                    Jugador.bonecabeza = WorldToScreen(IntoTheMatrix(), Jugador.cabezaX, Jugador.cabezaZ, Jugador.cabezaY, Jugador, Width, Height, false, false);
                    Jugador.bonecabezatop = WorldToScreen(IntoTheMatrix(), Jugador.cabezaX, Jugador.cabezaZ, Jugador.cabezaY, Jugador, Width, Height, false, true); ;
                    Jugadores.Add(Jugador);
                }
                panel1.Refresh();
                Thread.Sleep(10);
            }    
            

        }

        Point WorldToScreen(ViewMatrix mtx,float Xcord,float Zcord, float Ycord, ESPPP ent, int width, int height, bool head,bool headscuare)
        {

            if (head && ent.m_flag != 263 && ent.m_flag != 775 && headscuare == false)
                Ycord += 72;

            if (head && ent.m_flag == 775 || head && ent.m_flag == 263 && headscuare == false)
                Ycord += 50;

            if (headscuare == true)
            {
                Ycord += 8;
            }


            var twoD = new Point();

            float screenW = (mtx.m14 * Xcord) + (mtx.m24 * Zcord) + (mtx.m34 * Ycord) + mtx.m44;

            if (screenW > 0.001f)
            {
                float screenX = (mtx.m11 * Xcord) + (mtx.m21 * Zcord) + (mtx.m31 * Ycord) + mtx.m41;

                float screenY = (mtx.m12 * Xcord) + (mtx.m22 * Zcord) + (mtx.m32 * Ycord) + mtx.m42;


                float camX = width / 2f;
                float camY = height / 2f;


                float X = camX + (camX * screenX / screenW);
                float Y = camY - (camY * screenY / screenW);


                twoD.X = (int)X;
                twoD.Y = (int)Y;

                return twoD;

            }
            else
            {
                return new Point(-99, -99);
            }
        }




        Pen EnemyHP(int hp)
        {
            if (hp > 80)
                return new Pen(Color.FromArgb(16, 255, 0), 3);
            else if (hp > 60)
                return new Pen(Color.FromArgb(64, 204, 0), 3);
            else if (hp > 40)
                return new Pen(Color.FromArgb(112, 153, 0), 3);
            else if (hp > 20)
                return new Pen(Color.FromArgb(159, 102, 0), 3);
            else if (hp > 1)
                return new Pen(Color.FromArgb(207, 51, 0), 3);
            else if (hp > 1)
                return new Pen(Color.FromArgb(255, 0, 0), 3);
            return new Pen(Color.Black);
        }


        ViewMatrix IntoTheMatrix()
        {
            var matrix = new ViewMatrix();

            byte[] buffer = new byte[16 * 4];

            var bytes = m.ReadBytes(VIEWMATRIX, (long)buffer.Length);

            matrix.m11 = BitConverter.ToSingle(bytes, (0 * 4));
            matrix.m12 = BitConverter.ToSingle(bytes, (4 * 4));
            matrix.m13 = BitConverter.ToSingle(bytes, (8 * 4));
            matrix.m14 = BitConverter.ToSingle(bytes, (12 * 4));

            matrix.m21 = BitConverter.ToSingle(bytes, (1 * 4));
            matrix.m22 = BitConverter.ToSingle(bytes, (5 * 4));
            matrix.m23 = BitConverter.ToSingle(bytes, (9 * 4));
            matrix.m24 = BitConverter.ToSingle(bytes, (13 * 4));

            matrix.m31 = BitConverter.ToSingle(bytes, (2 * 4));
            matrix.m32 = BitConverter.ToSingle(bytes, (6 * 4));
            matrix.m33 = BitConverter.ToSingle(bytes, (10 * 4));
            matrix.m34 = BitConverter.ToSingle(bytes, (14 * 4));

            matrix.m41 = BitConverter.ToSingle(bytes, (3 * 4));
            matrix.m42 = BitConverter.ToSingle(bytes, (7 * 4));
            matrix.m43 = BitConverter.ToSingle(bytes, (11 * 4));
            matrix.m44 = BitConverter.ToSingle(bytes, (15 * 4));

            return matrix;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;

            foreach (var ent in Jugadores)
            {
                try
                {
                    if (ent.Teamnum == MyTeam && ent.VidaA > 0 && ent.VidaA < 102)
                    {
                        if (ent.bottom.X > 0 && ent.bottom.Y < Height && ent.bottom.X < Width && ent.bottom.Y > 0)
                        {
                            g.DrawString(ent.VidaA.ToString() + "%", siuu, ajaaa, ent.bottom.X, ent.bottom.Y);
                            g.DrawRectangle(TeamPen, ent.rect());
                        }
                    }
                    else
                    {
                        if (ent.bottom.X > 0 && ent.bottom.Y < Height && ent.bottom.X < Width && ent.bottom.Y > 0)
                        {
                            g.DrawRectangle(CabezaPen, ent.headrect());
                            g.DrawLine(snapLine, Width / 2, Height / 2 ,ent.bonecabeza.X, ent.bonecabeza.Y);
                            g.DrawString(ent.VidaA.ToString() + "%", siuu, ajaa, ent.bottom.X, ent.bottom.Y);
                            g.DrawRectangle(EnemyHP(ent.VidaA), ent.rect());
                            g.DrawRectangle(Healthbar, ent.healthbar(ent));
                            g.DrawRectangle(ArmorBarr, ent.ArmourBar(ent));
                        }
                    }

                }
                catch
                {

                }
            }
        }
    }
}
