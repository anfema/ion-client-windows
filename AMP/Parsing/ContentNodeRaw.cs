﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Parsing
{
    public class ContentNodeRaw
    {
        public string type { get; set; }
        public string variation { get; set; }
        public string outlet { get; set; }
        public string mime_type { get; set; }
        public bool is_searchable { get; set; }
        public int file_size { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int length { get; set; }
        public string checksum { get; set; }
        public string file { get; set; }
        public string name { get; set; }
        public string original_mime_type { get; set; }
        public int original_width { get; set; }
        public int original_height { get; set; }
        public int original_length { get; set; }
        public int original_file_size { get; set; }
        public string original_checksum { get; set; }
        public string original_file { get; set; }
        public List<KeyValuePairRaw> values { get; set; } // special treatment after parsing
        public string value { get; set; }
        public string datetime { get; set; }
        public bool? is_enabled { get; set; }
        public int? r { get; set; }
        public int? g { get; set; }
        public int? b { get; set; }
        public int? a { get; set; }
        public string image { get; set; }
        public string original_image { get; set; }
        public int? translation_x { get; set; }
        public int? translation_y { get; set; }
        public double? scale { get; set; }
        public string text { get; set; }
        public bool is_multiline { get; set; }
        public string connection_string { get; set; }
    }
}