using System.Collections.Generic;

namespace LostAngeles.Client.Core.CharacterCustomizer
{
    static class Variants
    {
        public static KeyValuePair<string, string> HairDecorDefault =
            new KeyValuePair<string, string>("mpbeach_overlays", "FM_hair_fuzz");

        public static readonly Dictionary<int, KeyValuePair<string, string>> FemaleHairDecor =
            new Dictionary<int, KeyValuePair<string, string>>()
            {
                [0] = new KeyValuePair<string, string>("", ""),
                [1] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_F_Hair_001"),
                [2] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_F_Hair_002"),
                [3] = new KeyValuePair<string, string>("multiplayer_overlays", "FM_F_Hair_003_a"),
                [4] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_F_Hair_004"),
                [5] = new KeyValuePair<string, string>("multiplayer_overlays", "FM_F_Hair_005_a"),
                [6] = new KeyValuePair<string, string>("multiplayer_overlays", "FM_F_Hair_006_a"),
                [7] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_F_Hair_007"),
                [8] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_F_Hair_008"),
                [9] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_F_Hair_009"),
                [10] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_F_Hair_010"),
                [11] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_F_Hair_011"),
                [12] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_F_Hair_012"),
                [13] = new KeyValuePair<string, string>("multiplayer_overlays", "FM_F_Hair_013_a"),
                [14] = new KeyValuePair<string, string>("multiplayer_overlays", "FM_F_Hair_014_a"),
                [15] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_M_Hair_015"),
                [16] = new KeyValuePair<string, string>("multiplayer_overlays", "NGBea_F_Hair_000"),
                [17] = new KeyValuePair<string, string>("mpbusiness_overlays", "FM_Bus_F_Hair_a"),
                [18] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_F_Hair_007"),
                [19] = new KeyValuePair<string, string>("multiplayer_overlays", "NGBus_F_Hair_000"),
                [20] = new KeyValuePair<string, string>("multiplayer_overlays", "NGBus_F_Hair_001"),
                [21] = new KeyValuePair<string, string>("multiplayer_overlays", "NGBea_F_Hair_001"),
                [22] = new KeyValuePair<string, string>("mphipster_overlays", "FM_Hip_F_Hair_000_a"),
                [23] = new KeyValuePair<string, string>("multiplayer_overlays", "NGInd_F_Hair_000"),
                // 24
                [25] = new KeyValuePair<string, string>("mplowrider_overlays", "LR_F_Hair_000"),
                [26] = new KeyValuePair<string, string>("mplowrider_overlays", "LR_F_Hair_001"),
                [27] = new KeyValuePair<string, string>("mplowrider_overlays", "LR_F_Hair_002"),
                [29] = new KeyValuePair<string, string>("mplowrider2_overlays", "LR_F_Hair_003"),
                [30] = new KeyValuePair<string, string>("mplowrider2_overlays", "LR_F_Hair_004"),
                [31] = new KeyValuePair<string, string>("mplowrider2_overlays", "LR_F_Hair_006"),
                [32] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_000_F"),
                [33] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_001_F"),
                [34] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_002_F"),
                [35] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_003_F"),
                [38] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_004_F"),
                [36] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_005_F"),
                [37] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_005_F"),

                [76] = new KeyValuePair<string, string>("mpgunrunning_overlays", "MP_Gunrunning_Hair_F_000_F"),
                [77] = new KeyValuePair<string, string>("mpgunrunning_overlays", "MP_Gunrunning_Hair_F_001_F"),
                [78] = new KeyValuePair<string, string>("mpvinewood_overlays", "MP_Vinewood_Hair_F_000_F"),
                [79] = new KeyValuePair<string, string>("mptuner_overlays", "MP_Tuner_Hair_000_F"),
                [80] = new KeyValuePair<string, string>("mpsecurity_overlays", "MP_Security_Hair_000_F"),
            };

