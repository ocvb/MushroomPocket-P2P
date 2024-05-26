using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;

using static MushroomPocket.MainExtra;
using Microsoft.IdentityModel.Tokens;

// Name: Kai Jie
// Admin No: 234412H

namespace MushroomPocket
{
    public class MushroomP2P
    {
        public static void Multiplayer(Users user)
        {
            IPAddress DeviceIp = Server.getIp();
            IPAddress LocalIp;
            IPAddress.TryParse("127.0.0.1", out LocalIp);


            TcpListener listener = null;

            try
            {
                PrintColor("Multiplayer Mode");

                PrintColor("(1) Connect to a peer");
                PrintColor("(2) Create a peer server");
                PrintColor("Select an option: ", newLine: false);
                // string choice = Convert.ToString(Console.ReadLine());

                var key = Console.ReadKey(false);
                string choice = key.KeyChar.ToString();

                if (choice != "1" && choice != "2")
                {
                    PrintColor("Invalid input! Please only enter (1, 2).");
                    return;
                }

                if (choice.IsNullOrEmpty())
                {
                    PrintColor("Cannot be empty! Please only enter (1, 2).");
                    return;
                }

                Console.WriteLine();


                switch (choice)
                {
                    case "1":
                        {
                            PrintColor("You are using Local Device IP: " + DeviceIp);
                            listener = new TcpListener(DeviceIp, Server.ConnectingToServer_port);
                            listener.Start();

                            PrintColor("Enter the peer IP or (Press Enter to use Local IP): ", newLine: false);
                            string peerip = Console.ReadLine();

                            if (string.IsNullOrEmpty(peerip))
                            {
                                peerip = LocalIp.ToString();
                            }

                            Server.ConnectToPeer(peerip, Server.ServerPort, Message.CLIENT_CONNECT);
                            Server.SendToPeer(peerip, Server.ServerPort, new Dictionary<string, string> { { "player", user.Username }, { "ip", DeviceIp.ToString() } });

                            Message result = TcpListenerExtensions.ListenForMessage(listener);

                            object ServerNetwork = RetrievePeerNetworkDetails(listener);
                            string ServerPeerUsername = ((Dictionary<string, string>)ServerNetwork)["player"];
                            IPAddress ServerNetworkIP = IPAddress.Parse(((Dictionary<string, string>)ServerNetwork)["ip"]);

                            PrintColor($"{ServerPeerUsername} has connected from {ServerNetworkIP}!", ConsoleColor.Green);

                            if (result == Message.SERVER_CONNECT)
                            {
                                PrintColor("Connected!", ConsoleColor.Green);

                                Game(listener, Server.ServerPort, user, ServerNetworkIP);

                            }

                        }
                        break;
                    case "2":
                        {
                            PrintColor("This server uses Local IP: " + LocalIp);
                            listener = new TcpListener(LocalIp, Server.ServerPort);
                            listener.Start();

                            PrintColor("Waiting for a peer to connect...", ConsoleColor.Yellow, newLine: false);
                            Console.WriteLine();

                            Message message = TcpListenerExtensions.ListenForMessage(listener);

                            object PeerNetwork = RetrievePeerNetworkDetails(listener);
                            string peerUsername = ((Dictionary<string, string>)PeerNetwork)["player"];
                            IPAddress peerNetworkIP = IPAddress.Parse(((Dictionary<string, string>)PeerNetwork)["ip"]);


                            PrintColor($"{peerUsername} has connected from {peerNetworkIP}!", ConsoleColor.Green);


                            if (message == Message.CLIENT_CONNECT)
                            {
                                PrintColor("Client connected!", ConsoleColor.Green);
                                Server.ConnectToPeer(peerNetworkIP.ToString(), Server.ConnectingToServer_port, Message.SERVER_CONNECT);

                                // Send server ip to peer - Future proofing
                                Server.SendToPeer(peerNetworkIP.ToString(), Server.ConnectingToServer_port, new Dictionary<string, string> { { "player", user.Username }, { "ip", LocalIp.ToString() } });


                                Game(listener, Server.ConnectingToServer_port, user, peerNetworkIP);
                            }
                        }

                        break;
                    default:
                        Console.WriteLine("Invalid input! Please only enter (1, 2).");
                        break;
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                }
                ContinueWhenPressed();
            }
        }

        protected static object RetrievePeerNetworkDetails(TcpListener listener)
        {
            string ip = Server.PeerResponse(listener);
            var ipTrim = ip.Trim('"');
            Dictionary<string, string> getPeerIP = JsonConvert.DeserializeObject<Dictionary<string, string>>(ipTrim);
            return getPeerIP;
        }

        protected static string GameMenu()
        {
            PrintColor("(1) Attack");
            PrintColor("(2) Exit");
            PrintColor("Select an option: ", newLine: false);

            string option = Console.ReadLine();

            return option;
        }

