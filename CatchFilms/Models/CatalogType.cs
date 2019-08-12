using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchFilms.Models
{
    public class CatalogType
    {
        public int typeID { get; set; }
        public string description { get; set; }
        private List<CatalogType> moviesTypes;
        private List<CatalogType> functionsTypes;
        public CatalogType() { }
        public void init ()
        {
            functionsTypes = new List<CatalogType>();
            moviesTypes = new List<CatalogType>();

            //Tipos de funciones
            functionsTypes.Add(new CatalogType() { typeID = 1, description = "Gran premier" });
            functionsTypes.Add(new CatalogType() { typeID = 2, description = "Estreno" });
            functionsTypes.Add(new CatalogType() { typeID = 3, description = "Próximamente" });

            //Tipos de películas
            moviesTypes.Add(new CatalogType() { typeID = 1, description = "2D" });
            moviesTypes.Add(new CatalogType() { typeID = 2, description = "3D" });
        }

        public IEnumerable<SelectListItem> getFunctionsTypes
        {
            get { return new SelectList(functionsTypes, "typeID", "description"); }
        }

        public IEnumerable<SelectListItem> getMoviesTypes
        {
            get { return new SelectList(moviesTypes, "typeID", "description"); }
        }
    }
}