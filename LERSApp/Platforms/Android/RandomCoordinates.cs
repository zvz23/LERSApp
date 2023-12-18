using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LERSApp.Platforms.Android
{
    public static class RandomCoordinates
    {

        public static readonly List<string> Coordinates = new List<string>()
        {
            "8.606707007806321,123.42042495352953",
            "8.601658116237484,123.41867471657878",
            "8.603540308085144,123.41756503048872",
            "8.604806991151326,123.41759209603205",
            "8.610159126352551,123.41974831565832",
            "8.609686357469199,123.4187739569116",
            "8.598516330575984,123.41274095244017",
            "8.599916836504262,123.41116213060948",
            "8.6069934077821,123.41611200264448",
            "8.605837133043252,123.4140198796062",
            "8.60919986730183,123.42216306619163",
            "8.611576040006552,123.42275315217981",
            "8.607301039959587,123.42617565089662",
            "8.607757183701368,123.42396551065",
            "8.608542173969926,123.4220235913071",
            "8.605858349125603,123.42034989288858"

        };
        public static string GetRandomCoordinate()
        {
            Random rand = new Random();
            int index = rand.Next(0, Coordinates.Count);
            return Coordinates[index];
        }
    }
}
