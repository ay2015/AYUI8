using System.Windows.Media;

namespace ay.Controls
{
    public class AyTransition : AyPropertyChanged
    {
        private System.Windows.Media.Brush imBrush;

        public System.Windows.Media.Brush ImBrush
        {
            get { return imBrush; }
            set
            {
                if (imBrush != value)
                {
                    imBrush = value;
                    OnPropertyChanged("ImBrush");
                }
              
            }
        }


        private double radius;

        public double Radius
        {
            get { return radius; }
            set
            {
                if (radius != value)
                {
                    radius = value;
                    OnPropertyChanged("Radius");
                }
            }
        }

        private Brush stroke;

        public Brush Stroke
        {
            get { return stroke; }
            set
            {
                if (stroke != value)
                {
                    stroke = value;
                    OnPropertyChanged("Stroke");
                }
            }
        }


        private double strokeThickness;

        public double StrokeThickness
        {
            get { return strokeThickness; }
            set
            {
                if (strokeThickness != value)
                {
                    strokeThickness = value;
                    OnPropertyChanged("StrokeThickness");
                }
            }
        }



        public AyTransition() { }
        public AyTransition(string uri)
        {
            Uri = uri;
        }



        private string _uri;

        public string Uri
        {
            get
            {
                return _uri;
            }

            set
            {
                if (_uri != value)
                {
                    _uri = value;
                    OnPropertyChanged("Uri");
                }
            }
        }
    }
    public class AyTransitionPicture : AyTransition
    {
        public AyTransitionPicture(string uri)
            : base(uri)
        {
            Uri = uri;
        }

        private double width;

        public double Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    width = value;
                    OnPropertyChanged("Width");
                }
            }
        }
        private double height;

        public double Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    OnPropertyChanged("Height");
                }
            }
        }




    }

    public class AyTransitionColor : AyTransition
    {
        public AyTransitionColor(string uri)
            : base(uri)
        {
            Uri = uri;
        }

    }
}
