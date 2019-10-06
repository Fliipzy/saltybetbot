using System;
using System.Collections.Generic;
using System.Text;

namespace SaltyLibrary.Saltybet.Enums
{
    public enum SaltyRank
    {
        LEVEL_1 = 1,
        LEVEL_2 = 2,
        LEVEL_3 = 3,
        LEVEL_4 = 4,
        LEVEL_5 = 5,
        SALTY_BETTOR = 6,
        SALTY_SEASONED = 7,
        SALTY_PRO = 8,
        SALTY_MASTER = 9,
        SALTY_BOSS = 10,
        SALTY_WIZARD = 11,
        ILLUMINATUS = 12,
        SUPER_ILLUMINATUS = 13,
        SUPER_ILLUMINATUS_EX = 14,
        SKULL_N_BONES = 15,
        HYPER_SKULL_N_BONES = 16,
        ULTRA_SKULL_N_BONES = 17,
        WAIFU = 18,
        SALTY = 19,
        SUPER_SALTY = 20,
        OMEGA_SALTY = 21
    }

    static class SaltyRankExtensions
    {
        public static string GetString(this SaltyRank rank)
        {
            switch (rank)
            {
                case SaltyRank.LEVEL_1:
                    return "Level 1";
                case SaltyRank.LEVEL_2:
                    return "Level 2";
                case SaltyRank.LEVEL_3:
                    return "Level 3";
                case SaltyRank.LEVEL_4:
                    return "Level 4";
                case SaltyRank.LEVEL_5:
                    return "Level 5";
                case SaltyRank.SALTY_BETTOR:
                    return "Salty Bettor";
                case SaltyRank.SALTY_SEASONED:
                    return "Salty Seasoned";
                case SaltyRank.SALTY_PRO:
                    return "Salty Pro";
                case SaltyRank.SALTY_MASTER:
                    return "Salty Master";
                case SaltyRank.SALTY_BOSS:
                    return "Salty Boss";
                case SaltyRank.SALTY_WIZARD:
                    return "Salty Wizard";
                case SaltyRank.ILLUMINATUS:
                    return "Illuminatus";
                case SaltyRank.SUPER_ILLUMINATUS:
                    return "Super Illuminatus";
                case SaltyRank.SUPER_ILLUMINATUS_EX:
                    return "Super Illuminatus EX";
                case SaltyRank.SKULL_N_BONES:
                    return "Skull & Bones";
                case SaltyRank.HYPER_SKULL_N_BONES:
                    return "Hyper Skull & Bones";
                case SaltyRank.ULTRA_SKULL_N_BONES:
                    return "Ultra Skull & Bones";
                case SaltyRank.WAIFU:
                    return "Waifu";
                case SaltyRank.SALTY:
                    return "Salty";
                case SaltyRank.SUPER_SALTY:
                    return "Super Salty";
                case SaltyRank.OMEGA_SALTY:
                    return "Omega Salty";
                default:
                    return null;
            }
        }
    }
}
