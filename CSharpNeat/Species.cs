using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpNeat
{
    class Species
    {

        private Indiv headIndiv;
        private List<Indiv> speciesList;

        public Species()
        {
            speciesList = new List<Indiv>();
        }

        public Species(Indiv headIndiv)
        {
            this.headIndiv = headIndiv;
            speciesList = new List<Indiv>();

        }

        internal List<Indiv> SpeciesList { get => speciesList; set => speciesList = value; }


    }
}
