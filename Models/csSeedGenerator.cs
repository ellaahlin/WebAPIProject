using System.Collections.Generic;

namespace Models
{
    public interface ISeed<T> 
    {
        //In order to separate from real and seeded instances
        public bool Seeded { get; set; }

        //Seeded The instance
        public T Seed(csSeedGenerator seedGenerator);
    }
    
    public class csSeedGenerator : Random
    {
        string[] _attractionNames = "Coloseum, Eiffel Tower, Central Park, Disney Land, The Needle".Split(", ");

        string[] _comments = "Great food, Amazing service, Terrific vegan options, Good options for kids, Loads of fun, Perfect for families, Lovely day with the kids, Great service during our visit, Lovely spot for the active ones, Top facility, Amazing views, Just like we had imagined, Recommend everyone to visit".Split(", ");
           
        string[] _city =
            {
                "Stockholm", "Göteborg", "Malmö", "Uppsala", "Linköping", "Örebro",
                "Oslo", "Bergen", "Trondheim", "Stavanger", "Dramen",
                "Köpenhamn", "Århus", "Odense", "Aahlborg", "Esbjerg",
                "Helsingfors", "Espoo", "Tampere", "Vaanta", "Oulu",
                "Madrid", "Barcelona", "Malaga", "Marbella", "Ibiza", "Palma",
                "Washington DC", "Seattle", "New York", "Boston", "Chicago", "Miami", "Los Angeles",
             };
        
        string[] _countries = "Sweden, Norway, Denmark, Finland, Spain, USA".Split(", ");

        string[] _categories = "Food, Entertainment, Sports, Parks".Split(", ");

        string[] _firstnames = "Adam, Anna, James, Jane".Split(", ");
        string[] _lastnames = "Adams, Smith, Jones, Simpson, Clark, Woods, Gomez".Split(", ");

        string[] _domains = "icloud.com, me.com, mac.com, hotmail.com, gmail.com".Split(", ");

        string[][] _titles = {
            // Titles for food attractions
            "Café, Bar, Restaurant, Bistro, Food Stand".Split(", "),
            // Titles for entertainments
            "Amusement Park, Funfield, Museum, Theatre".Split(", "),
            // Titles for sports
            "Tennis Court, Surf Shack, Equestrian Centre, Golf Club".Split(", "),
            // Titles for Parks
            "National Park, City Park, Community Park".Split(", "),
        };

        string[][] _descriptions =
        {
            // Descriptions for food
            "Asian Fusion, Steakhouse, Wine Bar, Fine Dining, Michelin, Club".Split(", "),
            // Description for entertainment
            "Big Amusement Park, Mall, Cinema".Split(", "),
            // Description for sports
            "Local golf club, Hotspot for surfers, Tennis court".Split(", "),
            // Description for national parks
            "Amazing views, Experience the nature".Split(", "),

        };
        public string Title(string Categories = null) // Method that based on the given category will assign a corresponding title to the attraction
        {
            var cIdx = this.Next(0, _titles.Length);
            if (Categories != null)
            {
                // Give comment to specific category
                cIdx = Array.FindIndex(_categories, c => c.ToLower() == Categories.Trim().ToLower());

                if (cIdx == -1) throw new Exception("Category not found");
            }
            return _titles[cIdx][this.Next(0, _titles[cIdx].Length)];
        }

        public string Description(string Categories = null) // Method that based on the given category will assign a corresponding description to the attraction
        {
            var cIdx = this.Next(0, _descriptions.Length);
            if (Categories != null)
            {
                // Give comment to specific category
                cIdx = Array.FindIndex(_categories, c => c.ToLower() == Categories.Trim().ToLower());

                if (cIdx == -1) throw new Exception("Category not found");
            }
            return _descriptions[cIdx][this.Next(0, _descriptions[cIdx].Length)];
        }
        //General random truefalse
        public bool Bool => (this.Next(0, 10) < 5) ? true : false;
        // Randomly assigning country, comment, category and attraction name to the given attraction
        public string Country => _countries[this.Next(0, _countries.Length)];
        public string City => _city[this.Next(0, _city.Length)];
        public string Comments => _comments[this.Next(0, _comments.Length)];
        public string Categories => _categories[this.Next(0, _categories.Length)];
        public string AttractionNames => _attractionNames[this.Next(0, _attractionNames.Length)];
        public string FirstName => _firstnames[this.Next(0, _firstnames.Length)];
        public string LastName => _lastnames[this.Next(0, _lastnames.Length)];
        public string FullName => $"{FirstName} {LastName}";

        public string Email(string fname = null, string lname = null)
        {
            fname ??= FirstName;
            lname ??= LastName;

            return $"{fname}.{lname}@{_domains[this.Next(0, _domains.Length)]}";
        }

