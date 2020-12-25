using System.ComponentModel;

namespace MinecraftMETACreater {
    class BoxInfo {
        public event PropertyChangedEventHandler PropertyChanged;
        private int _Id;
        public int Id {
            get { return _Id; }
            set {
                _Id = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            }
        }

        private string _Title;
        public string Title {
            get { return _Title; }
            set {
                _Title = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }

    }
}
