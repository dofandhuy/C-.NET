using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


int[] n1= new int[10] {0,1,2,3,4,5,6,7,8,9};
var nQuery = from tmp in n1 where (tmp % 2) == 0 
                     select tmp;
//
var obj= n1.Where(x=> x%2==0).ToList();
//

int[] n = { 1, 3, -2, -4, -7, -3, -8, 12, 19, 6, 9, 10, 14 };
var nQuery2 = from tmp in n1 where tmp>0 where tmp<12 select tmp;

//
var o1= n.Where(x => x>0).Where(x=> x<12).ToList();
//


List<string> animals = new List<string> { "zebra","elephant ","cat","dog","ant","fox","buffalo","eagle" };
var selectedAnimals = animals.Where(s => s.Length >= 5).Select(x => x.ToUpper());

//
var o2= from animal in animals
        where animal.Length >= 5
        select animal.ToUpper();
//

List<int> numbers = new List<int> { 6, 0, 999, 11, 443, 6, 1, 24, 54 };
var top5= numbers.OrderByDescending(n => n).Take(5);

var n3 = (from ns in numbers orderby n descending select n).Take(5);
        
namespace abc {
    class Pet
    {
        public string Name { get; set; }
        public int Age { get; set; }
}
    
    class Program
    {
public static void OrderByEx1()
        {
            Pet[] pets = {new Pet {Name="Fluffy",Age=4},
                 new Pet{Name="Mittens",Age=1},
                 new Pet{Name="Boots",Age=3},
                 new Pet{Name="Shadow",Age=5} };
            IEnumerable<Pet> query = pets.OrderBy(p => p.Age);
        }
    }
}


namespace mcid
{
     
   class PetOwner
    {
        public string Name { get; set; }
        public List<string> Pets { get; set; }
    }

    class PRogram
    {
    public static void SelectManyEx1()
        {
            PetOwner[] petOwners = { new PetOwner{Name="Higa",Pets=new List<string>{"Scruffy","Sam" } },
                 new PetOwner{Name="Ashkenazi",Pets=new List<string>{"Walker","Sugar" } },
                 new PetOwner{Name="Price",Pets=new List<string>{"Scratches","Diesel" } },
                 new PetOwner{Name= "Hines", Pets= new List<string>{"Dusty" } } };

            var query = petOwners.SelectMany(petOwner => petOwner.Pets,
                                            (petOwner, petName) => new { petOwner ,  petName }).Where(ownerAndPet => ownerAndPet.petName.StartsWith("S")).Select(ownerAndPet => 
                                            new {Owner = ownerAndPet.petOwner.Name,
                                            Pet = ownerAndPet.petName});
        }
        
            
    }
}