        /*public string City(string Country = null) // Method that takes single parameter Country which can be null
        {
            var cIdx = this.Next(0, _city.Length); // Creates random index within the range of items in _city-array
            if (Country != null) // If a country has been assigned to the given attraction
            {
                // The method will proceed to assign a city within that country to the attraction
                cIdx = Array.FindIndex(_countries, c => c.ToLower() == Country.Trim().ToLower());

                if (cIdx == -1) throw new Exception("Country not found"); // Exception if country is not found
            }
            return _city[cIdx][this.Next(0, _city[cIdx].Length)];
            // Since _city is an array of an array, the return will first pick the index of cities
            // that corresponds to the given country, and then randomly pick a city within that country.
        }*/

        #region Seed from own datastructures
        public TEnum FromEnum<TEnum>() where TEnum:struct // Method constrained to only accept arguments of type (TEnum : struct), which ensures that TEnum is in fact a struct
        {
            if (typeof(TEnum).IsEnum) // Checks if the provided type TEnum is indeed an enum-type using typeof.
            {
               
                var _names = typeof(TEnum).GetEnumNames(); // If determined to be an enum, method retrieves the names of the enum values
                var _name = _names[this.Next(0, _names.Length)]; // usin GetEnumNames(), which will return an array of strings containing the name of an enum value
                // Method then generates a random index within the range of valid enum names
                // 'this' refers to an instance of this class (csSeedGenerator)
                return Enum.Parse<TEnum>(_name); // Converts the randomized enum name back into the corresponding enum value of type TEnum & returns this
            }
            throw new ArgumentException("Not an enum type"); // If type is not an enum the method will throw an exception
        }
        public TItem FromList<TItem>(List<TItem> items) // Method that takes a single parameter of type 'List<TItem>', also has type parameter 'TItem' used for return type
        {
            return items[this.Next(0, items.Count)]; // Generates random index within range of valid indices for the provided items-list
        } // Retrieves the item from the list at that index and returns it as its result
        #endregion
        // FromEnum: method for generating a random enum value from a given enum type,
        // provided that the type is indeed an enum.
        //FromList: method for generating a random item from a provided list of items.
        #region generate seeded Lists
        public List<TItem> ToList<TItem>(int NrOfItems)  // Generic method that will return a list of items of type TItem
            // It takes two parameters: TItem and NrOfItems, TItem represents the type of the items in the list, NrOfItems represents just that
            where TItem : ISeed<TItem>, new() // First constraint forces TItem to implement ISeed<TItem>,
                                              // which means that TItem is expected to have method Seed with parameter of type TItem
                                              // TItem must also have a parameterless constructor, which allows the code to create new instances of TItem using new-keyword
        {
            //Create a list of seeded items
            var _list = new List<TItem>(); // Initializes new list of TItem
            for (int c = 0; c < NrOfItems; c++) // Generates a specified number of items an adds them to the _list
                                                // Loop runs NrOfItems times
            {
                _list.Add(new TItem().Seed(this)); // Creates new instance of TItem, then calls Seed() passing this as parameter.
                                                   // The resulting item from the Seed-method is then added to _list
            }
            return _list; // Returns the populated _list, containing NrOfItems items of type TItem
        } // ToList: Method that generates a list of items of generic type 'TItem' by creating
         // new instances of TItem, invoking the Seed-method on each instance for customization and adding these to the list.
         // The constraints ensure that TItem implements ISeed<TItem> and has a parameterless constructor for instantiation.

        public List<TItem> ToListUnique<TItem>(int tryNrOfItems, List<TItem> appendToUnique = null)
             where TItem : ISeed<TItem>, IEquatable<TItem>, new()
        {
            //Create a list of uniquely seeded items
            HashSet<TItem> _set = (appendToUnique == null) ? new HashSet<TItem>() : new HashSet<TItem>(appendToUnique);

            while (_set.Count < tryNrOfItems)
            {
                var _item = new TItem().Seed(this);

                int _preCount = _set.Count();
                int tries = 0;
                do
                {
                    _set.Add(_item);
                    if (++tries >= 5)
                    {
                        //it takes more than 5 tries to generate a random item.
                        //Assume this is it. return the list
                        return _set.ToList();
                    }
                } while (!(_set.Count > _preCount));
            }
            return _set.ToList();
        }

        public List<TItem> FromListUnique<TItem>(int tryNrOfItems, List<TItem> list = null)
        where TItem : ISeed<TItem>, IEquatable<TItem>, new()
        {
            //Create a list of uniquely seeded items
            HashSet<TItem> _set = new HashSet<TItem>();

            while (_set.Count < tryNrOfItems)
            {
                var _item = list[this.Next(0, list.Count)];

                int _preCount = _set.Count();
                int tries = 0;
                do
                {
                    _set.Add(_item);
                    if (++tries >= 5)
                    {
                        //it takes more than 5 tries to generate a random item.
                        //Assume this is it. return the list
                        return _set.ToList();
                    }
                } while (!(_set.Count > _preCount));
            }

            return _set.ToList();
        }
        #endregion
    }
}

