using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPSharedPanel
{
    public class RealEstate
    {
        public double HomeRating;
        public double LocationRating;
        public double ConvenieceRating;
        public double TotalRating;
        public string Notes;
      
    }
    public class RealEstateViewMode:INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        RealEstate realEstate = new RealEstate();

        public RealEstate CurrentRealEstate {
            get { return this.realEstate; }
            set {
                this.realEstate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentRealEstate)));
            }
        }



    }
}
