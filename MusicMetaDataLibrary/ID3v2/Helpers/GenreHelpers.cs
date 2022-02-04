﻿using ExtensionsLibrary.Extensions;
using System.Text.RegularExpressions;

namespace MusicMetaDataLibrary.ID3v2.Helpers
{
    public static class GenreHelpers
    {
        public static string ToGenre(int value)
        {
            switch (value)
            {
                case 0: return "Blues";
                case 1: return "Classic Rock";
                case 2: return "Country";
                case 3: return "Dance";
                case 4: return "Disco";
                case 5: return "Funk";
                case 6: return "Grunge";
                case 7: return "Hip-Hop";
                case 8: return "Jazz";
                case 9: return "Metal";
                case 10: return "New Age";
                case 11: return "Oldies";
                case 12: return "Other";
                case 13: return "Pop";
                case 14: return "R&B";
                case 15: return "Rap";
                case 16: return "Reggae";
                case 17: return "Rock";
                case 18: return "Techno";
                case 19: return "Industrial";
                case 20: return "Alternative";
                case 21: return "Ska";
                case 22: return "Death Metal";
                case 23: return "Pranks";
                case 24: return "Soundtrack";
                case 25: return "Euro-Techno";
                case 26: return "Ambient";
                case 27: return "Trip-Hop";
                case 28: return "Vocal";
                case 29: return "Jazz+Funk";
                case 30: return "Fusion";
                case 31: return "Trance";
                case 32: return "Classical";
                case 33: return "Instrumental";
                case 34: return "Acid";
                case 35: return "House";
                case 36: return "Game";
                case 37: return "Sound Clip";
                case 38: return "Gospel";
                case 39: return "Noise";
                case 40: return "AlternRock";
                case 41: return "Bass";
                case 42: return "Soul";
                case 43: return "Punk";
                case 44: return "Space";
                case 45: return "Meditative";
                case 46: return "Instrumental Pop";
                case 47: return "Instrumental Rock";
                case 48: return "Ethnic";
                case 49: return "Gothic";
                case 50: return "Darkwave";
                case 51: return "Techno-Industrial";
                case 52: return "Electronic";
                case 53: return "Pop-Folk";
                case 54: return "Eurodance";
                case 55: return "Dream";
                case 56: return "Southern Rock";
                case 57: return "Comedy";
                case 58: return "Cult";
                case 59: return "Gangsta";
                case 60: return "Top 40";
                case 61: return "Christian Rap";
                case 62: return "Pop/Funk";
                case 63: return "Jungle";
                case 64: return "Native American";
                case 65: return "Cabaret";
                case 66: return "New Wave";
                case 67: return "Psychadelic";
                case 68: return "Rave";
                case 69: return "Showtunes";
                case 70: return "Trailer";
                case 71: return "Lo-Fi";
                case 72: return "Tribal";
                case 73: return "Acid Punk";
                case 74: return "Acid Jazz";
                case 75: return "Polka";
                case 76: return "Retro";
                case 77: return "Musical";
                case 78: return "Rock & Roll";
                case 79: return "Hard Rock";
                case 80: return "Folk";
                case 81: return "Folk-Rock";
                case 82: return "National Folk";
                case 83: return "Swing";
                case 84: return "Fast Fusion";
                case 85: return "Bebob";
                case 86: return "Latin";
                case 87: return "Revival";
                case 88: return "Celtic";
                case 89: return "Bluegrass";
                case 90: return "Avantgarde";
                case 91: return "Gothic Rock";
                case 92: return "Progressive Rock";
                case 93: return "Psychedelic Rock";
                case 94: return "Symphonic Rock";
                case 95: return "Slow Rock";
                case 96: return "Big Band";
                case 97: return "Chorus";
                case 98: return "Easy Listening";
                case 99: return "Acoustic";
                case 100: return "Humour";
                case 101: return "Speech";
                case 102: return "Chanson";
                case 103: return "Opera";
                case 104: return "Chamber Music";
                case 105: return "Sonata";
                case 106: return "Symphony";
                case 107: return "Booty Bass";
                case 108: return "Primus";
                case 109: return "Porn Groove";
                case 110: return "Satire";
                case 111: return "Slow Jam";
                case 112: return "Club";
                case 113: return "Tango";
                case 114: return "Samba";
                case 115: return "Folklore";
                case 116: return "Ballad";
                case 117: return "Power Ballad";
                case 118: return "Rhythmic Soul";
                case 119: return "Freestyle";
                case 120: return "Duet";
                case 121: return "Punk Rock";
                case 122: return "Drum Solo";
                case 123: return "A cappella";
                case 124: return "Euro-House";
                case 125: return "Dance Hall";
                default: return string.Empty;
            }
        }