        public static readonly Dictionary<int, KeyValuePair<string, string>> MaleHairDecor =
            new Dictionary<int, KeyValuePair<string, string>>()
            {
                [0] = new KeyValuePair<string, string>("", ""),
                [1] = new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_a"),
                [2] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_M_Hair_002"),
                [3] = new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_003_a"),
                [4] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_M_Hair_004"),
                [5] = new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_long_a"),
                [6] = new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_006_a"),
                // 7
                [8] = new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_008_a"),
                [9] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_M_Hair_009"),
                [10] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_M_Hair_013"),
                [11] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_M_Hair_002"),
                [12] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_M_Hair_011"),
                [13] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_M_Hair_012"),
                [14] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_M_Hair_014"),
                [15] = new KeyValuePair<string, string>("multiplayer_overlays", "NG_M_Hair_015"),
                [16] = new KeyValuePair<string, string>("multiplayer_overlays", "NGBea_M_Hair_000"),
                [17] = new KeyValuePair<string, string>("multiplayer_overlays", "NGBea_M_Hair_001"),
                [18] = new KeyValuePair<string, string>("mpbusiness_overlays", "FM_Bus_M_Hair_000_a"),
                [19] = new KeyValuePair<string, string>("mpbusiness_overlays", "FM_Bus_M_Hair_001_a"),
                [20] = new KeyValuePair<string, string>("mphipster_overlays", "FM_Hip_M_Hair_000_a"),
                [21] = new KeyValuePair<string, string>("mphipster_overlays", "FM_Hip_M_Hair_001_a"),
                [22] = new KeyValuePair<string, string>("multiplayer_overlays", "NGInd_M_Hair_000"),
                // 23
                [24] = new KeyValuePair<string, string>("mplowrider_overlays", "LR_M_Hair_000"),
                [25] = new KeyValuePair<string, string>("mplowrider_overlays", "LR_M_Hair_001"),
                [26] = new KeyValuePair<string, string>("mplowrider_overlays", "LR_M_Hair_002"),
                [27] = new KeyValuePair<string, string>("mplowrider_overlays", "LR_M_Hair_003"),
                [28] = new KeyValuePair<string, string>("mplowrider2_overlays", "LR_M_Hair_004"),
                [29] = new KeyValuePair<string, string>("mplowrider2_overlays", "LR_M_Hair_005"),
                [30] = new KeyValuePair<string, string>("mplowrider2_overlays", "LR_M_Hair_006"),
                [31] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_000_M"),
                [32] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_001_M"),
                [33] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_002_M"),
                [34] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_003_M"),
                [35] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_004_M"),
                [36] = new KeyValuePair<string, string>("mpbiker_overlays", "MP_Biker_Hair_005_M"),

                [72] = new KeyValuePair<string, string>("mpgunrunning_overlays", "MP_Gunrunning_Hair_M_000_M"),
                [73] = new KeyValuePair<string, string>("mpgunrunning_overlays", "MP_Gunrunning_Hair_M_001_M"),
                [74] = new KeyValuePair<string, string>("mpvinewood_overlays", "MP_Vinewood_Hair_M_000_M"),
                [75] = new KeyValuePair<string, string>("mptuner_overlays", "MP_Tuner_Hair_001_M"),
                [76] = new KeyValuePair<string, string>("mpsecurity_overlays", "MP_Security_Hair_001_M"),
            };
        
        
        public static readonly List<dynamic> MaleHairNames = new List<dynamic>()
        {
            "Close Shave", "Buzzcut", "Faux Hawk", "Hipster", "Side Parting", "Shorter Cut", "Biker", "Ponytail",
            "Cornrows", "Slicked", "Short Brushed", "Spikey", "Caesar", "Chopped", "Dreads", "Long Hair",
            "Shaggy Curls", "Surfer Dude", "Short Side Part", "High Slicked Sides", "Long Slicked", "Hipster Youth",
            "Mullet"
        };

        public static readonly List<dynamic> FemaleHairNames = new List<dynamic>()
        {
            "Close Shave", "Short", "Layered Bob", "Pigtails", "Ponytail", "Braided Mohawk", "Braids", "Bob",
            "Faux Hawk", "French Twist", "Long Bob", "Loose Tied", "Pixie", "Shaved Bangs", "Top Knot", "Wavy Bob",
            "Pin Up Girl", "Messy Bun", "Unknown", "Tight Bun", "Twisted Bob", "Big Bangs", "Braided Top Knot"
        };

        public static readonly List<dynamic> EyebrowsNames = new List<dynamic>()
        {
            "Balanced", "Fashion", "Cleopatra", "Quizzical", "Femme", "Seductive", "Pinched", "Chola", "Triomphe",
            "Carefree", "Curvaceous", "Rodent", "Double Tram", "Thin", "Penciled", "Mother Plucker",
            "Straight and Narrow", "Natural", "Fuzzy", "Unkempt", "Caterpillar", "Regular", "Mediterranean", "Groomed",
            "Bushels", "Feathered", "Prickly", "Monobrow", "Winged", "Triple Tram", "Arched Tram", "Cutouts",
            "Fade Away", "Solo Tram"
        };

        public static readonly List<dynamic> BeardNames = new List<dynamic>()
        {
            "Clean Shaven", "Light Stubble", "Balbo", "Circle Beard", "Goatee", "Chin", "Chin Fuzz",
            "Pencil Chin Strap", "Scruffy", "Musketeer", "Mustache",
            "Trimmed Beard", "Stubble", "Thin Circle Beard", "Horseshoe", "Pencil and Chops", "Chin Strap Beard",
            "Balbo and Sideburns", "Mutton Chops", "Scruffy Beard", "Curly",
            "Curly and Deep Stranger", "Handlebar", "Faustic", "Otto and Patch", "Otto and Full Stranger",
            "Light Franz", "The Hampstead", "The Ambrose", "Lincoln Curtain"
        };

