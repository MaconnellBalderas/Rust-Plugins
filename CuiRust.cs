using Oxide.Core;
using Oxide.Plugins;
using Oxide.Core.Plugins;
using System;
using System.Collections.Generic;
using UnityEngine;
using Rust;
using static TechTreeData;
using Oxide.Core.Libraries.Covalence;
using System.ComponentModel;
using Oxide.Game.Rust.Cui;
using static PlantProperties;
using System.Numerics;
using System.Linq;
using Oxide.Game.Rust.Libraries;
using ConVar;
using TinyJSON;
using Newtonsoft.Json;
using System.Runtime;

namespace Oxide.Plugins
{
    [Info("GUIRustServer", "Maconnell Balderas", "0.0.1")]
    public class CuiRust : RustPlugin
    {

        void Init()
        {
            timer.Every(20f, UpdatePlayers);
        }
        
        private static string template = @"
        [
                        {
                        ""name"": ""Stage Timer"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.89 0.5 0.16 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.728 0.085"",
                            ""anchormax"": ""0.822 0.134""
                            }
                        ]
                        },
                        {
                        ""name"": ""Timer"",
                        ""parent"": ""Stage Timer"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 0.71"",
                            ""fontSize"": 20,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""Phase: Peace""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.075 0.257"",
                            ""anchormax"": ""0.95 0.886""
                            }
                        ]
                        }, 


                        {
                        ""name"": ""Stage Phase"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.89 0.5 0.16 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.728 0.024"",
                            ""anchormax"": ""0.822 0.073""
                            }
                        ]
                        }, 
                        {
                        ""name"": ""Phase"",
                        ""parent"": ""Stage Phase"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 1"",
                            ""fontSize"": 20,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""11H : 58M""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.017 0.229"",
                            ""anchormax"": ""0.958 0.857""
                            }
                        ]
                        }, 

                        {
                        ""name"": ""Logo"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.RawImage"",
                            ""color"": ""1 1 1 1"",
                            ""url"": ""https://www.crusadergames.net/img/logo.0b421101.png""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0 0.861"",
                            ""anchormax"": ""0.078 1""
                            }
                        ]
                        }, 


                        {
                        ""name"": ""First place Panel 1"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.89 0.5 0.16 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.842 0.960"",
                            ""anchormax"": ""0.866 0.983""
                            }
                        ]
                        },
                        {
                        ""name"": ""#1"",
                        ""parent"": ""First place Panel 1"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 1"",
                            ""fontSize"": 12,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""#1""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.267 0.176"",
                            ""anchormax"": ""0.767 1""
                            }
                        ]
                        },
                        { 
                        ""name"": ""First place Panel 2"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.73 0.31 0 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.866 0.960"",
                            ""anchormax"": ""0.960 0.983""
                            }
                        ]
                        },
                        { 
                        ""name"": ""Username 1"",
                        ""parent"": ""First place Panel 2"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 1"",
                            ""fontSize"": 12,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""{firstPlaceName}""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.025 0.176"",
                            ""anchormax"": ""0.417 1""
                            }
                        ]
                        },
                        { 
                        ""name"": ""First place Panel 3"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.89 0.5 0.16 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.961 0.960"",
                            ""anchormax"": ""1 0.983""
                            }
                        ]
                        },
                        {
                        ""name"": ""Point Count 1"",
                        ""parent"": ""First place Panel 3"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 1"",
                            ""fontSize"": 12,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""{firstPlaceScore}""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.04 0.168"",
                            ""anchormax"": ""0.46 .93""
                            }
                        ]
                        }, 


                        {
                        ""name"": ""Second place Panel 1"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.89 0.5 0.16 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.842 0.935"",
                            ""anchormax"": ""0.866 0.958""
                            }
                        ]
                        },
                        {
                        ""name"": ""#2"",
                        ""parent"": ""Second place Panel 1"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 1"",
                            ""fontSize"": 12,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""#2""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.267 0.176"",
                            ""anchormax"": ""0.767 1""
                            }
                        ]
                        },
                        { 
                        ""name"": ""Second place Panel 2"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.73 0.31 0 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.866 0.935"",
                            ""anchormax"": ""0.960 0.958""
                            }
                        ]
                        },
                        { 
                        ""name"": ""Username 2"",
                        ""parent"": ""Second place Panel 2"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 1"",
                            ""fontSize"": 12,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""{secondPlaceName}""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.025 0.176"",
                            ""anchormax"": ""0.417 1""
                            }
                        ]
                        },
                        { 
                        ""name"": ""Second place Panel 3"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.89 0.5 0.16 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.961 0.935"",
                            ""anchormax"": ""1 0.958""
                            }
                        ]
                        },
                        {
                        ""name"": ""Point Count 2"",
                        ""parent"": ""Second place Panel 3"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 1"",
                            ""fontSize"": 12,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""{secondPlaceScore}""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.04 0.168"",
                            ""anchormax"": ""0.46 .93""
                            }
                        ]
                        }, 


                        {
                        ""name"": ""Third place Panel 1"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.89 0.5 0.16 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.842 0.910"",
                            ""anchormax"": ""0.866 0.933""
                            }
                        ]
                        },
                        {
                        ""name"": ""#3"",
                        ""parent"": ""Third place Panel 1"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 1"",
                            ""fontSize"": 12,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""#3""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.267 0.176"",
                            ""anchormax"": ""0.767 1""
                            }
                        ]
                        },
                        { 
                        ""name"": ""Third place Panel 2"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.73 0.31 0 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.866 0.910"",
                            ""anchormax"": ""0.960 0.933""
                            }
                        ]
                        },
                        { 
                        ""name"": ""Username 3"",
                        ""parent"": ""Third place Panel 2"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 1"",
                            ""fontSize"": 12,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""{thirdPlaceName}""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.025 0.176"",
                            ""anchormax"": ""0.417 1""
                            }
                        ]
                        },
                        { 
                        ""name"": ""Third place Panel 3"",
                        ""parent"": ""Hud"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Image"",
                            ""color"": ""0.89 0.5 0.16 0.65""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.961 0.910"",
                            ""anchormax"": ""1 0.933""
                            }
                        ]
                        },
                        {
                        ""name"": ""Point Count 3"",
                        ""parent"": ""Third place Panel 3"",
                        ""components"": [
                            {
                            ""type"": ""UnityEngine.UI.Text"",
                            ""color"": ""1 1 1 1"",
                            ""fontSize"": 12,
                            ""align"": ""MiddleCenter"",
                            ""text"": ""{thirdPlaceScore}""
                            },
                            {
                            ""type"": ""RectTransform"",
                            ""anchormin"": ""0.04 0.168"",
                            ""anchormax"": ""0.46 .93""
                            }
                        ]
                        }
                    ]";

       
        void deleteUI(BasePlayer player)
        {
            CuiHelper.DestroyUi(player, "Stage Timer");
            CuiHelper.DestroyUi(player, "Stage Phase");
            CuiHelper.DestroyUi(player, "Logo");

            CuiHelper.DestroyUi(player, "First place Panel 1");
            CuiHelper.DestroyUi(player, "First place Panel 2");
            CuiHelper.DestroyUi(player, "First place Panel 3");

            CuiHelper.DestroyUi(player, "Second place Panel 1");
            CuiHelper.DestroyUi(player, "Second place Panel 2");
            CuiHelper.DestroyUi(player, "Second place Panel 3");

            CuiHelper.DestroyUi(player, "Third place Panel 1");
            CuiHelper.DestroyUi(player, "Third place Panel 2");
            CuiHelper.DestroyUi(player, "Third place Panel 3");
        }

        void OnPlayerSleepEnded(BasePlayer player)
        {
            deleteUI(player);
            UpdatePlayerUi(player);
        }

        void UpdatePlayerUi(BasePlayer player)
        {
            deleteUI(player);

            Dictionary<string, int> clanInfo = Interface.Oxide.DataFileSystem.ReadObject<Dictionary<string, int>>("ClanScores");

            string firstPlaceName = "";
            string firstPlaceScore = "";
            string secondPlaceName = "";
            string secondPlaceScore = "";
            string thirdPlaceName = "";
            string thirdPlaceScore = "";

            if (clanInfo != null)
            {
                Dictionary<string, int> sortedClanInfo = clanInfo.OrderByDescending(item => item.Value).ToList()
                                                                .ToDictionary(kv => kv.Key, kv => kv.Value);

                if (sortedClanInfo.Count > 0)
                {
                    if (sortedClanInfo.Count >= 1)
                    {
                        firstPlaceName = sortedClanInfo.First().Key;
                        firstPlaceScore = sortedClanInfo.First().Value.ToString();
                    }
                    if(sortedClanInfo.Count >= 2)
                    {
                        secondPlaceName = sortedClanInfo.Skip(1).First().Key;
                        secondPlaceScore = sortedClanInfo.Skip(1).First().Value.ToString();
                    }
                    if(sortedClanInfo.Count >= 3)
                    {
                        thirdPlaceName = sortedClanInfo.Skip(2).First().Key;
                        thirdPlaceScore = sortedClanInfo.Skip(2).First().Value.ToString();
                    }
                }
            }

            String filledTempalte = template
                                        .Replace("{firstPlaceName}", firstPlaceName)
                                        .Replace("{firstPlaceScore}", firstPlaceScore)
                                        .Replace("{secondPlaceName}", secondPlaceName)
                                        .Replace("{secondPlaceScore}", secondPlaceScore)
                                        .Replace("{thirdPlaceName}", thirdPlaceName)
                                        .Replace("{thirdPlaceScore}", thirdPlaceScore);

            CuiHelper.AddUi(player, filledTempalte);
        }

        void UpdatePlayers()
        {
            foreach (BasePlayer player in BasePlayer.activePlayerList as ListHashSet<BasePlayer>)
            {
                UpdatePlayerUi(player);
            }
        }

        void OnPlayerInit(BasePlayer player)
        {
            Puts($"Player {player.displayName} Is Trying To Join.");
            List<string> whiteListInfo = Interface.Oxide.DataFileSystem.ReadObject<List<string>>("Whitelist");

            if (whiteListInfo.Contains(player.displayName))
            {
                player.Kick("You Need To Be Whitelisted To Join This Tournament. To Learn More, Go To https://rust.crusadergames.net/.");
                Puts($"Player {player.displayName} ({player.userID}) was denied access (not whitelisted).");
            }
        }

        void OnUserConnected(IPlayer player)
        {
            Puts($"Player {player.Name} Is Trying To Join.");
            List<string> whiteListInfo = Interface.Oxide.DataFileSystem.ReadObject<List<string>>("Whitelist");

            if (whiteListInfo.Contains(player.Name))
            {
                player.Kick("You Need To Be Whitelisted To Join This Tournament. To Learn More, Go To https://rust.crusadergames.net/.");
                Puts($"Player {player.Name} ({player.Id}) was denied access (not whitelisted).");
            }
        }


        void OnEntityDeath(BaseCombatEntity entity, HitInfo info)
        {
            Dictionary<string, string> playerClanInfo = Interface.Oxide.DataFileSystem.ReadObject<Dictionary<string, string>>("PlayerClanList");

            if(entity is BuildingPrivlidge toolCupboard)
            {
                if (info?.Initiator is BasePlayer player)
                {
                    var owner = BasePlayer.FindByID(toolCupboard.OwnerID);
                    if (playerClanInfo[player.displayName.Replace(" ", "_")] == playerClanInfo[owner.displayName.Replace(" ", "_")]) return;

                    TrackTCKill(playerClanInfo[player.displayName.Replace(" ", "_")]);
                }
            } else
            {
                var victim = entity as BasePlayer;
                if (victim == null || !victim.IsConnected) return;

                var attacker = info?.Initiator as BasePlayer;
                if (attacker == null || !attacker.IsConnected) return;

                if (attacker.userID == victim.userID) return;

                if (playerClanInfo[attacker.displayName.Replace(" ", "_")] == playerClanInfo[victim.displayName.Replace(" ", "_")]) return;

                string attackerClan = playerClanInfo[attacker.displayName.Replace(" ", "_")];
                Puts($"{attacker.displayName} has earned a kill point for killing {victim.displayName}");
                TrackKill(attackerClan);
            }
        }

        private void TrackTCKill(string attackerClan)
        {
            Dictionary<string, int> clanInfo = Interface.Oxide.DataFileSystem.ReadObject<Dictionary<string, int>>("ClanScores");

            int originalClanScore = clanInfo[attackerClan];
            Puts($"{originalClanScore} destroyed the ToolCupboard ");
            int updatedClanScore = originalClanScore + 12;
            Puts($"{updatedClanScore} destroyed the ToolCupboard ");
            clanInfo[attackerClan] = updatedClanScore;

            Interface.Oxide.DataFileSystem.WriteObject("ClanScores", clanInfo);
        }

        private void TrackKill(string attackerClan)
        {
            Dictionary<string, int> clanInfo = Interface.Oxide.DataFileSystem.ReadObject<Dictionary<string, int>>("ClanScores");

            int originalClanScore = clanInfo[attackerClan];
            int updatedClanScore = originalClanScore+ 1;
            clanInfo[attackerClan] = updatedClanScore;

            Interface.Oxide.DataFileSystem.WriteObject("ClanScores", clanInfo);
        }

    }
}