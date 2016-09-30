using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBoundApp1.ViewModels
{
    public class DeleteItemsViewModel
    {
        public List<string> DeleteOptions {
            get
            {
                return new List<string>() { "Vše", "Starší než týden", "Starší než měsíc" };
            }
        }

    }
}
