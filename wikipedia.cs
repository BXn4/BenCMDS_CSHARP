using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BenCMDS
{
    class wikipedia
    {
        public class Category
        {
            public string sortkey { get; set; }

    }
        public class Thumbnail
        {
            public string source { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Iwlink
    {
        public string prefix { get; set; }
        public string url { get; set; }

}

public class Langlink
{
    public string lang { get; set; }
    public string url { get; set; }
    public string langname { get; set; }
    public string autonym { get; set; }

    }

    public class Link
{
    public int ns { get; set; }
    public string exists { get; set; }
    }

    public class Parse
{
    public string title { get; set; }
    public int pageid { get; set; }
    public int revid { get; set; }
    public Text text { get; set; }
    public List<Langlink> langlinks { get; set; }
    public List<Category> categories { get; set; }
    public List<Link> links { get; set; }
    public List<Template> templates { get; set; }
    public List<string> images { get; set; }
    public List<string> externallinks { get; set; }
    public List<Section> sections { get; set; }
    public string showtoc { get; set; }
    public List<object> parsewarnings { get; set; }
    public string displaytitle { get; set; }
    public List<Iwlink> iwlinks { get; set; }
    public List<Property> properties { get; set; }
}

public class Property
{
    public string name { get; set; }

    }

    public class Root
{
    public Parse parse { get; set; }
}

public class Section
{
    public int toclevel { get; set; }
    public string level { get; set; }
    public string line { get; set; }
    public string number { get; set; }
    public string index { get; set; }
    public string fromtitle { get; set; }
    public int byteoffset { get; set; }
    public string anchor { get; set; }
}

public class Template
{
    public int ns { get; set; }
    public string exists { get; set; }

    }

    public class Text
{

    }

        public class rootx
        {
            public Parse Parse { get; set; }
            public Category Category { get; set; }
            public Iwlink Iwlink { get; set; }
            public Langlink Langlink { get; set; }
            public Property Property { get; set; }
            public Root Root { get; set; }
            public Section Section { get; set; }
            public Template template { get; set; }
            public Text Text { get; set; }
            public Thumbnail Thumbnail { get; set; }
        }
    }
}
