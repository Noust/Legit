using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Memory;
using WindowsInput;
using WindowsInput.Native;
using System.Threading;
using System.Diagnostics;

namespace Legit
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetCursorPos(int X, int Y);

        #region OffsetsFijos
        string m_flFlashMaxAlpha = ",1046C";
        string engine = "engine.dll+";
        string m_bIsScoped = ",9974";
        string m_aimPunchAngleX = ",3040";
        string m_aimPunchAngleY = ",303C";
        string M_Mflags = ",104";
        string Client = "client.dll+";
        string FOVVVVVV = ",31F4";
        string FOVVVVVV1 = ",31F8";
        string FOV = ",333C";
        string Health = ",100";
        string TeamNum = ",F8";
        string BoneMatrix = ",26A8";
        string cabesaX = ",18C";
        string cabesaZ = ",19C";
        string cabesaY = ",1AC";
        string Dormant = ",ED";
        string viewmodel = ",3388";
        string Bspoted = ",93D";
        string m_clrRender = ",70";
        string Glowindex = ",10488";
        string m_iCrosshairId = ",11838";
        string m_iShotsFired = ",103E0";
        #endregion

        #region OffsetsNoFijos
        string VIEWMATRIX = "client.dll+" + (signatures.dwViewMatrix).ToString("X");
        string ForceJump = "client.dll+" + (signatures.dwForceJump).ToString("X");
        string Playerbase = "client.dll+" + (signatures.dwLocalPlayer).ToString("X");
        int entityoff = signatures.dwEntityList;
        string ViewX = "engine.dll+" + (signatures.dwClientState).ToString("X") + ",4D94";
        string ViewY = "engine.dll+" + (signatures.dwClientState).ToString("X") + ",4D90";
        int model_ambient_min1 = signatures.model_ambient_min;
        string GlowObjectManager = "client.dll+" + (signatures.dwGlowObjectManager).ToString("X");
        #endregion


        int rojo = 0x0000FF; 

        int Azul = 16711680;

        int fov;

        //int Blanco = 0xFF69B4;

        int chamsvalue;


        int apuntando = 1;
        int disparos;

        float oldpunchX;
        float oldpunchY;
        /*#region Offsets

        string VIEWMATRIX = "client.dll+4DC0494";
        string LOCALPLAYER = "client.dll+DB35EC";
        string entitylist1 = "client.dll+";
        int entityoff1 = 0x4DCEB7C;
        string HP = ",100";
        string XYZ = ",138";
        string team = ",F4";
        string dormant = ",ED";
        string flags = ",104";
        string BonecabezaX = ",18C";
        string BonecabezaZ = ",19C";
        string BonecabezaY = ",1AC";
        string BoneMatrix = ",26A8";

        #endregion*/


        Mem m = new Mem();
        InputSimulator sim = new InputSimulator();

        Ahuevoo whall = new Ahuevoo();
        SkinChanger changer = new SkinChanger();

        public Form1()
        {
            InitializeComponent();
        }
        bool ProcOpen = false;
        bool Open = false;
        bool SkinOpen = false;
        bool Alive = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            this.TopMost = true;
            int PID = m.GetProcIdFromName("csgo.exe");
            if (PID > 0)
            {
                m.OpenProcess(PID);
                Thread LB = new Thread(Labels) { IsBackground = true };
                LB.Start();
                Thread HCR = new Thread(HacksRapidos) { IsBackground = true };
                HCR.Start();
                Thread CH = new Thread(Chamsss) { IsBackground = true };
                CH.Start();
                Thread GL = new Thread(Glow) { IsBackground = true };
                GL.Start();
            }
            else
            {
                MessageBox.Show("Habre csgo antes de ejecutar el Cheat");
            }
        }

        void Aliveee()
        {
            int Vida = m.ReadInt(Client + (signatures.dwLocalPlayer).ToString("X") + "," + (netvars.m_iHealth).ToString("X"));
            if (Vida > 0 && Vida < 102)
            {
                Alive = true;
            }
            else
            {
                Alive = false;
            }
        }

        void Labels()
        {
            while (true)
            {
                labelAimbhot();
                labelFOV();
                labelBhop();
                labeltrigger();
                labelwhall();
                labelaimassist();
                labeltercera();
                cerrar();
                labelradar();
                labelchams();
                viewmodellabel();
                recoillabel();
                labelglow();
                labelflash();
                labelSkin();
                Aliveee();
                Thread.Sleep(50);
            }
        }
        void Glow()
        {
            while (true)
            {
                GlowEsp();
                Thread.Sleep(5);
            }
        }

        void HacksRapidos()
        {
            while (true)
            {
                Triggerboto();
                BHOPP();
                Aimbot();
                Fov();
                AimAssist();
                Radarrrr();
                viewmodelll();
                //ultracham();
                noflash();
                recoilcontrol();
                //skinChanger();
                Thread.Sleep(1);
            }
        }

        void Chamsss()
        {
            while (true)
            {
                updatechams();
                Chams();
                Thread.Sleep(50);
            }
        }

        void labelSkin()
        {
            if (SkinOpen == true)
            {
                label21.Text = "Abierto";
            }
            else
            {
                label21.Text = "Cerrado";
            }
        }

        void viewmodelll()
        {
            if (checkBox10.Checked)
            {
                if (checkBox3.Checked)
                {
                    m.WriteMemory(Playerbase + FOVVVVVV, "int", "0");
                    m.WriteMemory(Playerbase + FOVVVVVV1, "int", "90");
                    m.WriteMemory(Playerbase + FOV, "int", "90");
                }
                else
                {
                    if (Alive == true)
                    {
                        int flag = m.ReadInt(Playerbase + m_bIsScoped);
                        if (flag == apuntando)
                        {
                            m.WriteMemory(Playerbase + FOV, "int", "90");
                            m.WriteMemory(Playerbase + FOVVVVVV, "int", "0");
                        }
                        else
                        {
                            fov = m.ReadInt(Playerbase + FOV);
                            m.WriteMemory(Playerbase + FOVVVVVV, "int", "90");
                        }
                        if (fov < 121)
                        {
                            if (GetAsyncKeyState(Keys.F7) < 0)
                            {
                                fov = fov - 1;
                                m.WriteMemory(Playerbase + FOV, "int", fov.ToString());
                            }

                            if (GetAsyncKeyState(Keys.F8) < 0)
                            {
                                fov = fov + 1;
                                m.WriteMemory(Playerbase + FOV, "int", fov.ToString());
                            }

                            if (GetAsyncKeyState(Keys.F9) < 0)
                            {
                                fov = fov = 90;
                                m.WriteMemory(Playerbase + FOV, "int", fov.ToString());
                            }
                        }
                        else
                        {
                            m.WriteMemory(Playerbase + FOV, "int", "120");
                        }

                        if (fov < 24)
                        {
                            m.WriteMemory(Playerbase + FOV, "int", "25");
                        }
                    }
                }
              
              
            }
            else
            {
                if (checkBox3.Checked == false)
                {
                    int flag = m.ReadInt(Playerbase + "," + (netvars.m_bIsScoped).ToString("X"));
                    if (flag != apuntando)
                    {
                        m.WriteMemory(Playerbase + FOVVVVVV, "int", "0");
                    }
                }
                m.WriteMemory(Playerbase + FOV, "int", "90");
            }
        }


        void viewmodellabel()
        {
            if (checkBox10.Checked)
            {
                checkBox10.ForeColor = Color.Red;
                label16.Text = "Activado";
            }
            else
            {
                checkBox10.ForeColor = Color.White;
                label16.Text = "Desactivado";
            }
        }



        void labelaimassist()
        {
            if (checkBox6.Checked)
            {
                checkBox6.ForeColor = Color.Red;
                label12.Text = "Activado";
            }
            else
            {
                checkBox6.ForeColor = Color.White;
                label12.Text = "Desactivado";
            }
        }

        void labelglow()
        {
            if (checkBox12.Checked)
            {
                checkBox12.ForeColor = Color.Red;
                label19.Text = "Activado";
            }
            else
            {
                checkBox12.ForeColor = Color.White;
                label19.Text = "Desactivado";
            }
            
        }


        void Triggerboto()
        {
            if (checkBox1.Checked)
            {
                if (Alive == true)
                {
                    if (GetAsyncKeyState(Keys.XButton2) < 0)
                    {
                        int flag = m.ReadInt(Playerbase + m_iCrosshairId);
                        if (flag > 0 && flag < 50)
                        {
                            int flagenemy = ((flag - 1) * 0x10);
                            string newentityoff = Client + (entityoff + flagenemy).ToString("X");
                            int enemyteam = m.ReadInt(newentityoff + TeamNum);
                            int myteam = m.ReadInt(Playerbase + TeamNum);
                            if (enemyteam != myteam)
                            {
                                sim.Mouse.LeftButtonDown();
                                sim.Mouse.LeftButtonUp();
                            }
                        }
                        
                    }
                }


            }
        }

        void labelflash()
        {
            if (checkBox13.Checked)
            {
                checkBox13.ForeColor = Color.Red;
                label20.Text = "Activado";
            }
            else
            {
                checkBox13.ForeColor = Color.White;
                label20.Text = "Desactivado";
            }
        }

            
       void labeltercera()
       {
            if (checkBox7.Checked)
            {
                checkBox7.ForeColor = Color.Red;
                label13.Text = "Activado";
            }
            else
            {
                checkBox7.ForeColor = Color.White;
                label13.Text = "Desactivado";
            }
       }

        void cerrar()
        {
            if (GetAsyncKeyState(Keys.Insert)<0)
            {
                Open = !Open;
                if (Open == true)
                {
                    this.Show();
                }
                else
                {
                    this.Hide();
                }
            }
        }

        void Radarrrr()
        {
            if (checkBox8.Checked)
            {
                for (int i = 0; i < 64; i++)
                {
                    string CurrentPlayer = Client + (entityoff + i * 0x10).ToString("X");
                    m.WriteMemory(CurrentPlayer + Bspoted, "int", "1");
                }

            }
        }


        void labelFOV()
        {
            if (checkBox3.Checked)
            {
                checkBox3.ForeColor = Color.Red;
                label8.Text = "Activado";
                label9.Text = "FOV: " + trackBar1.Value;
                trackBar1.Show();
            }
            else
            {
                trackBar1.Hide();
                checkBox3.ForeColor = Color.White;
                label9.Text = "";
                label8.Text = "Desactivado";
            }
        }

      
       

        void labelAimbhot()
        {
            if (checkBox4.Checked)
            {
                checkBox4.ForeColor = Color.Red;
                label10.Text = "Activado";
            }
            else
            {
                label10.Text = "Desactivado";
                checkBox4.ForeColor = Color.White;
            }
        }



        void labelBhop()
        {
            if (checkBox2.Checked)
            {
                checkBox2.ForeColor = Color.Red;
                int Flag = m.ReadInt(Playerbase + M_Mflags);
                label5.Text = "Activado";
                label6.Text = "Estas";
                if (Flag == 257)
                {
                    label7.Text = "Parado";
                }else if (Flag == 263)
                {
                    label7.Text = "Agachado";
                }
                if (Flag == 256)
                {
                    label7.Text = "Saltando";
                }
            }
            else
            {
                checkBox2.ForeColor = Color.White;
                label7.Text = "";
                label6.Text = "";
                label5.Text = "Desactivado";
            }
        }

        void BHOPP()
        {
            if (checkBox2.Checked)
            {
                if (Alive == true)
                {
                    if (GetAsyncKeyState(Keys.Space) < 0)
                    {
                        int flag = m.ReadInt(Playerbase + M_Mflags);
                        if (flag == 257 || flag == 769 || flag == 263)
                        {
                            m.WriteMemory(ForceJump, "int", "6");
                        }
                        else
                        {
                            m.WriteMemory(ForceJump, "int", "4");
                        }
                    }
                }
            }
        }

        void labelradar()
        {
            if (checkBox8.Checked)
            {
                checkBox8.ForeColor = Color.Red;
                label14.Text = "Activado";
            }
            else
            {
                checkBox8.ForeColor = Color.White;
                label14.Text = "Desactivado";
            }
        }

        void labeltrigger()
        {
            if (checkBox1.Checked)
            {
                checkBox1.ForeColor = Color.Red;
                int flag = m.ReadInt(Playerbase + m_iCrosshairId);
                if (flag > 50)
                {
                    label4.Text = "Objeto Desconocido";
                }
                if (flag == 0)
                {
                    label4.Text = "Nada";
                }
                if (flag != 0 && flag < 50)
                {
                    int flagenemy = ((flag - 1) * 0x10);
                    string newentityoff = Client + (entityoff + flagenemy).ToString("X");
                    int enemyteam = m.ReadInt(newentityoff + TeamNum);
                    int myteam = m.ReadInt(Playerbase + TeamNum);
                    if (enemyteam == myteam)
                    {
                        label4.Text = "Compañero";
                    }
                    else
                    {
                        label4.Text = "Enemigo";
                    }
                }

                label1.Text = "Activado";
                label3.Text = "Apuntando A";
            }
            else
            {
                checkBox1.ForeColor = Color.White;
                label1.Text = "Desactivado";
                label3.Text = "";
                label4.Text = "";

            }

        }

        void GlowEsp()
        {
            if (checkBox12.Checked)
            {
                for (int i = 0; i < 64; i++)
                {
                    string currentPlayer = Client + (entityoff + i * 0x10).ToString("X");
                    int team = m.ReadInt(currentPlayer + TeamNum);
                    int vida = m.ReadInt(currentPlayer + TeamNum);
                    int myteam = m.ReadInt(Playerbase + TeamNum);
                    int glowindex = m.ReadInt(currentPlayer + Glowindex);
                    int dormant = m.ReadInt(currentPlayer + Dormant);

                    if (team != myteam && dormant == 0 && vida > 0 && vida < 102)
                    {
                        m.WriteMemory(GlowObjectManager + "," + ((glowindex * 0x38) + 0x8).ToString("X"), "float", 1.0f.ToString());
                        m.WriteMemory(GlowObjectManager + "," + ((glowindex * 0x38) + 0xC).ToString("X"), "float", 0.0f.ToString());
                        m.WriteMemory(GlowObjectManager + "," + ((glowindex * 0x38) + 0x10).ToString("X"), "float", 0.0f.ToString());
                        m.WriteMemory(GlowObjectManager + "," + ((glowindex * 0x38) + 0x14).ToString("X"), "float", 1.0f.ToString());

                        m.WriteMemory(GlowObjectManager + "," + ((glowindex * 0x38) + 0x27).ToString("X"), "int", 1.ToString());
                        m.WriteMemory(GlowObjectManager + "," + ((glowindex * 0x38) + 0x28).ToString("X"), "int", 1.ToString());
                    }
                }
            }
        }
        void labelwhall()
        {
            if (checkBox5.Checked)
            {
                checkBox5.ForeColor = Color.Red;
                label11.Text = "Activado";
            }
            else
            {
                checkBox5.ForeColor = Color.White;
                label11.Text = "Desactivado";
            }
        }

        void recoillabel()
        {
            if (checkBox11.Checked)
            {
                checkBox11.ForeColor = Color.Red;
                label17.Text = "Activado";
                label18.Text = trackBar2.Value.ToString() + "%";
                trackBar2.Show();
            }
            else
            {
                checkBox11.ForeColor = Color.White;
                label17.Text = "Desactivado";
                label18.Text = "";
                trackBar2.Hide();
            }
        }

        void noflash()
        {
            if (checkBox13.Checked)
            {
                if (Alive == true)
                {
                    m.WriteMemory(Playerbase + m_flFlashMaxAlpha, "float", 0.0f.ToString());
                }
            }
            else
            {
                int Vida = m.ReadInt(Playerbase + Health);
                if (Vida > 0)
                {
                    m.WriteMemory(Playerbase + m_flFlashMaxAlpha, "float", 255.0f.ToString());
                }
            }
        }
        

        void recoilcontrol()
        {
            if (checkBox11.Checked)
            {
                if (Alive == true)
                {
                    int disparos = m.ReadInt(Playerbase + m_iShotsFired);
                    if (disparos > 1)
                    {
                        float viewangleX = m.ReadFloat(ViewX);
                        float viewangleY = m.ReadFloat(ViewY);

                        float punchangleX = m.ReadFloat(Playerbase + m_aimPunchAngleX);
                        float punchangleY = m.ReadFloat(Playerbase + m_aimPunchAngleY);

                        float newangleX = viewangleX + oldpunchX - punchangleX * (trackBar2.Value * 0.02f);
                        float newangleY = viewangleY + oldpunchY - punchangleY * (trackBar2.Value * 0.02f);

                        m.WriteMemory(ViewX, "float", newangleX.ToString());
                        m.WriteMemory(ViewY, "float", newangleY.ToString());

                        oldpunchX = punchangleX * (trackBar2.Value * 0.02f);
                        oldpunchY = punchangleY * (trackBar2.Value * 0.02f);
                    }
                    else
                    {
                        oldpunchX = 0.0f;
                        oldpunchY = 0.0f;
                    }
                }
            }
        }

        void updatechams()
        {
            int PID = m.GetProcIdFromName("Chamsss.exe");
            if (PID > 0)
            {
                int aja = m.ReadInt(engine + (model_ambient_min1).ToString("X"));
                chamsvalue = aja;
                checkBox9.Checked = true;
            }

        }


        void Chams()
        {
            if (checkBox9.Checked)
            {
                if (chamsvalue != 0)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        string CurrentPlayer = Client + (entityoff + i * 0x10).ToString("X");
                        int vida = m.ReadInt(CurrentPlayer + Health);
                        if (vida > 0)
                        {
                            int team = m.ReadInt(CurrentPlayer + TeamNum);
                            int myteam = m.ReadInt(Playerbase + TeamNum);
                            if (team != myteam)
                            {
                                m.WriteMemory(CurrentPlayer + m_clrRender, "int", rojo.ToString());
                            }
                            else
                            {
                                m.WriteMemory(CurrentPlayer + m_clrRender, "int", Azul.ToString());
                            }
                        }

                    }

                    m.WriteMemory(engine + (model_ambient_min1).ToString("X"), "int", chamsvalue.ToString());
                }
                
            }
            else
            {
                if (chamsvalue != 0)
                {
                    int aja = m.ReadInt(engine + (model_ambient_min1 - 0x2C).ToString("X"));
                    m.WriteMemory(engine + (model_ambient_min1).ToString("X"), "int", aja.ToString());
                }
               
                int flag = m.ReadInt(Playerbase + m_clrRender);
                if (flag != -1)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        string CurrentPlayer = Client + (entityoff + i * 0x10).ToString("X");
                        m.WriteMemory(CurrentPlayer + m_clrRender, "int", "-1");
                    }
                }
               
            }
        }

        void labelchams()
        {
            if (checkBox9.Checked)
            {
                checkBox9.ForeColor = Color.Red;
                label15.Text = "Activado";
            }
            else
            {
                checkBox9.ForeColor = Color.White;
                label15.Text = "Desactivado";
            }
        }

        void Fov()
        {
            if (checkBox3.Checked)
            {
                if (checkBox10.Checked)
                {
                    m.WriteMemory(Playerbase + FOVVVVVV, "int", "0");
                    m.WriteMemory(Playerbase + FOVVVVVV1, "int", "90");
                    m.WriteMemory(Playerbase + FOV, "int", "90");
                }
                else
                {
                    if (Alive == true)
                    {
                        int Healthh = m.ReadInt(Playerbase + Health);
                        int flag = m.ReadInt(Playerbase + m_bIsScoped);
                        if (flag == apuntando)
                        {
                            if (Healthh > 0)
                            {
                                m.WriteMemory(Playerbase + FOVVVVVV, "int", "0");
                                m.WriteMemory(Playerbase + FOVVVVVV1, "int", "90");
                            }
                        }
                        else
                        {
                            if (Healthh > 0)
                            {
                                m.WriteMemory(Playerbase + FOVVVVVV, "int", trackBar1.Value.ToString());
                                m.WriteMemory(Playerbase + FOVVVVVV1, "int", trackBar1.Value.ToString());
                            }
                        }
                    }
                }
                
            }



        }

        void AimAssist()
        {

            if (checkBox6.Checked)
            {
                if (Alive == true)
                {
                    int flag = m.ReadInt(Playerbase + m_iCrosshairId);
                    if (flag > 0 && flag < 50)
                    {
                        int flagenemy = ((flag - 1) * 0x10);
                        string newentityoff = Client + (entityoff + flagenemy).ToString("X");
                        int enemyteam = m.ReadInt(newentityoff + TeamNum);
                        int myteam = m.ReadInt(Playerbase + TeamNum);
                        if (enemyteam != myteam)
                        {
                            var LocalPlayer2 = JugadorLocal();
                            var Players2 = LeerJugadores(LocalPlayer2);

                            Players2 = Players2.OrderBy(o => o.Magnitud).ToList();
                            var ent = new ESPPP();

                            if (Players2.Count != 0)
                            {
                                ent.bonecabeza = WorldToScreen(IntoTheMatrix(), Players2[0], 1920, 1080);
                                if (ent.bonecabeza.X > 0 && ent.bonecabeza.Y < 1920 && ent.bonecabeza.X < 1080 && ent.bonecabeza.Y > 0)
                                {
                                    SetCursorPos(ent.bonecabeza.X, ent.bonecabeza.Y);
                                }
                            }
                        }
                    }
                }
            }

        }


        void Aimbot()
        {
            if (checkBox4.Checked)
            {
                if (GetAsyncKeyState(Keys.XButton1) < 0)
                {
                    if (Alive == true)
                    {
                        var Jugador = JugadorLocal();
                        var Jugadores = LeerJugadores(Jugador);

                        Jugadores = Jugadores.OrderBy(o => o.Magnitud).ToList();



                        if (Jugadores.Count != 0)
                        {
                            Aim(Jugador, Jugadores[0]);
                        }

                    }
                }
            }
            
        }

        Jugador JugadorLocal()
        {
            var Jugador = new Jugador
            {
                X = m.ReadFloat(Playerbase + BoneMatrix + cabesaX),
                Z = m.ReadFloat(Playerbase + BoneMatrix + cabesaZ),
                Y = m.ReadFloat(Playerbase + BoneMatrix + cabesaY),
                Teamnum = m.ReadInt(Playerbase + TeamNum)
            };
            return Jugador;
        }

        double GetMag(Jugador ent)
        {
            double mag;

            mag = Math.Sqrt(Math.Pow((1920 / 2) - ent.BoneCabeza.X, 2) + Math.Pow((1080 / 2) - ent.BoneCabeza.Y, 2));

            return mag;
        }

        void Aim(Jugador local,Jugador ent)
        {
            if (checkBox11.Checked)
            {

                float deltaX = ent.X - local.X;
                float deltaZ = ent.Z - local.Z;
                float deltaY = ent.Y - local.Y;

                double distancia = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaZ, 2));

                float viewX = (float)(Math.Atan2(deltaZ, deltaX) * 180 / Math.PI);
                float viewY = (float)(Math.Atan2(deltaY, distancia) * 180 / Math.PI);

                viewY = viewY - viewY - viewY;

                disparos = m.ReadInt(Playerbase + m_iShotsFired);
                
                if (disparos > 1)
                {
                    recoilcontrol();
                }
                else
                {
                    if (GetAsyncKeyState(Keys.XButton1) < 0)
                    {
                        m.WriteMemory(ViewX, "float", viewX.ToString());
                        m.WriteMemory(ViewY, "float", viewY.ToString());
                    }
                }


            }
            else
            {
                float deltaX = ent.X - local.X;
                float deltaZ = ent.Z - local.Z;
                float deltaY = ent.Y - local.Y;

                double distancia = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaZ, 2));

                float viewX = (float)(Math.Atan2(deltaZ, deltaX) * 180 / Math.PI);
                float viewY = (float)(Math.Atan2(deltaY, distancia) * 180 / Math.PI);

                viewY = viewY - viewY - viewY;

                m.WriteMemory(ViewX, "float", viewX.ToString());
                m.WriteMemory(ViewY, "float", viewY.ToString());
            }
            
        }
        List<Jugador> LeerJugadores(Jugador local)
        {
            var jugadores = new List<Jugador>();

            for (int i = 0; i < 64; i++)
            {
                string CurrentPlayer = Client + (entityoff + i * 0x10).ToString("X");

                var Jugador = new Jugador
                {
                    X = m.ReadFloat(CurrentPlayer + BoneMatrix + cabesaX),
                    Z = m.ReadFloat(CurrentPlayer + BoneMatrix + cabesaZ),
                    Y = m.ReadFloat(CurrentPlayer + BoneMatrix + cabesaY),
                    Teamnum = m.ReadInt(CurrentPlayer + TeamNum),
                    Vida = m.ReadInt(CurrentPlayer + Health),
                    dormant = m.ReadInt(CurrentPlayer + Dormant)
                };
                Jugador.BoneCabeza = WorldToScreen(IntoTheMatrix(), Jugador, 1920, 1080);

                Jugador.Magnitud = GetMag(Jugador);

                if (Jugador.Vida > 0 && Jugador.Vida < 102)
                    jugadores.Add(Jugador);

                if (Jugador.Teamnum == local.Teamnum)
                    jugadores.Remove(Jugador);

                if (Jugador.dormant == 1)
                    jugadores.Remove(Jugador);
            }

            return jugadores;
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

        Point WorldToScreen(ViewMatrix mtx, Jugador ent, int width, int height)
        {

            var twoD = new Point();

            float screenW = (mtx.m14 * ent.X) + (mtx.m24 * ent.Z) + (mtx.m34 * ent.Y) + mtx.m44;

            if (screenW > 0.001f)
            {
                float screenX = (mtx.m11 * ent.X) + (mtx.m21 * ent.Z) + (mtx.m31 * ent.Y) + mtx.m41;

                float screenY = (mtx.m12 * ent.X) + (mtx.m22 * ent.Z) + (mtx.m32 * ent.Y) + mtx.m42;


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

        private void BGworker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcOpen = m.OpenProcess("csgo.exe");
            if (!ProcOpen)
            {
                Thread.Sleep(1000);
                return;
            }
            ProcOpen = true;
            Thread.Sleep(1000);
            BGworker.ReportProgress(0);
        }

        private void BGworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (ProcOpen)
                label2.Text = "Juego Encontrado";
        }

        private void BGworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BGworker.RunWorkerAsync();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            BGworker.RunWorkerAsync();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox5.Checked == true)
            {
                
                whall.Show();
            }
            else
            {
                whall.Hide();
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                m.WriteMemory(Playerbase + viewmodel, "int", "1");
            }
            else
            {
                m.WriteMemory(Playerbase + viewmodel, "int", "0");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Discord MamuelGG#0277");
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                MessageBox.Show("No uses el Fov Changer si esta activado el Viewmodel Changer");
            }
            
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                MessageBox.Show("No uses el Viewmodel Changer si esta activado el Fov Changer");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SkinOpen = !SkinOpen;
            if (SkinOpen == true)
            {
                changer.Show();
            }
            else
            {
                changer.Hide();
            }
        }
    }
}
