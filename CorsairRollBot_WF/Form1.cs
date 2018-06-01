﻿using EliteMMO.API;
using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorsairRollBot_WF
{
    public partial class Form1 : MetroForm

    {
        #region "CUSTOM CLASS: RollData"

        public class RollData : List<RollData>
        {
            public string Roll_name
            {
                get; set;
            }

            public int Lucky
            {
                get; set;
            }

            public int Unlucky
            {
                get; set;
            }

            public int Buff_id
            {
                get; set;
            }

            public int Position
            {
                get; set;
            }
        }

        #endregion

        #region "CUSTOM CLASS: PartyRequirements"

        public class PartyRequirements
        {
            public string CharacterName { get; set; }
            public bool Checked { get; set; }
        }



        #endregion

        #region "CUSTOM CLASS: BadgeValue"
        public class ViewModel : INotifyPropertyChanged
        {
            private int _badgeValue;
            public int BadgeValue
            {
                get { return _badgeValue; }
                set { _badgeValue = value; NotifyPropertyChanged(); }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region "PUBLIC METHODS"

        public List<PartyRequirements> Member_List = new List<PartyRequirements>();

        public ListBox processids = new ListBox();

        public static EliteAPI _api;

        public List<RollData> rolls = new List<RollData>();

        public string WindowerMode = "Windower";

        private bool botRunning = false;

        private int CurrentRoll = 0;

        private int LastKnownRoll = 0;

        private bool firstSelect = false;

        public int lastCommand = 0;

        public bool timerBusy = false;

        private bool FollowerStuck = false;

        private int ListeningPort = 19701;

        public bool Blocked = false;

        public bool AllInRange = false;


        private string IP_Address = "127.0.0.1";

        public List<int> knownCities = new List<int> { 50, 80, 94, 87, 222, 223, 224, 225, 226, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239,
                                                                240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 256, 257 };

        public int RollActive = 0; // OPTIONS (0 = NONE), (1 = ROLL ONE), (2 = ROLL TWO)


        #endregion

        public Form1()
        {
            InitializeComponent();

            RollOne_ComboBox.SelectedIndex = 3;
            RollTwo_ComboBox.SelectedIndex = 10;

            #region "ADD ALL THE ROLLS TO THE CUSTOM CLASS"

            rolls.Add(new RollData
            {
                Roll_name = "Corsair's Roll",
                Lucky = 5,
                Unlucky = 9,
                Buff_id = 326,
                Position = 0
            });
            rolls.Add(new RollData
            {
                Roll_name = "Ninja Roll",
                Lucky = 4,
                Unlucky = 8,
                Buff_id = 322,
                Position = 1
            }); rolls.Add(new RollData
            {
                Roll_name = "Hunter's Roll",
                Lucky = 4,
                Unlucky = 8,
                Buff_id = 320,
                Position = 2
            });
            rolls.Add(new RollData
            {
                Roll_name = "Chaos Roll",
                Lucky = 4,
                Unlucky = 8,
                Buff_id = 317,
                Position = 3
            }); rolls.Add(new RollData
            {
                Roll_name = "Magus's Roll",
                Lucky = 2,
                Unlucky = 6,
                Buff_id = 325,
                Position = 4
            });
            rolls.Add(new RollData
            {
                Roll_name = "Healer's Roll",
                Lucky = 3,
                Unlucky = 7,
                Buff_id = 312,
                Position = 5
            });
            rolls.Add(new RollData
            {
                Roll_name = "Drachen Roll",
                Lucky = 4,
                Unlucky = 8,
                Buff_id = 323,
                Position = 6
            });
            rolls.Add(new RollData
            {
                Roll_name = "Choral Roll",
                Lucky = 2,
                Unlucky = 6,
                Buff_id = 319,
                Position = 7
            });
            rolls.Add(new RollData
            {
                Roll_name = "Monk's Roll",
                Lucky = 3,
                Unlucky = 7,
                Buff_id = 311,
                Position = 8
            });
            rolls.Add(new RollData
            {
                Roll_name = "Beast Roll",
                Lucky = 4,
                Unlucky = 8,
                Buff_id = 318,
                Position = 9
            });
            rolls.Add(new RollData
            {
                Roll_name = "Samurai Roll",
                Lucky = 2,
                Unlucky = 6,
                Buff_id = 321,
                Position = 10
            });
            rolls.Add(new RollData
            {
                Roll_name = "Evoker's Roll",
                Lucky = 5,
                Unlucky = 9,
                Buff_id = 324,
                Position = 11
            });
            rolls.Add(new RollData
            {
                Roll_name = "Rogue's Roll",
                Lucky = 5,
                Unlucky = 9,
                Buff_id = 315,
                Position = 12
            });
            rolls.Add(new RollData
            {
                Roll_name = "Warlock's Roll",
                Lucky = 4,
                Unlucky = 8,
                Buff_id = 314,
                Position = 13
            });
            rolls.Add(new RollData
            {
                Roll_name = "Fighter's Roll",
                Lucky = 5,
                Unlucky = 9,
                Buff_id = 310,
                Position = 14
            });
            rolls.Add(new RollData
            {
                Roll_name = "Puppet Roll",
                Lucky = 3,
                Unlucky = 7,
                Buff_id = 327,
                Position = 15
            });
            rolls.Add(new RollData
            {
                Roll_name = "Gallant's Roll",
                Lucky = 3,
                Unlucky = 7,
                Buff_id = 316,
                Position = 16
            });
            rolls.Add(new RollData
            {
                Roll_name = "Wizard's Roll",
                Lucky = 5,
                Unlucky = 9,
                Buff_id = 313,
                Position = 17
            });
            rolls.Add(new RollData
            {
                Roll_name = "Dancer's Roll",
                Lucky = 3,
                Unlucky = 7,
                Buff_id = 328,
                Position = 18
            });
            rolls.Add(new RollData
            {
                Roll_name = "Scholar's Roll",
                Lucky = 2,
                Unlucky = 6,
                Buff_id = 329,
                Position = 19
            });
            rolls.Add(new RollData
            {
                Roll_name = "Naturalist's Roll",
                Lucky = 3,
                Unlucky = 7,
                Buff_id = 339,
                Position = 20
            });
            rolls.Add(new RollData
            {
                Roll_name = "Runeist's Roll",
                Lucky = 4,
                Unlucky = 8,
                Buff_id = 600,
                Position = 21
            });
            rolls.Add(new RollData
            {
                Roll_name = "Bolter's Roll",
                Lucky = 3,
                Unlucky = 9,
                Buff_id = 330,
                Position = 22
            });
            rolls.Add(new RollData
            {
                Roll_name = "Caster's Roll",
                Lucky = 2,
                Unlucky = 7,
                Buff_id = 331,
                Position = 23
            });
            rolls.Add(new RollData
            {
                Roll_name = "Courser's Roll",
                Lucky = 3,
                Unlucky = 9,
                Buff_id = 332,
                Position = 24
            });
            rolls.Add(new RollData
            {
                Roll_name = "Blitzer's Roll",
                Lucky = 4,
                Unlucky = 9,
                Buff_id = 333,
                Position = 26
            });
            rolls.Add(new RollData
            {
                Roll_name = "Tactician's Roll",
                Lucky = 5,
                Unlucky = 8,
                Buff_id = 334,
                Position = 25
            });

            rolls.Add(new RollData
            {
                Roll_name = "Allies' Roll",
                Lucky = 3,
                Unlucky = 10,
                Buff_id = 335,
                Position = 27
            });
            rolls.Add(new RollData
            {
                Roll_name = "Miser's Roll",
                Lucky = 5,
                Unlucky = 7,
                Buff_id = 336,
                Position = 28
            });
            rolls.Add(new RollData
            {
                Roll_name = "Companion's Roll",
                Lucky = 2,
                Unlucky = 10,
                Buff_id = 337,
                Position = 29
            });
            rolls.Add(new RollData
            {
                Roll_name = "Avenger's Roll",
                Lucky = 4,
                Unlucky = 8,
                Buff_id = 338,
                Position = 30
            });

            #endregion

            #region "Check for the Elite API and POL instances"

            if (File.Exists("eliteapi.dll") && File.Exists("elitemmo.api.dll"))
            {
                var pol = Process.GetProcessesByName("pol");

                if (pol.Length < 1)
                {
                    MetroMessageBox.Show(this, "Notice:", "No POL instances were able to be located." + "\n\n" +
                        "Please note: If you use a private server make sure the program used to access it has been renamed to POL " +
                        "otherwise this bot will not be able to locate it.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    for (var i = 0; i < pol.Length; i++)
                    {
                        this.POLID.Items.Add(pol[i].MainWindowTitle);
                        this.processids.Items.Add(pol[i].Id);
                    }
                    this.POLID.SelectedIndex = 0;
                    this.processids.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("This program can not function without EliteMMO.API.dll and EliteAPI.dll");
                // this.Close();
            }

            #endregion

        }

        private void ActivityButton_Click(object sender, EventArgs e)
        {

            if (botRunning == false)
            {
                if (this.ActivityButton.InvokeRequired)
                {
                    this.ActivityButton.Invoke(new MethodInvoker(delegate () { this.ActivityButton.BackColor = Color.Green; this.ActivityButton.Text = "RUNNING!"; }));
                }
                else
                {
                    this.ActivityButton.BackColor = Color.Green; this.ActivityButton.Text = "RUNNING!";
                }

                botRunning = true;
            }
            else
            {
                if (this.ActivityButton.InvokeRequired)
                {
                    this.ActivityButton.Invoke(new MethodInvoker(delegate () { this.ActivityButton.BackColor = Color.Red; this.ActivityButton.Text = "PAUSED!"; }));
                }
                else
                {
                    this.ActivityButton.BackColor = Color.Red; this.ActivityButton.Text = "PAUSED!";
                }

                botRunning = false;
            }
        }

        #region "A POLID was selected so create an API instance and run the Addon"

        private void Select_POLID_Click(object sender, EventArgs e)
        {

            this.processids.SelectedIndex = this.POLID.SelectedIndex;
            _api = new EliteAPI((int)this.processids.SelectedItem);
            this.Select_POLID.Text = "SELECTED";
            this.Select_POLID.BackColor = Color.Green;

            foreach (var dats in Process.GetProcessesByName("pol").Where(dats => POLID.Text == dats.MainWindowTitle))
            {
                for (int i = 0; i < dats.Modules.Count; i++)
                {
                    if (dats.Modules[i].FileName.Contains("Ashita.dll"))
                    {
                        WindowerMode = "Ashita";
                    }
                    else if (dats.Modules[i].FileName.Contains("Hook.dll"))
                    {
                        WindowerMode = "Windower";
                    }
                }
            }

            if (firstSelect == false)
            {

                EliteAPI.ChatEntry cl = _api.Chat.GetNextChatLine();
                while (cl != null) cl = _api.Chat.GetNextChatLine();

                if (WindowerMode == "Windower")
                {
                    _api.ThirdParty.SendString("//lua load crb_addon");
                }
                else if (WindowerMode == "Ashita")
                {
                    _api.ThirdParty.SendString("/addon load crb_addon");
                }


                AddonReader.RunWorkerAsync();


                GrabParty();

                firstSelect = true;
            }
        }

        #endregion "A POLID was selected so create an API instance and run the Addon"






        private async void Roll_Timer_TickAsync(object sender, EventArgs e)
        {
            if (_api == null || _api.Player.LoginStatus != (int)LoginStatus.LoggedIn || _api.Player.LoginStatus == (int)LoginStatus.Loading)
            {
                return;
            }

            if (timerBusy == true || (LastKnownRoll == CurrentRoll && BuffChecker(309) == true)) // The bot is BUSY or The Current Roll has not changed yet, so wait for another tick.
            {
                return;
            }

            if (BuffChecker(309) == false) // You can no longer double up, so LastKnownRoll can be reset to 0.
            {
                LastKnownRoll = 0;
            }

            try
            {
                timerBusy = true;

                if (BuffChecker(308) == false)
                {
                    CurrentRoll = 0;
                    CurrentRoll_Number.Text = "0";
                }

                // ONCE ALL THE CHECKS ARE DONE LETS CONTINUE WITH THE CHECKS

                if (botRunning == true && !knownCities.Contains(_api.Player.ZoneId))
                {
                    // FIRST GRAB THE REQUIRED ROLL DATA
                    var rollOne = rolls.Where(r => r.Position == RollOne_ComboBox.SelectedIndex).FirstOrDefault();
                    var rollTwo = rolls.Where(r => r.Position == RollTwo_ComboBox.SelectedIndex).FirstOrDefault();

                    // NOW BEFORE EVEN ATTEMPTING THE ROLLS CHECK IF ANY PT MEMBERS ARE REQUIRED TO BE
                    // NEARBY AND IF THEY ARE THAT THEY'RE CLOSE BY THEN IF THEY ARE BEGIN ROLLS

                    var res = (from item in Member_List where item.Checked == true select item).ToList<PartyRequirements>();
                    if (res.Count() >= 1)
                    {
                        foreach (PartyRequirements item in res)
                        {
                            if (item.Checked)
                            {
                                if (DistanceChecker(item.CharacterName) == true)
                                    AllInRange = true;
                                else
                                    AllInRange = false;
                            }
                        }
                    }
                    else
                        AllInRange = true;

                    if (AllInRange)
                    {
                        int lucky = 0;
                        int unlucky = 0;

                        if (RollActive == 1)
                        {
                            lucky = rollOne.Lucky;
                            unlucky = rollOne.Unlucky;
                        }
                        else if (RollActive == 2)
                        {
                            lucky = rollTwo.Lucky;
                            unlucky = rollTwo.Unlucky;
                        }

                        // DOUBLE-UP CHANCE IS ACTIVE DOUBLE UP, SNAKE EYE, RANDOM DEAL IF NEEDED
                        if (BuffChecker(308) == true)
                        {

                            if (Blocked == false)
                            {

                                if (CurrentRoll == 11 || CurrentRoll == lucky)
                                {
                                    // YOU ROLLED 11 OR HAVE THE LUCKY NUMBER SO DO NOTHING AND MOVE ON TO
                                    // THE NEXT ROLL

                                    // MessageBox.Show("Current roll number: " + CurrentRoll + "/" + lucky);

                                    Blocked = true;

                                }
                                else
                                {
                                    if (CurrentRoll == unlucky)
                                    {
                                        // AN UNLUCKY ROLL IS ACTIVE IF ENABLED USE SNAKE EYE IF NOT
                                        // AVAILABLE AND RANDOM DEAL IS ENABLED TRY TO RESET SNAKE EYE TO
                                        // ROLL A BETTER NUMBER

                                        if (SnakeEye_Switch.Checked == true && HasAbility("Snake Eye") == true && AbilityRecast("Snake Eye") == 0 && BuffChecker(357) != true)
                                        {
                                            _api.ThirdParty.SendString("/ja \"Snake Eye\" <me>");
                                        }
                                        else if (SnakeEye_Switch.Checked == true && HasAbility("Snake Eye") == true && AbilityRecast("Snake Eye") != 0 &&
                                            RandomDeal_Switch.Checked == true && HasAbility("Random Deal") == true && AbilityRecast("Random Deal") == 0 && BuffChecker(357) != true)
                                        {
                                            _api.ThirdParty.SendString("/ja \"Random Deal\" <me>");
                                        }
                                        else
                                        {
                                            // DOUBLE UP ANYWAY, AN UNLUCKY ROLL IS NOT WORTH KEEPING
                                            if (AbilityRecast("Double-Up") == 0)
                                            {
                                                _api.ThirdParty.SendString("/ja \"Double-Up\" <me>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // SNAKE EYE IS UP SO DOUBLE-UP
                                        if (BuffChecker(357) == true && AbilityRecast("Double-Up") == 0)
                                        {
                                            _api.ThirdParty.SendString("/ja \"Double-Up\" <me>");
                                            await Task.Delay(TimeSpan.FromSeconds(1));

                                        }
                                        else if (CurrentRoll == 10 && ((SnakeEye_Switch.Checked == true && AbilityRecast("Snake Eye") == 0) || (SnakeEye_Switch.Checked == true && RandomDeal_Switch.Checked == true && AbilityRecast("Random Deal") == 0)))
                                        {
                                            // IF THE ROLL IS 10 THEN USE SNAKE EYE
                                            if (SnakeEye_Switch.Checked == true && HasAbility("Snake Eye") == true && AbilityRecast("Snake Eye") == 0 && BuffChecker(357) != true)
                                            {
                                                // SNAKE EYE CAN BE USED SO DO SO
                                                _api.ThirdParty.SendString("/ja \"Snake Eye\" <me>");
                                                await Task.Delay(TimeSpan.FromSeconds(1));
                                            }
                                            else if (SnakeEye_Switch.Checked == true && HasAbility("Snake Eye") == true && AbilityRecast("Snake Eye") != 0 && BuffChecker(357) != true
                                                && RandomDeal_Switch.Checked == true && HasAbility("Random Deal") == true && AbilityRecast("Random Deal") == 0)
                                            {
                                                // SNAKE EYE IS ON RECAST BUT RANDOM DEAL IS NOT, TRY TO
                                                // RESET SNAKE EYE
                                                _api.ThirdParty.SendString("/ja \"Random Deal\" <me>");
                                                await Task.Delay(TimeSpan.FromSeconds(1));
                                            }
                                            else
                                            {
                                                // DO NOTHING AS YOU WOULD HAVE TO BE VERY LUCKY TO ROLL A
                                                // NUMBER ONE
                                            }
                                        }
                                        else if (CurrentRoll <= 6 && CurrentRoll != lucky)
                                        {
                                            // DOUBLE-UP ANYTHING BELOW OR AT 6
                                            if (AbilityRecast("Double-Up") == 0)
                                            {
                                                _api.ThirdParty.SendString("/ja \"Double-Up\" <me>");
                                                await Task.Delay(TimeSpan.FromSeconds(1));
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            // UNBLOCK DOUBLE UP FOR FUTURE ROLL USAGE
                            Blocked = false;

                            // CHECK IF YOU HAVE A BUST STATUS TO REMOVE
                            if (BuffChecker(309) == true && HasAbility("Fold") == true && AbilityRecast("Fold") == 0)
                            {
                                _api.ThirdParty.SendString("/ja \"Fold\" <me>");
                                await Task.Delay(TimeSpan.FromSeconds(1));
                            }

                            // NOW CHECK IF THE FIRST ROLL IS ACTIVE
                            if (BuffChecker(rollOne.Buff_id) != true && HasAbility("Phantom Roll") == true && AbilityRecast("Phantom Roll") == 0)
                            {
                                // NOW CHECK IF CROOKED ROLL IS ENABLED AND IF SO USE IT
                                if (CrookedCards_Switch.Checked == true && HasAbility("Crooked Cards") == true && AbilityRecast("Crooked Cards") == 0)
                                {
                                    _api.ThirdParty.SendString("/ja \"Crooked Cards\" <me>");
                                    await Task.Delay(TimeSpan.FromSeconds(1));
                                }
                                // OTHERWISE USE THE ROLL
                                else
                                {
                                    RollActive = 1;
                                    CurrentRoll = 0;
                                    _api.ThirdParty.SendString("/ja \"" + rollOne.Roll_name + "\" <me>");
                                    await Task.Delay(TimeSpan.FromSeconds(1));

                                }
                            }

                            // FIRST ROLL CHECK HAS PASSED, NOW CHECK THE SECOND ROLL
                            else if (BuffChecker(rollOne.Buff_id) == true && BuffChecker(rollTwo.Buff_id) != true && BuffChecker(309) == false && HasAbility("Phantom Roll") == true && AbilityRecast("Phantom Roll") == 0)
                            {
                                RollActive = 2;
                                CurrentRoll = 0;
                                _api.ThirdParty.SendString("/ja \"" + rollTwo.Roll_name + "\" <me>");
                                await Task.Delay(TimeSpan.FromSeconds(1));

                            }
                            else if (BuffChecker(rollOne.Buff_id) == true && BuffChecker(rollTwo.Buff_id) == true)
                            {
                                RollActive = 0;
                            }
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromMilliseconds(1000));

            }
            finally
            {
                timerBusy = false;
            }
        }

        private void Follow_Timer_Tick(object sender, EventArgs e)
        {
            // Check if you have the required Buffs active, if not then run them.
            if (_api == null || _api.Player.LoginStatus != (int)LoginStatus.LoggedIn || _api.Player.LoginStatus == (int)LoginStatus.Loading)
            {
                return;
            }

            string followerName = String.Empty;

            if (FollowerTarget.Text != String.Empty)
            {
                followerName = FollowerTarget.Text;
            }

            if (followerName != String.Empty && botRunning != false)
            {

                // The base followID needed to store the future ID.
                int followID = 0;

                // Search all possible entities for the follow tartget
                for (var x = 0; x < 2048; x++)
                {
                    var entity = _api.Entity.GetEntity(x);
                    if (entity.Name != null && entity.Name.ToLower().Equals(followerName.ToLower()))
                    {
                        // If the follow Name = Entity Name then set the followID to this target
                        followID = Convert.ToInt32(entity.TargetID);
                        // Since no more checks are needed save resources and break the for command
                        break;
                    }
                }

                // iIf the FollowID is not the base 0 then you have a valid target to follow so check distance and follow.
                if (followID != 0)
                {
                    // Last known positions, used for stuck checker.
                    float lastX;
                    float lastY;
                    float lastZ;

                    // Grab the person you're to follow's entity.
                    var followTarget = _api.Entity.GetEntity(followID);

                    // Check if you are further than the maximum distance from the followTarget
                    if (Math.Truncate(followTarget.Distance) >= 6 && Math.Truncate(followTarget.Distance) < 45)
                    {
                        if (_api.AutoFollow.IsAutoFollowing != true)
                        {
                            // First remove the target as we don't want that interfering
                            _api.Target.SetTarget(0);

                            // While to run the follow function until below 6 yalms of the follow target
                            while (Math.Truncate(followTarget.Distance) >= 6)
                            {
                                // Grab the targer ID's float info
                                float Target_X = followTarget.X;
                                float Target_Y = followTarget.Y;
                                float Target_Z = followTarget.Z;

                                // Grab the player float info
                                float Player_X = _api.Player.X;
                                float Player_Y = _api.Player.Y;
                                float Player_Z = _api.Player.Z;

                                // Set auto follow co-ordinates
                                _api.AutoFollow.SetAutoFollowCoords(Target_X - Player_X,
                                                                    Target_Y - Player_Y,
                                                                    Target_Z - Player_Z);

                                // Run the follow action
                                _api.AutoFollow.IsAutoFollowing = true;

                                // Stuck checker
                                lastX = _api.Player.X;
                                lastY = _api.Player.Y;
                                lastZ = _api.Player.Z;

                                // Thread.Sleep(TimeSpan.FromSeconds(0.1));

                                // STUCK CHECKER
                                float genX = lastX - _api.Player.X;
                                float genY = lastY - _api.Player.Y;
                                float genZ = lastZ - _api.Player.Z;
                                double distance = Math.Sqrt(genX * genX + genY * genY + genZ * genZ);

                                if (distance < .1)
                                {
                                    FollowerStuck = true;
                                    _api.AutoFollow.IsAutoFollowing = false;
                                }
                            }
                            _api.AutoFollow.IsAutoFollowing = false;
                            FollowerStuck = false;
                        }
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_api != null)
            {
                if (WindowerMode == "Ashita")
                {
                    _api.ThirdParty.SendString("/addon unload crb_addon");
                }
                else if (WindowerMode == "Windower")
                {
                    _api.ThirdParty.SendString("//lua unload crb_addon");
                }
            }

            _api = null;
        }

        #region "ADDON READER BACKGROUND TASK"

        private void AddonReader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            bool done = false;

            int listenPort = Convert.ToInt32(ListeningPort);

            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(IP_Address), listenPort);

            string received_data;

            byte[] receive_byte_array;

            try
            {
                while (!done)
                {
                    receive_byte_array = listener.Receive(ref groupEP);

                    received_data = Encoding.ASCII.GetString(receive_byte_array, 0, receive_byte_array.Length);

                    //MessageBox.Show("FOUND " + received_data);

                    string[] data_received = received_data.Split(' ');



                    if (data_received[0] == "crollbot_addon")
                    {

                        LastKnownRoll = CurrentRoll;


                        // UPDATE ROLL INFORMATION
                        CurrentRoll = Convert.ToInt32(data_received[1]);

                        if (this.CurrentRoll_Number.InvokeRequired)
                        {
                            this.CurrentRoll_Number.Invoke(new MethodInvoker(delegate () { this.CurrentRoll_Number.Text = data_received[1]; }));
                        }
                        else
                        {
                            this.CurrentRoll_Number.Text = data_received[1];
                        }

                    }
                    else if (data_received[0] == "crb")
                    {
                        if (data_received[1].ToLower() == "validated")
                        {
                            if (this.AddonActive.InvokeRequired)
                            {
                                this.AddonActive.Invoke(new MethodInvoker(delegate () { this.AddonActive.Text = "YES"; this.AddonActive.BackColor = Color.Green; }));
                            }
                            else
                            {
                                this.AddonActive.Text = "YES"; this.AddonActive.BackColor = Color.Green;
                            }
                        }
                        else if (data_received[1].ToLower() == "toggle")
                        {
                            if (botRunning == false)
                            {
                                if (this.ActivityButton.InvokeRequired)
                                {
                                    this.ActivityButton.Invoke(new MethodInvoker(delegate () { this.ActivityButton.BackColor = Color.Green; this.ActivityButton.Text = "RUNNING!"; }));
                                }
                                else
                                {
                                    this.ActivityButton.BackColor = Color.Green; this.ActivityButton.Text = "RUNNING!";
                                }

                                botRunning = true;
                            }
                            else
                            {
                                if (this.ActivityButton.InvokeRequired)
                                {
                                    this.ActivityButton.Invoke(new MethodInvoker(delegate () { this.ActivityButton.BackColor = Color.Red; this.ActivityButton.Text = "PAUSED!"; }));
                                }
                                else
                                {
                                    this.ActivityButton.BackColor = Color.Red; this.ActivityButton.Text = "PAUSED!";
                                }

                                botRunning = false;
                            }

                        }
                        else if (data_received[1].ToLower() == "stop" || data_received[1].ToLower() == "pause")
                        {

                            if (this.ActivityButton.InvokeRequired)
                            {
                                this.ActivityButton.Invoke(new MethodInvoker(delegate () { this.ActivityButton.BackColor = Color.Red; this.ActivityButton.Text = "PAUSED!"; }));
                            }
                            else
                            {
                                this.ActivityButton.BackColor = Color.Red; this.ActivityButton.Text = "PAUSED!";
                            }
                            botRunning = false;
                        }
                        else if (data_received[1].ToLower() == "start" || data_received[1].ToLower() == "unpause")
                        {
                            if (this.ActivityButton.InvokeRequired)
                            {
                                this.ActivityButton.Invoke(new MethodInvoker(delegate () { this.ActivityButton.BackColor = Color.Green; this.ActivityButton.Text = "RUNNING!"; }));
                            }
                            else
                            {
                                this.ActivityButton.BackColor = Color.Green; this.ActivityButton.Text = "RUNNING!";
                            }
                            botRunning = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                listener.Close();

            }

            Thread.Sleep(TimeSpan.FromSeconds(1));

            AddonReader.CancelAsync();
        }

        private void AddonReader_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            AddonReader.RunWorkerAsync();
        }

        #endregion

        #region "ADDON ACTIVE CHECKER"

        private void AddonActive_Click(object sender, EventArgs e)
        {
            if (_api != null)
            {
                if (WindowerMode == "Windower")
                {
                    _api.ThirdParty.SendString("//CorsairRollBot verify");
                }
                else if (WindowerMode == "Ashita")
                {
                    _api.ThirdParty.SendString("/CorsairRollBot verify");
                }
            }
        }

        #endregion

        #region "DISTANCE CHECKER"

        public bool DistanceChecker(string MemberName)
        {
            string CharName = MemberName.ToLower();

            for (var x = 0; x < 2048; x++)
            {
                var entity2 = _api.Entity.GetEntity(x);

                if (entity2.Name != null && entity2.Name.ToLower() == CharName && (int)entity2.Distance < 8)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region "FUNCTION TO CHECK IF AVAILABLE BUFF IS ACTIVE - BuffChecker(int ID)"

        public bool BuffChecker(int buffID)
        {
            if (_api.Player.GetPlayerInfo().Buffs.Any(b => b == buffID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region "PARTY MEMBER CHECKER TIMER"

        private void GrabParty()
        {
            // IF IN A PARTY THEN SET EACH CHECKBOX FIELD AS ENABLED AND POSSIBLE TO CHECK WITH THE
            // PT MEMBERS NAME SHOWN

            if (_api != null)
            {
                var PartyMembers = _api.Party.GetPartyMembers();

                PartyMembersRequired.Items.Clear();

                if (PartyMembers.Count() > 1)
                {
                    foreach (var PT_Data in PartyMembers)
                    {
                        if (PT_Data.Name != _api.Player.Name && !PartyMembersRequired.Items.Contains(PT_Data.Name) && PT_Data.Name != "" && PT_Data.Active >= 1)
                        {
                            PartyMembersRequired.Items.Add(PT_Data.Name);
                        }
                    }


                }
            }
        }

        #endregion


        #region "CHECK IF THE USER HAS AN ABILITY AND IT'S RECAST TIMER"

        public int AbilityRecast(string checked_abilityName)
        {
            int id = _api.Resources.GetAbility(checked_abilityName, 0).TimerID;
            var IDs = _api.Recast.GetAbilityIds();
            for (var x = 0; x < IDs.Count; x++)
            {
                if (IDs[x] == id)
                    return _api.Recast.GetAbilityRecast(x);
            }
            return 0;
        }

        public static bool HasAbility(string checked_abilityName)
        {

            uint AbilityID = _api.Resources.GetAbility(checked_abilityName, 0).ID;


            if (_api.Player.HasAbility(AbilityID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion

        private void DEBUG_Click(object sender, EventArgs e)
        {
            string Debug_MSG = String.Empty;

            if (Member_List != null && Member_List.Count() > 0)
            {
                foreach (var CharacterD in Member_List)
                {
                    Debug_MSG = Debug_MSG + " " + CharacterD.CharacterName + "\n";
                }
            }

            MessageBox.Show(Debug_MSG);

        }

        private void PartyMembersRequired_SelectedValueChanged(object sender, EventArgs e)
        {
            Member_List.Clear();

            int count = PartyMembersRequired.Items.Count;

            foreach (Object selecteditem in PartyMembersRequired.SelectedItems)
            {
                String strItem = selecteditem as String;
                Member_List.Add(new PartyRequirements { Checked = true, CharacterName = strItem });
            }
        }

        private void ReloadParty_Click(object sender, EventArgs e)
        {
            GrabParty();
        }
    }
}
