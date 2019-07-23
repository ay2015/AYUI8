using ay.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Ay.Framework.WPF.Controls.Transitions
{
    public class AyDuijiaoRotateTransition : Transition3D
    {
        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(AyDuijiaoRotateTransition), new UIPropertyMetadata(Duration.Automatic));

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(AyDuijiaoRotateTransition), new UIPropertyMetadata(90.0), IsAngleValid);

        private static bool IsAngleValid(object value)
        {
            double angle = (double)value;
            return angle >= 0 && angle < 180;
        }

        public string OutOrIn
        {
            get { return (string)GetValue(OutOrInProperty); }
            set { SetValue(OutOrInProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutOrIn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutOrInProperty =
            DependencyProperty.Register("OutOrIn", typeof(string), typeof(AyDuijiaoRotateTransition), new PropertyMetadata("In"));


        protected override void BeginTransition3D(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent, Viewport3D viewport)
        {
            Size size = transitionElement.RenderSize;
            Point3D rotationCenter = new Point3D(0.5, 0.5, 0);
            Vector3D rotationAxis = new Vector3D(-1, 1, 0);
            Transform3D translation = null;
            double endAngle = Angle;

            var g = (PerspectiveCamera)viewport.Camera;
            Console.WriteLine(g.Position);
            ModelVisual3D m1;
            if (OutOrIn == "In")
            {

                //translation = new TranslateTransform3D(0,0,-1);
                //DoubleAnimation y = new DoubleAnimation(0, TimeSpan.FromMilliseconds(300));
                //y.AccelerationRatio = 0.3;
                //y.DecelerationRatio = 0.7;
                //translation.BeginAnimation(TranslateTransform3D.OffsetZProperty, y);

                //DoubleAnimation y2 = new DoubleAnimation(40, TimeSpan.FromMilliseconds(500));
                //y2.From = 900;
                //y2.AccelerationRatio = 0.3;
                //y2.DecelerationRatio = 0.7;
                //translation.BeginAnimation(TranslateTransform3D.OffsetZProperty, y2);

                //translation = new ScaleTransform3D();
                //DoubleAnimation w1 = new DoubleAnimation(1, TimeSpan.FromMilliseconds(600));
                //w1.From = 0.7;
                //w1.AccelerationRatio = 0.8;
                //w1.DecelerationRatio = 0.2;
                //translation.BeginAnimation(ScaleTransform3D.ScaleXProperty, w1);
                //translation.BeginAnimation(ScaleTransform3D.ScaleYProperty, w1);
                //translation.BeginAnimation(ScaleTransform3D.ScaleZProperty, w1);



                translation = new ScaleTransform3D();
                DoubleAnimation w1 = new DoubleAnimation(1, Duration);
                w1.From = 0.7;
                w1.AccelerationRatio = 0.3;
                w1.DecelerationRatio = 0.7;
                translation.BeginAnimation(ScaleTransform3D.ScaleXProperty, w1);
                translation.BeginAnimation(ScaleTransform3D.ScaleYProperty, w1);
                translation.BeginAnimation(ScaleTransform3D.ScaleZProperty, w1);

                DoubleAnimation w2 = new DoubleAnimation(0, 1, Duration);
                newContent.BeginAnimation(FrameworkElement.OpacityProperty, w2);

                rotationCenter = new Point3D(0.5, 0.5, 0);
                rotationAxis = new Vector3D(-1, -1, 0);
                endAngle = Angle;
                viewport.Children.Add(m1 = MakeSide(newContent, new Point3D(), new Vector3D(size.Width, 0, 0), new Vector3D(0, size.Height, 0), endAngle, rotationCenter, rotationAxis,
              delegate
              {
                  EndTransition(transitionElement, oldContent, newContent);
              }));
                m1.Transform = translation;
            }
            else
            {
                //translation = new TranslateTransform3D();
                //DoubleAnimation y = new DoubleAnimation(-300, Duration);
                //y.From = 0;
                //y.AccelerationRatio = 0.7;
                //y.DecelerationRatio = 0.3;
                //translation.BeginAnimation(TranslateTransform3D.OffsetYProperty, y);

                translation = new ScaleTransform3D();
                DoubleAnimation w1 = new DoubleAnimation(0.7, Duration);
                w1.From = 1;
                w1.AccelerationRatio = 0.7;
                w1.DecelerationRatio = 0.3;
                translation.BeginAnimation(ScaleTransform3D.ScaleXProperty, w1);
                translation.BeginAnimation(ScaleTransform3D.ScaleYProperty, w1);
                translation.BeginAnimation(ScaleTransform3D.ScaleZProperty, w1);

                DoubleAnimation w2 = new DoubleAnimation(1, 0, Duration);
                oldContent.BeginAnimation(FrameworkElement.OpacityProperty, w2);



                rotationCenter = new Point3D(0.5, 0.5, 0);
                rotationAxis = new Vector3D(-1, -1, 0);
                endAngle = 1 * Angle;
                viewport.Children.Add(m1 = MakeSide(oldContent, new Point3D(), new Vector3D(size.Width, 0, 0), new Vector3D(0, size.Height, 0), endAngle, rotationCenter, rotationAxis,
              delegate
              {
                  EndTransition(transitionElement, oldContent, newContent);
              }));
                m1.Transform = translation;
            }




        }

        private ModelVisual3D MakeSide(
            ContentPresenter content,
            Point3D origin,
            Vector3D u,
            Vector3D v,
            double endAngle,
            Point3D rotationCenter,
            Vector3D rotationAxis,
            EventHandler onCompleted)
        {
            MeshGeometry3D sideMesh = CreateMesh(origin, u, v, 1, 1, new Rect(0, 0, 1, 1));

            GeometryModel3D sideModel = new GeometryModel3D();
            sideModel.Geometry = sideMesh;

            Brush clone = CreateBrush(content);
            sideModel.Material = new DiffuseMaterial(clone);

            if (OutOrIn == "In")
            {

                AxisAngleRotation3D rotation = new AxisAngleRotation3D(rotationAxis, endAngle);
                sideModel.Transform = new RotateTransform3D(rotation, rotationCenter);

                DoubleAnimation da = new DoubleAnimation(0, Duration);
                da.AccelerationRatio = 0.3;
                da.DecelerationRatio = 0.7;
                if (onCompleted != null)
                    da.Completed += onCompleted;

                rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

                ModelVisual3D side = new ModelVisual3D();
                side.Content = sideModel;
                return side;
            }
            else
            {
                AxisAngleRotation3D rotation = new AxisAngleRotation3D(rotationAxis, 0);
                sideModel.Transform = new RotateTransform3D(rotation, rotationCenter);

                DoubleAnimation da = new DoubleAnimation(endAngle, Duration);
                da.AccelerationRatio = 0.7;
                da.DecelerationRatio = 0.3;
                if (onCompleted != null)
                    da.Completed += onCompleted;

                rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

                ModelVisual3D side = new ModelVisual3D();
                side.Content = sideModel;
                return side;
            }

        }

        protected override void OnTransitionEnded(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            newContent.ClearValue(ContentPresenter.RenderTransformProperty);
            oldContent.ClearValue(ContentPresenter.RenderTransformProperty);
        }
    }
}