        protected static void Game(TcpListener listener, int Port, Users user, IPAddress ipAddress)
        {
            // Console.Clear();
            PrintColor($"My IP: {ipAddress.ToString()}", ConsoleColor.Green);

            try
            {
                while (true)
                {
                    var option = GameMenu();
                    if (option.IsNullOrEmpty()) continue;
                    else if (option == "2") break;
                    else if (option == "1")
                    {
                        var nowdb = new MushroomDbContext();
                        var currentUser = nowdb.Users.FirstOrDefault(x => x.Username == user.Username);
                        var heroes = nowdb.MushroomHeroes.Where(x => x.UserId == currentUser.Id).ToList();

                        if (heroes.Count <= 0)
                        {
                            PrintColor("You have no characters to play with!");
                            break;
                        }

                        PrintColor(string.Concat(Enumerable.Repeat("-", 30)));
                        foreach (MushroomHeroes h in heroes)
                        {
                            if (!h.isDead)
                            {
                                PrintColor($"(ID: {h.Id}), Name; {h.Name}, HP: {h.Hp}, Damage: {h.damage}, Exp: {h.Exp}");
                            }
                        }
                        PrintColor(string.Concat(Enumerable.Repeat("-", 30)));

                        PrintColor("Select a character ID: ", newLine: false);
                        string selectedHeroID = Console.ReadLine();
                        if (selectedHeroID == null || selectedHeroID == "")
                        {
                            PrintColor("Invalid input! Please enter a valid character ID.");
                            continue;
                        }

                        if (!int.TryParse(selectedHeroID, out int selectedID))
                        {
                            PrintColor("Invalid input! Please enter a valid character ID.");
                            continue;
                        }

                        var hero = heroes.Find(x => x.Id == selectedID);

                        if (!heroes.Exists(x => x.Id == selectedID))
                        {
                            PrintColor("Invalid character selected!");
                            continue;
                        }

                        if (hero.isDead)
                        {
                            PrintColor("Character is dead!", ConsoleColor.Red);
                            continue;
                        }

                        Dictionary<string, string> userData = new Dictionary<string, string> { { "player", user.Username }, { "name", hero.Name }, { "hp", hero.Hp.ToString() }, { "isDead", hero.isDead.ToString() } };

                        // Send the Dictionary
                        Server.SendToPeer(ipAddress.ToString(), Port, userData);
                        PrintColor("Waiting for your opponent to select a character...", ConsoleColor.Yellow, newLine: false);

                        string peerHeroJson = Server.PeerResponse(listener);
                        peerHeroJson = peerHeroJson.Trim('"');
                        Dictionary<string, string> responseUserData = JsonConvert.DeserializeObject<Dictionary<string, string>>(peerHeroJson);
                        Console.WriteLine();

                        double IntroEnemyHp = Convert.ToDouble(responseUserData["hp"]);
                        string IntroEnemyName = responseUserData["name"];

                        PrintColor($"Your opponent's character: {IntroEnemyName}");
                        PrintColor($"Your opponent's character HP: {IntroEnemyHp}");

                        PrintColor("Battle begins!", ConsoleColor.Green);
                        Thread.Sleep(1000);

                        // int coolDown = 0;

                        if (hero.Hp > 0 && IntroEnemyHp > 0)
                        {
                            // prompt user to use skill or not
                            // Console.WriteLine("Do you want to use your character's skill? (y/n)");
                            // string useSkill = Console.ReadLine();

                            // if (useSkill.ToLower() == "y")
                            // {
                            //     if (hero is MushroomHeroes)
                            //     {
                            //         if (hero.Name == "Daisy")
                            //         {
                            //             hero.Hp = hero.Hp;
                            //             hero.UseSkill(10);
                            //             coolDown = 3;
                            //         }
                            //         else if (hero.Name == "Wario")
                            //         {
                            //             hero.Hp = hero.Hp;
                            //             hero.UseSkill(10);
                            //             coolDown = 3;
                            //         }
                            //     }
                            //     else if (hero is EvolvedMushroom)
                            //     {
                            //         if (hero.Name == "Peach")
                            //         {
                            //             hero.Hp = hero.Hp;
                            //             hero.UseSkill(10);
                            //             coolDown = 3;
                            //         }
                            //         else if (hero.Name == "Mario")
                            //         {
                            //             hero.UseSkill(hero.Hp);
                            //             coolDown = 3;
                            //         }
                            //     }
                            // }


                            // Damage Enemy Character
                            Random rng = new Random();
                            double damage = Math.Round(1 + rng.NextDouble() * (Convert.ToDouble(hero.damage) - 1), 2);

                            Server.SendToPeer(ipAddress.ToString(), Port, new Dictionary<string, string> { { "damage", damage.ToString() }, { "hp", hero.Hp.ToString() } });
                            PrintColor($"You attacked your opponent's character with {damage} damage.", ConsoleColor.Green);
                            Console.WriteLine();

                            Thread.Sleep(1000);

                            // Receive Damage
                            string peerDamageJson = Server.PeerResponse(listener);
                            peerDamageJson = peerDamageJson.Trim('"');
                            Dictionary<string, string> responseDamageData = JsonConvert.DeserializeObject<Dictionary<string, string>>(peerDamageJson);

                            double enemyHp = Convert.ToDouble(responseDamageData["hp"]);
                            double enemyDamage = Convert.ToDouble(responseDamageData["damage"]);
                            double enemyFinalHp = enemyHp - enemyDamage;

                            PrintColor($"Enemy attacked you with {enemyDamage} damage.", ConsoleColor.Red);
                            Console.WriteLine();

                            hero.Hp -= enemyDamage;

                            if (hero.Hp <= 0)
                            {
                                hero.isDead = true;
                            }

                            if (enemyFinalHp <= 0)
                            {
                                hero.Exp += 10;
                            }

                            PrintColor($"Your character HP after attack: {hero.Hp}", ConsoleColor.Green);
                            Console.WriteLine();

                            nowdb.SaveChanges();

                        }
                        else
                        {
                            PrintColor("Character is dead!", ConsoleColor.Red);
                        }
                    }
                }
            }
            catch (SocketException)
            {
                PrintColor("The connection was lost or opponent has disconnected.", ConsoleColor.Red);
                return;

            }
            catch (Exception ex)
            {
                PrintColor(ex.Message, ConsoleColor.Red);
                return;
            }
            finally
            {
                listener.Stop();
            }


        }

    }
}