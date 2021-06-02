using System;
using System.Collections.Generic;
namespace Alexa_proj.Additional_APIs
{

    public enum DoggoQuality 
    {
        Common,
        Rare,
        Mythical,
        Legendary,
        Unknown,
    }

    [Serializable]
    public class DoggoInfo : ExecutableInfo
    {
        public DoggoInfo()
        {

        }

        public class Weight
        {
            public string imperial { get; set; }
            public string metric { get; set; }
        }

        public class Height
        {
            public string imperial { get; set; }
            public string metric { get; set; }
        }

        public class Breed
        {
            public Weight weight { get; set; }
            public Height height { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string bred_for { get; set; }
            public string breed_group { get; set; }
            public string life_span { get; set; }
            public string temperament { get; set; }
            public string reference_image_id { get; set; }
        }

        public List<Breed> breeds { get; set; }
        public string id { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public DoggoQuality Quality { get; set; } = DoggoQuality.Unknown;
    }
}