        public static readonly List<dynamic> Blemishes = new List<dynamic>()
        {
            "None", "Measles", "Pimples", "Spots", "Break Out", "Blackheads", "Build Up", "Pustules", "Zits",
            "Full Acne", "Acne", "Cheek Rash", "Face Rash",
            "Picker", "Puberty", "Eyesore", "Chin Rash", "Two Face", "T Zone", "Greasy", "Marked", "Acne Scarring",
            "Full Acne Scarring", "Cold Sores", "Impetigo"
        };

        public static readonly List<dynamic> AgingNames = new List<dynamic>()
        {
            "None", "Crow's Feet", "First Signs", "Middle Aged", "Worry Lines", "Depression", "Distinguished", "Aged",
            "Weathered", "Wrinkled", "Sagging", "Tough Life",
            "Vintage", "Retired", "Junkie", "Geriatric"
        };

        public static readonly List<dynamic> Complexion = new List<dynamic>()
        {
            "None", "Rosy Cheeks", "Stubble Rash", "Hot Flush", "Sunburn", "Bruised", "Alchoholic", "Patchy", "Totem",
            "Blood Vessels", "Damaged", "Pale", "Ghostly"
        };

        public static readonly List<dynamic> Molefreckle = new List<dynamic>()
        {
            "None", "Cherub", "All Over", "Irregular", "Dot Dash", "Over the Bridge", "Baby Doll", "Pixie",
            "Sun Kissed", "Beauty Marks", "Line Up", "Modelesque",
            "Occasional", "Speckled", "Rain Drops", "Double Dip", "One Sided", "Pairs", "Growth"
        };

        public static readonly List<dynamic> SunDamage = new List<dynamic>()
        {
            "None", "Uneven", "Sandpaper", "Patchy", "Rough", "Leathery", "Textured", "Coarse", "Rugged", "Creased",
            "Cracked", "Gritty"
        };

        public static readonly List<dynamic> EyeColorNames = new List<dynamic>()
        {
            "Green", "Emerald", "Light Blue", "Ocean Blue", "Light Brown", "Dark Brown", "Hazel", "Dark Gray",
            "Light Gray", "Pink", "Yellow", "Purple", "Blackout",
            "Shades of Gray", "Tequila Sunrise", "Atomic", "Warp", "ECola", "Space Ranger", "Ying Yang", "Bullseye",
            "Lizard", "Dragon", "Extra Terrestrial", "Goat", "Smiley", "Possessed",
            "Demon", "Infected", "Alien", "Undead", "Zombie"
        };

        public static readonly List<dynamic> Makeups = new List<dynamic>()
        {
            "None", "Smoky Black", "Bronze", "Soft Gray", "Retro Glam", "Natural Look", "Cat Eyes", "Chola", "Vamp",
            "Vinewood Glamour", "Bubblegum", "Aqua Dream", "Pin up", "Purple Passion", "Smoky Cat Eye",
            "Smoldering Ruby", "Pop Princess"
        };

        public static readonly List<dynamic> Lipsticks = new List<dynamic>()
        {
            "None", "Color Matte", "Color Gloss", "Lined Matte", "Lined Gloss", "Heavy Lined Matte",
            "Heavy Lined Gloss", "Lined Nude Matte", "Liner Nude Gloss", "Smudged", "Geisha"
        };

        public static readonly List<dynamic> Blushes = new List<dynamic>()
        {
            "None", "Full", "Angled", "Round", "Horizontal", "High", "Sweetheart", "Eighties"
        };
        
        public static readonly Dictionary<int, KeyValuePair<int, int>> MaleDefaultOutfits =
            new Dictionary<int, KeyValuePair<int, int>>()
            {
                [3] = new KeyValuePair<int, int>(15, 0),
                [4] = new KeyValuePair<int, int>(61, 0),
                [6] = new KeyValuePair<int, int>(34, 0),
                [8] = new KeyValuePair<int, int>(15, 0),
                [11] = new KeyValuePair<int, int>(15, 0)
            };
        
        public static readonly Dictionary<int, KeyValuePair<int, int>> FemaleDefaultOutfits =
            new Dictionary<int, KeyValuePair<int, int>>()
            {
                [3] = new KeyValuePair<int, int>(15, 0),
                [4] = new KeyValuePair<int, int>(15, 0),
                [6] = new KeyValuePair<int, int>(35, 0),
                [8] = new KeyValuePair<int, int>(15, 0),
                [11] = new KeyValuePair<int, int>(15, 0)
            };
    }
}