        public static int ToGenre(string value)
        {
            switch (value)
            {
                case "Blues": return 0;
                case "Classic Rock": return 1;
                case "Country": return 2;
                case "Dance": return 3;
                case "Disco": return 4;
                case "Funk": return 5;
                case "Grunge": return 6;
                case "Hip-Hop": return 7;
                case "Jazz": return 8;
                case "Metal": return 9;
                case "New Age": return 10;
                case "Oldies": return 11;
                case "Other": return 12;
                case "Pop": return 13;
                case "R&B": return 14;
                case "Rap": return 15;
                case "Reggae": return 16;
                case "Rock": return 17;
                case "Techno": return 18;
                case "Industrial": return 19;
                case "Alternative": return 20;
                case "Ska": return 21;
                case "Death Metal": return 22;
                case "Pranks": return 23;
                case "Soundtrack": return 24;
                case "Euro-Techno": return 25;
                case "Ambient": return 26;
                case "Trip-Hop": return 27;
                case "Vocal": return 28;
                case "Jazz+Funk": return 29;
                case "Fusion": return 30;
                case "Trance": return 31;
                case "Classical": return 32;
                case "Instrumental": return 33;
                case "Acid": return 34;
                case "House": return 35;
                case "Game": return 36;
                case "Sound Clip": return 37;
                case "Gospel": return 38;
                case "Noise": return 39;
                case "AlternRock": return 40;
                case "Bass": return 41;
                case "Soul": return 42;
                case "Punk": return 43;
                case "Space": return 44;
                case "Meditative": return 45;
                case "Instrumental Pop": return 46;
                case "Instrumental Rock": return 47;
                case "Ethnic": return 48;
                case "Gothic": return 49;
                case "Darkwave": return 50;
                case "Techno-Industrial": return 51;
                case "Electronic": return 52;
                case "Pop-Folk": return 53;
                case "Eurodance": return 54;
                case "Dream": return 55;
                case "Southern Rock": return 56;
                case "Comedy": return 57;
                case "Cult": return 58;
                case "Gangsta": return 59;
                case "Top 40": return 60;
                case "Christian Rap": return 61;
                case "Pop/Funk": return 62;
                case "Jungle": return 63;
                case "Native American": return 64;
                case "Cabaret": return 65;
                case "New Wave": return 66;
                case "Psychadelic": return 67;
                case "Rave": return 68;
                case "Showtunes": return 69;
                case "Trailer": return 70;
                case "Lo-Fi": return 71;
                case "Tribal": return 72;
                case "Acid Punk": return 73;
                case "Acid Jazz": return 74;
                case "Polka": return 75;
                case "Retro": return 76;
                case "Musical": return 77;
                case "Rock & Roll": return 78;
                case "Hard Rock": return 79;
                case "Folk": return 80;
                case "Folk-Rock": return 81;
                case "National Folk": return 82;
                case "Swing": return 83;
                case "Fast Fusion": return 84;
                case "Bebob": return 85;
                case "Latin": return 86;
                case "Revival": return 87;
                case "Celtic": return 88;
                case "Bluegrass": return 89;
                case "Avantgarde": return 90;
                case "Gothic Rock": return 91;
                case "Progressive Rock": return 92;
                case "Psychedelic Rock": return 93;
                case "Symphonic Rock": return 94;
                case "Slow Rock": return 95;
                case "Big Band": return 96;
                case "Chorus": return 97;
                case "Easy Listening": return 98;
                case "Acoustic": return 99;
                case "Humour": return 100;
                case "Speech": return 101;
                case "Chanson": return 102;
                case "Opera": return 103;
                case "Chamber Music": return 104;
                case "Sonata": return 105;
                case "Symphony": return 106;
                case "Booty Bass": return 107;
                case "Primus": return 108;
                case "Porn Groove": return 109;
                case "Satire": return 110;
                case "Slow Jam": return 111;
                case "Club": return 112;
                case "Tango": return 113;
                case "Samba": return 114;
                case "Folklore": return 115;
                case "Ballad": return 116;
                case "Power Ballad": return 117;
                case "Rhythmic Soul": return 118;
                case "Freestyle": return 119;
                case "Duet": return 120;
                case "Punk Rock": return 121;
                case "Drum Solo": return 122;
                case "A cappella": return 123;
                case "Euro-House": return 124;
                case "Dance Hall": return 125;
                default: return 12;
            }
        }

        public static string ParseGenre(string value)
        {
            if (Regex.IsMatch(value, @"^\([0-9]+\)$")) return ToGenre(value.Trim(new char[] { '(', ')' }).ToInt32());
            else if (Regex.IsMatch(value, @"^\([0-9]+\)[\w\s]+$"))
            {
                string[] array = value.Split(')');
                return $"{ToGenre(array[0].Trim('(').ToInt32())} {array[1].Trim()}";
            }
            return value;
        }
    }
}